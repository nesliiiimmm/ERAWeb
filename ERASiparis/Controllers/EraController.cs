using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AYAK.Common.NetCore;
using ERASiparis.Models;

namespace ERASiparis.Controllers
{
    [Authorize]
    public class EraController : Controller
    {

        public EraController(IHttpContextAccessor httpContextAccessor)
        {

        }

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
        public ActionResult Cari()
        {
            var carim = CARIKARTORM.Current.Select();
            return View(carim.Data);
        }

        public ActionResult Index()
        {

            return View();
        }
        [AllowAnonymous]
        public ActionResult Giris()
        {
            return View();
        }
        public PartialViewResult Menu()
        {

            return PartialView(AktifUser);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Giris(string KullaniciAdi, string Parola, string remember)
        {
            var cari = CARIKARTORM.Current.FirstOrDefault(x => x.WEBKULKODU == KullaniciAdi & x.WEBSIFRE == Parola);
            KULLAN kul = null;
            int id = 0;
            if (int.TryParse(KullaniciAdi, out id))
            {
                kul = KULLANORM.Current.FirstOrDefault(x => (x.ID == id | x.ADI == KullaniciAdi) & x.SIFRE == Parola & x.AKTIF == true & x.WEBKULLANICI == true);
            }
            else
            {
                kul = KULLANORM.Current.FirstOrDefault(x => x.ADI == KullaniciAdi & x.SIFRE == Parola & x.AKTIF == true & x.WEBKULLANICI == true);
            }

            //string prl = Models.User.Hash(Parola);

            if (kul != null)
            {
                KULLANVIEW user = new KULLANVIEW();
                user.KullaniciAta(kul);
                user.FIRMAADI = kul.ADI;
                user.ISCARI = false;
                user.KARTTIPI = "2-Satıcı";
                //user.SATICIKODU = kul.SATICIKODU;
                AktifUser = user;

                if (remember != null)
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, kul.ADI), new Claim(ClaimTypes.IsPersistent, "true", "bool") };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    //FormsAuthentication.RedirectFromLoginPage(kul.ADI, true);
                    return RedirectToAction("Index", "Cari");
                }
                else
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, kul.ADI) };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    //FormsAuthentication.RedirectFromLoginPage(kul.ADI, true);
                    return RedirectToAction("Index", "Cari");
                }

            }
            else
            if (cari != null)
            {

                AktifUser = new KULLANVIEW { ID = cari.ID, KARTTIPI = cari.KARTTIPI, FIRMAADI = cari.FIRMAADI, ADI = cari.ADI, ISCARI = true, KREDITAHSILAT = false, NAKITTAHSILAT = false,CEKSENETTAHSILAT=false, CARIACMA = false, CARIARAMA = true, SIPARIS = true, FATURA = true, SATICIKODU = "" };
                if (cari.KARTTIPI == "1-Müşteri")
                {
                    AktiCariKart = cari;
                    AktifUser.FATURA = false;
                    AktifUser.CARIARAMA = false;
                    //seach görünürlülüğünü gizle 
                }
                else
                {
                }
                if (remember != null)
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, cari.FIRMAADI), new Claim(ClaimTypes.IsPersistent, "true", "bool") };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("Index", "Cari");
                    //FormsAuthentication.RedirectFromLoginPage(cari.FIRMAADI, true);
                }
                else
                {
                    var claims = new List<Claim> { new Claim(ClaimTypes.Name, cari.FIRMAADI) };
                    var userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("Index", "Cari");
                    //FormsAuthentication.RedirectFromLoginPage(cari.FIRMAADI, false);
                }
                //return RedirectToAction("Index", "Cari");

            }
            else
            {
                ViewBag.Message = "Giriş Yapılamadı Lütfen Tekrar Deneyin";
                return View();
            }


        }

        public async Task<ActionResult> CikisYap()
        {
            Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Giris");
        }

        public ActionResult Resultlar()
        {
            return View(Results);
        }
    }
}
