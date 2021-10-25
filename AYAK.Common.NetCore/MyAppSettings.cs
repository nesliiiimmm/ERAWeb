using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AYAK.Common.NetCore
{
    public class MyAppSettings
    {
        public static string DataBase { get => "Server=BOTANIKSQL;Database=Db_200;user=sa;pwd=123;"; }
        public static string GENEL { get => "Server=BOTANIKSQL;Database=Genel;user=sa;pwd=123;"; }
        //public static IConfiguration Configuration { get; set; }

    }
}
