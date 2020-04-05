using ServerManagev1._0.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagev1._0
{
    class ListSessions
    {
        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSLogoffSession(IntPtr hServer, int sessionId, bool bWait);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);

        [DllImport("wtsapi32.dll")]
        static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName);

        [DllImport("wtsapi32.dll")]
        static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 Reserved,
            [MarshalAs(UnmanagedType.U4)] Int32 Version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref Int32 pCount);

        [DllImport("wtsapi32.dll")]
        static extern void WTSFreeMemory(IntPtr pMemory);

        [DllImport("Wtsapi32.dll")]
        static extern bool WTSQuerySessionInformation(
            System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);

        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public Int32 SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public String pWinStationName;

            public WTS_CONNECTSTATE_CLASS State;
        }

        public static bool DisconnectUserSession(IntPtr hServer, int sessionId, bool bWait)
        {
            return WTSDisconnectSession(hServer, sessionId, bWait);
        }

        public static bool LogofftUserSession(IntPtr hServer, int sessionId, bool bWait)
        {
            return WTSLogoffSession(hServer, sessionId, bWait);
        }

        public static IntPtr OpenServer(String Name)
        {
            IntPtr server = WTSOpenServer(Name);
            return server;
        }
        public static void CloseServer(IntPtr ServerHandle)
        {
            WTSCloseServer(ServerHandle);
        }
        public static List<Session> ListUsers(String ServerName)
        {
            IntPtr serverHandle = IntPtr.Zero;
            List<String> resultList = new List<string>();
            serverHandle = OpenServer(ServerName);

            List<Session> listSessions = new List<Session>();

            try
            {
                IntPtr SessionInfoPtr = IntPtr.Zero;
                IntPtr userPtr = IntPtr.Zero;
                IntPtr domainPtr = IntPtr.Zero;
                IntPtr sessionIDPtr = IntPtr.Zero;
                IntPtr statePtr = IntPtr.Zero;
                Int32 sessionCount = 0;
                Int32 retVal = WTSEnumerateSessions(serverHandle, 0, 1, ref SessionInfoPtr, ref sessionCount);
                Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                IntPtr currentSession = SessionInfoPtr;
                uint bytes = 0;

                if (retVal != 0)
                {
                    for (int i = 0; i < sessionCount; i++)
                    {
                        WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)currentSession, typeof(WTS_SESSION_INFO));
                        currentSession += dataSize;

                        WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSUserName, out userPtr, out bytes);
                        WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSDomainName, out domainPtr, out bytes);
                        WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSSessionId, out sessionIDPtr, out bytes);
                        WTSQuerySessionInformation(serverHandle, si.SessionID, WTS_INFO_CLASS.WTSConnectState, out statePtr, out bytes);

                        if (Marshal.PtrToStringAnsi(domainPtr) != "")
                        {
                            Session session = new Session(Marshal.PtrToStringAnsi(userPtr), (int)Marshal.PtrToStructure<Int64>(sessionIDPtr), Marshal.PtrToStringAnsi(domainPtr), (CONNECTSTATE_CLASS)Marshal.PtrToStructure<Int64>(statePtr));
                            listSessions.Add(session);
                            //Console.WriteLine("Domain and User + ID: " + Marshal.PtrToStringAnsi(domainPtr) + "\\" + Marshal.PtrToStringAnsi(userPtr)
                            //    + "\\" + Marshal.PtrToStructure<Int64>(sessionIDPtr)
                            //    + "\\" + Enum.GetName(typeof(CONNECTSTATE_CLASS), Marshal.PtrToStructure<Int64>(statePtr)));
                        }

                        WTSFreeMemory(userPtr);
                        WTSFreeMemory(domainPtr);
                        WTSFreeMemory(sessionIDPtr);
                        WTSFreeMemory(statePtr);
                    }

                    WTSFreeMemory(SessionInfoPtr);
                }

                return listSessions;
            }
            finally
            {
                CloseServer(serverHandle);
            }

        }
    }
}
