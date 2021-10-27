using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AYAK.Common.NetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ERASiparis.Models;

namespace ERASiparis.Controllers
{
    [Authorize]
    public class SiparisController : Controller
    {

        ISession Session => HttpContext.Session;

        //HttpContext.Session.GetString("",2);
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
        public SIPUSTVIEW SipUst
        {
            get
            {
                if (Session.GetString("AktifSiparis") != null)
                    return JsonConvert.DeserializeObject<SIPUSTVIEW>(Session.GetString("AktifSiparis"));
                else
                    return new SIPUSTVIEW();
            }
            set
            {
                Session.SetString("AktifSiparis", JsonConvert.SerializeObject(value));
            }
        }//ve bunu kullanacaz
        public List<SIPARISVIEW> SipAlt
        {
            get
            {
                if (Session.GetString("AktifSiparisDetay") != null)
                    return JsonConvert.DeserializeObject<List<SIPARISVIEW>>(Session.GetString("AktifSiparisDetay"));
                else
                    return new List<SIPARISVIEW>();
            }
            set
            {
                Session.SetString("AktifSiparisDetay", JsonConvert.SerializeObject(value));
            }
        }
        public List<SIPARISVIEW> YeniSiparisAlt
        {
            get
            {
                if (Session.GetString("YeniSiparisDetay") != null)
                    return JsonConvert.DeserializeObject<List<SIPARISVIEW>>(Session.GetString("YeniSiparisDetay"));
                else
                    return new List<SIPARISVIEW>();
            }
            set
            {
                Session.SetString("YeniSiparisDetay", JsonConvert.SerializeObject(value));
            }
        }
        public SIPUST YeniSiparisUst
        {
            get
            {
                if (Session.GetString("YeniSiparisUst") == null)
                {
                    var spuy = new SIPUST();
                    spuy.FISTIPI = "11-Alınan Sipariş";
                    spuy.DR = "K";
                    spuy.ACIKLAMA = "Web Siparis";
                    spuy.WEBSIPARIS = true;
                    spuy.GORULDU = false;
                    Session.SetString("YeniSiparisUst", JsonConvert.SerializeObject(spuy));
                }
                return JsonConvert.DeserializeObject<SIPUST>(Session.GetString("YeniSiparisUst"));
            }
            set
            {
                Session.SetString("YeniSiparisUst", JsonConvert.SerializeObject(value));
            }
        }
        public List<SIPARISVIEW> SiparisDetayAlt
        {
            get
            {
                if (Session.GetString("SiparisDetay") != null)
                    return JsonConvert.DeserializeObject<List<SIPARISVIEW>>(Session.GetString("SiparisDetay"));
                else
                    return new List<SIPARISVIEW>();
            }
            set
            {
                Session.SetString("SiparisDetay", JsonConvert.SerializeObject(value));
            }
        }

