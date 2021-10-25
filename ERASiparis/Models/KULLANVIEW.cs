using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERASiparis.Models
{

    public class KULLANVIEW : KULLAN
    {
        public int? IDCari { get; set; }
        public string KARTTIPI { get; set; }
        public string FIRMAADI { get; set; }
        public bool? ISCARI { get; set; }

        public void KullaniciAta(KULLAN k)
        {
            this.ADI = k.ADI;
            this.AKTIF = k.AKTIF;
            this.CARIACMA = k.CARIACMA;
            this.FATURA = k.FATURA;
            this.ID = k.ID;
            this.KREDITAHSILAT = k.KREDITAHSILAT;
            this.NAKITTAHSILAT = k.NAKITTAHSILAT;
            this.CEKSENETTAHSILAT = k.CEKSENETTAHSILAT;
            this.SIFRE = k.SIFRE;
            this.SIPARIS = k.SIPARIS;
            this.CARIARAMA = k.CARIARAMA;
            this.WEBKULLANICI = k.WEBKULLANICI;
            this.SATICIKODU = k.SATICIKODU;

        }
    }

}