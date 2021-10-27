using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AYAK.Common.NetCore;
using ERASiparis.Models;

namespace ERASiparis.Controllers
{
    [Authorize]
    public class FaturaController : Controller
    {
        public static string aktifFatName = "AktifFatura";
        public static string aktifFatAltName = "AktifFaturaAlt";
        public static string aktifYeniFatName = "AktifYeniFatura";
        public static string aktifYeniFatAltName = "AktifYeniFaturaAlt";

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
        public STOKUSTVIEW Master
        {
            get
            {
                if (Session.GetString(aktifFatName) != null)
                    return JsonConvert.DeserializeObject<STOKUSTVIEW>(Session.GetString(aktifFatName));
                else
                    return new STOKUSTVIEW();
            }
            set
            {
                Session.SetString(aktifFatName, JsonConvert.SerializeObject(value));
            }
        }//ve bunu kullanacaz
        public List<STOKALTVIEW> Detail
        {
            get
            {
                if (Session.GetString(aktifFatAltName) != null)
                    return JsonConvert.DeserializeObject<List<STOKALTVIEW>>(Session.GetString(aktifFatAltName));
                else
                    return new List<STOKALTVIEW>();
            }
            set
            {
                Session.SetString(aktifFatAltName, JsonConvert.SerializeObject(value));
            }
        }
        public STOKUST NewMaster
        {
            get
            {
                if (Session.GetString(aktifYeniFatName) == null)
                {
                    var spuy = new STOKUST();
                    spuy.STOK_FISTIPI = "25-Satış Faturası";
                    spuy.FATURA_FISTIPI = "25-Satış Faturası";
                    spuy.TIP = "F";
                    spuy.TP = 0;
                    spuy.DR = "K";
                    spuy.SUBEKODU = "000";
                    spuy.FATURA_ACIKLAMA = "Web Siparis";
                    spuy.SATICIKODU = AktifUser.SATICIKODU;
                    Session.SetString(aktifYeniFatName, JsonConvert.SerializeObject(spuy));
                }
                return JsonConvert.DeserializeObject<STOKUST>(Session.GetString(aktifYeniFatName));
            }
            set
            {
                Session.SetString(aktifYeniFatName, JsonConvert.SerializeObject(value));
            }
        }
        public List<STOKALTVIEW> NewDetail
        {
            get
            {
                if (Session.GetString(aktifYeniFatAltName) != null)
                    return JsonConvert.DeserializeObject<List<STOKALTVIEW>>(Session.GetString(aktifYeniFatAltName));
                else
                    return new List<STOKALTVIEW>();
            }
            set
            {
                Session.SetString(aktifYeniFatAltName, JsonConvert.SerializeObject(value));
            }
        }