        public List<SIPUSTVIEW> sipUstGetir(int? id)
        {
            if (AktifUser.ISCARI == false)
            {
                string sipustwhere = id != null ? $" SIPUST.ID ={id} AND" : "";
                Result<List<SIPUSTVIEW>> siparis;

                siparis = Tools.Select<SIPUSTVIEW>($@"select top 100
                    CARIKART.FIRMAADI,
                    SIPUST.*             
                    from SIPUST  
                    left join CARIKART on SIPUST.CARIID =CARIKART.ID WHERE {sipustwhere} SIPUST.DR='K'  order by SIPUST.ID desc");
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(new Result { Description = string.Format("Siparisleri getirme komutu" + siparis.Data.Count), Message = siparis.Message, State = siparis.State });
                Results = rslt;
                //Results.Add(new Result { Description = string.Format("Siparisleri getirme komutu" + siparis.Data.Count), Message = siparis.Message, State = siparis.State });
                return siparis.Data;
            }
            else
            {
                string sipustwhere = id != null ? $" SIPUST.ID ={id} AND" : "";
                Result<List<SIPUSTVIEW>> siparis;

                siparis = Tools.Select<SIPUSTVIEW>($@"select top 100
                    CARIKART.FIRMAADI,
                    SIPUST.*             
                    from SIPUST  
                    left join CARIKART on SIPUST.CARIID =CARIKART.ID WHERE {sipustwhere} CARIID={AktifUser.ID} AND SIPUST.DR='K'  order by SIPUST.ID desc");
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(new Result { Description = string.Format("Siparisleri getirme komutu" + siparis.Data.Count), Message = siparis.Message, State = siparis.State });
                Results = rslt;
                //Results.Add(new Result { Description = string.Format("Siparisleri getirme komutu" + siparis.Data.Count), Message = siparis.Message, State = siparis.State });
                return siparis.Data;
            }

            //return new List<SIPUSTVIEW>();
        }
        public List<SIPARISVIEW> sipAltGetir(int id)
        {
            var siparis = Tools.Select<SIPARISVIEW>($@" select 
                    STOKKARTI.KODU,
                    STOKKARTI.ADI,
                    STOKKARTI.ID AS URUNID,
                    STOKKARTI.BRM1 AS BIRIM, 
                    STOKKARTI.BRM1, 
                    STOKKARTI.BRM2,
                    STOKKARTI.BRM3,
                    SIPALT.*
                    from SIPALT  
                    left join STOKKARTI on STOKKARTI.ID = SIPALT.URUNID
                    WHERE SIPALT.SIPID={id} AND SIPALT.DR='K'");
            List<Result> rslt = Results ?? new List<Result>();
            rslt.Add(new Result { Description = string.Format("Siparis detayları getirme komutu" + siparis.Data.Count), Message = siparis.Message, State = siparis.State });
            Results = rslt;
            //Results.Add(new Result { Description = string.Format("Siparis detayları getirme komutu" + siparis.Data.Count), Message = siparis.Message, State = siparis.State });
            return siparis.Data;
        }
        void TutarAyarla(SIPARISVIEW s)
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
            //burda miktarnı değiştirdiğimiz nesneyi güncelleyeceğiz..
        }
        public string SipUstGenelToplamVer(int id)
        {
            var sipust = sipUstGetir(id);
            var gt = sipust?.FirstOrDefault()?.GENELTOPLAM;
            return gt.HasValue ? gt.Value.ToString("N2") : "0,00";
        }
        private void SipUstGenelToplamGuncelle()//burası kaydediyor mu kontrol ett 
        {
            SIPUSTVIEW sp = new SIPUSTVIEW();
            sp = SipUst;
            sp.GENELTOPLAM = SipAlt.Sum(x => x.GENELTOPLAM);
            sp.SATIRISKONTOTOPLAM = SipAlt.Sum(x => x.SATIRISKONTOTOPLAM);
            SipUst = sp;
            var ustRes = SIPUSTORM.Current.Update(SipUst);
            List<Result> rslt = Results ?? new List<Result>();
            rslt.Add(ustRes);
            Results = rslt;
        }



        public ActionResult Siparis()
        {
            //var sipust = sipUstGetir(null);
            //return View(sipust);
            //if (AktiCariKart.KARTTIPI == "1-Müşteri")
            //{
            //    var sipust1 = sipUstGetir(AktiCariKart.ID);
            //    return View(sipust1);
            //}
            var sipust = sipUstGetir(null);
            return View(sipust);
        }
        public ActionResult SiparisDetay(int id)
        {
            SipUst = sipUstGetir(id).FirstOrDefault();
            var cari = CARIKARTORM.Current.FirstOrDefault(x => x.ID == SipUst.CARIID);
            var siparis = sipAltGetir(id);
            SipAlt = siparis;
            ViewBag.ID = id;
            ViewBag.cari = cari;
            return View(siparis);
        }
        public PartialViewResult SiparisDetayUrunEkle(int id)
        {
            return PartialView();
        }
        public ActionResult SiparisFiyatDegistir(int urunid, string fiyat)
        {
            var sa = SipAlt ?? new List<SIPARISVIEW>();
            var urun = sa.FirstOrDefault(x => x.URUNID == urunid);
            if (urun != null)
            {
                fiyat = fiyat.Replace(".", ",");
                decimal f;
                if (decimal.TryParse(fiyat, out f))
                {
                    urun.FIYATKDAHIL = f;
                    TutarAyarla(urun);
                    var sipaltRes = SIPALTORM.Current.Update(urun);
                    List<Result> rslt = Results ?? new List<Result>();
                    rslt.Add(sipaltRes);
                    Results = rslt;
                    SipAlt = sa;
                    SipUstGenelToplamGuncelle();
                }
                SipAlt = sa;
            }
            return RedirectToAction("SiparisDetay", new { id = SipUst.ID });

        }
        public ActionResult SiparisMiktarDegistir(int urunid, string miktar)
        {
            var sa = SipAlt ?? new List<SIPARISVIEW>();

            List<Result> rslt = Results ?? new List<Result>();


            var urun = sa.FirstOrDefault(x => x.URUNID == urunid);
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
                    var miktarDegistirRes = SIPALTORM.Current.Update(urun);
                    rslt.Add(miktarDegistirRes);
                    SipAlt = sa;
                    SipUstGenelToplamGuncelle();
                    var count = SipAlt.Count(x => x.MIKTAR > 0);//Kontrol yerleri baslangıc
                    if (count == 0)
                    {
                        SIPUSTVIEW slm = new SIPUSTVIEW();
                        slm = SipUst;
                        slm.DR = "S";
                        SipUst = slm;
                        var miktarsilresult = SIPUSTORM.Current.Update(SipUst);
                        rslt.Add(miktarsilresult);
                    }
                    //kontrol yeri bitis
                }
                SipAlt = sa;
                Results = rslt;
            }
            return RedirectToAction("SiparisDetay", new { id = SipUst.ID });

        }
        /// <summary>
        /// Seçilen Siparişe Yeni Ürün Atar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SipariseUrunEkle(int id)
        {
            var sa = SipAlt ?? new List<SIPARISVIEW>();
            List<Result> rslt = Results ?? new List<Result>();


            var s = STOKKARTIORM.Current.FirstOrDefault(x => x.ID == id);
            var l = sa.FirstOrDefault(x => x.URUNID == id);

            if (s != null)
            {
                if (l != null)
                {
                    l.MIKTAR++;
                    l.TUTAR = l.FIYAT * l.MIKTAR;
                    TutarAyarla(l);
                    //db de update yapmak lazım
                    var altUpRes = SIPALTORM.Current.Update(l);
                    rslt.Add(altUpRes);
                    //Results.Add(altUpRes);
                }
                else
                {

                    var yeni = (new SIPARISVIEW
                    {
                        ID = SIRAORM.Current.SiraVer(),
                        SUBEKODU = "000",
                        SIPID = SipUst.ID,
                        URUNID = s.ID,
                        KODU = s.KODU,
                        ADI = s.ADI,
                        BRM1 = s.BRM1,
                        BRM2 = s.BRM2,
                        BRM3 = s.BRM3,
                        BIRIM = s.BRM1,
                        MIKTAR = 1,
                        FIYAT = s.B1FIYAT_ST1,
                        FIYATKDAHIL = s.B1FIYAT_ST1,
                        TUTAR = s.B1FIYAT_ST1,
                        ISK1Y = 0,
                        ISK2Y = 0,
                        KDVY = 0,
                        DEPOGIRIS = "000",
                        TESLIMATTARIHI = DateTime.Now,
                        GENELTOPLAM = s.B1FIYAT_ST1,
                        CARIID = AktiCariKart.ID,
                        DR = "K",
                        TP = 0,
                        PLAKA = ""

                    });
                    TutarAyarla(yeni);

                    var altInsRes = SIPALTORM.Current.Insert(yeni);
                    rslt.Add(altInsRes);
                    sa.Add(yeni);
                    //Results.Add(altInsRes);
                }
                SipAlt = sa;
                Results = rslt;
                SipUstGenelToplamGuncelle();

            }


            return RedirectToAction("SiparisDetay", new { id = SipUst.ID });
        }
        public ActionResult SiparisUrunSil(int id)
        {
            var sa = SipAlt ?? new List<SIPARISVIEW>();
            var u = sa.FirstOrDefault(x => x.ID == id);
            if (u != null)
            {
                var upres = SIPALTORM.Current.UpdatePart("S".CreateParameters("DR"), id);
                sa.Remove(u);
                List<Result> rslt = Results ?? new List<Result>();
                rslt.Add(upres);
                Results = rslt;
                //Results.Add(upres);
            }
            SipAlt = sa;
            SipUstGenelToplamGuncelle();

            return RedirectToAction("SiparisDetay", new { id = SipUst.ID });

        }

