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
using ServerManagev1._0.Forms;

namespace ServerManagev1._0
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        int INTERVAL = 5000; // 5000 mili giây. Mỗi chu kỳ của timer là 5 giây.
        int MAXTIME = 900; // 300 giây. Sau số thời gian này, những User nào đang ở trạng thái disconnect sẽ bị log off. Tránh tốn tài nguyên của máy chủ Server.
        int flagToNotification = 0;
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

        int CaclTimeToLogoff(ref int minutes, ref int seconds)
        {
            if(minutes <= 0 && seconds <=0)
            {
                return 0;
            }
            int _Minutes, _Seconds;
            _Minutes = seconds / 60;
            _Minutes += minutes;
            _Seconds = seconds % 60;
            minutes = _Minutes;
            seconds = _Seconds;
            return _Minutes * 60 + _Seconds;
        }

        void LoadTimeToLogoff(ref int minutes, ref int seconds)
        {
            MAXTIME = CaclTimeToLogoff(ref minutes, ref seconds);
            barEditItemTimeToLogOff_Minutes.EditValue = minutes;
            barEditItemTimeToLogOff.EditValue = seconds;
            PDCSS.MaxTime = MAXTIME;
        }

        void LoadTimeToLogoff()
        {
            int minutes = 0, seconds = MAXTIME;
            CaclTimeToLogoff(ref minutes, ref seconds);
            barEditItemTimeToLogOff_Minutes.EditValue = minutes;
            barEditItemTimeToLogOff.EditValue = seconds;
            PDCSS.MaxTime = MAXTIME;
        }

        void ShowNotification(string notification, bool isError)
        {
            if (isError)
            {
                lbl_Notification.ForeColor = Color.DarkRed;
            }
            else
            {
                lbl_Notification.ForeColor = Color.DarkGreen;
            }
            lbl_Notification.Visible = true;
            lbl_Notification.Text = notification;
            flagToNotification = 3;
        }

        private void btnReload_ItemClick(object sender, ItemClickEventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            LoadListSession(true);
            gridView1.FocusedRowHandle = index;
        }

        public void LoadListSession(bool isCheck)
        {
            var listOldSessions = listSessions;
            listSessions = ListSessions.ListUsers(Environment.MachineName);
            gridListSession.DataSource = listSessions;
            if(isCheck)
            {
                ListSessions.CheckSessionsChange(listOldSessions, listSessions);
            }
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
            LoadListSession(false);
            LoadListDrives();
            PDCSS = new ProcessDisconnectSessions(listSessions.ToArray(), MAXTIME);
            processMSSQL = new ProcessMSSQL();
            LoadTimeToLogoff();
            LoadComboBox_ServerRole();
            lbl_Notification.Text = "";
        }

        private void btnDisconnectAllSession_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<FilterLogoffUser> listFilter = PDCSS.Load_FilterLogoffUsers();
            bool isDisconnect, result;
            int numError = 0;
            timerSession.Enabled = false;
            for (int i = 0; i < listSessions.Count; i++)
            {
                if (listSessions[i].connectedState == CONNECTSTATE_CLASS.Active)
                {
                    isDisconnect = true;
                    foreach (var obj in listFilter)
                    {
                        if (obj.UserName.Equals(listSessions[i].userName))
                        {
                            isDisconnect = false;
                            break;
                        }
                    }
                    if (isDisconnect == true)
                    {
                        result = ListSessions.DisconnectUserSession(WTS_CURRENT_SERVER_HANDLE, listSessions[i].sessionID, false);
                        if (result)
                        {
                            Logging.WriteLogSessions("User " + listSessions[i].userName + " đã bị ngắt kết nối bởi Admin", "Manual Disconnect");
                        }
                        else
                        {
                            Logging.WriteLogSessions("User " + listSessions[i].userName + " đã bị ngắt kết nối thất bại bởi Admin", "Manual Disconnect");
                            numError++;
                        }
                    }
                }
            }
            if(numError == 0)
            {
                ShowNotification("Xong", false);
            }
            else
            {
                ShowNotification("Thất bại", true);
            }
            LoadListSession(false);
            timerSession.Enabled = true;
        }

        private void btnDisconnectSession_ItemClick(object sender, ItemClickEventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            int sessionID = listSessions[index].sessionID;
            string userName = listSessions[index].userName;

            bool result = ListSessions.DisconnectUserSession(WTS_CURRENT_SERVER_HANDLE, sessionID, false);

            if (result)
            {
                Logging.WriteLogSessions("User " + userName + " đã bị ngắt kết nối bởi Admin", "Manual Disconnect");
                ShowNotification("Thành công", false);
                LoadListSession(false);
            }
            else
            {
                Logging.WriteLogSessions("User " + userName + " đã bị ngắt kết nối thất bại bởi Admin", "Manual Disconnect");
                ShowNotification("Thất bại", true);
            }
        }

        private void btnLogoffAllSession_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<FilterLogoffUser> listFilter = PDCSS.Load_FilterLogoffUsers();
            bool isLogoff, result;
            int numError = 0;
            timerSession.Enabled = false;
            for (int i = 0; i < listSessions.Count; i++)
            {
                isLogoff = true;
                foreach (var obj in listFilter)
                {
                    if(obj.UserName.Equals(listSessions[i].userName))
                    {
                        isLogoff = false;
                        break;
                    }
                }
                if (isLogoff == true)
                {
                    PDCSS.RemoveDisconnectSessions(i);
                    result = ListSessions.LogofftUserSession(WTS_CURRENT_SERVER_HANDLE, listSessions[i].sessionID, false);
                    if (result)
                    {
                        Logging.WriteLogSessions("User " + listSessions[i].userName + " đã bị tắt bởi Admin", "Manual Disconnect");
                    }
                    else
                    {
                        Logging.WriteLogSessions("User " + listSessions[i].userName + " đã bị tắt thất bại bởi Admin", "Manual Disconnect");
                        numError++;
                    }
                }
            }
            if (numError == 0)
            {
                ShowNotification("Xong", false);
            }
            else
            {
                ShowNotification("Thất bại " + numError + " Users", false);
            }
            LoadListSession(false);
            timerSession.Enabled = true;
        }

        private void btnLogoffSession_ItemClick(object sender, ItemClickEventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            int sessionID = listSessions[index].sessionID;
            string userName = listSessions[index].userName;

            PDCSS.RemoveDisconnectSessions(sessionID);
            bool result = ListSessions.LogofftUserSession(WTS_CURRENT_SERVER_HANDLE, sessionID, false);

            if (result)
            {
                Logging.WriteLogSessions("User " + userName + " đã bị tắt bởi Admin", "Manual Logoff");
                ShowNotification("Thành công", false);
                LoadListSession(false);
            }
            else
            {
                Logging.WriteLogSessions("User " + userName + " đã bị tắt thất bại bởi Admin", "Manual Logoff");
                ShowNotification("Thất bại", true);
            }
        }

        private void timerSession_Tick(object sender, EventArgs e)
        {
            btnReload.PerformClick();
            PDCSS.UpdateTimeDisconnection(listSessions.ToArray(), INTERVAL / 1000);
            if (flagToNotification > 1)
            {
                flagToNotification--;
            }
            if(flagToNotification == 1)
            {
                lbl_Notification.Text = "";
                lbl_Notification.Visible = false;
                flagToNotification = 0;
            }
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
            int min, max, numError = 0;
            string name = "";

            if (!barEditItem_Min.EditValue.ToString().Equals("") || !barEditItem_Max.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItem_Min.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItem_Max.EditValue.ToString(), @"^\d+$"))
                {
                    ShowNotification("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", true);
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
                ShowNotification("Vui lòng chọn phân vùng muốn tạo folder mới", true);
                return;
            }
            else
            {
                if (min == 0 && max == 0)
                {
                    try
                    {
                        name = barEditItemFolderName.EditValue.ToString();
                        if(RunCmd.createFolder(name, partitionLabel))
                        {
                            ShowNotification("Thành công", false);
                        }
                        else
                        {
                            ShowNotification("Thất bại", true);
                        }
                    }
                    catch
                    {
                        ShowNotification("Vui lòng nhập tên folder muốn tạo mới", true);
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
                            if(RunCmd.createFolder(name + i, partitionLabel) == false)
                            {
                                numError++;
                            }
                        }
                        catch (Exception)
                        {
                            ShowNotification("Vui lòng nhập tên folder muốn tạo mới", true);
                            break;
                        }
                        if (i >= max)
                        {
                            if (numError != 0)
                            {
                                ShowNotification("Thất bại " + numError + " folders", true);
                            }
                            else
                            {
                                ShowNotification("Thành công", false);
                            }
                        }
                    }
                }
            }
        }

        private void btnDeleteFolders_ItemClick(object sender, ItemClickEventArgs e)
        {
            int min, max, numError = 0;
            string name = "";

            if (!barEditItem_Min.EditValue.ToString().Equals("") || !barEditItem_Max.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItem_Min.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItem_Max.EditValue.ToString(), @"^\d+$"))
                {
                    ShowNotification("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", true);
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
                ShowNotification("Vui lòng chọn phân vùng muốn xóa folder", true);
                return;
            }
            else
            {
                if (min == 0 && max == 0)
                {
                    try
                    {
                        name = barEditItemFolderName.EditValue.ToString();
                        if(RunCmd.removeFolder(name, partitionLabel))
                        {
                            ShowNotification("Thành công", false);
                        }
                        else
                        {
                            ShowNotification("Thất bại", true);
                        }
                    }
                    catch
                    {
                        ShowNotification("Vui lòng nhập tên folder muốn xóa bỏ", true);
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
                            if (RunCmd.removeFolder(name + i, partitionLabel) == false)
                            {
                                numError++;
                            }
                        }
                        catch (Exception)
                        {
                            ShowNotification("Vui lòng nhập tên folder muốn tạo mới", true);
                            break;
                        }
                        if (i >= max)
                        {
                            if (numError != 0)
                            {
                                ShowNotification("Thất bại " + numError + " folders", true);
                            }
                            else
                            {
                                lbl_Notification.ForeColor = Color.DarkGreen;
                                lbl_Notification.Text = "Thành công";
                                ShowNotification("Thành công", false);
                            }
                        }
                    }
                }
            }
        }

        private void btnActiveUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            string name = "";
            int min, max, numError = 0;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") || !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    ShowNotification("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", true);
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
                    if(RunCmd.activeUser(name))
                    {
                        ShowNotification("Thành công", false);
                    }
                    else
                    {
                        ShowNotification("Thất bại", true);
                    }
                }
                catch
                {
                    ShowNotification("Vui lòng nhập tên User muốn kích hoạt", true);
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
                        if(RunCmd.activeUser(name + i) == false)
                        {
                            numError++;
                        }
                    }
                    catch (Exception)
                    {
                        ShowNotification("Vui lòng nhập tên User muốn kích hoạt", true);
                        break;
                    }
                    if (i >= max)
                    {
                        if (numError != 0)
                        {
                            ShowNotification("Thất bại " + numError + " User", true);
                        }
                        else
                        {
                            ShowNotification("Thành công", false);
                        }
                    }
                }
            }
        }

        private void btnDeactiveUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            string name = "";
            int min, max, numError = 0;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") || !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    ShowNotification("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", true);
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
                    if (RunCmd.deactiveUser(name) == true)
                    {
                        ShowNotification("Thành công", false);
                    }
                    else
                    {
                        ShowNotification("Thất bại", true);
                    }
                }
                catch
                {
                    ShowNotification("Vui lòng nhập tên User muốn hủy kích hoạt", true);
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
                        if(RunCmd.deactiveUser(name + i) == false)
                        {
                            numError++;
                        }
                    }
                    catch (Exception)
                    {
                        ShowNotification("Vui lòng nhập tên User muốn hủy kích hoạt", true);
                        break;
                    }
                    if (i >= max)
                    {
                        if (numError != 0)
                        {
                            ShowNotification("Thất bại " + numError + " User", true);
                        }
                        else
                        {
                            ShowNotification("Thành công", false);
                        }
                    }
                }
            }
        }

        private void btnAddUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            string name = "";
            int min, max, numError = 0;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") || !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    ShowNotification("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", true);
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
                    if(RunCmd.createUser(name, name))
                    {
                        ShowNotification("Thành công", false);
                    }
                    else
                    {
                        ShowNotification("Thất bại", true);
                    }
                }
                catch
                {
                    ShowNotification("Vui lòng nhập tên User muốn thêm", true);
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
                        if (RunCmd.createUser(name + i, name + i) == false)
                        {
                            numError++;
                        }
                    }
                    catch (Exception)
                    {
                        ShowNotification("Vui lòng nhập tên User muốn thêm", true);
                        break;
                    }
                    if (i >= max)
                    {
                        if (numError != 0)
                        {
                            ShowNotification("Thất bại " + numError + " User", true);
                        }
                        else
                        {
                            ShowNotification("Thành công", false);
                        }
                    }
                }
            }
        }

        private void btnDeleteUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            string name = "";
            int min, max, numError = 0;
            if (!barEditItemUserMin.EditValue.ToString().Equals("") || !barEditItemUserMax.EditValue.ToString().Equals(""))
            {
                if (!Regex.IsMatch(barEditItemUserMin.EditValue.ToString(), @"^\d+$") || !Regex.IsMatch(barEditItemUserMax.EditValue.ToString(), @"^\d+$"))
                {
                    ShowNotification("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", true);
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
                    if(RunCmd.deleteUser(name))
                    {
                        ShowNotification("Thành công", false);
                    }
                    else
                    {
                        ShowNotification("Thất bại", true);
                    }
                }
                catch
                {
                    ShowNotification("Vui lòng nhập tên User muốn xóa", true);
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
                        if(RunCmd.deleteUser(name + i) == false)
                        {
                            numError++;
                        }
                    }
                    catch (Exception)
                    {
                        ShowNotification("Vui lòng nhập tên User muốn xóa", true);
                        break;
                    }
                    if (i >= max)
                    {
                        if (numError != 0)
                        {
                            ShowNotification("Thất bại " + numError + " User", true);
                        }
                        else
                        {
                            ShowNotification("Thành công", false);
                        }
                    }
                }
            }
        }

        private void cmnu_DeleteUser_Click(object sender, EventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            string userName = gridView1.GetRowCellValue(index, "userName").ToString();

            if(RunCmd.deleteUser(userName))
            {
                ShowNotification("Thành công", false);
            }
            else
            {
                ShowNotification("Thất bại", true);
            }
        }

        private void cmnu_ActiveUser_Click(object sender, EventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            string userName = gridView1.GetRowCellValue(index, "userName").ToString();

            if(RunCmd.activeUser(userName))
            {
                ShowNotification("Thành công", false);
            }
            else
            {
                ShowNotification("Thất bại", true);
            }
        }

        private void cmnu_DeactiveUser_Click(object sender, EventArgs e)
        {
            int index = gridView1.FocusedRowHandle;
            string userName = gridView1.GetRowCellValue(index, "userName").ToString();

            if(RunCmd.deactiveUser(userName))
            {
                ShowNotification("Thành công", false);
            }
            else
            {
                ShowNotification("Thất bại", true);
            }
        }

        private void barEditItemTimeToLogOff_EditValueChanged(object sender, EventArgs e)
        {
            if(!Regex.IsMatch(barEditItemTimeToLogOff.EditValue.ToString(), @"^\d+$"))
            {
                ShowNotification("Thời gian (giây) phải là số tự nhiên lớn hơn 0. Không được chứa chữ cái hay ký tự!!", true);
                LoadTimeToLogoff();
            }
        }

        private void barEditItemTimeToLogOff_Minutes_EditValueChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(barEditItemTimeToLogOff_Minutes.EditValue.ToString(), @"^\d+$"))
            {
                ShowNotification("Thời gian (giây) phải là số tự nhiên lớn hơn 0. Không được chứa chữ cái hay ký tự!!", true);
                LoadTimeToLogoff();
            }
        }

        private void btnUpdateTimeToLogoff_ItemClick(object sender, ItemClickEventArgs e)
        {
            int seconds = Int32.Parse(barEditItemTimeToLogOff.EditValue.ToString());
            int minutes = Int32.Parse(barEditItemTimeToLogOff_Minutes.EditValue.ToString());
            LoadTimeToLogoff(ref minutes, ref seconds);
            Logging.WriteLog("", "Thay đổi thời gian tự động logoff thành " + minutes + " phút " + seconds + " giây");
            ShowNotification("Đã thay đổi thời gian tự động logoff", false);
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
                        MessageBox.Show("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", "Thông báo");
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
                        MessageBox.Show("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", "Thông báo");
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
                        MessageBox.Show("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", "Thông báo");
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
                        MessageBox.Show("Bạn đã nhập sai định dạng min, max. Chỉ được nhập số tự nhiên", "Thông báo");
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

        private void btnFilterUsersLogoff_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form frm = new FormFilterLogoffUsers();
            frm.ShowDialog();
        }

        private void btnViewLogSessions_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLog form = new frmLog();
            form.FileName = "LogSessions.txt";
            form.Text = "Log - Nhật ký hoạt động của các phiên";
            form.Show();
        }

        private void btnViewLogAction_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmLog form = new frmLog();
            form.FileName = "LogActions.txt";
            form.Text = "Log - Nhật ký hoạt động của Admin";
            form.Show();
        }
    }
}