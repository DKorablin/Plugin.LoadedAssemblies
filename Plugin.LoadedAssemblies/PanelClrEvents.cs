using System;
using System.Drawing;
using System.Windows.Forms;
using Plugin.LoadedAssemblies.Core;
using SAL.Windows;

namespace Plugin.LoadedAssemblies
{
	public partial class PanelClrEvents : UserControl
	{
		private static readonly Color NotModified = Color.Gray;

		private Plugin Plugin => (Plugin)this.Window.Plugin;

		private IWindow Window => (IWindow)base.Parent;

		private IDisposable _eventListener;

		public PanelClrEvents()
		{
			this.InitializeComponent();
			this.HandleDestroyed += this.PanelClrEvents_HandleDestroyed;
		}

		private void PanelClrEvents_HandleDestroyed(Object sender, EventArgs e)
		{
			_eventListener?.Dispose();
			_eventListener = null;
		}

		protected override void OnCreateControl()
		{
			this.Window.Caption = "CLR Events";
			base.OnCreateControl();
		}

		private void tscbEventSources_DropDown(Object sender, EventArgs e)
		{
			if(tscbEventSources.Items.Count > 0)
				return;

			String[] eventSources = RuntimeClrStatsListener.GetEventSourceNames();
			tscbEventSources.Items.AddRange(eventSources);

			if(tscbEventSources.Items.Count == 0)
				lvEvents.Enabled = false;
		}

		private void tscbEventSources_SelectedIndexChanged(Object sender, EventArgs e)
		{
			// Dispose previous listener if exists
			_eventListener?.Dispose();
			_eventListener = null;

			if(tscbEventSources.SelectedItem != null)
			{
				String selectedSourceName = tscbEventSources.SelectedItem.ToString();
				_eventListener = RuntimeClrStatsListener.CreateEventListener(selectedSourceName, new Action<EventArgs>(this.OnEventReceived));
			}
			lvEvents.Items.Clear();
		}

		private void OnEventReceived(EventArgs eventData)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action<EventArgs>(this.OnEventReceived), eventData);
				return;
			}

			lvEvents.SuspendLayout();
			try
			{
				foreach(var item in RuntimeClrStatsListener.FormatEventData(eventData))
				{
					Boolean itemExists = false;
					foreach(ListViewItem listItem in lvEvents.Items)
					{
						if(listItem.SubItems[colEventName.Index].Text == item.Key)
						{
							itemExists = true;
							
							var value = item.Value?.ToString() ?? "null";
							if(value == listItem.SubItems[colEventMean.Index].Text)
								listItem.ForeColor = NotModified;
							else
							{
								listItem.ForeColor = Control.DefaultForeColor;
								listItem.SubItems[colEventMean.Index].Text = value;
							}
							break;
						}
					}

					if(!itemExists)
					{
						ListViewItem newItem = new ListViewItem(item.Key);
						newItem.SubItems.Add(item.Value?.ToString() ?? "null");
						lvEvents.Items.Add(newItem);
						lvEvents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
					}
				}
			} finally
			{
				lvEvents.ResumeLayout();
			}
		}
	}
}