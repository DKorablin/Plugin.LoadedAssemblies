using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;

#if NETCOREAPP
using System.Diagnostics.Tracing;
#endif

namespace Plugin.LoadedAssemblies.Core
{
	public static class RuntimeClrStatsListener
	{
		/// <summary>Gets all available EventSource names or Performance Counter categories</summary>
		public static String[] GetEventSourceNames()
		{
#if NETCOREAPP
			return EventSource.GetSources().Select(s => s.Name).ToArray();
#else
			// Get all .NET CLR performance counter categories
			List<String> clrCategories = new List<String>();
			PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories();
			foreach(PerformanceCounterCategory category in categories)
			{
				if(category.CategoryName.StartsWith(".NET CLR", StringComparison.OrdinalIgnoreCase))
					clrCategories.Add(category.CategoryName);
			}
			return clrCategories.ToArray();
#endif
		}

		/// <summary>Creates an event listener for the specified EventSource name or Performance Counter category</summary>
		/// <param name="eventSourceName">The name of the EventSource or Performance Counter category to listen to</param>
		/// <param name="onEventReceived">Callback to handle received events</param>
		/// <returns>A disposable event listener, or null if the EventSource/Category was not found</returns>
		public static IDisposable CreateEventListener(String eventSourceName, Action<EventArgs> onEventReceived)
		{
			if(String.IsNullOrEmpty(eventSourceName))
				return null;

#if NETCOREAPP
			EventSource selectedSource = EventSource.GetSources().FirstOrDefault(s => s.Name == eventSourceName);
			if(selectedSource == null)
				return null;

			ClrEventListener listener = new ClrEventListener(selectedSource, onEventReceived);
			listener.EnableEvents();
			return listener;
#else
			// Create Performance Counter listener for .NET Framework
			try
			{
				String processName = Process.GetCurrentProcess().ProcessName;
				PerformanceCounterListener listener = new PerformanceCounterListener(eventSourceName, processName, onEventReceived);
				listener.Start();
				return listener;
			} catch
			{
				return null;
			}
#endif
		}

		/// <summary>Formats event data into a readable string</summary>
		public static IEnumerable<KeyValuePair<String, Object>> FormatEventData(EventArgs eventData)
		{
#if NETCOREAPP
			EventWrittenEventArgs args = (EventWrittenEventArgs)eventData;
			// Build payload information
			if(args.Payload?.Count > 0)
			{
				foreach(var payloadItem in args.Payload)
					if(payloadItem is IDictionary<String, Object> payloadFields)
					{
						Object name = null;
						if((payloadFields.TryGetValue("DisplayName", out var displayName) ||  payloadFields.TryGetValue("Name", out name)) &&
							payloadFields.TryGetValue("Mean", out var mean))
						{
							yield return new KeyValuePair<String, Object>((String)(displayName ?? name), mean);
						}
					}
			}
#else
			PerformanceCounterEventArgs pcArgs = (PerformanceCounterEventArgs)eventData;
			if(pcArgs.Counters != null)
			{
				foreach(var counter in pcArgs.Counters)
					yield return counter;
			}
#endif
		}

#if NETCOREAPP
		private sealed class ClrEventListener : EventListener
		{
			private readonly EventSource _selectedSource;
			private Action<EventArgs> _onEventReceived;
			private volatile Boolean _disposed;

			public ClrEventListener(EventSource selectedSource, Action<EventArgs> onEventReceived)
			{
				this._selectedSource = selectedSource ?? throw new ArgumentNullException(nameof(selectedSource));
				this._onEventReceived = onEventReceived;
				this._disposed = false;
			}

			public void EnableEvents()
				=> this.EnableEvents(this._selectedSource, EventLevel.Informational, EventKeywords.All, new Dictionary<String, String?>
				{
					["EventCounterIntervalSec"] = "1"
				});

			protected override void OnEventWritten(EventWrittenEventArgs eventData)
			{
				if(!_disposed)
					_onEventReceived?.Invoke(eventData);
			}

			public override void Dispose()
			{
				_disposed = true;
				this.DisableEvents(this._selectedSource);
				this._onEventReceived = null;
				base.Dispose();
			}
		}
#else
		/// <summary>Event args for Performance Counter data</summary>
		private sealed class PerformanceCounterEventArgs : EventArgs
		{
			public IEnumerable<KeyValuePair<String, Object>> Counters { get; set; }
		}

		/// <summary>Performance Counter listener for .NET Framework</summary>
		private class PerformanceCounterListener : IDisposable
		{
			private readonly String _categoryName;
			private readonly String _instanceName;
			private readonly Action<EventArgs> _onEventReceived;
			private readonly Timer _timer;
			private readonly List<PerformanceCounter> _counters;
			private volatile Boolean _disposed;

			public PerformanceCounterListener(String categoryName, String instanceName, Action<EventArgs> onEventReceived)
			{
				this._categoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
				this._instanceName = instanceName ?? throw new ArgumentNullException(nameof(instanceName));
				this._onEventReceived = onEventReceived ?? throw new ArgumentNullException(nameof(onEventReceived));
				this._counters = new List<PerformanceCounter>();
				this._disposed = false;

				// Initialize performance counters
				this.InitializeCounters();

				// Create timer with 1 second interval
				this._timer = new Timer(this.OnTimerCallback, null, Timeout.Infinite, Timeout.Infinite);
			}

			private void InitializeCounters()
			{
				PerformanceCounterCategory category = new PerformanceCounterCategory(this._categoryName);

				// Check if category has instances
				if(category.CategoryType == PerformanceCounterCategoryType.MultiInstance)
				{
					String[] counterNames = category.GetCounters(this._instanceName).Select(c => c.CounterName).ToArray();
					foreach(String counterName in counterNames)
					{
						PerformanceCounter counter = new PerformanceCounter(this._categoryName, counterName, this._instanceName, true);
						this._counters.Add(counter);
					}
				} else
				{
					// Single instance category
					PerformanceCounter[] categoryCounters = category.GetCounters();
					foreach(PerformanceCounter counter in categoryCounters)
					{
						PerformanceCounter newCounter = new PerformanceCounter(this._categoryName, counter.CounterName, true);
						this._counters.Add(newCounter);
					}
				}
			}

			public void Start()
			{
				if(!_disposed && this._counters.Count > 0)
					this._timer.Change(0, 1000);// Start timer with 1 second interval
			}

			private void OnTimerCallback(Object state)
			{
				if(this._disposed)
					return;

				List<KeyValuePair<String, Object>> counterValues = new List<KeyValuePair<String, Object>>();

				foreach(PerformanceCounter counter in this._counters)
				{
					Single value = counter.NextValue();
					counterValues.Add(new KeyValuePair<String, Object>(counter.CounterName, value));
				}

				if(counterValues.Count > 0 && !_disposed)
				{
					PerformanceCounterEventArgs eventArgs = new PerformanceCounterEventArgs
					{
						Counters = counterValues
					};
					this._onEventReceived?.Invoke(eventArgs);
				}
			}

			public void Dispose()
			{
				if(this._disposed)
					return;

				this._disposed = true;

				// Stop timer
				this._timer?.Change(Timeout.Infinite, Timeout.Infinite);
				this._timer?.Dispose();

				// Dispose all counters
				foreach(PerformanceCounter counter in this._counters)
					counter.Dispose();

				this._counters.Clear();
			}
		}
#endif
	}
}