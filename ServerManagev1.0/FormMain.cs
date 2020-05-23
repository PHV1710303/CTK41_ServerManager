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
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;

namespace ServerManagev1._0
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        int INTERVAL = 5000; // 5000 mili giây. Mỗi chu kỳ của timer là 5 giây.
        int MAXTIME = 300; // 300 giây. Sau số thời gian này, những User nào đang ở trạng thái disconnect sẽ bị log off. Tránh tốn tài nguyên của máy chủ Server.
        List<Session> listSessions;
        List<Drive> listDrive;
        ProcessDisconnectSessions PDCSS;
        ProcessMSSQL processMSSQL;

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
            timerSession.Interval = INTERVAL;
            LoadListSession();
            LoadListDrives();
            PDCSS = new ProcessDisconnectSessions(listSessions.ToArray(), MAXTIME);
            processMSSQL = new ProcessMSSQL();
            barEditItemTimeToLogOff.EditValue = MAXTIME;
            LoadComboBox_ServerRole();
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
                    PDCSS.RemoveDisconnectSessions(i);
                    ListSessions.LogofftUserSession(WTS_CURRENT_SERVER_HANDLE, i, false);
                }
            }
            LoadListSession();
        }

        private void btnLogoffSession_ItemClick(object sender, ItemClickEventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            int sessionID = listSessions[index].sessionID;

            PDCSS.RemoveDisconnectSessions(sessionID);
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
            PDCSS.UpdateTimeDisconnection(listSessions.ToArray(), INTERVAL / 1000);
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
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
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
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
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
            string name = "";
            string result = "";
            int min, max;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") && !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
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
            string name = "";
            string result = "";
            int min, max;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") && !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
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
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
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
                    MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
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

        private void barEditItemTimeToLogOff_EditValueChanged(object sender, EventArgs e)
        {
            if(!Regex.IsMatch(barEditItemTimeToLogOff.EditValue.ToString(), @"^\d+$"))
            {
                MessageBox.Show("Thời gian (giây) phải là số tự nhiên lớn hơn 0. Không được chứa chữ cái hay ký tự!!", "Thông báo");
                barEditItemTimeToLogOff.EditValue = MAXTIME;
            }
            else
            {
                MAXTIME = Int32.Parse(barEditItemTimeToLogOff.EditValue.ToString());
                PDCSS.MaxTime = MAXTIME;
            }
        }

        private void btnThemUserMSSQL_ItemClick(object sender, ItemClickEventArgs e)
        {
            int max, min, result;

            if (barEditItemSQLUsername.EditValue != null)
            {
                if (barEditItemSQLUserMin.EditValue.ToString().Trim() != "" && barEditItemSQLUserMax.EditValue.ToString().Trim() != "")
                {
                    if (!Regex.IsMatch(barEditItemSQLUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemSQLUserMax.EditValue.ToString(), @"^\d+$"))
                    {
                        MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
                        return;
                    }
                    min = Int32.Parse(barEditItemSQLUserMin.EditValue.ToString());
                    max = Int32.Parse(barEditItemSQLUserMax.EditValue.ToString());
                }
                else
                {
                    min = 0;
                    max = 0;
                }

                if (min == 0 && max == 0)
                {
                    string username = barEditItemSQLUsername.EditValue.ToString();
                    string password = barEditItemSQLUsername.EditValue.ToString();
                    result = processMSSQL.CreateNewLogin(username, password);

                    if (result == 0 || result == -1)
                    {
                        MessageBox.Show("Thất bại!", "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show("Thành công!", "Thông báo");
                    }
                }
                else
                {
                    for (int i = min; i <= max; i++)
                    {
                        string username = barEditItemSQLUsername.EditValue.ToString() + i.ToString();
                        string password = barEditItemSQLUsername.EditValue.ToString() + i.ToString();

                        result = processMSSQL.CreateNewLogin(username, password);

                        if (result == 0 || result == -1)
                        {
                            MessageBox.Show("Thất bại!", "Thông báo");
                            return;
                        }
                    }
                    MessageBox.Show("Thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập vào tên account SQL muốn tạo mới!", "Thông báo");
            }
        }

        private void btnXoaUserMSSQL_ItemClick(object sender, ItemClickEventArgs e)
        {
            int max, min, result;

            if (barEditItemSQLUsername.EditValue != null)
            {
                if (barEditItemSQLUserMin.EditValue.ToString().Trim() != "" && barEditItemSQLUserMax.EditValue.ToString().Trim() != "")
                {
                    if (!Regex.IsMatch(barEditItemSQLUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemSQLUserMax.EditValue.ToString(), @"^\d+$"))
                    {
                        MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
                        return;
                    }
                    min = Int32.Parse(barEditItemSQLUserMin.EditValue.ToString());
                    max = Int32.Parse(barEditItemSQLUserMax.EditValue.ToString());
                }
                else
                {
                    min = 0;
                    max = 0;
                }

                if (min == 0 && max == 0)
                {
                    string username = barEditItemSQLUsername.EditValue.ToString();
                    string password = barEditItemSQLUsername.EditValue.ToString();
                    result = processMSSQL.RemoveLogin(username, password);

                    if (result == 0 || result == -1)
                    {
                        MessageBox.Show("Thất bại!", "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show("Thành công!", "Thông báo");
                    }
                }
                else
                {
                    for (int i = min; i <= max; i++)
                    {
                        string username = barEditItemSQLUsername.EditValue.ToString() + i.ToString();
                        string password = barEditItemSQLUsername.EditValue.ToString() + i.ToString();

                        result = processMSSQL.RemoveLogin(username, password);

                        if (result == 0 || result == -1)
                        {
                            MessageBox.Show("Thất bại!", "Thông báo");
                            return;
                        }
                    }
                    MessageBox.Show("Thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập vào tên account SQL muốn xóa bỏ!", "Thông báo");
            }
        }

        private void LoadComboBox_ServerRole()
        {
            List<string> listServerRoles = processMSSQL.LoadListServerRoles();
            foreach(var serverRoleName in listServerRoles)
            {
                cbbEdit_ServerRole.Items.Add(serverRoleName);
            }
        }

        private void barCbb_ServerRole_EditValueChanged(object sender, EventArgs e)
        {
            BarEditItem item = sender as BarEditItem;
            var serverRoleName = item.EditValue.ToString();
            ribbonPageGroup_Permission.Text = serverRoleName;
        }

        private void btn_GrantPermission_ItemClick(object sender, ItemClickEventArgs e)
        {
            int max, min, result;
            string username = "", permissions = "";

            if(ribbonPageGroup_Permission.Text == null || ribbonPageGroup_Permission.Text.Trim().Equals(""))
            {
                MessageBox.Show("Vui lòng chọn quyền muốn cấp!", "Thông báo");
            }
            else
            {
                permissions = ribbonPageGroup_Permission.Text.Trim();
            }

            if (barEditItemSQLUsername.EditValue != null)
            {
                if (barEditItemSQLUserMin.EditValue.ToString().Trim() != "" && barEditItemSQLUserMax.EditValue.ToString().Trim() != "")
                {
                    if (!Regex.IsMatch(barEditItemSQLUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemSQLUserMax.EditValue.ToString(), @"^\d+$"))
                    {
                        MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
                        return;
                    }
                    min = Int32.Parse(barEditItemSQLUserMin.EditValue.ToString());
                    max = Int32.Parse(barEditItemSQLUserMax.EditValue.ToString());
                }
                else
                {
                    min = 0;
                    max = 0;
                }

                if (min == 0 && max == 0)
                {
                    username = barEditItemSQLUsername.EditValue.ToString();

                    result = processMSSQL.GrantPermissionAccount(username, permissions);

                    if (result == 0 || result == -1)
                    {
                        MessageBox.Show("Thất bại!", "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show("Thành công!", "Thông báo");
                    }
                }
                else
                {
                    for (int i = min; i <= max; i++)
                    {
                        username = barEditItemSQLUsername.EditValue.ToString() + i.ToString();

                        result = processMSSQL.GrantPermissionAccount(username, permissions);

                        if (result == 0 || result == -1)
                        {
                            MessageBox.Show("Thất bại!", "Thông báo");
                            return;
                        }
                    }
                    MessageBox.Show("Thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập vào tên account SQL muốn cấp quyền!", "Thông báo");
            }
        }

        private void btn_DenyPermission_ItemClick(object sender, ItemClickEventArgs e)
        {
            int max, min, result;
            string username = "", permissions = "";

            if (ribbonPageGroup_Permission.Text == null || ribbonPageGroup_Permission.Text.Trim().Equals(""))
            {
                MessageBox.Show("Vui lòng chọn quyền muốn cấp!", "Thông báo");
            }
            else
            {
                permissions = ribbonPageGroup_Permission.Text.Trim();
            }

            if (barEditItemSQLUsername.EditValue != null)
            {
                if (barEditItemSQLUserMin.EditValue.ToString().Trim() != "" && barEditItemSQLUserMax.EditValue.ToString().Trim() != "")
                {
                    if (!Regex.IsMatch(barEditItemSQLUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemSQLUserMax.EditValue.ToString(), @"^\d+$"))
                    {
                        MessageBox.Show("Bạn đã nhập sai định dạng min, max.\n\rChỉ được nhập số tự nhiên", "Thông báo");
                        return;
                    }
                    min = Int32.Parse(barEditItemSQLUserMin.EditValue.ToString());
                    max = Int32.Parse(barEditItemSQLUserMax.EditValue.ToString());
                }
                else
                {
                    min = 0;
                    max = 0;
                }

                if (min == 0 && max == 0)
                {
                    username = barEditItemSQLUsername.EditValue.ToString();

                    result = processMSSQL.DenyPermissionAccount(username, permissions);

                    if (result == 0 || result == -1)
                    {
                        MessageBox.Show("Thất bại!", "Thông báo");
                    }
                    else
                    {
                        MessageBox.Show("Thành công!", "Thông báo");
                    }
                }
                else
                {
                    for (int i = min; i <= max; i++)
                    {
                        username = barEditItemSQLUsername.EditValue.ToString() + i.ToString();

                        result = processMSSQL.DenyPermissionAccount(username, permissions);

                        if (result == 0 || result == -1)
                        {
                            MessageBox.Show("Thất bại!", "Thông báo");
                            return;
                        }
                    }
                    MessageBox.Show("Thành công!", "Thông báo");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập vào tên account SQL muốn cấp quyền!", "Thông báo");
            }
        }
    }
}
