using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerManagev1._0
{
    public class Log
    {
        public string DateTime { get; set; }
        public string Description { get; set; }
        public string LogNotification { get; set; }
        public Log()
        {

        }

        public Log(string dateTime, string description, string logString)
        {
            this.DateTime = dateTime;
            this.Description = description;
            this.LogNotification = logString;
        }
    }
    public class Logging
    {
        private static string fileName = @"LogActions.txt";
        private static string fileName_Sessions = @"LogSessions.txt";
        
        public Logging()
        {

        }

        public static void WriteLog(string log, string description)
        {
            FileStream fs;
            DateTime dateTime;
            if (File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.Append);
            }
            else
            {
                fs = new FileStream(fileName, FileMode.Create);
            }
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            dateTime = DateTime.Now;
            string str = dateTime.ToString() +" || "+ description + " || "+ log.Split('\r')[0];
            sw.WriteLine(str);
            sw.Flush();
            fs.Close();
        }

        public static void WriteLog(string log)
        {
            FileStream fs;
            DateTime dateTime;
            if (File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.Append);
            }
            else
            {
                fs = new FileStream(fileName, FileMode.Create);
            }
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            dateTime = DateTime.Now;
            string str = dateTime.ToString() + " || " + log.Split('\r')[0];
            sw.WriteLine(str);
            sw.Flush();
            fs.Close();
        }

        public static void WriteLogSessions(string log, string description)
        {
            FileStream fs;
            DateTime dateTime;
            if (File.Exists(fileName_Sessions))
            {
                fs = new FileStream(fileName_Sessions, FileMode.Append);
            }
            else
            {
                fs = new FileStream(fileName_Sessions, FileMode.Create);
            }
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            dateTime = DateTime.Now;
            string str = dateTime.ToString() + " || " + description.PadRight(17) + " || " + log.Split('\r')[0];
            sw.WriteLine(str);
            sw.Flush();
            fs.Close();
        }

        public static void WriteLogSessions(string log)
        {
            FileStream fs;
            DateTime dateTime;
            if (File.Exists(fileName_Sessions))
            {
                fs = new FileStream(fileName_Sessions, FileMode.Append);
            }
            else
            {
                fs = new FileStream(fileName_Sessions, FileMode.Create);
            }
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            dateTime = DateTime.Now;
            string str = dateTime.ToString() + " || " + log.Split('\r')[0];
            sw.WriteLine(str);
            sw.Flush();
            fs.Close();
        }

        public static bool ClearLog(string fileName)
        {
            FileStream fs;
            if (File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.Create);
                fs.Close();
            }
            else
            {
                return false;
            }

            return true;
        }

        public static List<Log> ReadLog(string fileName)
        {
            List<Log> result = new List<Log>();
            FileStream fs;
            if (File.Exists(fileName_Sessions))
            {
                fs = new FileStream(fileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    string[] listStr = str.Split(new string[] { "||" },StringSplitOptions.None);
                    Log item = new Log(listStr[0].Trim(), listStr[1].Trim(), listStr[2].Trim());
                    result.Add(item);
                }
                fs.Close();
            }
            else
            {
                return null;
            }
            return result;
        }
    }
}