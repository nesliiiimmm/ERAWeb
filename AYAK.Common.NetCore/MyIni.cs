using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
    public class MyIni
    {
        public static string Dir
        {
            get
            {
                var loc = Assembly.GetExecutingAssembly().Location;
                var dir = Path.GetDirectoryName(loc);
                return dir;
            }
        }
        public static string AppName
        {
            get
            {
                //var loc = Assembly.GetExecutingAssembly().Location;
                //var filename = Path.GetFileNameWithoutExtension(loc);
                //return filename;

                return "Ayar";
            }
        }
        public static string DefaultFileName
        {
            get
            {
                return $"{Dir}\\{AppName}.ini";
            }
        }

        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public static string Read(string key, string section = null, string file = null)
        {
            if (string.IsNullOrEmpty(file))
            {
                file = DefaultFileName;
            }
            if (string.IsNullOrEmpty(section)) section = AppName;
            StringBuilder res=new StringBuilder();
            GetPrivateProfileString(section, key, string.Empty, res, int.MaxValue, file);
            return res.ToString();
        }
        public static void Write(string key, string value, string section = null, string file = null)
        {
            if (string.IsNullOrEmpty(file))
            {
                file = DefaultFileName;
            }
            if (string.IsNullOrEmpty(section)) section = AppName;

            if (!File.Exists(file))
            {
                File.Create(file);
            }
            WritePrivateProfileString(section, key, value, file);
        }


    }
}
