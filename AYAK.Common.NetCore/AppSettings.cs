using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
    public class AppSettings
    {
        public static AppSettings Current { get; set; }
        public List<Setting> ConnectionStrings { get; set; }
        public List<Setting> Settings { get; set; }
    }
    public class Setting
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
