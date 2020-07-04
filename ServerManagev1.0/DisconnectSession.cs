using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagev1._0
{
    class DisconnectSession
    {
        public int SessionID { get; set; }
        public string UserName { get; set; }
        public int TimeHasDisconnected { get; set; }

        public DisconnectSession()
        {

        }

        public DisconnectSession(int sessionID, string userName, int timeHasDisconnected)
        {
            SessionID = sessionID;
            UserName = userName;
            TimeHasDisconnected = timeHasDisconnected;
        }
    }
}
