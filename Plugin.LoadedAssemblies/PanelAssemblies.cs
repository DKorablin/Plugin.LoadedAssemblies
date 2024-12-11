using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using SAL.Windows;

namespace Plugin.LoadedAssemblies
{
	public partial class PanelAssemblies : UserControl
	{
		private const String CaptionArgs1 = "Assemblies ({0:N0})";

		private static readonly Color DynamicColor = Color.Green;
		private static readonly Color ErrorColor = Color.Red;
		private static readonly Color DublicateColor = Color.Orange;

		private PluginWindows Plugin => (PluginWindows)this.Window.Plugin;

		private IWindow Window => (IWindow)base.Parent;

		public PanelAssemblies()
		{
			this.InitializeComponent();
			splitMain.Panel2Collapsed = true;
		}

		protected override void OnCreateControl()
		{
			this.ShowAssemblies();

			base.OnCreateControl();
		}

		private void ShowAssemblies()
		{
			String selectedAssemblyFullName = lvAssemblies.SelectedItems.Count == 1 ? lvAssemblies.SelectedItems[0].SubItems[colName.Index].Text : null;
			lvAssemblies.Items.Clear();
			lvReferences.Items.Clear();
			splitMain.Panel2Collapsed = true;
			Stopwatch sw = new Stopwatch();
			sw.Start();

			try
			{
				base.Cursor = Cursors.WaitCursor;

				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				List<ListViewItem> itemsToAdd = new List<ListViewItem>(assemblies.Length);
				HashSet<String> assemblyPath = new HashSet<String>();
				String[] subItems = Array.ConvertAll<String, String>(new String[lvAssemblies.Columns.Count], delegate(String a) { return String.Empty; });

				foreach(Assembly assembly in assemblies)
				{
					ListViewItem item = new ListViewItem(subItems) { Tag = assembly, Group=lvAssemblies.Groups[0], };
					item.SubItems[colName.Index].Text = assembly.FullName;
					item.SubItems[colRuntimeVersion.Index].Text = assembly.ImageRuntimeVersion;
					try
					{
						if(assembly.EntryPoint != null)
							item.SubItems[colEntryPoint.Index].Text = assembly.EntryPoint.Name;

						if(selectedAssemblyFullName == assembly.FullName)
							item.Selected = true;

						String location;
						if(assembly.ManifestModule.Name == "<In Memory Module>")
						{
							location = assembly.ManifestModule.Name;
							item.ForeColor = DynamicColor;//IsDynamic
						} else
						{
							location = assembly.Location;
							assemblyPath.Add(location);
						}
						item.SubItems[colPath.Index].Text = location;

						// Checking for dublicate assemblies. For example different plugins may reference different versions
						if(Array.FindAll(assemblies, a => { return a.GetName().Name == assembly.GetName().Name; }).Length > 1)
							item.ForeColor = DublicateColor;
					} catch(FileNotFoundException exc)
					{
						item.SubItems[colPath.Index].Text = exc.Message;
						item.ForeColor = ErrorColor;
						this.Plugin.Trace.TraceData(System.Diagnostics.TraceEventType.Error, 10, exc);
					} catch(NotSupportedException exc)
					{
						item.SubItems[colPath.Index].Text = exc.Message;
						item.ForeColor = ErrorColor;
						this.Plugin.Trace.TraceData(System.Diagnostics.TraceEventType.Error, 10, exc);
					}

					itemsToAdd.Add(item);
				}
				foreach(ProcessModule module in Process.GetCurrentProcess().Modules)
				{
					if(assemblyPath.Contains(module.FileName))
						continue;//We placed this module to assembly list already

					ListViewItem item = new ListViewItem(subItems) { Tag = module, Group = lvAssemblies.Groups[1], };
					try
					{
						item.SubItems[colName.Index].Text = module.ModuleName;
						item.SubItems[colPath.Index].Text = module.FileName;
						item.SubItems[colEntryPoint.Index].Text = module.EntryPointAddress.ToString();
						item.SubItems[colRuntimeVersion.Index].Text = module.FileVersionInfo.ProductVersion;
					} catch(FileNotFoundException exc)
					{
						item.SubItems[colPath.Index].Text = exc.Message;
						item.ForeColor = Color.Red;
						this.Plugin.Trace.TraceData(TraceEventType.Error, 10, exc);
					}
					itemsToAdd.Add(item);
				}

				/*foreach(_AppDomain domain in MsCorEE.EnumAppDomains())
				{
					ListViewItem item = new ListViewItem(subItems) { Tag = domain, Group = lvAssemblies.Groups[2], };
					item.SubItems[colName.Index].Text = domain.FriendlyName;
					item.SubItems[colPath.Index].Text = domain.BaseDirectory;
					itemsToAdd.Add(item);
				}*/

				lvAssemblies.Items.AddRange(itemsToAdd.ToArray());
				lvAssemblies.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				this.Window.Caption = String.Format(PanelAssemblies.CaptionArgs1, itemsToAdd.Count);
			} finally
			{
				base.Cursor = Cursors.Default;
				sw.Stop();
				lblElapsed.Text = sw.Elapsed.ToString();
			}
		}

