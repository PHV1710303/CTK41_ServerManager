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

        public static bool LogOff(int sessionID)
        {
            return executeCommand("logoff", sessionID.ToString());
        }

        public static String createUser(String username, String password)
        {
            // NET USER %% A %% A / ADD
            //  net localgroup "Remote Desktop Users" %% A / add

            //executeCommand("NET USER " + username + " " + username + "// ADD", "");
            //The command completed successfully.

            //String para = "user "+ 2 + 2 + " /add";

            string result = "";

            result += execCommand("net", "user " + username + " " + password + " /add");

            result += execCommand("net", "localgroup " + @"""Remote Desktop Users""" + " " + username + " /add");

            return result;

        }

        public static String deleteUser(String username)
        {
            return execCommand("net", "user " + username + " /delete");
        }

        public static String activeUser(String username)
        {
            string result = "";

            result += execCommand("cmd.exe", "/C net user " + username + " /active:yes");

            //Change password logon
            //net user username /logonpasswordchg:no
            result += execCommand("cmd.exe", "/C net user " + username + " /logonpasswordchg:no");

            // change password
            // Net user username / Passwordchg:No
            result += execCommand("cmd.exe", "/C net user " + username + " /Passwordchg:No");

            //wmic UserAccount where name = 'John Doe' set Passwordexpires = true
            //execCommand("wmic", "UserAccount where name = '" + username + "' set Passwordexpires = no");

            return result;
        }

        public static String deactiveUser(String username)
        {
            execCommand("cmd.exe", "/C net user " + username + " /active:no");

            //Change password logon
            //net user username /logonpasswordchg:no
            execCommand("cmd.exe", "/C net user " + username + " /logonpasswordchg:yes");

            // change password
            // Net user username / Passwordchg:No
            execCommand("cmd.exe", "/C net user " + username + " /Passwordchg:yes");

            return username + ": The command completed successfully.";
        }

        public static String createFolder(String username, String partition)
        {
            //mkdir %dir%:\%%A
            //cacls % dir %:\%% A / e / p %% A:f

            execCommand("cmd.exe", "/c mkdir " + partition + @":\" + username);
            //execCommand("mkdir D:\\100", "");

            execCommand("cmd.exe", "/c cacls " + partition + @":\" + username + " /e /p " + username + ":f");
            //execCommand("cacls", @"D:\1 /e /p 1:f");

            return username + ": The command completed successfully.";
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
        public static String removeFolder(String username, String partition)
        {
            //@RD /S /Q %dir%:\%%A
            //rmdir "%%p" /s /q

            String kq = execCommand("cmd.exe", "/c rmdir " + partition + @":\" + username + " /s /q");

            return username + ": The command completed successfully.";
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

        private static bool executeCommand(String filename, string arguments)
        {
            StreamReader outputReader = null;
            StreamReader errorReader = null;

            try
            {
                //Create Process Start information
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

                    //MessageBox.Show(outputReader.ReadToEnd());

                    //Display the result

                }
                return true;
            }
            catch (Exception ex)
            {
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
