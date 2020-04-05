using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                processStartInfo.CreateNoWindow = false;

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

        private IEnumerable<DirectoryInfo> LayDanhSachThuMuc(IEnumerable<string> dsDuongDan)
        {
            return dsDuongDan
                .Select(duongDan => new DirectoryInfo(duongDan))
                .SelectMany(thuMuc => thuMuc.GetDirectories())
                .ToList();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            string str = Environment.ExpandEnvironmentVariables("%SystemRoot%") + @"\System32\cmd.exe";
            string cmd = textBox.Text.Trim().ToString();
            //execCommand("cmd.exe", "/K net user Zoro /active:yes");
            rickTextBox.Text = execCommand("cmd.exe", "/C " + cmd);
        }
    }
}
