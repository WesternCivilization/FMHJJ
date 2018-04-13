using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FMHJJService.Common.Enum;

namespace FMHJJService.Common
{
    public class LogHelper
    {
        public static void SaveLog(string strMsg)
        {
            string s = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n";
            SaveLogFile(s + strMsg + "\n");
        }

        private static void SaveLogFile(string strErrMsg)
        {
            string logRootDirectory = ("c:/Logs/");
            if (!Directory.Exists(logRootDirectory))
            {
                Directory.CreateDirectory(logRootDirectory);
            }

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            FileInfo inf = new FileInfo(logRootDirectory + fileName);
            StreamWriter wri = new StreamWriter(logRootDirectory + fileName, true, Encoding.UTF8, 1024);
            wri.WriteLine(strErrMsg);
            wri.Close();
        }
    }
}