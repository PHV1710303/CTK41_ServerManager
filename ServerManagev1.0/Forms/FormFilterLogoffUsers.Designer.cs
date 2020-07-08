namespace ServerManagev1._0
{
    partial class FormFilterLogoffUsers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFilterLogoffUsers));
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.gridListFilterLogoffUsers = new DevExpress.XtraGrid.GridControl();
            this.gridView_FilterUsers = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ribbonPage3 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.fluentDesignFormContainer1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.toolbarFormManager1 = new DevExpress.XtraBars.ToolbarForm.ToolbarFormManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.lbl_Notification = new System.Windows.Forms.Label();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnInsert = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.gridListUser = new DevExpress.XtraGrid.GridControl();
            this.gridView_ListUsers = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnRemoveUser = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddUser = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textEdit_UserName = new DevExpress.XtraEditors.TextEdit();
            this.timerRefreshSessions = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridListFilterLogoffUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_FilterUsers)).BeginInit();
            this.fluentDesignFormContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridListUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_ListUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_UserName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "ribbonPage2";
            // 
            // gridListFilterLogoffUsers
            // 
            this.gridListFilterLogoffUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridListFilterLogoffUsers.Location = new System.Drawing.Point(12, 29);
            this.gridListFilterLogoffUsers.MainView = this.gridView_FilterUsers;
            this.gridListFilterLogoffUsers.Name = "gridListFilterLogoffUsers";
            this.gridListFilterLogoffUsers.Size = new System.Drawing.Size(252, 327);
            this.gridListFilterLogoffUsers.TabIndex = 0;
            this.gridListFilterLogoffUsers.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_FilterUsers});
            this.gridListFilterLogoffUsers.Click += new System.EventHandler(this.gridListFilterLogoffUsers_Click);
            // 
            // gridView_FilterUsers
            // 
            this.gridView_FilterUsers.DetailHeight = 328;
            this.gridView_FilterUsers.GridControl = this.gridListFilterLogoffUsers;
            this.gridView_FilterUsers.Name = "gridView_FilterUsers";
            this.gridView_FilterUsers.OptionsBehavior.Editable = false;
            this.gridView_FilterUsers.OptionsBehavior.ReadOnly = true;
            this.gridView_FilterUsers.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView_FilterUsers.OptionsView.ShowGroupPanel = false;
            // 
            // ribbonPage3
            // 
            this.ribbonPage3.Name = "ribbonPage3";
            this.ribbonPage3.Text = "ribbonPage3";
            // 
            // fluentDesignFormContainer1
            // 
            this.fluentDesignFormContainer1.Controls.Add(this.labelControl5);
            this.fluentDesignFormContainer1.Controls.Add(this.labelControl4);
            this.fluentDesignFormContainer1.Controls.Add(this.textEdit2);
            this.fluentDesignFormContainer1.Controls.Add(this.textEdit1);
            this.fluentDesignFormContainer1.Controls.Add(this.lbl_Notification);
            this.fluentDesignFormContainer1.Controls.Add(this.btnDelete);
            this.fluentDesignFormContainer1.Controls.Add(this.btnInsert);
            this.fluentDesignFormContainer1.Controls.Add(this.labelControl3);
            this.fluentDesignFormContainer1.Controls.Add(this.gridListUser);
            this.fluentDesignFormContainer1.Controls.Add(this.btnRemoveUser);
            this.fluentDesignFormContainer1.Controls.Add(this.btnAddUser);
            this.fluentDesignFormContainer1.Controls.Add(this.labelControl2);
            this.fluentDesignFormContainer1.Controls.Add(this.labelControl1);
            this.fluentDesignFormContainer1.Controls.Add(this.textEdit_UserName);
            this.fluentDesignFormContainer1.Controls.Add(this.gridListFilterLogoffUsers);
            this.fluentDesignFormContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fluentDesignFormContainer1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormContainer1.Name = "fluentDesignFormContainer1";
            this.fluentDesignFormContainer1.Size = new System.Drawing.Size(781, 545);
            this.fluentDesignFormContainer1.TabIndex = 4;
            // 
            // labelControl5
            // 
            this.labelControl5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(386, 455);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 22);
            this.labelControl5.TabIndex = 14;
            this.labelControl5.Text = "Max";
            // 
            // labelControl4
            // 
            this.labelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(386, 409);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(32, 22);
            this.labelControl4.TabIndex = 13;
            this.labelControl4.Text = "Min";
            // 
            // textEdit2
            // 
            this.textEdit2.Location = new System.Drawing.Point(430, 451);
            this.textEdit2.MenuManager = this.toolbarFormManager1;
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEdit2.Properties.Appearance.Options.UseFont = true;
            this.textEdit2.Size = new System.Drawing.Size(78, 28);
            this.textEdit2.TabIndex = 12;
            // 
            // toolbarFormManager1
            // 
            this.toolbarFormManager1.DockControls.Add(this.barDockControlTop);
            this.toolbarFormManager1.DockControls.Add(this.barDockControlBottom);
            this.toolbarFormManager1.DockControls.Add(this.barDockControlLeft);
            this.toolbarFormManager1.DockControls.Add(this.barDockControlRight);
            this.toolbarFormManager1.Form = this;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.toolbarFormManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(781, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 545);
            this.barDockControlBottom.Manager = this.toolbarFormManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(781, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.toolbarFormManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 545);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(781, 0);
            this.barDockControlRight.Manager = this.toolbarFormManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 545);
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(430, 405);
            this.textEdit1.MenuManager = this.toolbarFormManager1;
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEdit1.Properties.Appearance.Options.UseFont = true;
            this.textEdit1.Size = new System.Drawing.Size(78, 28);
            this.textEdit1.TabIndex = 11;
            // 
            // lbl_Notification
            // 
            this.lbl_Notification.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_Notification.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Notification.Location = new System.Drawing.Point(0, 529);
            this.lbl_Notification.Name = "lbl_Notification";
            this.lbl_Notification.Size = new System.Drawing.Size(781, 16);
            this.lbl_Notification.TabIndex = 10;
            this.lbl_Notification.Text = "label1";
            this.lbl_Notification.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnDelete
            // 
            this.btnDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.ImageOptions.Image")));
            this.btnDelete.Location = new System.Drawing.Point(270, 192);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(40, 29);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnInsert.ImageOptions.Image")));
            this.btnInsert.Location = new System.Drawing.Point(270, 152);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(40, 29);
            this.btnInsert.TabIndex = 8;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(424, 7);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(201, 17);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "Danh sách các phiên đang remote";
            // 
            // gridListUser
            // 
            this.gridListUser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridListUser.Location = new System.Drawing.Point(316, 29);
            this.gridListUser.MainView = this.gridView_ListUsers;
            this.gridListUser.MenuManager = this.toolbarFormManager1;
            this.gridListUser.Name = "gridListUser";
            this.gridListUser.Size = new System.Drawing.Size(453, 327);
            this.gridListUser.TabIndex = 6;
            this.gridListUser.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_ListUsers});
            this.gridListUser.Click += new System.EventHandler(this.gridListUser_Click);
            // 
            // gridView_ListUsers
            // 
            this.gridView_ListUsers.GridControl = this.gridListUser;
            this.gridView_ListUsers.Name = "gridView_ListUsers";
            this.gridView_ListUsers.OptionsBehavior.Editable = false;
            this.gridView_ListUsers.OptionsBehavior.ReadOnly = true;
            this.gridView_ListUsers.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView_ListUsers.OptionsView.ShowGroupPanel = false;
            // 
            // btnRemoveUser
            // 
            this.btnRemoveUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveUser.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveUser.Appearance.Options.UseFont = true;
            this.btnRemoveUser.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveUser.ImageOptions.Image")));
            this.btnRemoveUser.Location = new System.Drawing.Point(196, 442);
            this.btnRemoveUser.Name = "btnRemoveUser";
            this.btnRemoveUser.Size = new System.Drawing.Size(121, 47);
            this.btnRemoveUser.TabIndex = 5;
            this.btnRemoveUser.Text = "Xóa User";
            this.btnRemoveUser.Click += new System.EventHandler(this.btnRemoveUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddUser.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddUser.Appearance.Options.UseFont = true;
            this.btnAddUser.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddUser.ImageOptions.Image")));
            this.btnAddUser.Location = new System.Drawing.Point(26, 442);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(139, 47);
            this.btnAddUser.TabIndex = 4;
            this.btnAddUser.Text = "Thêm User";
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(12, 379);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(73, 22);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Tên User";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(252, 17);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Danh sách Users không bị tự động Logoff ";
            // 
            // textEdit_UserName
            // 
            this.textEdit_UserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textEdit_UserName.Location = new System.Drawing.Point(12, 406);
            this.textEdit_UserName.MenuManager = this.toolbarFormManager1;
            this.textEdit_UserName.Name = "textEdit_UserName";
            this.textEdit_UserName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textEdit_UserName.Properties.Appearance.Options.UseFont = true;
            this.textEdit_UserName.Size = new System.Drawing.Size(327, 28);
            this.textEdit_UserName.TabIndex = 1;
            // 
            // timerRefreshSessions
            // 
            this.timerRefreshSessions.Enabled = true;
            this.timerRefreshSessions.Interval = 5000;
            this.timerRefreshSessions.Tick += new System.EventHandler(this.timerRefreshSessions_Tick);
            // 
            // FormFilterLogoffUsers
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 545);
            this.Controls.Add(this.fluentDesignFormContainer1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Font = new System.Drawing.Font("Times New Roman", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(791, 580);
            this.MinimumSize = new System.Drawing.Size(791, 580);
            this.Name = "FormFilterLogoffUsers";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Danh sách lọc";
            this.Load += new System.EventHandler(this.FormFilterLogoffUsers_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridListFilterLogoffUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_FilterUsers)).EndInit();
            this.fluentDesignFormContainer1.ResumeLayout(false);
            this.fluentDesignFormContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridListUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_ListUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit_UserName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraGrid.GridControl gridListFilterLogoffUsers;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_FilterUsers;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage3;
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormContainer fluentDesignFormContainer1;
        private DevExpress.XtraBars.ToolbarForm.ToolbarFormManager toolbarFormManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.SimpleButton btnRemoveUser;
        private DevExpress.XtraEditors.SimpleButton btnAddUser;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textEdit_UserName;
        private DevExpress.XtraGrid.GridControl gridListUser;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_ListUsers;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnInsert;
        private System.Windows.Forms.Label lbl_Notification;
        private System.Windows.Forms.Timer timerRefreshSessions;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.TextEdit textEdit1;
    }
}