using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.Runtime.InteropServices;
using ServerManagev1._0.Enums;

namespace ServerManagev1._0
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        List<Session> listSessions;
        List<Drive> listDrive;

        string partitionLabel = "";
        static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        public frmMain()
        {
            InitializeComponent();
        }

        public void LoadListSession()
        {
            listSessions = ListSessions.ListUsers(Environment.MachineName);
            gridListSession.DataSource = listSessions;
        }

        public void LoadListDrives()
        {
            cbb_Partition.Items.Clear();
            listDrive = Drive.listDrive();
            foreach (Drive drive in listDrive)
            {
                cbb_Partition.Items.Add(drive.Name);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadListSession();
            LoadListDrives();
        }

        private void btnDisconnectAllSession_ItemClick(object sender, ItemClickEventArgs e)
        {
            for (int i = 0; i < listSessions.Count; i++)
            {
                if (listSessions[i].connectedState == Enums.CONNECTSTATE_CLASS.Active)
                {
                    bool kq = ListSessions.DisconnectUserSession(WTS_CURRENT_SERVER_HANDLE, i, false);
                }
            }
            LoadListSession();
        }

        private void btnDisconnectSession_ItemClick(object sender, ItemClickEventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            int sessionID = listSessions[index].sessionID;

            bool kq = ListSessions.DisconnectUserSession(WTS_CURRENT_SERVER_HANDLE, sessionID, false);

            if (kq)
            {
                MessageBox.Show("Ngắt kết nối " + listSessions[index].userName + " thành công!!");
                LoadListSession();
            }
            else
            {
                MessageBox.Show("Lỗi!!!!");
            }
        }

        private void btnLogoffAllSession_ItemClick(object sender, ItemClickEventArgs e)
        {
            for (int i = 0; i < listSessions.Count; i++)
            {
                if (listSessions[i].connectedState == Enums.CONNECTSTATE_CLASS.Active)
                {
                    ListSessions.LogofftUserSession(WTS_CURRENT_SERVER_HANDLE, i, false);
                }
            }
            LoadListSession();
        }

        private void btnLogoffSession_ItemClick(object sender, ItemClickEventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            int sessionID = listSessions[index].sessionID;

            bool kq = ListSessions.LogofftUserSession(WTS_CURRENT_SERVER_HANDLE, sessionID, false);

            if (kq)
            {
                MessageBox.Show("Ngắt kết nối " + listSessions[index].userName + " thành công!!");
                LoadListSession();
            }
            else
            {
                MessageBox.Show("Lỗi!!!!");
            }
        }

        private void btnReload_ItemClick(object sender, ItemClickEventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            LoadListSession();
            gridView1.FocusedRowHandle = index;
        }

        private void timerSession_Tick(object sender, EventArgs e)
        {
            btnReload.PerformClick();
        }

        private void cmnu_Disconnect_Click(object sender, EventArgs e)
        {
            btnDisconnectSession.PerformClick();
        }

        private void cmnu_LogoffUser_Click(object sender, EventArgs e)
        {
            btnLogoffSession.PerformClick();
        }

        private void barEditItemPartition_EditValueChanged(object sender, EventArgs e)
        {
            partitionLabel = barEditItemPartition.EditValue.ToString();
        }

        private void btnCreateFolders_ItemClick(object sender, ItemClickEventArgs e)
        {
            int min, max;
            string name = "";

            if (!barEditItem_Min.EditValue.ToString().Equals("") && !barEditItem_Max.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItem_Min.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItem_Max.EditValue.ToString(), @"^\d+$"))
                {
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số (0 -> 65535)", "Thông báo");
                    return;
                }
                min = Int32.Parse(barEditItem_Min.EditValue.ToString());
                max = Int32.Parse(barEditItem_Max.EditValue.ToString());
            }
            else
            {
                min = 0;
                max = 0;
            }

            if (partitionLabel.Equals(null) || partitionLabel.Equals(""))
            {
                MessageBox.Show("Vui lòng chọn ổ đĩa muốn tạo Folder!!");
            }
            else
            {
                if (min == 0 && max == 0)
                {
                    try
                    {
                        name = barEditItemFolderName.EditValue.ToString();
                        RunCmd.createFolder(name, partitionLabel);
                    }
                    catch
                    {
                        MessageBox.Show("Vui lòng nhập tên Folder muốn tạo!!", "Thông báo");
                        return;
                    }
                }
                else
                {
                    for (int i = min; i <= max; i++)
                    {
                        try
                        {
                            name = barEditItemFolderName.EditValue.ToString();
                            RunCmd.createFolder(name + i, partitionLabel);
                        }
                        catch (Exception)
                        {
                            RunCmd.createFolder(i.ToString(), partitionLabel);
                        }
                    }
                }
                
                MessageBox.Show("Xong!!", "Thông báo");
            }
        }

        private void btnDeleteFolders_ItemClick(object sender, ItemClickEventArgs e)
        {
            int min, max;
            string name = "";

            if (!barEditItem_Min.EditValue.ToString().Equals("") && !barEditItem_Max.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItem_Min.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItem_Max.EditValue.ToString(), @"^\d+$"))
                {
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số (0 -> 65535)", "Thông báo");
                    return;
                }
                min = Int32.Parse(barEditItem_Min.EditValue.ToString());
                max = Int32.Parse(barEditItem_Max.EditValue.ToString());
            }
            else
            {
                min = 0;
                max = 0;
            }

            if (partitionLabel.Equals(null) || partitionLabel.Equals(""))
            {
                MessageBox.Show("Vui lòng chọn ổ đĩa muốn xóa Folder!!");
            }
            else
            {
                if (min == 0 && max == 0)
                {
                    try
                    {
                        name = barEditItemFolderName.EditValue.ToString();
                        RunCmd.removeFolder(name, partitionLabel);
                    }
                    catch
                    {
                        MessageBox.Show("Vui lòng nhập tên Folder muốn tạo!!", "Thông báo");
                        return;
                    }
                }
                else
                {
                    for (int i = min; i <= max; i++)
                    {
                        try
                        {
                            name = barEditItemFolderName.EditValue.ToString();
                            RunCmd.removeFolder(name + i, partitionLabel);
                        }
                        catch (Exception)
                        {
                            RunCmd.removeFolder(i.ToString(), partitionLabel);
                        }
                    }
                }
                
                MessageBox.Show("Xong!!", "Thông báo");
            }
        }

        private void btnActiveUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int index = gridView1.FocusedRowHandle;
            //string userName = gridView1.GetRowCellValue(index, "userName").ToString();

            //string resultStr = RunCmd.activeUser(userName);
            //MessageBox.Show(resultStr, "Thông báo");

            string name = "";
            string result = "";
            int min, max;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") && !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số (0 -> 65535)", "Thông báo");
                    return;
                }
                min = Int32.Parse(barEditItemUserMin.EditValue.ToString());
                max = Int32.Parse(barEditItemUserMax.EditValue.ToString());
            }
            else
            {
                min = 0;
                max = 0;
            }

            if (min == 0 && max == 0)
            {
                try
                {
                    name = barEditItemUsername.EditValue.ToString();
                    result = RunCmd.activeUser(name);
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập tên User muốn xóa!!", "Thông báo");
                    return;
                }
            }
            else
            {
                for (int i = min; i <= max; i++)
                {
                    try
                    {
                        name = barEditItemUsername.EditValue.ToString();
                        result += RunCmd.activeUser(name + i);
                    }
                    catch (Exception)
                    {
                        result += RunCmd.activeUser(i.ToString());
                    }
                }
            }

            MessageBox.Show(result);
        }

        private void btnDeactiveUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int index = gridView1.FocusedRowHandle;
            //string userName = gridView1.GetRowCellValue(index, "userName").ToString();

            //string resultStr = RunCmd.deactiveUser(userName);
            //MessageBox.Show(resultStr, "Thông báo");

            string name = "";
            string result = "";
            int min, max;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") && !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số (0 -> 65535)", "Thông báo");
                    return;
                }
                min = Int32.Parse(barEditItemUserMin.EditValue.ToString());
                max = Int32.Parse(barEditItemUserMax.EditValue.ToString());
            }
            else
            {
                min = 0;
                max = 0;
            }

            if (min == 0 && max == 0)
            {
                try
                {
                    name = barEditItemUsername.EditValue.ToString();
                    result = RunCmd.deactiveUser(name);
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập tên User muốn xóa!!", "Thông báo");
                    return;
                }
            }
            else
            {
                for (int i = min; i <= max; i++)
                {
                    try
                    {
                        name = barEditItemUsername.EditValue.ToString();
                        result += RunCmd.deactiveUser(name + i);
                    }
                    catch (Exception)
                    {
                        result += RunCmd.deactiveUser(i.ToString());
                    }
                }
            }

            MessageBox.Show(result);
        }

        private void btnAddUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            string name = "";
            string result = "";
            int min, max;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") && !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số (0 -> 65535)", "Thông báo");
                    return;
                }
                min = Int32.Parse(barEditItemUserMin.EditValue.ToString());
                max = Int32.Parse(barEditItemUserMax.EditValue.ToString());
            }
            else
            {
                min = 0;
                max = 0;
            }

            if (min == 0 && max == 0)
            {
                try
                {
                    name = barEditItemUsername.EditValue.ToString();
                    result = RunCmd.createUser(name, name);
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập tên User muốn tạo!!", "Thông báo");
                    return;
                }
            }
            else
            {
                for (int i = min; i <= max; i++)
                {
                    try
                    {
                        name = barEditItemUsername.EditValue.ToString();
                        result += RunCmd.createUser(name + i, name + i);
                    }
                    catch (Exception)
                    {
                        result += RunCmd.createUser(i.ToString(), i.ToString());
                    }
                }
            }

            MessageBox.Show(result);
        }

        private void btnDeleteUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            string name = "";
            string result = "";
            int min, max;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") && !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số (0 -> 65535)", "Thông báo");
                    return;
                }
                min = Int32.Parse(barEditItemUserMin.EditValue.ToString());
                max = Int32.Parse(barEditItemUserMax.EditValue.ToString());
            }
            else
            {
                min = 0;
                max = 0;
            }

            if (min == 0 && max == 0)
            {
                try
                {
                    name = barEditItemUsername.EditValue.ToString();
                    result = RunCmd.deleteUser(name);
                }
                catch
                {
                    MessageBox.Show("Vui lòng nhập tên User muốn xóa!!", "Thông báo");
                    return;
                }
            }
            else
            {
                for (int i = min; i <= max; i++)
                {
                    try
                    {
                        name = barEditItemUsername.EditValue.ToString();
                        result += RunCmd.deleteUser(name + i);
                    }
                    catch (Exception)
                    {
                        result += RunCmd.deleteUser(i.ToString());
                    }
                }
            }

            MessageBox.Show(result);
        }

        private void cmnu_DeleteUser_Click(object sender, EventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            string userName = gridView1.GetRowCellValue(index, "userName").ToString();

            string resultStr = RunCmd.deleteUser(userName);
            MessageBox.Show(resultStr, "Thông báo");
        }

        private void cmnu_ActiveUser_Click(object sender, EventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            string userName = gridView1.GetRowCellValue(index, "userName").ToString();

            string resultStr = RunCmd.activeUser(userName);
            MessageBox.Show(resultStr, "Thông báo");
        }

        private void cmnu_DeactiveUser_Click(object sender, EventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            string userName = gridView1.GetRowCellValue(index, "userName").ToString();

            string resultStr = RunCmd.deactiveUser(userName);
            MessageBox.Show(resultStr, "Thông báo");
        }
    }
}
