using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERASiparis.Models
{
    [Table(TableName ="",PrimaryKey ="ID",IdentityColumn ="YOK ")]
    public class CEKSENET
    {
        public int ID { get; set; }
        public string SUBEKODU { get; set; }
        public string TIPI { get; set; }
        public bool KENDI { get; set; }
        public string PORTFOYNO { get; set; }
        public DateTime TARIH { get; set; }
        public DateTime VADE { get; set; }
        public decimal TUTAR { get; set; }
        public int CARIKARTID { get; set; }
        public int CIKISCARIKARTID { get; set; }
        public string BANKAADI { get; set; }
        public string BANKASUBESI { get; set; }
        public string BORCLU { get; set; }
        public string SERINO { get; set; }
        public int BANKAID { get; set; }
        public string DURUMU { get; set; }
        public string KESIDEYERI { get; set; }
        public string ACIKLAMA { get; set; }
        public string OZEL1 { get; set; }
        public string OZEL2 { get; set; }
        public string PROJE { get; set; }
        public string SATICI { get; set; }
        public int CIKISBANKA_ID { get; set; }
        public int SEC { get; set; }
        public string DR { get; set; }
        public int AVUKATID { get; set; }
        public string AVUKAT_NO { get; set; }
        public decimal KALAN { get; set; }
        public DateTime AVUKAT_TARIH { get; set; }
        public int BAGLANTIID { get; set; }
        public int YETKI { get; set; }
        public int TP { get; set; }
        public string DURUMBANKAKARTID { get; set; }
    }
    public class CEKSENETORM:ORMBase<CEKSENET,CEKSENETORM>
    {

    }
}
