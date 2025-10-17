namespace Plugin.LoadedAssemblies
{
	partial class PanelAssemblies
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.tsbnRefresh = new System.Windows.Forms.ToolStripButton();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.lvAssemblies = new AlphaOmega.Windows.Forms.DbListView();
			this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colRuntimeVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colEntryPoint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cmsAssemblies = new AlphaOmega.Windows.Forms.ContextMenuStripCopy();
			this.tabAssembly = new System.Windows.Forms.TabControl();
			this.tabModules = new System.Windows.Forms.TabPage();
			this.lvReferences = new System.Windows.Forms.ListView();
			this.colModuleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ssMain = new System.Windows.Forms.StatusStrip();
			this.lblElapsed = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsmiAssembliesExplore = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAssembliesShowDependencies = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.cmsAssemblies.SuspendLayout();
			this.tabAssembly.SuspendLayout();
			this.tabModules.SuspendLayout();
			this.ssMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsMain
			// 
			this.tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbnRefresh});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(200, 27);
			this.tsMain.TabIndex = 0;
			this.tsMain.Text = "toolStrip1";
			// 
			// tsbnRefresh
			// 
			this.tsbnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbnRefresh.Image = global::Plugin.LoadedAssemblies.Properties.Resources.iconRefresh;
			this.tsbnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbnRefresh.Name = "tsbnRefresh";
			this.tsbnRefresh.Size = new System.Drawing.Size(29, 24);
			this.tsbnRefresh.ToolTipText = "Refresh";
			this.tsbnRefresh.Click += new System.EventHandler(this.tsbnRefresh_Click);
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.Location = new System.Drawing.Point(0, 27);
			this.splitMain.Margin = new System.Windows.Forms.Padding(4);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.lvAssemblies);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.tabAssembly);
			this.splitMain.Size = new System.Drawing.Size(200, 136);
			this.splitMain.SplitterDistance = 67;
			this.splitMain.SplitterWidth = 5;
			this.splitMain.TabIndex = 2;
			this.splitMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.splitMain_MouseDoubleClick);
			// 
			// lvAssemblies
			// 
			this.lvAssemblies.AllowColumnReorder = true;
			this.lvAssemblies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colRuntimeVersion,
            this.colEntryPoint,
            this.colPath});
			this.lvAssemblies.ContextMenuStrip = this.cmsAssemblies;
			this.lvAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvAssemblies.FullRowSelect = true;
			this.lvAssemblies.HideSelection = false;
			this.lvAssemblies.Location = new System.Drawing.Point(0, 0);
			this.lvAssemblies.Margin = new System.Windows.Forms.Padding(4);
			this.lvAssemblies.Name = "lvAssemblies";
			this.lvAssemblies.Size = new System.Drawing.Size(200, 67);
			this.lvAssemblies.TabIndex = 1;
			this.lvAssemblies.UseCompatibleStateImageBehavior = false;
			this.lvAssemblies.View = System.Windows.Forms.View.Details;
			this.lvAssemblies.SelectedIndexChanged += new System.EventHandler(this.lvAssemblies_SelectedIndexChanged);
			this.lvAssemblies.ColumnClick += this.lvAssemblies_ColumnClick;
			// 
			// colName
			// 
			this.colName.Text = "Name";
			// 
			// colRuntimeVersion
			// 
			this.colRuntimeVersion.Text = "Runtime Version";
			// 
			// colEntryPoint
			// 
			this.colEntryPoint.Text = "EntryPoint";
			// 
			// colPath
			// 
			this.colPath.Text = "Path";
			// 
			// cmsAssemblies
			// 
			this.cmsAssemblies.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.cmsAssemblies.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.tsmiAssembliesExplore,
			this.tsmiAssembliesShowDependencies});
			this.cmsAssemblies.Name = "cmsAssemblies";
			this.cmsAssemblies.Size = new System.Drawing.Size(211, 80);
			this.cmsAssemblies.Opening += new System.ComponentModel.CancelEventHandler(this.cmsAssemblies_Opening);
			this.cmsAssemblies.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsAssemblies_ItemClicked);
			// 
			// tabAssembly
			// 
			this.tabAssembly.Controls.Add(this.tabModules);
			this.tabAssembly.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabAssembly.Location = new System.Drawing.Point(0, 0);
			this.tabAssembly.Margin = new System.Windows.Forms.Padding(4);
			this.tabAssembly.Name = "tabAssembly";
			this.tabAssembly.SelectedIndex = 0;
			this.tabAssembly.Size = new System.Drawing.Size(200, 64);
			this.tabAssembly.TabIndex = 0;
			// 
			// tabModules
			// 
			this.tabModules.Controls.Add(this.lvReferences);
			this.tabModules.Location = new System.Drawing.Point(4, 25);
			this.tabModules.Margin = new System.Windows.Forms.Padding(4);
			this.tabModules.Name = "tabModules";
			this.tabModules.Padding = new System.Windows.Forms.Padding(4);
			this.tabModules.Size = new System.Drawing.Size(192, 35);
			this.tabModules.TabIndex = 0;
			this.tabModules.Text = "References";
			this.tabModules.UseVisualStyleBackColor = true;
			// 
			// lvReferences
			// 
			this.lvReferences.AllowColumnReorder = true;
			this.lvReferences.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colModuleName});
			this.lvReferences.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvReferences.FullRowSelect = true;
			this.lvReferences.HideSelection = false;
			this.lvReferences.Location = new System.Drawing.Point(4, 4);
			this.lvReferences.Margin = new System.Windows.Forms.Padding(4);
			this.lvReferences.Name = "lvReferences";
			this.lvReferences.Size = new System.Drawing.Size(184, 27);
			this.lvReferences.TabIndex = 0;
			this.lvReferences.UseCompatibleStateImageBehavior = false;
			this.lvReferences.View = System.Windows.Forms.View.Details;
			// 
			// colModuleName
			// 
			this.colModuleName.Text = "Name";
			// 
			// ssMain
			// 
			this.ssMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblElapsed});
			this.ssMain.Location = new System.Drawing.Point(0, 163);
			this.ssMain.Name = "ssMain";
			this.ssMain.Size = new System.Drawing.Size(200, 22);
			this.ssMain.TabIndex = 1;
			this.ssMain.Text = "statusStrip1";
			// 
			// lblElapsed
			// 
			this.lblElapsed.Name = "lblElapsed";
			this.lblElapsed.Size = new System.Drawing.Size(0, 16);
			// 
			// tsmiAssembliesExplore
			// 
			this.tsmiAssembliesExplore.Name = "tsmiAssembliesExplore";
			this.tsmiAssembliesExplore.Size = new System.Drawing.Size(210, 24);
			this.tsmiAssembliesExplore.Text = "Show in &Folder";
			// 
			// tsmiAssembliesShowDependencies
			// 
			this.tsmiAssembliesShowDependencies.Name = "tsmiAssembliesShowDependencies";
			this.tsmiAssembliesShowDependencies.Size = new System.Drawing.Size(210, 24);
			this.tsmiAssembliesShowDependencies.Text = "Show in &Dependencies";
			// 
			// PanelAssemblies
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Controls.Add(this.tsMain);
			this.Controls.Add(this.ssMain);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "PanelAssemblies";
			this.Size = new System.Drawing.Size(200, 185);
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.cmsAssemblies.ResumeLayout(false);
			this.tabAssembly.ResumeLayout(false);
			this.tabModules.ResumeLayout(false);
			this.ssMain.ResumeLayout(false);
			this.ssMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsMain;
		private AlphaOmega.Windows.Forms.DbListView lvAssemblies;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colPath;
		private System.Windows.Forms.ToolStripButton tsbnRefresh;
		private System.Windows.Forms.ColumnHeader colRuntimeVersion;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.TabControl tabAssembly;
		private System.Windows.Forms.TabPage tabModules;
		private System.Windows.Forms.ListView lvReferences;
		private System.Windows.Forms.ColumnHeader colModuleName;
		private System.Windows.Forms.ColumnHeader colEntryPoint;
		private System.Windows.Forms.StatusStrip ssMain;
		private System.Windows.Forms.ToolStripStatusLabel lblElapsed;
		private AlphaOmega.Windows.Forms.ContextMenuStripCopy cmsAssemblies;
		private System.Windows.Forms.ToolStripMenuItem tsmiAssembliesExplore;
		private System.Windows.Forms.ToolStripMenuItem tsmiAssembliesShowDependencies;
	}
}
