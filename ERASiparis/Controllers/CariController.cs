using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AYAK.Common.NetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ERASiparis.Models;
using Microsoft.Data.SqlClient;

namespace ERASiparis.Controllers
{
    [Authorize]
    public class CariController : Controller
    {
        ISession Session => HttpContext.Session;
        public FIRMA Firma
        {
            get
            {
                if (Session.GetString("Firma") != null)
                    return JsonConvert.DeserializeObject<FIRMA>(Session.GetString("Firma"));
                else
                {
                    string database = Tools.Connections["database"].ConnectionString;
                    SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(database);
                    var catalog = csb.InitialCatalog.ToUpper();
                    var fc = catalog.Replace("DB_", "").Replace("DB", "");
                    var _firma = FIRMAORM.Current.FirstOrDefault(x => x.KODU == fc);
                    return _firma;
                }
            }
        }
        public CARIKART AktiCariKart
        {
            get
            {
                if (Session.GetString("AktifCariKart") != null)
                    return JsonConvert.DeserializeObject<CARIKART>(Session.GetString("AktifCariKart"));
                else
                    return new CARIKART();
            }
            set
            {
                Session.SetString("AktifCariKart", JsonConvert.SerializeObject(value));
            }
        }
        public KULLANVIEW AktifUser
        {
            get
            {
                if (Session.GetString("AktifUser") != null)
                    return JsonConvert.DeserializeObject<KULLANVIEW>(Session.GetString("AktifUser"));
                else
                    return new KULLANVIEW();
            }
            set
            {
                Session.SetString("AktifUser", JsonConvert.SerializeObject(value));
            }
        }
        public CARITOPLAM AktifCariToplam
        {
            get
            {
                if (Session.GetString("AktifCariToplam") != null)
                    return JsonConvert.DeserializeObject<CARITOPLAM>(Session.GetString("AktifCariToplam"));
                else
                    return new CARITOPLAM();
            }
            set
            {
                Session.SetString("AktifCariToplam", JsonConvert.SerializeObject(value));
            }
        }
        public CARITOPLAMTIP AktifCariToplamTP0
        {
            get
            {
                if (Session.GetString("AktifCariToplamTP0") != null)
                    return JsonConvert.DeserializeObject<CARITOPLAMTIP>(Session.GetString("AktifCariToplamTP0"));
                else
                    return new CARITOPLAMTIP();
            }
            set
            {
                Session.SetString("AktifCariToplamTP0", JsonConvert.SerializeObject(value));
            }
        }
        public CARITOPLAMTIP AktifCariToplamTP1
        {
            get
            {
                if (Session.GetString("AktifCariToplamTP1") != null)
                    return JsonConvert.DeserializeObject<CARITOPLAMTIP>(Session.GetString("AktifCariToplamTP1"));
                else
                    return new CARITOPLAMTIP();
            }
            set
            {
                Session.SetString("AktifCariToplamTP1", JsonConvert.SerializeObject(value));
            }
        }
        public CARITOPLAMTIP AktifCariToplamTP2
        {
            get
            {
                if (Session.GetString("AktifCariToplamTP2") != null)
                    return JsonConvert.DeserializeObject<CARITOPLAMTIP>(Session.GetString("AktifCariToplamTP2"));
                else
                    return new CARITOPLAMTIP();
            }
            set
            {
                Session.SetString("AktifCariToplamTP2", JsonConvert.SerializeObject(value));
            }
        }
        public List<Result> Results
        {
            get
            {
                if (Session.GetString("Resultlar") == null)
                    Session.SetString("Resultlar", JsonConvert.SerializeObject(new List<Result>()));
                return JsonConvert.DeserializeObject<List<Result>>(Session.GetString("Resultlar"));
            }
            set
            {
                Session.SetString("Resultlar", JsonConvert.SerializeObject(value));
            }
        }
        public List<OZEL> CariOzel1
        {
            get
            {
                if (Session.GetString("CariOzelKod1") != null)
                    return JsonConvert.DeserializeObject<List<OZEL>>(Session.GetString("CariOzelKod1"));
                else
                    return new List<OZEL>();
            }
            set
            {
                Session.SetString("CariOzelKod1", JsonConvert.SerializeObject(value));
            }
        }
        public int SeciliTP
        {
            get
            {
                if (Session.GetInt32("SeciliTip") != null)
                    return Session.GetInt32("SeciliTip").ToInt();
                else
                    return 0;
            }
            set
            {
                Session.SetInt32("SeciliTip", value);
            }
        }
        public List<OZEL> CariOzel2
        {
            get
            {
                if (Session.GetString("CariOzelKod2") != null)
                    return JsonConvert.DeserializeObject<List<OZEL>>(Session.GetString("CariOzelKod2"));
                else
                    return new List<OZEL>();
            }
            set
            {
                Session.SetString("CariOzelKod2", JsonConvert.SerializeObject(value));
            }
        }

