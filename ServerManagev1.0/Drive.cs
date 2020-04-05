using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagev1._0
{
    class Drive
    {
        public String Name { get; set; }

        public Drive(String Name)
        {
            this.Name = Name;
        }

        public static List<Drive> listDrive()
        {
            List<Drive> drives = new List<Drive>();

            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    drives.Add(new Drive(d.Name.Split(':')[0]));
                }
            }

            return drives;
        }
    }
}
