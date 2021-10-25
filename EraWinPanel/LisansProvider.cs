using AYAK.Common.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EraWinPanel
{
    public class LisansProvider
    {
        const string apiurl = "http://servis.ayasak.com";
        //const string apiurl = "http://localhost:45794/";
        public static string sk = "64830508-09d5-45e3-b3d7-c6c9748e643b";
        public static LisansCevap Kontrol(LisansTalep talep)
        {
            talep.HashUret();
            var talepjson = JsonSerializer.Serialize(talep);
            var res = WebTools.POSTJson($"{apiurl}/api/Licence/Kontrol", talepjson);
            if (!string.IsNullOrEmpty(res) && res.Trim().StartsWith("{"))
            {
                var cevap = JsonSerializer.Deserialize<LisansCevap>(res);
                return cevap;
            }
            else
            {

                return new LisansCevap { Durum = LisansDurum.BaglantiHatasi, Mesaj = res };
            }

        }
    }

    public class LisansTalep
    {
        public LisansTalep()
        {
            SunucuBilgisayar = Dns.GetHostName();
            Tarih = DateTime.Now;
            Program = "EraB2B";
        }
        public string Program { get; set; }
        public Guid LisansAnahtari { get; set; }
        public string FirmaAdi { get; set; }
        public string FirmaSehir { get; set; }
        public string FirmaVKN { get; set; }
        public string SunucuBilgisayar { get; set; }
        public DateTime Tarih { get; set; }
        public string Hash { get; set; }

        public void HashUret()
        {
            Hash = Tools.CreateHash($"{LisansAnahtari}{Program}{FirmaAdi}", LisansProvider.sk);
        }
        public static LisansTalep AyardanDoldur(Ayarlar a=null)
        {
            LisansTalep talep = new LisansTalep();
            if (a == null)
                a = Ayarlar.AyarGetir();
            if (!string.IsNullOrEmpty(a?.LisansAnahtar))
                talep.LisansAnahtari = Guid.Parse(a.LisansAnahtar);
            talep.FirmaAdi = a?.FirmaAdi;
            talep.FirmaSehir = a?.FirmaSehir;
            talep.FirmaVKN = a?.FirmaVergiNo;
            return talep;
        }
    }

    public class LisansCevap
    {
        public DateTime Tarih { get; set; }
        public LisansDurum Durum { get; set; }
        public string Hash { get; set; }
        public string Mesaj { get; set; }
    }
    public enum LisansDurum
    {
        LisansYok = 0,
        Bloke = 1,
        Demo = 2,
        HashHatasi = 3,
        Lisansli = 10,
        BaglantiHatasi = 99
    }
}

//lisans anahtarı alanında leave metondua kontrol çalışır
//program açılırken form1 const içinde kontrol çalışır 
//çalışığında gelen cevabı json dosyasına yazar. 