        public ActionResult StokKarti()
        {
            var stok = STOKKARTIORM.Current.Select();
            List<Result> rslt = Results ?? new List<Result>();
            rslt.Add(new Result { Description = string.Format("StokKarti getirme komutu" + stok.Data.Count), Message = stok.Message, State = stok.State });
            Results = rslt;
            //Results.Add(new Result { Description = string.Format("StokKarti getirme komutu" + stok.Data.Count), Message = stok.Message, State = stok.State });
            return PartialView(stok.Data);
        }
        public PartialViewResult UrunArama(string aranan)
        {
            var prm = aranan.CreateParameters("@p");//stok null ise 

            var stok = STOKKARTIORM.Current.Select("Select TOP 20 * from STOKKARTI WHERE (ADI like '%'+@p+'%' OR ACIKLAMA like '%'+@p+'%' OR KODU like '%'+@p+'%' OR BRM1BARKOD like '%'+@p+'%' OR BRM2BARKOD like '%'+@p+'%' OR BRM3BARKOD like '%'+@p+'%' ) AND DR='K'", prm, SelectType.Text);
            //Results.Add(new Result { Description = string.Format("Aramada Urun Getirme" + stok.Data.Count), Message = stok.Message, State = stok.State });
            if (stok.Data != null)
            {
                foreach (var item in stok.Data)
                {
                    var s = StokToplamORM.Current.FirstOrDefault(x => x.TURUNID == item.ID);

                    if (s != null)
                    {
                        item.STOK = s.MIKTAR;
                        //Results.Add(new Result { Description = string.Format("Stok Toplam Hesaplama: " + item.STOK) });
                    }
                    else
                    {
                        item.STOK = 0;
                        //Results.Add(new Result { Description = string.Format("Stok Toplam Hesaplama: Null (0) ") });
                    }

                }
                return PartialView(stok.Data);

            }
            return PartialView();
        }



