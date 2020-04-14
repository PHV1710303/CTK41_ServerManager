using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagev1._0
{
    class ProcessDisconnectSessions
    {
        static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        public List<DisconnectSession> ListDisconnectedSessions { get; set; }

        public int MaxTime { get; set; }

        public ProcessDisconnectSessions()
        {
            ListDisconnectedSessions = new List<DisconnectSession>();
        }

        public ProcessDisconnectSessions(Session[] listSessions, int maxTime)
        {
            MaxTime = maxTime;
            ListDisconnectedSessions = new List<DisconnectSession>();
            // dcss là Disconnected Sessions
            for(int i = 0; i < listSessions.Length; i++)
            {
                if(listSessions[i].connectedState == Enums.CONNECTSTATE_CLASS.Disconnected)
                {
                    ListDisconnectedSessions.Add(new DisconnectSession(listSessions[i].sessionID, 0));
                }
            }
        }

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
            for (int i = 0; i < listSessions.Length; i++)
            {
                int currSessionID = listSessions[i].sessionID;
                if (listSessions[i].connectedState == Enums.CONNECTSTATE_CLASS.Disconnected)
                {
                    k = 0;
                    if(ListDisconnectedSessions.Count > 0)
                    {
                        foreach (DisconnectSession dcss in ListDisconnectedSessions)
                        {
                            if (dcss.SessionID == currSessionID)
                            {
                                if(dcss.TimeHasDisconnected >= MaxTime)
                                {
                                    ListDisconnectedSessions.RemoveAt(k);
                                    ListSessions.LogofftUserSession(WTS_CURRENT_SERVER_HANDLE, currSessionID, false);
                                    break;
                                }
                                else
                                {
                                    dcss.TimeHasDisconnected += timeUpdate;
                                    break;
                                }
                            }
                            else if(++k >= ListDisconnectedSessions.Count)
                            {
                                ListDisconnectedSessions.Add(new DisconnectSession(currSessionID, timeUpdate));
                                break;
                            }
                        }
                    }
                    else
                    {
                        ListDisconnectedSessions.Add(new DisconnectSession(currSessionID, timeUpdate));
                    }
                }
                else
                {
                    k = 0;

                    foreach(DisconnectSession dcss in ListDisconnectedSessions)
                    {
                        if(dcss.SessionID == currSessionID)
                        {
                            ListDisconnectedSessions.RemoveAt(k);
                            break;
                        }
                        k++;
                    }
                }
            }
        }
    }
}
