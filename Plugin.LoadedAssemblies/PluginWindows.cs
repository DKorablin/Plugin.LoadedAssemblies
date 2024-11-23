using System;
using System.Collections.Generic;
using System.Diagnostics;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.LoadedAssemblies
{
	public class PluginWindows : IPlugin
	{
		private TraceSource _trace;
		private Dictionary<String, DockState> _documentTypes;
		private IMenuItem _menuTest;
		private IMenuItem _menuAssembly;

		internal TraceSource Trace { get { return this._trace ?? (this._trace = PluginWindows.CreateTraceSource<PluginWindows>()); } }

		internal IHostWindows HostWindows { get; }

		private Dictionary<String, DockState> DocumentTypes
		{
			get
			{
				if(this._documentTypes == null)
					this._documentTypes = new Dictionary<String, DockState>()
					{
						{ typeof(PanelAssemblies).ToString(), DockState.DockRightAutoHide },
					};
				return this._documentTypes;
			}
		}

		public PluginWindows(IHostWindows hostWindows)
		{
			this.HostWindows = hostWindows ?? throw new ArgumentNullException(nameof(hostWindows));
		}

		public IWindow GetPluginControl(String typeName, Object args)
		{
			return this.CreateWindow(typeName, false, args);
		}

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			IMenuItem menuView = this.HostWindows.MainMenu.FindMenuItem("View");
			if(menuView == null)
			{
				this.Trace.TraceEvent(TraceEventType.Error, 10, "Menu item 'View' not found");
				return false;
			}

			this._menuTest = menuView.FindMenuItem("Test");
			if(this._menuTest == null)
			{
				this._menuTest = menuView.Create("Test");
				this._menuTest.Name = "View.Test";
				menuView.Items.Add(this._menuTest);
			}

			this._menuAssembly = this._menuTest.Create("Loaded &Assemblies");
			this._menuAssembly.Name = "View.Test.LoadedAssemblies";
			this._menuAssembly.Click += (sender, e) => { this.CreateWindow(typeof(PanelAssemblies).ToString(), false); };

			this._menuTest.Items.Add(this._menuAssembly);
			return true;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			if(this._menuAssembly != null)
				this.HostWindows.MainMenu.Items.Remove(this._menuAssembly);

			if(this._menuTest != null && this._menuTest.Items.Count == 0)
				this.HostWindows.MainMenu.Items.Remove(this._menuTest);
			return true;
		}

		private IWindow CreateWindow(String typeName, Boolean searchForOpened, Object args = null)
		{
			DockState state;
			return this.DocumentTypes.TryGetValue(typeName, out state)
				? this.HostWindows.Windows.CreateWindow(this, typeName, searchForOpened, state, args)
				: null;
		}

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}
	}
}