        public List<STOKUSTVIEW> STOKUSTGetir(int? id)
        {
            if (AktifUser.ISCARI == false)
            {
                string STOKUSTwhere = id != null ? $" STOKUST.ID ={id} AND" : "";
                Result<List<STOKUSTVIEW>> siparis;

                siparis = Tools.Select<STOKUSTVIEW>($@"select top 100
                    CARIKART.FIRMAADI,
                    STOKUST.*             
                    from STOKUST  
                    left join CARIKART on STOKUST.CARIID =CARIKART.ID WHERE {STOKUSTwhere} STOKUST.DR='K'  order by STOKUST.ID desc");
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(new Result { Description = string.Format("Fatura getirme komutu" + siparis.Data.Count), Message = siparis.Message, State = siparis.State });
                Results = rslt;

                return siparis.Data;
            }
            else
            {
                string STOKUSTwhere = id != null ? $" STOKUST.ID ={id} AND" : "";
                Result<List<STOKUSTVIEW>> siparis;

                siparis = Tools.Select<STOKUSTVIEW>($@"select top 100
                    CARIKART.FIRMAADI,
                    STOKUST.*             
                    from STOKUST  
                    left join CARIKART on STOKUST.CARIID =CARIKART.ID WHERE {STOKUSTwhere} CARIID={AktifUser.ID} AND STOKUST.DR='K'  order by STOKUST.ID desc");
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(new Result { Description = string.Format("Fatura getirme komutu" + siparis.Data.Count), Message = siparis.Message, State = siparis.State });
                Results = rslt;

                return siparis.Data;
            }

        }
        public List<STOKALTVIEW> STOKALTGetir(int id)
        {
            var siparis = Tools.Select<STOKALTVIEW>($@" select 
                    STOKKARTI.KODU,
                    STOKKARTI.ADI,
                    STOKKARTI.ID AS URUNID,
                    STOKKARTI.BRM1 AS BIRIM, 
                    STOKKARTI.BRM1, 
                    STOKKARTI.BRM2,
                    STOKKARTI.BRM3,
                    STOKALT.*
                    from STOKALT  
                    left join STOKKARTI on STOKKARTI.ID = STOKALT.URUNID
                    WHERE STOKALT.STOKUST_ID={id} AND STOKALT.DR='K'");//Burada STOKUST_ID mi yoksa SIPID mi olacakk ????
            List<Result> rslt = Results ?? new List<Result>();
            rslt.Add(new Result { Description = string.Format("Fatura detayları getirme komutu" + siparis.Data.Count), Message = siparis.Message, State = siparis.State });
            Results = rslt;
            return siparis.Data;
        }
        void TutarAyarla(STOKALTVIEW s)
        {

            var dipIskonto1Y = s.DIPISKONTO1Y ?? 0;
            var miktar = s.MIKTAR ?? 0;
            var fiyat = s.FIYAT ?? 0;
            var isk1y = s.ISK1Y ?? 0;
            var isk2y = s.ISK2Y ?? 0;
            var kdvy = s.KDVY ?? 0;
            //var otvoran =s.OTVY;
            var fiyatkdahil = s.FIYATKDAHIL ?? 0;
            var tutarkdahil = s.TUTARKDAHIL ?? 0;
            if (true)//kdv dahil
            {
                fiyat = (fiyatkdahil / ((kdvy / 100) + 1));
            }
            //else
            //{
            //    fiyatkdahil = (fiyat * ((kdvy / 100) + 1));
            //}
            tutarkdahil = (fiyatkdahil * miktar);
            var tutar = (fiyat * miktar);
            decimal araToplam1 = 0, araToplam2 = 0, araToplam3 = 0;
            var iskt1 = (tutar * isk1y) / 100;
            araToplam1 = araToplam2 = araToplam3 = (tutar - iskt1);

            var iskt2 = (araToplam1 * isk2y) / 100;
            araToplam2 = araToplam3 = (araToplam1 - iskt2);

            var dipIskontoTutar = (araToplam3 * dipIskonto1Y) / 100;
            araToplam3 = tutar - (iskt1 + iskt2 + dipIskontoTutar);

            //var otvtutar = Math.Round(araToplam3 * otvoran / 100, 2);

            var kdvmatrah = araToplam3 /*+ otvtutar*/;
            var kdvtutar = Math.Round((kdvmatrah * kdvy) / 100, 2);
            var geneltoplam = Math.Round(kdvmatrah + kdvtutar, 2);

            //    geneltoplam = Math.Ceiling(geneltoplam);
            s.TUTAR = tutar;
            s.ARATOPLAM1 = araToplam1;
            s.ARATOPLAM2 = araToplam2;
            s.ARATOPLAM3 = araToplam3;
            s.ISK1TUTAR = iskt1;
            s.ISK2TUTAR = iskt2;
            s.KDVTUTAR = kdvtutar;
            //s.OTVYTUTAR= otvtutar;
            s.DIPISKONTOTUTAR = dipIskontoTutar;
            s.GENELTOPLAM = geneltoplam;

            s.TUTARKDAHIL = tutarkdahil;
            s.FIYATKDAHIL = fiyatkdahil;
            s.TUTAR = tutar;
            s.FIYAT = fiyat;
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




        public string STOKUSTGenelToplamVer(int id)
        {
            var STOKUST = STOKUSTGetir(id);
            var gt = STOKUST?.FirstOrDefault()?.GENELTOPLAM;
            return gt.HasValue ? gt.Value.ToString("N2") : "0,00";
        }
        private void STOKUSTGenelToplamGuncelle()
        {
            STOKUSTVIEW st = new STOKUSTVIEW();
            st = Master;
            st.GENELTOPLAM = Detail.Sum(x => x.GENELTOPLAM);
            st.SATIRISKONTOTOPLAM = Detail.Sum(x => x.SATIRISKONTOTOPLAM);
            Master = st;
            var ustRes = STOKUSTORM.Current.Update(Master);
            CARIHAR carr = CARIHARORM.Current.FirstOrDefault(x => x.FATURAID == Master.ID);
            carr.BORC = Master.GENELTOPLAM;
            var cariharup = CARIHARORM.Current.Update(carr);

        }


        public ActionResult Siparis()
        {
            //var STOKUST = STOKUSTGetir(null);
            //return View(STOKUST);
            var STOKUST = STOKUSTGetir(null);
            return View(STOKUST);
        }
        public ActionResult SiparisDetay(int id)
        {
            Master = STOKUSTGetir(id).FirstOrDefault();
            var cari = CARIKARTORM.Current.FirstOrDefault(x => x.ID == Master.CARIID);
            var siparis = STOKALTGetir(id);
            Detail = siparis;
            ViewBag.ID = id;
            ViewBag.cari = cari;
            return View(siparis);
        }
        public IActionResult HareketDetay(int id)
        {
            var siparis = STOKALTGetir(id);
            return View(siparis);
        }
        public PartialViewResult SiparisDetayUrunEkle(int id)
        {
            return PartialView();
        }
        public ActionResult SiparisFiyatDegistir(int urunid, string fiyat)
        {
            var da = Detail ?? new List<STOKALTVIEW>();
            var urun = da.FirstOrDefault(x => x.URUNID == urunid);
            if (urun != null)
            {
                fiyat = fiyat.Replace(".", ",");
                decimal f;
                if (decimal.TryParse(fiyat, out f))
                {
                    urun.FIYATKDAHIL = f;
                    TutarAyarla(urun);
                    var STOKALTRes = STOKALTORM.Current.Update(urun);
                    Detail = da;
                    STOKUSTGenelToplamGuncelle();
                }
                Detail = da;
            }
            return RedirectToAction("SiparisDetay", new { id = Master.ID });

        }
        public ActionResult SiparisMiktarDegistir(int urunid, string miktar)
        {
            var da = Detail ?? new List<STOKALTVIEW>();
            var urun = da.FirstOrDefault(x => x.URUNID == urunid);
            if (urun != null)
            {
                miktar = miktar.Replace(".", ",");
                decimal f;
                if (decimal.TryParse(miktar, out f))
                {
                    if (f <= 0)
                    {
                        urun.DR = "S";
                    }
                    urun.MIKTAR = f;
                    TutarAyarla(urun);
                    var miktarDegistirRes = STOKALTORM.Current.Update(urun);
                    Detail = da;
                    STOKUSTGenelToplamGuncelle();
                    var count = Detail.Count(x => x.MIKTAR > 0);
                    if (count == 0)
                    {
                        STOKUSTVIEW stm = new STOKUSTVIEW();
                        stm = Master;
                        stm.DR = "S";
                        Master = stm;
                        STOKUSTORM.Current.Update(Master);
                    }
                }
                Detail = da;
            }
            return RedirectToAction("SiparisDetay", new { id = Master.ID });

        }
        /// <summary>
        /// Seçilen Siparişe Yeni Ürün Atar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SipariseUrunEkle(int id)
        {
            var da = Detail ?? new List<STOKALTVIEW>();
            var s = STOKKARTIORM.Current.FirstOrDefault(x => x.ID == id);
            var l = da.FirstOrDefault(x => x.URUNID == id);

            if (s != null)
            {
                if (l != null)
                {
                    l.MIKTAR++;
                    l.TUTAR = l.FIYAT * l.MIKTAR;
                    TutarAyarla(l);
                    //db de update yapmak lazım
                    var altUpRes = STOKALTORM.Current.Update(l);
                    List<Result> rslt = Results ?? new List<Result>();
                    rslt.Add(altUpRes);
                    Results = rslt;
                    //Results.Add(altUpRes);
                }
                else
                {

                    var yeni = (new STOKALTVIEW
                    {
                        ID = SIRAORM.Current.SiraVer(),
                        STOKUST_ID = Master.ID,
                        KODU = s.KODU,
                        ADI = s.ADI,
                        URUNID = s.ID,
                        BRM1 = s.BRM1,
                        BRM2 = s.BRM2,
                        BRM3 = s.BRM3,
                        BIRIM = s.BRM1,
                        MIKTAR = 1,
                        FIYAT = s.B1FIYAT_ST1,
                        FIYATKDAHIL = s.B1FIYAT_ST1,
                        TUTAR = s.B1FIYAT_ST1,
                        GENELTOPLAM = s.B1FIYAT_ST1,
                        KDVMATRAH = s.B1FIYAT_ST1,
                        CARIID = AktiCariKart.ID,
                        DR = "K",
                        TP = 0,
                        GC = "C",
                        DEPOCIKIS = "000",
                        ISLEMTIPI = "F",
                        SUBEKODU = "000",
                        IRSALIYEUST_ID = 0,
                        ISK1Y = 0,
                        ISK2Y = 0,
                        KDVY = 0,
                        DIPISKONTO1Y = 0,
                        OTVYTUTAR = 0,
                        SATIRISKONTOTOPLAM = 0,
                        PLAKA = "",
                        FATURAUST_ID = Master.ID
                    });
                    TutarAyarla(yeni);

                    var altInsRes = STOKALTORM.Current.Insert(yeni);
                    List<Result> rslt = Results ?? new List<Result>();
                    rslt.Add(altInsRes);
                    Results = rslt;
                    //Results.Add(altInsRes);
                    da.Add(yeni);

                }
                Detail = da;
                STOKUSTGenelToplamGuncelle();

            }


            return RedirectToAction("SiparisDetay", new { id = Master.ID });
        }
        public ActionResult SiparisUrunSil(int id)
        {
            var da = Detail ?? new List<STOKALTVIEW>();
            var u = da.FirstOrDefault(x => x.ID == id);
            if (u != null)
            {
                //var upres = STOKALTORM.Current.UpdatePart("S".CreateParameters("DR"), id);
                var delres = STOKALTORM.Current.Delete(u);
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(delres);
                Results = rslt;
                //Results.Add(delres);
                da.Remove(u);

            }
            Detail = da;
            STOKUSTGenelToplamGuncelle();
            if (!Detail.Any())
            {
                var ustUpdRes = STOKUSTORM.Current.UpdatePart("S".CreateParameters("DR"), id);
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(ustUpdRes);
                Results = rslt;
                //Results.Add(ustUpdRes);

            }
            return RedirectToAction("SiparisDetay", new { id = Master.ID });

        }

        public ActionResult StokKarti()
        {
            var stok = STOKKARTIORM.Current.Select();
            return PartialView(stok.Data);
        }
        public PartialViewResult UrunArama(string aranan)
        {
            var prm = aranan.CreateParameters("@p");
            var stok = STOKKARTIORM.Current.Select("Select TOP 20 * from STOKKARTI WHERE (ADI like '%'+@p+'%' OR ACIKLAMA like '%'+@p+'%' OR SUBEKODU like '%'+@p+'%' OR BRM1BARKOD like '%'+@p+'%' OR BRM2BARKOD like '%'+@p+'%' OR BRM3BARKOD like '%'+@p+'%') AND DR='K'", prm, SelectType.Text);//SUBEKODU yazan yer kodu mu şube kodu mu
            if (stok.Data != null)
            {
                foreach (var item in stok.Data)
                {
                    var s = StokToplamORM.Current.FirstOrDefault(x => x.TURUNID == item.ID);
                    if (s != null)
                    {
                        item.STOK = s.MIKTAR;
                    }
                    else
                    {
                        item.STOK = 0;
                    }
                }
                return PartialView(stok.Data);
            }
            return PartialView();

        }


        public ActionResult YeniSiparis()
        {
            var sipozel1 = OZELORM.Current.Where(x => x.YER == "FATURAOZEL1");
            var sipozel2 = OZELORM.Current.Where(x => x.YER == "FATURAOZEL2");
            var sipozel3 = OZELORM.Current.Where(x => x.YER == "FATURAOZEL3");
            Tuple<List<OZEL>, List<OZEL>, List<OZEL>> ozeller = new Tuple<List<OZEL>, List<OZEL>, List<OZEL>>(sipozel1.Data, sipozel2.Data, sipozel3.Data);
            return View(ozeller);
        }
        public ActionResult YeniSiparisList(string Hata)////bu kısma KDV ile ilgili olan ayarları yapacağız 
        {
            NewDetail = NewDetail ?? new List<STOKALTVIEW>();
            decimal toplam = NewDetail.Sum(x => x.GENELTOPLAM ?? 0);
            decimal aratoplam = toplam / 1.18m;
            decimal kdv = aratoplam * 0.18m;
            List<Tuple<string, string>> toplamlar = new List<Tuple<string, string>>();
            // toplamlar.Add(new Tuple<string, string>("ARA TOPLAM", aratoplam.ToString("N2")));
            // toplamlar.Add(new Tuple<string, string>("KDV", kdv.ToString("N2")));
            toplamlar.Add(new Tuple<string, string>("TOPLAM", toplam.ToString("N2")));

            ViewBag.Toplamlar = toplamlar;
            ViewBag.Kullanici = AktiCariKart;
            ViewBag.Hata = Hata;

            if (AktifUser.KARTTIPI.StartsWith("1"))////Buraya Bir bakkkk
                return View(NewDetail);
            else
                return View("YeniSiparisListPersonel", NewDetail);

        }
        public ActionResult YeniSiparisFiyatDegistir(int urunid, string fiyat)
        {
            var nda = NewDetail ?? new List<STOKALTVIEW>();
            var urun = nda.FirstOrDefault(x => x.URUNID == urunid);
            if (urun != null)
            {
                fiyat = fiyat.Replace(".", ",");
                decimal f;
                if (decimal.TryParse(fiyat, out f))
                {
                    urun.FIYATKDAHIL = f;
                    TutarAyarla(urun);
                }
                NewDetail = nda;
            }
            return RedirectToAction("YeniSiparisList");
        }
        public ActionResult YeniSiparisMiktarDegistir(int urunid, string miktar)
        {
            var nda = NewDetail ?? new List<STOKALTVIEW>();
            var urun = nda.FirstOrDefault(x => x.URUNID == urunid);
            if (urun != null)
            {
                miktar = miktar.Replace(".", ",");
                decimal f;
                if (decimal.TryParse(miktar, out f))
                {
                    urun.MIKTAR = f;
                    TutarAyarla(urun);
                }
                NewDetail = nda;
            }
            return RedirectToAction("YeniSiparisList");
        }
        public ActionResult YeniSipariseUrunEkle(int id, string Fiyat)
        {
            decimal fiyat = Fiyat.Replace(".", ",").ToDecimal();
            var nda = NewDetail ?? new List<STOKALTVIEW>();

            var s = STOKKARTIORM.Current.FirstOrDefault(x => x.ID == id);
            var l = nda.FirstOrDefault(x => x.URUNID == id);

            if (s != null)
            {
                if (l != null)
                {
                    l.MIKTAR++;
                    l.TUTAR = l.FIYAT * l.MIKTAR;
                    TutarAyarla(l);
                }
                else
                {
                    var yeni = (new STOKALTVIEW
                    {
                        KODU = s.KODU,
                        ADI = s.ADI,
                        URUNID = s.ID,
                        BRM1 = s.BRM1,
                        BRM2 = s.BRM2,
                        BRM3 = s.BRM3,
                        BIRIM = s.BRM1,
                        MIKTAR = 1,
                        FIYAT = fiyat,
                        FIYATKDAHIL = fiyat,
                        TUTAR = fiyat,
                        KDVMATRAH = fiyat,
                        GENELTOPLAM = fiyat,
                        DR = "K"
                    });
                    TutarAyarla(yeni);
                    nda.Add(yeni);
                }
                NewDetail = nda;
            }
            return RedirectToAction("YeniSiparisList");
        }
        public ActionResult YeniSiparisTemizle()
        {
            NewDetail = null;
            return RedirectToAction("YeniSiparisList");

        }
        public ActionResult YeniSiparisUrunSil(int id)
        {
            var nda = NewDetail ?? new List<STOKALTVIEW>();
            var u = nda.FirstOrDefault(x => x.URUNID == id);
            if (u != null)
            {
                nda.Remove(u);
            }
            NewDetail = nda;
            return RedirectToAction("YeniSiparisList");
        }
        public ActionResult YeniSiparisKaydet()
        {
            if (NewDetail.Count != 0)
            {

                CARIHAR car = new CARIHAR();
                STOKUST stu = new STOKUST();
                if (NewMaster != null)
                    stu = NewMaster;
                if (stu.TP == null)
                    stu.TP = 0;


                stu.ID = SIRAORM.Current.SiraVer();
                stu.STOK_FISNO = FISNOORM.Current.FisnoVer("FATURA");
                stu.FATURA_FISNO = stu.STOK_FISNO;
                stu.IRSALIYE_ID = 0;
                stu.FATURA_TARIH = DateTime.Now;
                stu.STOK_TARIH = DateTime.Now;
                stu.VADE = DateTime.Now;
                stu.CARIID = AktiCariKart.ID;
                stu.T_ADI = AktiCariKart.ADI;
                stu.T_SOYADI = AktiCariKart.SOYADI;
                stu.T_ADRES = AktiCariKart.ADRES;
                stu.T_EMAIL = AktiCariKart.EMAIL;
                stu.T_FIRMAADI = AktiCariKart.FIRMAADI;
                stu.T_TELNO = AktiCariKart.TEL1;
                stu.FATURA_ID = stu.ID;
                stu.GENELTOPLAM = NewDetail.Sum(x => x.GENELTOPLAM);
                stu.SATIRISKONTOTOPLAM = NewDetail.Sum(x => x.SATIRISKONTOTOPLAM);
                stu.FATURA_ID = stu.ID;
                stu.FATURA_CARIISLE = true;
                stu.DEPOCIKIS = "000";
                stu.SUBE = 0;
                stu.FATURANO = "";
                stu.FATBILGIID = 0;
                stu.KDV1Y = 0;
                stu.KDV2Y = 0;
                stu.KDV3Y = 0;
                stu.KDV4Y = 0;
                stu.KDV5Y = 0;
                stu.KDV1MATRAH = 0;
                stu.KDV2MATRAH = 0;
                stu.KDV3MATRAH = 0;
                stu.KDV4MATRAH = 0;
                stu.KDV5MATRAH = 0;
                stu.DIPISKY = 0;
                stu.DIPISKY1 = 0;
                stu.DIPISKY2 = 0;
                stu.DIPISKY3 = 0;
                stu.DIPISKY4 = 0;
                stu.DIPISKONTOTUTAR = 0;
                stu.DIPISKONTOTUTAR1 = 0;
                stu.DIPISKONTOTUTAR2 = 0;
                stu.DIPISKONTOTUTAR3 = 0;
                stu.DIPISKONTOTUTAR4 = 0;
                stu.TEDARIKSURESI = 0;
                stu.ONAYVEREN = 0;
                stu.KDVDAHIL = false;
                stu.DIPISKONTOSABIT = 0;
                stu.DEPOGIRIS = "000";
                stu.DEPOCIKIS = "000";
                stu.BAGLANTI_ID = 0;
                stu.TEKIFATORAN1 = 0;
                stu.TEKIFATORAN2 = 0;
                stu.TEKIFATTUTAR = 0;
                stu.OIVY = 0;
                stu.OIVYTUTAR = 0;
                stu.ISKONTOTOPLAM = 0;
                stu.YETKI = 0;
                stu.MUSTERIID = 0;
                stu.OTVTUTAR = 0;
                stu.FATURASAAT = DateTime.Now.TimeOfDay.ToString();
                stu.FATURAYAZSAY = 0;
                stu.STOK_FISTIPI = "25-Satış Faturası";
                stu.FATURA_FISTIPI = "25-Satış Faturası";
                stu.TIP = "F";
                stu.DR = "K";
                stu.SUBEKODU = "000";
                stu.FATURA_ACIKLAMA = "Web Siparis";
                NewMaster = stu;
                //CARIHAR tanımla
                car.SUBEKODU = "000";
                car.ID = NewMaster.ID;
                car.CARIKARTID = AktiCariKart.ID;
                car.FISNO = FISNOORM.Current.FisnoVer("CARIHAR");
                car.FISTIPI = "11-Borc Dekontu";
                car.TARIH = DateTime.Now;
                car.VADE = DateTime.Now;
                car.ACIKLAMA = "25-Satış Faturası " + stu.FATURA_FISNO + " ile (Web İslemi)";//burda Fatura Bedelini de ekliyor 
                car.ACIKLAMA1 = "";
                car.BORC = stu.GENELTOPLAM;
                car.ALACAK = 0;
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
                car.DR = "K";
                car.KAYITYERI = "FATURA";
                car.YETKI = 0;
                car.TP = 0;
                car.KOMISYONTUTARI = 0;
                car.PLAKA = "";
                car.FATURAID = stu.ID;//bunu bir sormak lazım  0 mı diye
                var sonuc = CARIHARORM.Current.Insert(car);
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(sonuc);
                //Results.Add(sonuc);
                var spuyRes = STOKUSTORM.Current.Insert(NewMaster);
                rslt.Add(spuyRes);

                //Results.Add(spuyRes);
                if (spuyRes.State == ResultState.Success)
                {
                    foreach (var item in NewDetail)
                    {
                        item.ID = SIRAORM.Current.SiraVer();
                        item.TP = 0;
                        item.GC = "C";
                        item.DEPOCIKIS = "000";
                        item.ISLEMTIPI = "F";
                        item.SUBEKODU = "000";
                        item.IRSALIYEUST_ID = 0;
                        item.ISK1Y = 0;
                        item.ISK2Y = 0;
                        item.KDVY = 0;
                        item.DIPISKONTO1Y = 0;
                        item.OTVYTUTAR = 0;
                        item.SATIRISKONTOTOPLAM = 0;
                        item.PLAKA = "";
                        item.FATURAUST_ID = NewMaster.ID;
                        item.STOKUST_ID = NewMaster.ID;
                        item.CARIID = AktiCariKart.ID;
                        item.FATURAUST_ID = NewMaster.ID;
                        var altres = STOKALTORM.Current.Insert(item);
                        rslt.Add(altres);
                        //Results.Add(altres);
                    }

                    NewDetail = new List<STOKALTVIEW>();
                    NewMaster = null;//null ise yenisi oluşturuluyordu
                }
                Results = rslt;
                return RedirectToAction("YeniSiparisList",new { Hata= "Siparisiniz oluşturulmuştur.." });
            }
            else
            {
                return RedirectToAction("YeniSiparisList",new { Hata= "lütfen bir ürün giriniz.." });
            }
            
        }
        public ActionResult YeniSiparisUrunAzalt(int id)
        {
            var nda = NewDetail ?? new List<STOKALTVIEW>();

            var s = STOKKARTIORM.Current.FirstOrDefault(x => x.ID == id);
            var l = nda.FirstOrDefault(x => x.URUNID == id);
            if (l != null)
            {
                if (l.MIKTAR != 0)
                {
                    l.MIKTAR--;
                    l.TUTAR -= l.FIYAT * (l.MIKTAR - 1);
                    TutarAyarla(l);
                }

                if (l.MIKTAR <= 0)
                {
                    nda.Remove(l);
                }
                NewDetail = nda;
            }
            return RedirectToAction("YeniSiparisList");
        }
        public string OzelSec(int num, string kod)
        {
            var nm = NewMaster ?? new STOKUST();
            switch (num)
            {
                case 1:
                    nm.FATURA_OZEL1 = kod;
                    NewMaster = nm;
                    break;
                case 2:
                    nm.FATURA_OZEL2 = kod;
                    NewMaster = nm;
                    break;
                case 3:
                    nm.FATURA_OZEL3 = kod;
                    NewMaster = nm;
                    break;
            }
            return kod;
        }
        public string TPEkle(int id)
        {
            var nm = NewMaster ?? new STOKUST();
            nm.TP = id;
            NewMaster = nm;
            string reslt = "Tip " + id.ToString();
            return reslt;
        }
    }
    public class FaturaViewComponent : ViewComponent
    {
        ISession Session => HttpContext.Session;
        public static string aktifYeniFatAltName = "AktifYeniFaturaAlt";
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
        public List<STOKALTVIEW> NewDetail
        {
            get
            {
                if (Session.GetString(aktifYeniFatAltName) != null)
                    return JsonConvert.DeserializeObject<List<STOKALTVIEW>>(Session.GetString(aktifYeniFatAltName));
                else
                    return new List<STOKALTVIEW>();
            }
            set
            {
                Session.SetString(aktifYeniFatAltName, JsonConvert.SerializeObject(value));
            }
        }


        public IViewComponentResult Invoke()
        {
            NewDetail = NewDetail ?? new List<STOKALTVIEW>();
            decimal toplam = NewDetail.Sum(x => x.GENELTOPLAM ?? 0);
            decimal aratoplam = toplam / 1.18m;
            decimal kdv = aratoplam * 0.18m;
            List<Tuple<string, string>> toplamlar = new List<Tuple<string, string>>();
            toplamlar.Add(new Tuple<string, string>("TOPLAM", toplam.ToString("N2")));

            ViewBag.Toplamlar = toplamlar;
            ViewBag.Kullanici = AktiCariKart;
            return View(NewDetail);


        }
    }
}
