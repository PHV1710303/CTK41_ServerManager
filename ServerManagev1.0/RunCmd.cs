using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerManagev1._0
{
    class RunCmd
    {
        private const int bytes = 1048576; //1MB

        public static String LogOff(int sessionID)
        {
            return execCommand("logoff", sessionID.ToString());
        }

        public static bool createUser(String username, String password)
        {
            string result = null;

            if(execCommand("net", "user " + username + " " + password + " /add",ref result) == false)
            {
                Logging.WriteLog(result, "Tạo mới thất bại User: " + username);
                return false;
            }
            Logging.WriteLog(result, "Tạo mới User: " + username);

            if(execCommand("net", "localgroup " + @"""Remote Desktop Users""" + " " + username + " /add", ref result) == false)
            {
                Logging.WriteLog(result, "Thêm thất bại User " + username + " vào nhóm \"Remote Desktop Users\"");
                return false;
            }
            Logging.WriteLog(result, "Thêm User " + username + " vào nhóm \"Remote Desktop Users\"");

            return true;

        }

        public static bool deleteUser(String username)
        {
            string result = null;
            if(execCommand("net", "user " + username + " /delete",ref result) == false)
            {
                Logging.WriteLog(result, "Xóa bỏ thất bại User: " + username);
                return false;
            }
            Logging.WriteLog(result, "Xóa bỏ User: " + username);

            return true;
        }

        public static bool activeUser(String username)
        {
            string result = null;

            if (execCommand("cmd.exe", "/C net user " + username + " /active:yes", ref result) == false)
            {
                Logging.WriteLog(result, "Kích hoạt thất bại User: " + username);
                return false;
            }
            Logging.WriteLog(result, "Kích hoạt User: " + username);

            //Change password logon
            //net user username /logonpasswordchg:no
            if(execCommand("cmd.exe", "/C net user " + username + " /logonpasswordchg:no", ref result) == false)
            {
                return false;
            }

            // change password
            // Net user username / Passwordchg:No
            if(execCommand("cmd.exe", "/C net user " + username + " /Passwordchg:No", ref result) == false)
            {
                Logging.WriteLog(result, "Hủy quyền đổi mật khẩu thất bại User: " + username);
                return false;
            }
            Logging.WriteLog(result, "Không cho phép User [" + username + "] đổi mật khẩu");

            //wmic UserAccount where name = 'John Doe' set Passwordexpires = true
            //execCommand("wmic", "UserAccount where name = '" + username + "' set Passwordexpires = no");

            return true;
        }

        public static bool deactiveUser(String username)
        {
            string result = null;
            if(execCommand("cmd.exe", "/C net user " + username + " /active:no", ref result) == false)
            {
                Logging.WriteLog(result, "Hủy kích hoạt thất bại User: " + username);
                return false;
            }
            Logging.WriteLog(result, "Hủy kích hoạt User: " + username);

            //Change password after next login
            //net user username /logonpasswordchg:no
            if (execCommand("cmd.exe", "/C net user " + username + " /logonpasswordchg:yes", ref result) == false)
            {

            }

            // change password
            // Net user username / Passwordchg:No
            if (execCommand("cmd.exe", "/C net user " + username + " /Passwordchg:yes", ref result) == false)
            {
                Logging.WriteLog(result, "Cho phép thay đổi mật khẩu thất bại User: " + username);
                return false;
            }
            Logging.WriteLog(result, "Cho phép thay đổi mật khẩu User: " + username);

            return true;
        }

        public static bool createFolder(String username, String partition)
        {
            //cacls % dir %:\%% A / e / p %% A:f
            string result = null;

            // Lệnh tạo folder mới tương ứng với tên user
            if (execCommand("cmd.exe", "/c mkdir " + partition + @":\" + username, ref result) == false)
            {
                Logging.WriteLog(result, "Tạo thất bại folder tên [" + username + "] tại phân vùng " + partition);
                return false;
            }
            Logging.WriteLog(result, "Tạo folder mới tên [" + username + "] tại phân vùng " + partition);

            // Lệnh cấp full quyền folder cho user tương ứng
            if (execCommand("cmd.exe", "/c cacls " + partition + @":\" + username + " /e /p " + username + ":f", ref result) == false)
            {
                Logging.WriteLog(result, "Cấp quyền thất bại folder [" + partition + @":\" + username + "] cho User " + username);
                return false;
            }
            Logging.WriteLog(result, "Cấp quyền full quyền folder [" + partition + @":\" + username + "] cho User " + username);

            return true;
        }

        public static String createFolder(String username, String partition, String userGrant)
        {
            //mkdir %dir%:\%%A
            //cacls % dir %:\%% A / e / p %% A:f

            execCommand("cmd.exe", "/c mkdir " + partition + @":\" + username);
            //execCommand("mkdir D:\\100", "");

            execCommand("cmd.exe", "/c cacls " + partition + @":\" + username + " /e /p " + userGrant + ":f");
            //execCommand("cacls", @"D:\1 /e /p 1:f");

            return username + ": The command completed successfully.";
        }

        // chua chay
        public static bool removeFolder(String username, String partition)
        {
            //@RD /S /Q %dir%:\%%A
            //rmdir "%%p" /s /q
            string result = null;

            if(execCommand("cmd.exe", "/c rmdir " + partition + @":\" + username + " /s /q",ref result) == false)
            {
                Logging.WriteLog(result, "Xóa thất bại folder tên [" + username + "] tại phân vùng " + partition);
                return false;
            }
            Logging.WriteLog(result, "Xóa folder tên [" + username + "] tại phân vùng " + partition);
            
            return true;
        }

        public static String addPermissionFolder(String username, String partition)
        {
            //mkdir %dir%:\%%A
            //cacls % dir %:\%% A / e / p %% A:f

            execCommand("cacls", partition + @":\" + username + " /e /p " + username + ":f");

            execCommand("cacls", partition + @":\" + username + @"\*" + " /e /p " + username + ":f");

            return username + ": The command completed successfully.";
        }

        public static String removePermissionFolder(String username, String partition)
        {
            //ICACLS %dir%:\%%A\ /remove %%A /T /C
            //ICACLS % dir %:\%% A / remove %% A / T / C

            execCommand("ICACLS", partition + @":\" + username + @"\" + " /remove " + username + " /t /c");

            execCommand("ICACLS", partition + @":\" + username + " /remove " + username + " /t /c");

            //execCommand("cacls", partition + @":\" + username + @"\*" + " /e /p " + username + ":f");

            return username + ": The command completed successfully.";
        }

        public static String enableQuotaDriveC()
        {
            execCommand("fsutil", "quota enforce  C:");

            return "Enable Quota Drive C: The command completed successfully.";
        }

        public static String quotaPerUsers(String username, int quota)
        {
            int data = bytes * quota;
            execCommand("fsutil", "quota  modify C: " + data.ToString() + " " + data.ToString() + " " + username);

            return "Enable Quota Drive C: The command completed successfully.";
        }

        private static String execCommand(String filename, string arguments)
        {
            StreamReader outputReader = null;
            StreamReader errorReader = null;
            String result = "";

            try
            {
                //Create Process Start information
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.ErrorDialog = false;
                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                processStartInfo.FileName = filename;
                processStartInfo.Arguments = arguments;
                processStartInfo.Verb = "runas"; // run as Administrator.

                //Execute the process
                Process process = new Process();
                process.StartInfo = processStartInfo;
                bool processStarted = process.Start();
                if (processStarted)
                {
                    //Get the output stream
                    outputReader = process.StandardOutput;
                    errorReader = process.StandardError;
                    process.WaitForExit();

                    //read the result

                    result = outputReader.ReadToEnd();

                    //Display the result

                }
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                //MessageBox.Show(ex.Message);
                return result;
            }
            finally
            {
                if (outputReader != null)
                {
                    outputReader.Close();
                }
                if (errorReader != null)
                {
                    errorReader.Close();
                }
            }
        }

        private static bool execCommand(String filename, string arguments, ref string result)
        {
            StreamReader outputReader = null;
            StreamReader errorReader = null;
            result = "";
            string error = "", successful = "";

            try
            {
                //Create Process Start information
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.ErrorDialog = false;
                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                processStartInfo.FileName = filename;
                processStartInfo.Arguments = arguments;
                processStartInfo.Verb = "runas"; // run as Administrator.

                //Execute the process
                Process process = new Process();
                process.StartInfo = processStartInfo;
                bool processStarted = process.Start();
                if (processStarted)
                {
                    //Get the output stream
                    outputReader = process.StandardOutput;
                    errorReader = process.StandardError;
                    process.WaitForExit();

                    //Đọc log thành công
                    successful = outputReader.ReadToEnd();
                    // Đọc log báo lỗi (thất bại)
                    error = errorReader.ReadToEnd();
                    if(successful != "")
                    {
                        // Nếu thành công, successful sẽ khác rỗng
                        result = successful;
                        return true;
                    }
                    else if(error != "")
                    {
                        // Nếu thất bại, error sẽ khác rỗng
                        result = error;
                        return false;
                    }
                    // Nếu cả hai trường hợp đề không xảy ra, có thể là thành công. Do một vài trường hợp thành công, cmd không báo log
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                if (outputReader != null)
                {
                    outputReader.Close();
                }
                if (errorReader != null)
                {
                    errorReader.Close();
                }
            }
        }
    }
}
