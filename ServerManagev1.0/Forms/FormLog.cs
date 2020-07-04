using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.Diagnostics;
using DevExpress.XtraGrid.Views.Grid;

namespace ServerManagev1._0.Forms
{
    public partial class frmLog : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public string FileName { get; set; }
        List<Log> listLog = new List<Log>();
        public frmLog()
        {
            InitializeComponent();
        }

        private void btnOpenFileLog_ItemClick(object sender, ItemClickEventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = FileName;
            process.StartInfo = processStartInfo;
            try
            {
                process.Start();
            }
            catch
            {
                MessageBox.Show("Log hiện chưa được ghi lại", "Thông báo");
            }
        }

        private void frmLog_Load(object sender, EventArgs e)
        {
            listLog = Logging.ReadLog(FileName);
            gridListLog.DataSource = listLog;
            gridView_Log.MoveLast();
        }

        private void btnClearLog_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Logging.ClearLog(FileName))
            {
                gridListLog.DataSource = Logging.ReadLog(FileName);
                MessageBox.Show("Xóa danh sách log thành công", "Thông báo");
            }
            else
            {
                MessageBox.Show("Không tồn tại file log hoặc quá trình xóa thất bại!", "Thông báo");
            }
        }

        private void timerLog_Tick(object sender, EventArgs e)
        {
            var newList = Logging.ReadLog(FileName);
            if(listLog.Count != newList.Count)
            {
                listLog = newList;
                gridListLog.DataSource = newList;
                gridView_Log.MoveLast();
            }
        }
    }
}