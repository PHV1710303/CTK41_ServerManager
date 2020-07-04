using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DevExpress.Emf;

namespace ServerManagev1._0
{
    class FilterLogoffUser
    {
        public string UserName { get; set; }

        public FilterLogoffUser()
        {
            UserName = "";
        }

        public FilterLogoffUser(string userName)
        {
            UserName = userName;
        }
    }

    class ProcessDisconnectSessions
    {
        static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        public List<DisconnectSession> ListDisconnectedSessions { get; set; }
        private string fileName = "filter_logoff_users.txt";
        public List<FilterLogoffUser> List_FilterUsers { get; set; }

        public int MaxTime { get; set; }

        public ProcessDisconnectSessions()
        {
            ListDisconnectedSessions = new List<DisconnectSession>();
            List_FilterUsers = Load_FilterLogoffUsers();
        }

        public ProcessDisconnectSessions(Session[] listSessions, int maxTime)
        {
            MaxTime = maxTime;
            ListDisconnectedSessions = new List<DisconnectSession>();
            List_FilterUsers = Load_FilterLogoffUsers();
            // dcss là Disconnected Sessions
            for (int i = 0; i < listSessions.Length; i++)
            {
                if (listSessions[i].connectedState == Enums.CONNECTSTATE_CLASS.Disconnected)
                {
                    ListDisconnectedSessions.Add(new DisconnectSession(listSessions[i].sessionID, listSessions[i].userName, 0));
                }
            }
        }

        // Danh sách các User được lọc, không bị tự động Logoff

        public void Add_FilterLogoffUser(string userName)
        {
            FileStream fs;
            if (File.Exists(fileName))
            {
                fs = new FileStream(fileName, FileMode.Append);
            }
            else
            {
                fs = new FileStream(fileName, FileMode.Create);
            }
            FilterLogoffUser filterLogoffUser = new FilterLogoffUser(userName);
            List_FilterUsers.Add(filterLogoffUser);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.WriteLine(userName);
            sw.Flush();
            fs.Close();
        }

        public bool Remove_FilterLogoffUser(string userName)
        {
            FileStream fs;
            StreamWriter sw;
            bool isRemove = false;
            if (File.Exists(fileName))
            {
                foreach (var obj in List_FilterUsers)
                {
                    if (obj.UserName.Equals(userName))
                    {
                        isRemove = List_FilterUsers.Remove(obj);
                        break;
                    }
                }
                if (isRemove)
                {
                    fs = new FileStream(fileName, FileMode.Create);
                    sw = new StreamWriter(fs, Encoding.UTF8);
                    foreach (var obj in List_FilterUsers)
                    {
                        sw.WriteLine(obj.UserName);
                    }
                    sw.Flush();
                    fs.Close();
                    return true;
                }
            }
            return false;
        }

        public List<FilterLogoffUser> Load_FilterLogoffUsers()
        {
            FileStream fs;
            if(File.Exists(fileName))
            {
                List_FilterUsers = new List<FilterLogoffUser>();
                fs = new FileStream(fileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                string str;
                while((str = sr.ReadLine()) != null)
                {
                    List_FilterUsers.Add(new FilterLogoffUser(str));
                }
                fs.Close();
            }
            else
            {
                return null;
            }

            return List_FilterUsers;
        }

        // Tự động hóa việc Logoff các User ở trạng thái disconnected.

        public void RemoveDisconnectSessions(int sessionID)
        {
            int k = 0;
            foreach (DisconnectSession dcss in ListDisconnectedSessions)
            {
                if (dcss.SessionID == sessionID)
                {
                    ListDisconnectedSessions.RemoveAt(k);
                    ListSessions.LogofftUserSession(WTS_CURRENT_SERVER_HANDLE, sessionID, false);
                    break;
                }
                k++;
            }
        }

        public void UpdateTimeDisconnection(Session[] listSessions, int timeUpdate)
        {
            int k = 0;
            bool isDisconnect;
            for (int i = 0; i < listSessions.Length; i++)
            {
                int currSessionID = listSessions[i].sessionID;
                string currUserName = listSessions[i].userName;
                // Xét xem Session User này có nằm trong danh sách được lọc hay không.
                // Nếu có thì sẽ không tự động Logoff User này.
                isDisconnect = true;
                List_FilterUsers = Load_FilterLogoffUsers();
                foreach (var obj in List_FilterUsers)
                {
                    if(obj.UserName.Equals(currUserName))
                    {
                        isDisconnect = false;
                        break;
                    }
                }
                if(isDisconnect == true)
                {
                    if (listSessions[i].connectedState == Enums.CONNECTSTATE_CLASS.Disconnected)
                    {
                        k = 0;
                        if (ListDisconnectedSessions.Count > 0)
                        {
                            foreach (DisconnectSession dcss in ListDisconnectedSessions)
                            {
                                if (dcss.SessionID == currSessionID)
                                {
                                    if (dcss.TimeHasDisconnected >= MaxTime)
                                    {
                                        // Đã đạt ngưỡng thời gian giới hạn. Hệ thống sẽ tự động logoff user này.
                                        ListDisconnectedSessions.RemoveAt(k);
                                        ListSessions.LogofftUserSession(WTS_CURRENT_SERVER_HANDLE, currSessionID, false);
                                        Logging.WriteLogSessions("User " + currUserName + " bị hệ thống tự động tắt khỏi máy chủ", "Auto Logoff");
                                        break;
                                    }
                                    else
                                    {
                                        dcss.TimeHasDisconnected += timeUpdate;
                                        break;
                                    }
                                }
                                else if (++k >= ListDisconnectedSessions.Count)
                                {
                                    // Nếu user này vừa mới disconnect nên chưa tồn tại trong danh sách disconnect
                                    // Ta thêm user này vào danh sách disconnect
                                    ListDisconnectedSessions.Add(new DisconnectSession(currSessionID, currUserName, timeUpdate));
                                    // Logging.WriteLogSessions("User " + currUserName + " rời khỏi máy chủ", "Disconnect");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // Nếu danh sách disconnect chưa có gì, ta thêm user vừa mới disconnect
                            ListDisconnectedSessions.Add(new DisconnectSession(currSessionID, currUserName, timeUpdate));
                            // Logging.WriteLogSessions("User " + currUserName + " thoát khỏi máy chủ", "Disconnect");
                        }
                    }
                    else
                    {
                        // Nếu User kết nối lại thì xóa user này trong danh sách disconnect sessions
                        k = 0;
                        foreach (DisconnectSession dcss in ListDisconnectedSessions)
                        {
                            if (dcss.SessionID == currSessionID)
                            {
                                ListDisconnectedSessions.RemoveAt(k);
                                // Logging.WriteLogSessions("User " + currUserName + " kết nối lại vào máy chủ", "Reconnect");
                                break;
                            }
                            k++;
                        }
                    }
                }
            }
        }
    }
}