        public ActionResult YeniSiparis()
        {
            var sipozel1 = OZELORM.Current.Where(x => x.YER == "SIPARISOZEL1");
            //Results.Add(new Result { Description = string.Format("Siparis özel1 : " + sipozel1.Data.Count), Message = sipozel1.Message, State = sipozel1.State });
            var sipozel2 = OZELORM.Current.Where(x => x.YER == "SIPARISOZEL2");
            //Results.Add(new Result { Description = string.Format("Siparis özel2 : " + sipozel2.Data.Count), Message = sipozel2.Message, State = sipozel2.State });
            var sipozel3 = OZELORM.Current.Where(x => x.YER == "SIPARISOZEL3");
            //Results.Add(new Result { Description = string.Format("Siparis özel3 : " + sipozel3.Data.Count), Message = sipozel3.Message, State = sipozel3.State });
            Tuple<List<OZEL>, List<OZEL>, List<OZEL>> ozeller = new Tuple<List<OZEL>, List<OZEL>, List<OZEL>>(sipozel1.Data, sipozel2.Data, sipozel3.Data);
            return View(ozeller);
        }
        public ActionResult YeniSiparisList(string Hata)
        {
            YeniSiparisAlt = YeniSiparisAlt ?? new List<SIPARISVIEW>();
            decimal toplam = YeniSiparisAlt.Sum(x => x.GENELTOPLAM ?? 0);
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
                return View(YeniSiparisAlt);
            else
                return View("YeniSiparisListPersonel", YeniSiparisAlt);

        }
        public ActionResult YeniSiparisFiyatDegistir(int urunid, string fiyat)
        {
            var y = YeniSiparisAlt ?? new List<SIPARISVIEW>();
            var urun = y.FirstOrDefault(x => x.URUNID == urunid);
            if (urun != null)
            {
                fiyat = fiyat.Replace(".", ",");
                decimal f;
                if (decimal.TryParse(fiyat, out f))
                {
                    urun.FIYATKDAHIL = f;
                    TutarAyarla(urun);
                }
                YeniSiparisAlt = y;
            }
            return RedirectToAction("YeniSiparisList");
        }
        public ActionResult YeniSiparisMiktarDegistir(int urunid, string miktar)
        {
            var y = YeniSiparisAlt ?? new List<SIPARISVIEW>();
            var urun = y.FirstOrDefault(x => x.URUNID == urunid);
            if (urun != null)
            {
                miktar = miktar.Replace(".", ",");
                decimal f;
                if (decimal.TryParse(miktar, out f))
                {
                    urun.MIKTAR = f;
                    TutarAyarla(urun);
                }
                YeniSiparisAlt = y;
            }
            return RedirectToAction("YeniSiparisList");
        }
        public ActionResult YeniSipariseUrunEkle(int id)
        {
            var y = YeniSiparisAlt ?? new List<SIPARISVIEW>();

            var s = STOKKARTIORM.Current.FirstOrDefault(x => x.ID == id);
            var l = y.FirstOrDefault(x => x.URUNID == id);

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
                    var yeni = (new SIPARISVIEW
                    {
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
                        DR = "K"
                    });
                    TutarAyarla(yeni);
                    y.Add(yeni);
                }
                YeniSiparisAlt = y;
            }
            return RedirectToAction("YeniSiparisList");
        }
        public ActionResult YeniSiparisTemizle()
        {
            YeniSiparisAlt = null;
            return RedirectToAction("YeniSiparisList");

        }
        public ActionResult YeniSiparisUrunSil(int id)
        {
            var y = YeniSiparisAlt ?? new List<SIPARISVIEW>();
            var u = y.FirstOrDefault(x => x.URUNID == id);
            if (u != null)
            {
                y.Remove(u);
            }
            YeniSiparisAlt = y;
            return RedirectToAction("YeniSiparisList");
        }
        public ActionResult YeniSiparisKaydet()
        {
            if (YeniSiparisAlt.Count > 0)
            {
                CARIHAR car = new CARIHAR();
                SIPUST sp = new SIPUST();
                List<Result> rslt = Results ?? new List<Result>();

                if (YeniSiparisUst != null)
                    sp = YeniSiparisUst;
                if (sp.TP == null)
                    sp.TP = 0;
                sp.TARIH = DateTime.Now;
                sp.ID = SIRAORM.Current.SiraVer();
                sp.SUBEKODU = "000";
                sp.FISNO = FISNOORM.Current.FisnoVer("SIPARIS");
                sp.CARIID = AktiCariKart.ID;
                sp.KDV1Y = 0;
                sp.KDV2Y = 0;
                sp.KDV3Y = 0;
                sp.KDV4Y = 0;
                sp.KDV5Y = 0;
                sp.KDVTUTAR1 = 0;
                sp.KDVTUTAR2 = 0;
                sp.KDVTUTAR3 = 0;
                sp.KDVTUTAR4 = 0;
                sp.KDVTUTAR5 = 0;
                sp.KDV1MATRAH = 0;
                sp.KDV2MATRAH = 0;
                sp.KDV3MATRAH = 0;
                sp.KDV4MATRAH = 0;
                sp.KDV5MATRAH = 0;
                sp.DIPISKY = 0;
                sp.DIPISKONTOTUTAR = 0;
                sp.TEDARIKSURESI = 0;
                sp.ONAYVEREN = 0;
                sp.KDVDAHIL = false;
                sp.DIPISKONTOSABIT = 0;
                sp.DEPOGIRIS = "000";
                sp.PROJEKODU = "";
                sp.SATICIKODU = AktifUser.SATICIKODU;
                sp.SATIRISKONTOTOPLAM = 0;
                sp.YETKI = 0;
                sp.CARIYEISLE = false;
                sp.ISKONTOTOPLAM = 0;
                sp.PLAKA = "";
                sp.TESLIMATTARIHI = DateTime.Now;
                sp.GENELTOPLAM = YeniSiparisAlt.Sum(x => x.GENELTOPLAM);
                sp.TOPLAM = sp.GENELTOPLAM;
                sp.SATIRISKONTOTOPLAM = YeniSiparisAlt.Sum(x => x.SATIRISKONTOTOPLAM);
                sp.FISTIPI = "11-Alınan Sipariş";
                sp.DR = "K";
                sp.ACIKLAMA = "Web Siparis";
                sp.WEBSIPARIS = true;
                sp.GORULDU = false;
                YeniSiparisUst = sp;
                var spuyRes = SIPUSTORM.Current.Insert(sp);
                rslt.Add(spuyRes);

                if (spuyRes.State == ResultState.Success)
                {
                    foreach (var item in YeniSiparisAlt)
                    {
                        item.ID = SIRAORM.Current.SiraVer();
                        item.SIPID = YeniSiparisUst.ID;
                        item.CARIID = AktiCariKart.ID;
                        item.SUBEKODU = "000";
                        item.ISK1Y = 0;
                        item.ISK2Y = 0;
                        item.KDVY = 0;
                        item.DEPOGIRIS = "000";
                        item.TESLIMATTARIHI = DateTime.Now;
                        item.TP = 0;
                        item.PLAKA = "";
                        var altres = SIPALTORM.Current.Insert(item);
                        rslt.Add(altres);
                    }

                    YeniSiparisAlt = new List<SIPARISVIEW>();
                    YeniSiparisUst = null;
                }
                Results = rslt;
                return RedirectToAction("YeniSiparisList", new { Hata = "Siparisiniz oluşturulmuştur.." });
            }
            else
            {
                return RedirectToAction("YeniSiparisList", new { Hata = "lütfen bir ürün giriniz.." });
            }

        }
        public ActionResult YeniSiparisUrunAzalt(int id)
        {
            var y = YeniSiparisAlt ?? new List<SIPARISVIEW>();

            var s = STOKKARTIORM.Current.FirstOrDefault(x => x.ID == id);
            var l = y.FirstOrDefault(x => x.URUNID == id);
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
                    y.Remove(l);
                }
                YeniSiparisAlt = y;
            }
            return RedirectToAction("YeniSiparisList");
        }
        public string OzelSec(int num, string kod)
        {
            var sip = YeniSiparisUst ?? new SIPUST();
            switch (num)
            {
                case 1:
                    sip.OZEL1 = kod;
                    YeniSiparisUst = sip;
                    break;
                case 2:
                    sip.OZEL2 = kod;
                    YeniSiparisUst = sip;
                    break;
                case 3:
                    sip.OZEL3 = kod;
                    YeniSiparisUst = sip;
                    break;
            }
            return kod;
        }
        public string TPEkle(int id)
        {
            var nm = YeniSiparisUst ?? new SIPUST();
            nm.TP = id;
            YeniSiparisUst = nm;
            string rstl = "Tip " + id.ToString();
            return rstl;
        }

    }
    public class SiparisViewComponent : ViewComponent
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
        public List<SIPARISVIEW> YeniSiparisAlt
        {
            get
            {
                if (Session.GetString("YeniSiparisDetay") != null)
                    return JsonConvert.DeserializeObject<List<SIPARISVIEW>>(Session.GetString("YeniSiparisDetay"));
                else
                    return new List<SIPARISVIEW>();
            }
            set
            {
                Session.SetString("YeniSiparisDetay", JsonConvert.SerializeObject(value));
            }
        }


        public IViewComponentResult Invoke()
        {
            YeniSiparisAlt = YeniSiparisAlt ?? new List<SIPARISVIEW>();
            decimal toplam = YeniSiparisAlt.Sum(x => x.GENELTOPLAM ?? 0);
            decimal aratoplam = toplam / 1.18m;
            decimal kdv = aratoplam * 0.18m;
            List<Tuple<string, string>> toplamlar = new List<Tuple<string, string>>();
            toplamlar.Add(new Tuple<string, string>("TOPLAM", toplam.ToString("N2")));

            ViewBag.Toplamlar = toplamlar;
            ViewBag.Kullanici = AktiCariKart;
            return View(YeniSiparisAlt);
        }
    }
}