		private void lvAssemblies_SelectedIndexChanged(Object sender, EventArgs e)
		{
			ListViewItem selectedItem = lvAssemblies.SelectedItems.Count == 1 ? lvAssemblies.SelectedItems[0] : null;
			lvReferences.Items.Clear();
			if(selectedItem != null)
				if(selectedItem.Tag is Assembly assembly)
				{
					splitMain.Panel2Collapsed = false;
					String[] subItems = Array.ConvertAll<String, String>(new String[lvAssemblies.Columns.Count], delegate (String a) { return String.Empty; });
					AssemblyName[] assemblyNames = assembly.GetReferencedAssemblies();
					List<ListViewItem> itemsToAdd = new List<ListViewItem>(assemblyNames.Length);
					foreach(AssemblyName asmName in assemblyNames)
					{
						ListViewItem item = new ListViewItem() { Tag = assembly, };
						item.SubItems[colModuleName.Index].Text = asmName.FullName;
						itemsToAdd.Add(item);
					}

					lvReferences.Items.AddRange(itemsToAdd.ToArray());
					lvReferences.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
				}else if(selectedItem.Tag is ProcessModule module)
				{
					splitMain.Panel2Collapsed = true;
				}
		}

		private void tsbnRefresh_Click(Object sender, EventArgs e)
		{
			this.ShowAssemblies();
		}

		private void splitMain_MouseDoubleClick(Object sender, MouseEventArgs e)
		{
			if(splitMain.SplitterRectangle.Contains(e.Location))
				splitMain.Panel2Collapsed = true;
		}

		private void cmsAssemblies_Opening(Object sender, System.ComponentModel.CancelEventArgs e)
		{
			ListViewItem selectedItem = lvAssemblies.SelectedItems.Count == 1 ? lvAssemblies.SelectedItems[0] : null;
			Boolean isDirectoryExists = false;
			if(selectedItem != null)
			{
				String filePath = selectedItem.SubItems[colPath.Index].Text;
				isDirectoryExists = Directory.Exists(Path.GetDirectoryName(filePath));
			}
			tsmiAssembliesExplore.Enabled = isDirectoryExists;
		}

		private void cmsAssemblies_ItemClicked(Object sender, ToolStripItemClickedEventArgs e)
		{
			if(e.ClickedItem == tsmiAssembliesExplore)
			{
				ListViewItem selectedItem = lvAssemblies.SelectedItems.Count == 1 ? lvAssemblies.SelectedItems[0] : null;
				String filePath = selectedItem.SubItems[colPath.Index].Text;
				Shell32.OpenFolderAndSelectItem(Path.GetDirectoryName(filePath), Path.GetFileName(filePath));
			}
		}
	}
}