        public ActionResult Index()
        {
            ViewBag.AktifUser = AktifUser;
            ViewBag.AktifCariKart = AktiCariKart;

            ViewBag.Kartipi = AktifUser.KARTTIPI;
            List<CARIHAR> ch = new List<CARIHAR>();
            if (AktiCariKart.ID != 0)
            {
                var cah = Tools.Select<CARIHAR>($@" SELECT   CARIHAR.*,  
                            CARIHAR.BORC - CARIHAR.ALACAK AS BAKIYE,
                            OZEL1.ADI AS OZEL1ADI,
                            OZEL2.ADI AS OZEL2ADI, 
                            PROJE.ADI AS PROJEADI, 
                            SATICI.ADI AS SATICIADI,
                            BAGLAN.ADI AS BAGLANADI,
                            KASA.ADI AS KASAADI,
                            BANKA.ADI AS BANKADI 
                            FROM CARIHAR
                            LEFT JOIN GENEL.DBO.KASA  AS KASA  ON KASA.ID = CARIHAR.KASAID  
                            LEFT JOIN BANKA AS BANKA   ON BANKA.ID    = CARIHAR.BANKAID  
                            LEFT JOIN OZEL  AS OZEL1   ON OZEL1.KODU  = CARIHAR.OZEL1      AND OZEL1.YER = 'CHAR1'
                            LEFT JOIN OZEL  AS OZEL2   ON OZEL2.KODU  = CARIHAR.OZEL2      AND OZEL2.YER = 'CHAR2'
                            LEFT JOIN OZEL  AS PROJE   ON PROJE.KODU  = CARIHAR.PROJEKODU  AND PROJE.YER = 'PROJE'
                            LEFT JOIN OZEL  AS SATICI  ON SATICI.KODU = CARIHAR.SATICIKODU AND SATICI.YER = 'SATICI'
                            LEFT JOIN BAGLAN AS BAGLAN ON BAGLAN.ID   = CARIHAR.BAGLANTIID
                            WHERE CARIHAR.DR = 'K' AND  CARIHAR.CARIKARTID = '{AktiCariKart.ID}'
                            order by ID desc");
                ch = cah.Data;
            }

            return View(ch);
        }
        public void CariSecim(int id)
        {
            var cari = CARIKARTORM.Current.FirstOrDefault(x => x.ID == id);
            AktiCariKart = cari;
        }
        public PartialViewResult CariArama(string arama)
        {
            var prm = arama.CreateParameters("@a");
            var carim = CARIKARTORM.Current.Select("WHERE KODU like @a+'%' or FIRMAADI like '%'+@a+'%' AND DR='K' ", prm, SelectType.Where);
            return PartialView(carim.Data);
        }

        public PartialViewResult CariKart()
        {
            var cari = CARITOPLAMORM.Current.FirstOrDefault(x => x.CARIKARTID == AktiCariKart.ID & x.CARIKARTID != 0);
            var cari0 = CARITOPLAMTIPORM.Current.FirstOrDefault(x => x.CARIKARTID == AktiCariKart.ID & x.CARIKARTID != 0 & x.TP == 0);
            var cari1 = CARITOPLAMTIPORM.Current.FirstOrDefault(x => x.CARIKARTID == AktiCariKart.ID & x.CARIKARTID != 0 & x.TP == 1);
            var cari2 = CARITOPLAMTIPORM.Current.FirstOrDefault(x => x.CARIKARTID == AktiCariKart.ID & x.CARIKARTID != 0 & x.TP == 2);
            AktifCariToplam = cari;
            AktifCariToplamTP0 = cari0;
            AktifCariToplamTP1 = cari1;
            AktifCariToplamTP2 = cari2;
            return PartialView(new Tuple<CARIKART, CARITOPLAM>(AktiCariKart, cari));
        }
        public ActionResult CariEkle(string message)
        {
            ViewBag.Message = message;
            var cariozel1 = OZELORM.Current.Where(x => x.YER == "CKART1");
            CariOzel1 = cariozel1.Data;
            var cariozel2 = OZELORM.Current.Where(x => x.YER == "CKART2");
            CariOzel2 = cariozel2.Data;
            return View();
        }
        [HttpPost]
        public ActionResult CariKaydet(CARIKART cr)
        {
            if (cr.FIRMAADI != null)
            {
                if (cr.KODU == null)
                {
                    cr.KODU = FISNOORM.Current.FisnoVer("CARIKART");
                }
                cr.ID = SIRAORM.Current.SiraVer();
                cr.SUBEKODU = "000";
                cr.REFERANS = "";
                cr.OZEL3 = "";
                cr.OZEL4 = "";
                cr.WEB = "";
                cr.WEBKULKODU = "";
                cr.WEBSIFRE = "";
                cr.EMAIL = "";
                cr.RISKCEK = 0;
                cr.RSIKACIK = 0;
                cr.RISKKREDI = 0;
                cr.ISKONTOALIS = 0;
                cr.ISKONTOSATIS = 0;
                cr.KAYITTARIHI = DateTime.Now.Date;
                cr.VADEALIS = 0;
                cr.VADESATIS = 0;
                cr.CARIRISK = 0;
                cr.KEFIL1ID = 0;
                cr.KEFIL2ID = 0;
                cr.AKTIF = true;
                cr.SMS1 = false;
                cr.SMS2 = false;
                cr.SMS3 = false;
                cr.SMS4 = false;
                cr.DR = "K";
                cr.EFATURAMI = false;
                cr.YETKI = 0;
                var cariekle = CARIKARTORM.Current.Insert(cr);
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(cariekle);
                Results = rslt;
                //Results.Add(cariekle);
                if (cariekle.State == ResultState.Success)
                {
                    AktiCariKart = cr;
                }
                else
                {
                    ViewBag.Message = "Cari Eklenemedi.";
                    return RedirectToAction("CariEkle");
                }
            }
            else
            {
                ViewBag.Message = "Cari Eklenemedi.";
                return RedirectToAction("CariEkle", new { message = ViewBag.Message });
            }

            return RedirectToAction("Index");
        }

        public ActionResult CariList()
        {
            var res = CARIKARTORM.Current.Select();
            return View(res.Data);
        }
        public void nakitTahsilat(string nakit, int kasa, string aciklama)
        {
            if (nakit != null && nakit != "")
            {
                decimal tutar = nakit.Replace(".", ",").ToDecimal();
                KASAHAR kasah = new KASAHAR();
                kasah.ID = SIRAORM.Current.SiraVer(); ;
                kasah.SUBEKODU = "000";
                kasah.KASAID = kasa;
                kasah.CARIKARTID = AktiCariKart.ID;
                kasah.PERSONELID = 0;
                kasah.FATURAID = 0;
                kasah.CEKSENETID = 0;
                kasah.STOKKARTIID = 0;
                kasah.BAGLANTIID = 0;
                kasah.BANKAKARTID = 0;
                kasah.VIRMANKASAKARTID = 0;
                kasah.VIRMANID = 0;
                kasah.FISNO = FISNOORM.Current.FisnoVer("KASAHAR");
                kasah.FISTIPI = "11-Nakit Tahsilat";
                kasah.TARIH = DateTime.Now;
                kasah.ACIKLAMA = "";
                kasah.ACIKLAMA1 = "";
                kasah.BORC = tutar;
                kasah.ALACAK = 0;//UNDABIRGARIPLIKMIVAR
                kasah.OZEL1 = "";
                kasah.OZEL2 = "";
                kasah.SATICIKODU = AktifUser.SATICIKODU;
                kasah.PROJEKODU = "";
                kasah.KAYITYERI = "KASAISLEM";
                kasah.DR = "K";
                kasah.TP = SeciliTP;
                kasah.YETKI = 0;
                kasah.ACIKLAMA = aciklama;
                var control2 = KASAHARORM.Current.Insert(kasah);
                Results.Add(control2);

                CARIHAR car = new CARIHAR();
                car.ID = kasah.ID;
                car.SUBEKODU = "000";
                car.CARIKARTID = AktiCariKart.ID;
                car.FISTIPI = "11-Nakit Tahsilat";
                car.TARIH = DateTime.Now;
                car.VADE = DateTime.Now;
                car.FISNO = FISNOORM.Current.FisnoVer("CARIHAR");
                car.ALACAK = tutar;//bunları sor 
                car.BORC = 0;
                car.DR = "K";
                car.KAYITYERI = "KASAISLEM";
                car.TP = SeciliTP;
                car.KASAID = kasa;
                car.ACIKLAMA1 = "";
                car.DOVKURU = 0;
                car.DOVTUTAR = 0;
                car.PROJEKODU = "";
                car.SATICIKODU = AktifUser.SATICIKODU;
                car.VIRMANID = 0;
                car.VIRMANCARIKARTID = 0;
                car.KAPALIFIS = 0;
                car.BAGLANTIID = 0;
                car.KASAID = 0;
                car.BANKAID = 0;
                car.YETKI = 0;
                car.KOMISYONTUTARI = 0;
                car.PLAKA = "";
                car.FATURAID = 0;//bunu bir sormak lazım 
                car.ACIKLAMA = kasah.ID + " Nolu Kasa İşlem Fişi (Web İslemi)";//burda Fatura Bedelini de ekliyor 
                var control = CARIHARORM.Current.Insert(car);
                Results.Add(control);
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(control2);
                rslt.Add(control);
                Results = rslt;
            }
        }
        public void kartTahsilat(string nakit, int banka, string aciklama)
        {
            if (nakit != null && nakit != "")
            {
                decimal tutar = nakit.Replace(".", ",").ToDecimal();
                //carihar kayıt gir 
                //Tipi Tahsilat 
                //Tarih
                //cari id
                //id 
                //Kart işlem tablosuna
                //karthavuz tablosuna kayıt aç 
                KARTISLEM karislem = new KARTISLEM();
                karislem.ID = SIRAORM.Current.SiraVer();
                karislem.FISNO = FISNOORM.Current.FisnoVer("KARTISLEM");
                karislem.TARIH = DateTime.Now;
                karislem.FISTIPI = "11-Karttan Tahsilat";
                karislem.CARIKARTID = AktiCariKart.ID;
                karislem.BANKAID = banka;
                karislem.STOKKARTIID = 0;
                karislem.BANKAKAYITID = 0;
                karislem.MAILORDERCARIID = 0;
                karislem.KARTID = 0;
                karislem.KASAID = 0;
                karislem.SIPARISID = 0;
                karislem.SUBEKODU = "000";
                karislem.SATICIKODU = AktifUser.SATICIKODU;
                karislem.OZEL1 = "";
                karislem.OZEL2 = "";
                karislem.PROJE = "";
                karislem.BAGLANTIID = 0;
                karislem.ACIKLAMA = "";
                karislem.ALACAK = 0;
                karislem.BORC = tutar;
                karislem.TAKSITSAYISI = 1;
                karislem.KKDFORANI = 0;
                karislem.BSMVORAN = 0;
                karislem.FAISORANI = 0;
                karislem.KKFDFTUTAR = 0;
                karislem.BSMVTUTAR = 0;
                karislem.FAIZTUTAR = 0;
                karislem.SAHIBIADI = "";
                karislem.KARTNO = "";
                karislem.SONKULLANIMTARIHI = "";
                karislem.GUVENLIKKODU = "";
                karislem.KAYITYERI = "KARTISLEM";
                karislem.YETKI = 0;
                karislem.DR = "K";
                karislem.TP = SeciliTP;
                karislem.SUBE = 0;

                CARIHAR car = new CARIHAR();
                car.SUBEKODU = "000";
                car.ID = karislem.ID;
                car.CARIKARTID = AktiCariKart.ID;
                car.FISTIPI = "11-Karttan Tahsilat";
                car.TARIH = DateTime.Now;
                car.VADE = DateTime.Now;
                car.FISNO = FISNOORM.Current.FisnoVer("CARIHAR");
                car.ALACAK = tutar;//bunları sor 
                car.BORC = 0;
                car.DR = "K";
                car.KAYITYERI = "FATURA";
                car.TP = SeciliTP;
                car.BANKAID = banka;
                car.ACIKLAMA1 = "";
                car.DOVKURU = 0;
                car.DOVTUTAR = 0;
                car.PROJEKODU = "";
                car.SATICIKODU = AktifUser.SATICIKODU;
                car.VIRMANID = 0;
                car.VIRMANCARIKARTID = 0;
                car.KAPALIFIS = 0;
                car.BAGLANTIID = 0;
                car.KASAID = 0;
                car.YETKI = 0;
                car.KOMISYONTUTARI = 0;
                car.PLAKA = "";
                car.FATURAID = 0;

                KARTHAVUZ khavuz = new KARTHAVUZ();
                khavuz.ID = SIRAORM.Current.SiraVer();
                khavuz.KARTISLEMID = karislem.ID;
                khavuz.TAKSITSIRA = 1;
                khavuz.SUBE = "000";
                khavuz.TARIH = DateTime.Now;
                khavuz.VADE = DateTime.Now;
                khavuz.BANKAID = banka;
                khavuz.CARIKARTID = AktiCariKart.ID;
                khavuz.CARIHARID = 0;///???
                khavuz.TUTAR = tutar;
                khavuz.KESINTI = 0;
                khavuz.NETTUTAR = tutar;
                khavuz.ISLENDI = false;
                khavuz.KARTID = 0;
                khavuz.YETKI = 0;
                //khavuz.DR = "K";
                car.ACIKLAMA = karislem.FISNO + " Nolu Kredi Kartı İşlem Fişi (Web İslemi)";
                var control = CARIHARORM.Current.Insert(car);
                //Results.Add(control);
                var control2 = KARTISLEMORM.Current.Insert(karislem);
                //Results.Add(control2);
                var control3 = KARTHAVUZORM.Current.Insert(khavuz);
                //Results.Add(control3);
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(control);
                rslt.Add(control2);
                rslt.Add(control3);
                Results = rslt;
            }
        }
        public void ceksenetTahsilat(string ceknakit, int banka, string seciliCSTip, DateTime vade, string borclu, string serino, string keside, string aciklama)
        {
            if (ceknakit != null && ceknakit != "")
            {

                decimal tutar = ceknakit.Replace(".", ",").ToDecimal();
                BANKA b = BANKAORM.Current.FirstOrDefault(x => x.ID == banka);
                CEKSENET cs = new CEKSENET();
                cs.ID = SIRAORM.Current.SiraVer();
                cs.SUBEKODU = "000";
                cs.SATICI = AktifUser.SATICIKODU;
                cs.TIPI = seciliCSTip;
                cs.KENDI = false;
                cs.PORTFOYNO = FISNOORM.Current.FisnoVer("PORTNO");
                cs.CARIKARTID = AktiCariKart.ID;
                cs.TARIH = Convert.ToDateTime(DateTime.UtcNow.ToString("yyyy-MM-dd"));
                cs.VADE = Convert.ToDateTime(vade.ToString("yyyy-MM-dd"));
                cs.TUTAR = tutar;
                cs.CIKISCARIKARTID = 0;
                cs.BANKAADI = b.ADI;
                cs.BANKASUBESI = b.HESAPSUBESI;
                if (borclu == null)
                {

                    cs.BORCLU = AktiCariKart.ADI;//burayı sormam lazım 
                }
                else
                {
                    cs.BORCLU = borclu;
                }
                cs.SERINO = serino;
                cs.BANKAID = banka;
                cs.DURUMU = "Ciro";
                cs.KESIDEYERI = keside;//???
                cs.ACIKLAMA = aciklama;
                cs.OZEL1 = "";
                cs.OZEL2 = "";
                cs.PROJE = "";
                cs.CIKISBANKA_ID = 0;//???
                cs.SEC = 0;
                cs.DR = "K";
                cs.AVUKATID = 0;//??
                cs.BAGLANTIID = 0;
                cs.YETKI = 0;
                cs.TP = SeciliTP;
                var res = CEKSENETORM.Current.Insert(cs);

                CEKBOR cb = new CEKBOR();
                cb.ID = SIRAORM.Current.SiraVer();
                cb.SUBEKODU = "000";
                cb.FISNO = FISNOORM.Current.FisnoVer("CEKBOR");
                cb.TARIH = DateTime.Now;
                cb.FISTIPI = "12-Çek Senet Giriş Bordrosu";//???
                cb.ACIKLAMA = "Web Cek-Senet Tahsilatı";
                cb.CARIKARTID = AktiCariKart.ID;
                cb.KASAHARID = 0;
                cb.BANKAKARTID = 0;
                cb.OZEL1 = "";
                cb.OZEL2 = "";
                cb.PROJEKODU = "";
                cb.BAGLANTIID = 0;
                cb.SATICI = AktifUser.SATICIKODU;
                cb.EVRAKSAYISI = 0;
                cb.TOPLAMTUTAR = tutar;
                cb.ORTVADE = vade;
                cb.URUNID = 0;
                cb.ACIKLAMA1 = "";
                cb.DURUMU = "Ciro";
                cb.CEKID = 0;
                cb.DR = "K";
                cb.YETKI = 0;
                cb.TP = SeciliTP;
                var res2 = CEKBORORM.Current.Insert(cb);


                CEKDURUM cd = new CEKDURUM();
                cd.BORDROID = cb.ID;
                cd.CEKID = cs.ID;
                cd.DURUMU = "Portföy";
                cd.ACIKLAMA = "Çek Senet Giris Bordrosu";
                cd.DR = "K";
                cd.CARIID = AktiCariKart.ID;
                cd.BANKAID = banka;
                cd.TUTAR = tutar;
                var res3 = CEKDURUMORM.Current.Insert(cd);

                CARIHAR car = new CARIHAR();
                car.SUBEKODU = "000";
                car.ID = cb.ID;
                car.CARIKARTID = AktiCariKart.ID;
                car.FISTIPI = "41-Alacak Dekontu";
                car.TARIH = DateTime.Now;
                car.VADE = vade;
                car.FISNO = FISNOORM.Current.FisnoVer("CARIHAR");
                car.ALACAK = tutar;//bunları sor 
                car.BORC = 0;
                car.DR = "K";
                car.KAYITYERI = "CEKSENET";
                car.TP = SeciliTP;
                car.BANKAID = banka;
                car.ACIKLAMA1 = "";
                car.DOVKURU = 0;
                car.DOVTUTAR = 0;
                car.PROJEKODU = "";
                car.SATICIKODU = AktifUser.SATICIKODU;
                car.VIRMANID = 0;
                car.VIRMANCARIKARTID = 0;
                car.KAPALIFIS = 0;
                car.BAGLANTIID = 0;
                car.KASAID = 0;
                car.YETKI = 0;
                car.KOMISYONTUTARI = 0;
                car.PLAKA = "";
                car.FATURAID = 0;
                car.TP = SeciliTP;
                car.ACIKLAMA = cb.FISNO + " Nolu 12-Çek Senet Giriş Bordrosu İşlemi (Web işlem)";
                var res4 = CARIHARORM.Current.Insert(car);

                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(res);
                rslt.Add(res2);
                rslt.Add(res3);
                rslt.Add(res4);
                Results = rslt;
            }
        }


        public void nakitOdeme(string nakit, int kasa, string aciklama)
        {
            if (nakit != null && nakit != "")
            {
                decimal tutar = nakit.Replace(".", ",").ToDecimal();
                //carihar kayıt gir 
                //Tipi Tahsilat 
                //Tarih
                //cari id
                //id 
                //kasa tablosuna kayıt gir 
                KASAHAR kasah = new KASAHAR();
                kasah.ID = SIRAORM.Current.SiraVer(); ;
                kasah.SUBEKODU = "000";
                kasah.KASAID = kasa;
                kasah.CARIKARTID = AktiCariKart.ID;
                kasah.PERSONELID = 0;
                kasah.FATURAID = 0;
                kasah.CEKSENETID = 0;
                kasah.STOKKARTIID = 0;
                kasah.BAGLANTIID = 0;
                kasah.BANKAKARTID = 0;
                kasah.VIRMANKASAKARTID = 0;
                kasah.VIRMANID = 0;
                kasah.FISNO = FISNOORM.Current.FisnoVer("KASAHAR");
                kasah.FISTIPI = "11-Nakit Tahsilat";
                kasah.TARIH = DateTime.Now;
                kasah.ACIKLAMA = aciklama;
                kasah.ACIKLAMA1 = "Web işlem";
                kasah.BORC = 0;
                kasah.ALACAK = tutar;
                kasah.OZEL1 = "";
                kasah.OZEL2 = "";
                kasah.SATICIKODU = AktifUser.SATICIKODU;
                kasah.PROJEKODU = "";
                kasah.KAYITYERI = "KASAISLEM";
                kasah.DR = "K";
                kasah.TP = SeciliTP;
                kasah.YETKI = 0;
                var control2 = KASAHARORM.Current.Insert(kasah);
                Results.Add(control2);

                CARIHAR car = new CARIHAR();
                car.ID = kasah.ID;
                car.SUBEKODU = "000";
                car.CARIKARTID = AktiCariKart.ID;
                car.FISTIPI = "11-Nakit Tahsilat";
                car.TARIH = DateTime.Now;
                car.VADE = DateTime.Now;
                car.FISNO = FISNOORM.Current.FisnoVer("CARIHAR");
                car.ALACAK = 0;//bunları sor 
                car.BORC = tutar;
                car.DR = "K";
                car.KAYITYERI = "KASAISLEM";
                car.TP = SeciliTP;
                car.KASAID = kasa;
                car.ACIKLAMA1 = "";
                car.DOVKURU = 0;
                car.DOVTUTAR = 0;
                car.PROJEKODU = "";
                car.SATICIKODU = AktifUser.SATICIKODU;
                car.VIRMANID = 0;
                car.VIRMANCARIKARTID = 0;
                car.KAPALIFIS = 0;
                car.BAGLANTIID = 0;
                car.KASAID = 0;
                car.BANKAID = 0;
                car.YETKI = 0;
                car.KOMISYONTUTARI = 0;
                car.PLAKA = "";
                car.FATURAID = 0;//bunu bir sormak lazım 
                car.ACIKLAMA = kasah.ID + " Nolu Kasa İşlem Fişi (Web İslemi)";//burda Fatura Bedelini de ekliyor 
                var control = CARIHARORM.Current.Insert(car);
                Results.Add(control);
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(control2);
                rslt.Add(control);
                Results = rslt;
            }
        }
        public void kartOdeme(string nakit, int banka, string aciklama)
        {
            if (nakit != null && nakit != "")
            {
                decimal tutar = nakit.Replace(".", ",").ToDecimal();
                //carihar kayıt gir 
                //Tipi Tahsilat 
                //Tarih
                //cari id
                //id 
                //Kart işlem tablosuna
                //karthavuz tablosuna kayıt aç 
                KARTISLEM karislem = new KARTISLEM();
                karislem.ID = SIRAORM.Current.SiraVer();
                karislem.FISNO = FISNOORM.Current.FisnoVer("KARTISLEM");
                karislem.TARIH = DateTime.Now;
                karislem.FISTIPI = "11-Karttan Tahsilat";
                karislem.CARIKARTID = AktiCariKart.ID;
                karislem.BANKAID = banka;
                karislem.STOKKARTIID = 0;
                karislem.BANKAKAYITID = 0;
                karislem.MAILORDERCARIID = 0;
                karislem.KARTID = 0;
                karislem.KASAID = 0;
                karislem.SIPARISID = 0;
                karislem.SUBEKODU = "000";
                karislem.SATICIKODU = AktifUser.SATICIKODU;
                karislem.OZEL1 = "";
                karislem.OZEL2 = "";
                karislem.PROJE = "";
                karislem.BAGLANTIID = 0;
                karislem.ACIKLAMA = "";
                karislem.ALACAK = tutar;
                karislem.BORC = 0;
                karislem.TAKSITSAYISI = 1;
                karislem.KKDFORANI = 0;
                karislem.BSMVORAN = 0;
                karislem.FAISORANI = 0;
                karislem.KKFDFTUTAR = 0;
                karislem.BSMVTUTAR = 0;
                karislem.FAIZTUTAR = 0;
                karislem.SAHIBIADI = "";
                karislem.KARTNO = "";
                karislem.SONKULLANIMTARIHI = "";
                karislem.GUVENLIKKODU = "";
                karislem.KAYITYERI = "KARTISLEM";
                karislem.YETKI = 0;
                karislem.DR = "K";
                karislem.TP = SeciliTP;
                karislem.SUBE = 0;

                CARIHAR car = new CARIHAR();
                car.SUBEKODU = "000";
                car.ID = karislem.ID;
                car.CARIKARTID = AktiCariKart.ID;
                car.FISTIPI = "11-Karttan Tahsilat";
                car.TARIH = DateTime.Now;
                car.VADE = DateTime.Now;
                car.FISNO = FISNOORM.Current.FisnoVer("CARIHAR");
                car.ALACAK = 0;//bunları sor 
                car.BORC = tutar;
                car.DR = "K";
                car.KAYITYERI = "FATURA";
                car.TP = SeciliTP;
                car.BANKAID = banka;
                car.ACIKLAMA1 = "";
                car.DOVKURU = 0;
                car.DOVTUTAR = 0;
                car.PROJEKODU = "";
                car.SATICIKODU = AktifUser.SATICIKODU;
                car.VIRMANID = 0;
                car.VIRMANCARIKARTID = 0;
                car.KAPALIFIS = 0;
                car.BAGLANTIID = 0;
                car.KASAID = 0;
                car.YETKI = 0;
                car.KOMISYONTUTARI = 0;
                car.PLAKA = "";
                car.FATURAID = 0;

                KARTHAVUZ khavuz = new KARTHAVUZ();
                khavuz.ID = SIRAORM.Current.SiraVer();
                khavuz.KARTISLEMID = karislem.ID;
                khavuz.TAKSITSIRA = 1;
                khavuz.SUBE = "000";
                khavuz.TARIH = DateTime.Now;
                khavuz.VADE = DateTime.Now;
                khavuz.BANKAID = banka;
                khavuz.CARIKARTID = AktiCariKart.ID;
                khavuz.CARIHARID = 0;///???
                khavuz.TUTAR = tutar;
                khavuz.KESINTI = 0;
                khavuz.NETTUTAR = tutar;
                khavuz.ISLENDI = false;
                khavuz.KARTID = 0;
                khavuz.YETKI = 0;
                //khavuz.DR = "K";
                car.ACIKLAMA = karislem.FISNO + " Nolu Kredi Kartı İşlem Fişi (Web İslemi)";
                var control = CARIHARORM.Current.Insert(car);
                //Results.Add(control);
                var control2 = KARTISLEMORM.Current.Insert(karislem);
                //Results.Add(control2);
                var control3 = KARTHAVUZORM.Current.Insert(khavuz);
                //Results.Add(control3);
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(control);
                rslt.Add(control2);
                rslt.Add(control3);
                Results = rslt;
            }
        }

        public void CariHarSil(int id)
        {
            var res = CARIHARORM.Current.DeleteWithID(id);
        }
        public void TPSecim(int SeciliTip)
        {
            if (SeciliTip != 0)
            {
                SeciliTP = SeciliTip;
            }
        }

    }
    public class CariViewComponent : ViewComponent
    {
        ISession Session => HttpContext.Session;
        public CARIKART AktiCariKart
        {
            get
            {
                if (Session.GetString("AktifCariKart") != null)
                    return JsonConvert.DeserializeObject<CARIKART>(Session.GetString("AktifCariKart"));
                else
                    return new CARIKART();
            }
            set
            {
                Session.SetString("AktifCariKart", JsonConvert.SerializeObject(value));
            }
        }
        public KULLANVIEW AktifUser
        {
            get
            {
                if (Session.GetString("AktifUser") != null)
                    return JsonConvert.DeserializeObject<KULLANVIEW>(Session.GetString("AktifUser"));
                else
                    return new KULLANVIEW();
            }
            set
            {
                Session.SetString("AktifUser", JsonConvert.SerializeObject(value));
            }
        }
        public CARITOPLAM AktifCariToplam
        {
            get
            {
                if (Session.GetString("AktifCariKart") != null)
                    return JsonConvert.DeserializeObject<CARITOPLAM>(Session.GetString("AktifCariKart"));
                else
                    return new CARITOPLAM();
            }
            set
            {
                Session.SetString("AktifCariToplam", JsonConvert.SerializeObject(value));
            }
        }
        public CARITOPLAMTIP AktifCariToplamTP0
        {
            get
            {
                if (Session.GetString("AktifCariToplamTP0") != null)
                    return JsonConvert.DeserializeObject<CARITOPLAMTIP>(Session.GetString("AktifCariToplamTP0"));
                else
                    return new CARITOPLAMTIP();
            }
            set
            {
                Session.SetString("AktifCariToplamTP0", JsonConvert.SerializeObject(value));
            }
        }
        public CARITOPLAMTIP AktifCariToplamTP1
        {
            get
            {
                if (Session.GetString("AktifCariToplamTP1") != null)
                    return JsonConvert.DeserializeObject<CARITOPLAMTIP>(Session.GetString("AktifCariToplamTP1"));
                else
                    return new CARITOPLAMTIP();
            }
            set
            {
                Session.SetString("AktifCariToplamTP1", JsonConvert.SerializeObject(value));
            }
        }
        public CARITOPLAMTIP AktifCariToplamTP2
        {
            get
            {
                if (Session.GetString("AktifCariToplamTP2") != null)
                    return JsonConvert.DeserializeObject<CARITOPLAMTIP>(Session.GetString("AktifCariToplamTP2"));
                else
                    return new CARITOPLAMTIP();
            }
            set
            {
                Session.SetString("AktifCariToplamTP2", JsonConvert.SerializeObject(value));
            }
        }
        public int SecilenTip
        {
            get
            {
                if (Session.GetInt32("SecilenTip") != null)
                    return (int)Session.GetInt32("SecilenTip");
                else
                    return 0;
            }
            set
            {
                Session.SetInt32("SecilenTip", value);
            }
        }
        public IViewComponentResult Invoke()
        {
            var cari = CARITOPLAMORM.Current.FirstOrDefault(x => x.CARIKARTID == AktiCariKart.ID & x.CARIKARTID != 0);
            var cari0 = CARITOPLAMTIPORM.Current.FirstOrDefault(x => x.CARIKARTID == AktiCariKart.ID & x.CARIKARTID != 0 & x.TP == 0);
            var cari1 = CARITOPLAMTIPORM.Current.FirstOrDefault(x => x.CARIKARTID == AktiCariKart.ID & x.CARIKARTID != 0 & x.TP == 1);
            var cari2 = CARITOPLAMTIPORM.Current.FirstOrDefault(x => x.CARIKARTID == AktiCariKart.ID & x.CARIKARTID != 0 & x.TP == 2);
            AktifCariToplam = cari;
            AktifCariToplamTP0 = cari0;
            AktifCariToplamTP1 = cari1;
            AktifCariToplamTP2 = cari2;
            return View(new Tuple<CARIKART, CARITOPLAM>(AktiCariKart, cari));

        }

    }
}
