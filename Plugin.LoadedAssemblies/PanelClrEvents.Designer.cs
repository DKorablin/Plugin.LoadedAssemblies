namespace Plugin.LoadedAssemblies
{
	partial class PanelClrEvents
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
			this.tscbEventSources = new System.Windows.Forms.ToolStripComboBox();
			this.lvEvents = new AlphaOmega.Windows.Forms.DbListView();
			this.colEventName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colEventMean = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsMain
			// 
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbEventSources});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(150, 25);
			this.tsMain.TabIndex = 0;
			this.tsMain.Text = "toolStrip1";
			// 
			// tscbEventSources
			// 
			this.tscbEventSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tscbEventSources.DropDownWidth = 300;
			this.tscbEventSources.Name = "tscbEventSources";
			this.tscbEventSources.Size = new System.Drawing.Size(121, 25);
			this.tscbEventSources.Sorted = true;
			this.tscbEventSources.DropDown += this.tscbEventSources_DropDown;
			this.tscbEventSources.SelectedIndexChanged += new System.EventHandler(this.tscbEventSources_SelectedIndexChanged);
			// 
			// lvEvents
			// 
			this.lvEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colEventName,
            this.colEventMean});
			this.lvEvents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvEvents.FullRowSelect = true;
			this.lvEvents.GridLines = true;
			this.lvEvents.HideSelection = false;
			this.lvEvents.Location = new System.Drawing.Point(0, 25);
			this.lvEvents.Name = "lvEvents";
			this.lvEvents.Size = new System.Drawing.Size(150, 125);
			this.lvEvents.TabIndex = 1;
			this.lvEvents.UseCompatibleStateImageBehavior = false;
			this.lvEvents.View = System.Windows.Forms.View.Details;
			// 
			// colEventName
			// 
			this.colEventName.Text = "Name";
			// 
			// colEventMean
			// 
			this.colEventMean.Text = "Mean";
			// 
			// PanelClrEvents
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lvEvents);
			this.Controls.Add(this.tsMain);
			this.Name = "PanelClrEvents";
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripComboBox tscbEventSources;
		private AlphaOmega.Windows.Forms.DbListView lvEvents;
		private System.Windows.Forms.ColumnHeader colEventName;
		private System.Windows.Forms.ColumnHeader colEventMean;
	}
}
