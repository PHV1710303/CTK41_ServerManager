using ServerManagev1._0.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagev1._0
{
    //       WTSInitialProgram,
    //       WTSApplicationName,
    //       WTSWorkingDirectory,
    //       WTSOEMId,
    //       WTSSessionId,
    //       WTSUserName,
    //       WTSWinStationName,
    //       WTSDomainName,
    //       WTSConnectState,
    //       WTSClientBuildNumber,
    //       WTSClientName,
    //       WTSClientDirectory,
    //       WTSClientProductId,
    //       WTSClientHardwareId,
    //       WTSClientAddress,
    //       WTSClientDisplay,
    //       WTSClientProtocolType
    class Session
    {
        public Session()
        {

        }

        public Session(String userName, int sessionID, String domainName, CONNECTSTATE_CLASS connectedState)
        {
            this.userName = userName;
            this.sessionID = sessionID;
            this.domainName = domainName;
            this.connectedState = connectedState;
        }

        public String userName { get; set; }

        public int sessionID { get; set; }

        public String domainName { get; set; }

        public CONNECTSTATE_CLASS connectedState { get; set; }
    }
}
