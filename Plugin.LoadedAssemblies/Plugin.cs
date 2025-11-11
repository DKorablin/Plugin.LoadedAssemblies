using System;
using System.Collections.Generic;
using System.Diagnostics;
using SAL.Flatbed;
using SAL.Windows;

namespace Plugin.LoadedAssemblies
{
	public class Plugin : IPlugin
	{
		private TraceSource _trace;
		private Dictionary<String, DockState> _documentTypes;
		private IMenuItem _menuTest;
		private IMenuItem _menuAssembly;
		private IMenuItem _menuClrEvents;

		internal TraceSource Trace { get { return this._trace ?? (this._trace = Plugin.CreateTraceSource<Plugin>()); } }

		internal IHostWindows HostWindows { get; }

		private Dictionary<String, DockState> DocumentTypes
		{
			get
			{
				if(this._documentTypes == null)
					this._documentTypes = new Dictionary<String, DockState>()
					{
						{ typeof(PanelAssemblies).ToString(), DockState.DockRightAutoHide },
						{ typeof(PanelClrEvents).ToString(), DockState.DockRightAutoHide },
					};
				return this._documentTypes;
			}
		}

		public Plugin(IHostWindows hostWindows)
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

			this._menuTest = menuView.FindMenuItem("Executables");
			if(this._menuTest == null)
			{
				this._menuTest = menuView.Create("Executables");
				this._menuTest.Name = "View.Executable";
				menuView.Items.Add(this._menuTest);
			}

			this._menuAssembly = this._menuTest.Create("Loaded &Assemblies");
			this._menuAssembly.Name = "View.Executable.LoadedAssemblies";
			this._menuAssembly.Click += (sender, e) => this.CreateWindow(typeof(PanelAssemblies).ToString(), false);

			this._menuClrEvents = this._menuTest.Create("&CLR Events");
			this._menuClrEvents.Name = "View.Executable.ClrEvents";
			this._menuClrEvents.Click += (sender, e) => this.CreateWindow(typeof(PanelClrEvents).ToString(), true);

			this._menuTest.Items.AddRange(new IMenuItem[] { this._menuAssembly, this._menuClrEvents, });
			return true;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			if(this._menuAssembly != null)
				this.HostWindows.MainMenu.Items.Remove(this._menuAssembly);

			if(this._menuClrEvents != null)
				this.HostWindows.MainMenu.Items.Remove(this._menuClrEvents);

			if(this._menuTest != null && this._menuTest.Items.Count == 0)
				this.HostWindows.MainMenu.Items.Remove(this._menuTest);
			return true;
		}

		private IWindow CreateWindow(String typeName, Boolean searchForOpened, Object args = null)
			=> this.DocumentTypes.TryGetValue(typeName, out DockState state)
				? this.HostWindows.Windows.CreateWindow(this, typeName, searchForOpened, state, args)
				: null;

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