using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{
    public class SIPARISVIEW : SIPALT
    {
        public string KODU { get; set; }
        public string ADI { get; set; }

        public string BRM1 { get; set; }
        public string BRM2 { get; set; }
        public string BRM3 { get; set; }

    }
    public class SIPARISMAKBUZ
    {
        public SIPUST SIPUST { get; set; }
        public List<SIPARISVIEW> SIPALTs { get; set; }
        public FIRMA firma { get; set; }
        public CARIKART cari { get; set; }
        public string TARIH { get; set; }

    }
}