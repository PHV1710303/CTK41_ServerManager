using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
    public class MayTinh
    {
        public MayTinh(DirectoryInfo thuMuc)
        {
            ThuMuc = thuMuc;
        }

        public string TenMay
        {
            //tên máy ứng với tên mỗi thư mục
            get { return ThuMuc.Name; }
        }

        public DirectoryInfo ThuMuc { get; set; }

        public int SoMay
        {
            get
            {
                Match match = Regex.Match(TenMay, @"\d+");
                if (match.Success)
                    return Convert.ToInt32(match.Value);
                return 0;
            }
        }
    }
}
