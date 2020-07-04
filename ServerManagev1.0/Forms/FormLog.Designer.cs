namespace ServerManagev1._0.Forms
{
    partial class frmLog
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLog));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.btnClearLog = new DevExpress.XtraBars.BarButtonItem();
            this.btnOpenFileLog = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.gridListLog = new DevExpress.XtraGrid.GridControl();
            this.gridView_Log = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.timerLog = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridListLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Log)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.ribbon.SearchEditItem,
            this.btnClearLog,
            this.btnOpenFileLog});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 3;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbon.Size = new System.Drawing.Size(986, 178);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Caption = "Xóa log";
            this.btnClearLog.Id = 1;
            this.btnClearLog.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnClearLog.ImageOptions.LargeImage")));
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClearLog_ItemClick);
            // 
            // btnOpenFileLog
            // 
            this.btnOpenFileLog.Caption = "Mở tập tin log.txt";
            this.btnOpenFileLog.Id = 2;
            this.btnOpenFileLog.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnOpenFileLog.ImageOptions.LargeImage")));
            this.btnOpenFileLog.Name = "btnOpenFileLog";
            this.btnOpenFileLog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOpenFileLog_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1,
            this.ribbonPageGroup2});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Nhật ký - Log";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.btnClearLog);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.ShowCaptionButton = false;
            this.ribbonPageGroup1.Text = "Clear";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.btnOpenFileLog);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.ShowCaptionButton = false;
            this.ribbonPageGroup2.Text = "Open Log";
            // 
            // gridListLog
            // 
            this.gridListLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridListLog.Location = new System.Drawing.Point(0, 178);
            this.gridListLog.MainView = this.gridView_Log;
            this.gridListLog.MenuManager = this.ribbon;
            this.gridListLog.Name = "gridListLog";
            this.gridListLog.Size = new System.Drawing.Size(986, 381);
            this.gridListLog.TabIndex = 2;
            this.gridListLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_Log});
            // 
            // gridView_Log
            // 
            this.gridView_Log.GridControl = this.gridListLog;
            this.gridView_Log.Name = "gridView_Log";
            this.gridView_Log.OptionsBehavior.Editable = false;
            this.gridView_Log.OptionsBehavior.ReadOnly = true;
            this.gridView_Log.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView_Log.OptionsView.ShowGroupPanel = false;
            // 
            // timerLog
            // 
            this.timerLog.Enabled = true;
            this.timerLog.Interval = 5000;
            this.timerLog.Tick += new System.EventHandler(this.timerLog_Tick);
            // 
            // frmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 559);
            this.Controls.Add(this.gridListLog);
            this.Controls.Add(this.ribbon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLog";
            this.Ribbon = this.ribbon;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLog";
            this.Load += new System.EventHandler(this.frmLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridListLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_Log)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem btnClearLog;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraGrid.GridControl gridListLog;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_Log;
        private DevExpress.XtraBars.BarButtonItem btnOpenFileLog;
        private System.Windows.Forms.Timer timerLog;
    }
}