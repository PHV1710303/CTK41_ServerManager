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
        public int TimeHasDisconnected { get; set; }

        public DisconnectSession()
        {

        }

        public DisconnectSession(int sessionID, int timeHasDisconnected)
        {
            SessionID = sessionID;
            TimeHasDisconnected = timeHasDisconnected;
        }
    }
}
