using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagev1._0
{
    class FileFoldersUtilities
    {
        public static void clearFolder(string FolderName)
        {
            if (FolderName != "System Volume Information")
            {
                DirectoryInfo dir = new DirectoryInfo(FolderName);

                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    clearFolder(di.FullName);
                    di.Delete();
                }
            }
        }


        //fix this
        public static List<String> listFolders(string rootDirectory)
        {
            //IEnumerable<string> files = Enumerable.Empty<string>();
            String[] directories;
            List<String> listDirectories = new List<string>();

            try
            {
                // The test for UnauthorizedAccessException.
                var permission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, rootDirectory);
                permission.Demand();

                //files = Directory.GetFiles(rootDirectory);
                directories = Directory.GetDirectories(rootDirectory);

                foreach (string arrItem in directories)
                {
                    listDirectories.Add(arrItem);
                }



                return listDirectories;
            }
            catch
            {
                return null;
            }


        }
    }
}
