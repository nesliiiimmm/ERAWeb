using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    [Table(TableName ="FISNO",IdentityColumn = "YOK", PrimaryKey ="ID")]
    public class FISNO
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public string GRUB { get; set; }
        public int? KULKODU { get; set; }
        public string YERI { get; set; }
        public string BASLIK { get; set; }
        public string SERI { get; set; }
        public int? UZUNLUK { get; set; }
        public int? KODU { get; set; }
        public int? YETKI { get; set; }
    }
    public class FISNOORM:ORMBase<FISNO,FISNOORM>
    {
        public string  FisnoVer(string yer)
        {
            string seri = "A";
            string bb = "";
            int? kodu = 0;
            int uzunluk = 5;
            var fn=Current.FirstOrDefault(x => x.YERI == yer);
            seri = fn.SERI;
            kodu = fn.KODU;
            fn.KODU = fn.KODU + 1;
            var control=Current.Update(fn);
            for (int i = 0; i < uzunluk; i++)
            {
                bb = bb + "0";
            }
            string fisno = string.Format("{0}{1:" + bb + "}", seri, kodu, bb);
            return fisno;
        }
    }
}