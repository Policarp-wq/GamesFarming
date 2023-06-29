using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesFarming.MVVM.Models.Facilities
{
    public static class Exporter
    {
        public static void ExportText(string text, FileInfo fileInfo) 
        {
            FileSafeAccess.CreateFile(fileInfo.FullName, text);
        }

    }
}
