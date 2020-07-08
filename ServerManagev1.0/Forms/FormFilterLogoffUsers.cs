using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerManagev1._0
{
    public partial class FormFilterLogoffUsers : DevExpress.XtraEditors.XtraForm
    {
        List<Session> listSessions;
        ProcessDisconnectSessions PDCSS;
        string userName;

        public FormFilterLogoffUsers()
        {
            InitializeComponent();
        }

        public void Load_ListFilter()
        {
            gridListFilterLogoffUsers.DataSource = null;
            gridListFilterLogoffUsers.DataSource = PDCSS.Load_FilterLogoffUsers();
        }

        public void LoadListSession()
        {
            listSessions = ListSessions.ListUsers(Environment.MachineName);
            gridListUser.DataSource = listSessions;
        }

        private void FormFilterLogoffUsers_Load(object sender, EventArgs e)
        {
            PDCSS = new ProcessDisconnectSessions();
            lbl_Notification.Text = "";
            Load_ListFilter();
            LoadListSession();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            string uName = textEdit_UserName.Text.Trim();
            List<FilterLogoffUser> listFilter = PDCSS.Load_FilterLogoffUsers();
            bool isInsert = true;
            if (uName.Length != 0)
            {
                foreach (var obj in listFilter)
                {
                    if (obj.UserName.Equals(uName))
                    {
                        isInsert = false;
                        break;
                    }
                }
                if (isInsert == true)
                {
                    PDCSS.Add_FilterLogoffUser(uName);
                    Load_ListFilter();
                    textEdit_UserName.Text = "";
                    lbl_Notification.ForeColor = Color.DarkGreen;
                    lbl_Notification.Text = "Thêm thành công User vào danh sách lọc!";
                    Logging.WriteLog("", "Thêm User [" + uName + "] vào danh sách lọc");
                }
                else
                {
                    lbl_Notification.ForeColor = Color.DarkRed;
                    lbl_Notification.Text = "Đã tồn tại User này trong danh sách lọc!";
                }
            }
            else
            {
                lbl_Notification.ForeColor = Color.DarkRed;
                lbl_Notification.Text = "Vui lòng gõ tên User";
            }
        }

        private void btnRemoveUser_Click(object sender, EventArgs e)
        {
            string uName = textEdit_UserName.Text.Trim();
            if (uName.Length != 0)
            {
                PDCSS.Remove_FilterLogoffUser(uName);
                Load_ListFilter();
                textEdit_UserName.Text = "";
                lbl_Notification.ForeColor = Color.DarkGreen;
                lbl_Notification.Text = "Xóa thành công User khỏi danh sách lọc!";
                Logging.WriteLog("", "Loại bỏ User [" + uName + "] khỏi danh sách lọc");
            }
            else
            {
                lbl_Notification.ForeColor = Color.DarkRed;
                lbl_Notification.Text = "Vui lòng chọn User muốn xóa";
            }    
        }

        private void gridListFilterLogoffUsers_Click(object sender, EventArgs e)
        {
            int index = gridView_FilterUsers.FocusedRowHandle;
            textEdit_UserName.Text = gridView_FilterUsers.GetRowCellValue(index, "UserName").ToString();
        }

        private void gridListUser_Click(object sender, EventArgs e)
        {
            int index = gridView_ListUsers.FocusedRowHandle;
            userName = gridView_ListUsers.GetRowCellValue(index, "userName").ToString();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            List<FilterLogoffUser> listFilter = PDCSS.Load_FilterLogoffUsers();
            bool isInsert = true;
            if(userName != "" && userName != null)
            {
                foreach (var obj in listFilter)
                {
                    if (obj.UserName.Equals(userName))
                    {
                        isInsert = false;
                        break;
                    }
                }
                if (isInsert == true)
                {
                    PDCSS.Add_FilterLogoffUser(userName);
                    Load_ListFilter();
                    lbl_Notification.ForeColor = Color.DarkGreen;
                    lbl_Notification.Text = "Thêm thành công User vào danh sách lọc";
                    Logging.WriteLog("", "Thêm User [" + userName + "] vào danh sách lọc");
                }
                else
                {
                    lbl_Notification.ForeColor = Color.DarkRed;
                    lbl_Notification.Text = "Đã tồn tại User này trong danh sách lọc!";
                }
            }
            else
            {
                lbl_Notification.ForeColor = Color.DarkRed;
                lbl_Notification.Text = "Vui lòng chọn User muốn thêm!";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string uName = textEdit_UserName.Text.Trim();
            if (uName.Length != 0)
            {
                bool isRemove = PDCSS.Remove_FilterLogoffUser(uName);
                if (isRemove == true)
                {
                    Load_ListFilter();
                    lbl_Notification.ForeColor = Color.DarkGreen;
                    lbl_Notification.Text = "Loại bỏ thành công User khỏi danh sách lọc";
                    Logging.WriteLog("", "Loại bỏ User [" + uName + "] khỏi danh sách lọc");
                }
                else
                {
                    lbl_Notification.ForeColor = Color.DarkRed;
                    lbl_Notification.Text = "Loại bỏ thất bại. Không có User này!";
                }
                textEdit_UserName.Text = "";
            }
            else
            {
                lbl_Notification.ForeColor = Color.DarkRed;
                lbl_Notification.Text = "Vui lòng chọn User muốn loại bỏ!";
            }
        }

        private void timerRefreshSessions_Tick(object sender, EventArgs e)
        {
            int index1 = gridView_ListUsers.FocusedRowHandle;
            int index2 = gridView_FilterUsers.FocusedRowHandle;
            LoadListSession();
            gridView_ListUsers.FocusedRowHandle = index1;
            gridView_FilterUsers.FocusedRowHandle = index2;
        }
    }
}