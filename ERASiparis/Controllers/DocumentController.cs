using AYAK.Common.NetCore;
using ERASiparis.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using ERASiparis.Models.Entities;

namespace ERASiparis.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public DocumentController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        ISession Session => HttpContext.Session;

        private FIRMA _firma;
        public FIRMA Firma
        {
            get
            {
                string database = Tools.Connections["database"].ConnectionString;
                SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(database);
                var catalog=csb.InitialCatalog.ToUpper();
                var fc = catalog.Replace("DB_", "").Replace("DB", "");
                
                if (_firma == null)
                {
                    _firma = FIRMAORM.Current.FirstOrDefault(x => x.KODU == fc);
                }
                return _firma;
            }
        }

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

            return siparis.Data;
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
        public IActionResult Urunler()
        {
            return View();
        }
        public PartialViewResult UrunArama(string arama)
        {
            var prm = arama.CreateParameters("@p");
            var stok = STOKKARTIORM.Current.Select("Select TOP 20 * from STOKKARTI WHERE (ADI like '%'+@p+'%' OR ACIKLAMA like '%'+@p+'%' OR KODU like '%'+@p+'%' OR BRM1BARKOD like '%'+@p+'%' OR BRM2BARKOD like '%'+@p+'%' OR BRM3BARKOD like '%'+@p+'%' ) AND DR='K'", prm, SelectType.Text);

            return PartialView(stok.Data);
        }

        public FileStreamResult CreatePDF(string ControllerName, string ActionName, string Model, string PdfName)
        {

            StringBuilder sb = new StringBuilder();
            //Adresi ve portu çekeceğiz şimdilik gerek yok 
            string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            var externalIp = IPAddress.Parse(externalIpString);
            string dns = externalIp.ToString();
            string Port = PcData.port;
            Port = Port.Replace("http://0.0.0.0:","");
            sb.Append($"http://{dns}:{Port}/");

            //sb.Append($"http://localhost:1970/");
            sb.Append(ControllerName);
            sb.Append("/");
            sb.Append(ActionName);
            if (!string.IsNullOrEmpty(Model))
            {
                sb.Append("/");
                sb.Append(Model);
            }

            HtmlToPdfConverter converter = new HtmlToPdfConverter();
            WebKitConverterSettings settings = new WebKitConverterSettings();
            settings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "QtBinariesWindows");
            Console.WriteLine(settings.WebKitPath);
            converter.ConverterSettings = settings;
            PdfDocument document = converter.Convert(sb.ToString());
            MemoryStream ms = new MemoryStream();
            ms.Flush();
            document.Save(ms);
            document.Close(true);

            ms.Position = 0;

            FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");
            fileStreamResult.FileDownloadName = PdfName + ".pdf";
            return fileStreamResult;
        }
        [HttpGet]
        public FileStreamResult DownloadFile(string Makbuz)
        {
            Response.Headers.Add("content-disposition", $"attachment; filename={Makbuz}.pdf");
            return File(new FileStream(Makbuz + ".pdf", FileMode.Open),
                        "application/pdf");
        }
        public IActionResult SiparisPDF()
        {
            return View();
        }
        public IActionResult Index(int id, string faturatipi)
        {
            //    UserAdi = uAdi;
            //    UserFirmaAdi = uFirma;
            //    CariFirmaAdi = cFirma;
            //    CariAdres = cAdres;

            string DocName = faturatipi + "_" + DateTime.Now.ToString("yyMMddhhmm");
            var fileStreamResult = CreatePDF("Document", faturatipi + "Rapor", id.ToString(), DocName);
            var text = "http://0.0.0.0:1970/Document/DownloadFile?Makbuz=" + DocName;
            string url = "https://api.whatsapp.com/send/?phone&text=" + text;
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
            return fileStreamResult;
        }

        public IActionResult Indir(int id, string faturatipi)
        {

            string DocName = "Siparis" + "_" + DateTime.Now.ToString("yyMMddhhmm");
            var fileStreamResult = CreatePDF("Document", faturatipi + "Rapor", id.ToString(), DocName);
            return fileStreamResult;
        }
        public IActionResult SiparisRapor(string id)
        {
            int ID = Convert.ToInt32(id);
            SIPARISMAKBUZ sm = new SIPARISMAKBUZ();
            var sipust = SIPUSTORM.Current.FirstOrDefault(x => x.ID == ID);
            var cari = CARIKARTORM.Current.FirstOrDefault(x => x.ID == sipust.CARIID);
            sm.TARIH = sipust.TARIH?.ToShortDateString();
            sm.SIPUST = sipust;
            sm.firma = Firma;
            sm.cari = cari;
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
                    WHERE SIPALT.SIPID={ID} AND SIPALT.DR='K'").Data;

            sm.SIPALTs = siparis;
            return View(sm);
        }
        public IActionResult MakbuzRapor(string id)
        {
            string Port = PcData.port;
            Port = Port.Replace("http://0.0.0.0:", "");
            int ID = Convert.ToInt32(id);
            STOKALTMAKBUZ sam = new STOKALTMAKBUZ();
            var stokust = STOKUSTORM.Current.FirstOrDefault(x => x.ID == ID);
            var cari = CARIKARTORM.Current.FirstOrDefault(x => x.ID == stokust.CARIID);
            sam.TARIH = stokust.FATURA_TARIH?.ToShortDateString();
            sam.STOKUST = stokust;
            sam.firma = Firma;
            sam.cari = cari;
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
                    WHERE STOKALT.STOKUST_ID={ID} AND STOKALT.DR='K'").Data;
            sam.STOKALTS = siparis;
            return View(sam);
        }
        public IActionResult TahsilatRapor(string id)
        {
            int ID = Convert.ToInt32(id);
            return View();
        }
        //public void Whatsapp(int id)
        //{

        //    var text = "http://0.0.0.0:1970/Document/Indir?id=" + id;
        //    string url = "https://api.whatsapp.com/send/?phone&text=";
        //    var psi = new ProcessStartInfo
        //    {
        //        FileName = url,
        //        UseShellExecute = true
        //    };
        //    Process.Start(psi);
        //    return;
        //}
        //public void Mail(int id)
        //{

        //    var text = "http://0.0.0.0:1970/Document/Indir?id=" + id;
        //    string url = "https://mailto:?Subject=SimpleShareButtons&amp;Body=I%20saw%20this%20and%20thought%20of%20you!%20https://simplesharebuttons.com";
        //    var psi = new ProcessStartInfo
        //    {
        //        FileName = url,
        //        UseShellExecute = true
        //    };
        //    Process.Start(psi);
        //    return;
        //}
    }
}
