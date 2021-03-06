using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    public class STOKALTVIEW:STOKALT
    {
        public string KODU { get; set; }
        public string ADI { get; set; }
       
        public string BRM1 { get; set; }
        public string BRM2 { get; set; }
        public string BRM3 { get; set; }
       
    }
    public class STOKALTMAKBUZ
    {
        public List<STOKALTVIEW> STOKALTS { get; set; }
        public STOKUST STOKUST { get; set; }
        public FIRMA firma { get; set;}
        public CARIKART cari { get; set; }
        public string TARIH { get; set; }
    }
    public class TAHSILATMAKBUZ
    {
        public FIRMA Firma { get; set; }
        public CARIKART Cari { get; set; }
        public CARIHAR carihar { get; set; }
        public decimal? Tutar { get; set; }
        public string Tarih { get; set; }
    }
}