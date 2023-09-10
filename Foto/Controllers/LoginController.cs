using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Foto.Models;
using System.Data;
using System.Web.Security;

namespace Foto.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login
        FotoDbEntities db = new FotoDbEntities();
        public ActionResult loginPage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult loginPageGiris(MUSTERILER m, string benihatirla)
        {
            var kontrol = db.MUSTERILER.Where(x => x.MUSTERITELEFON == m.MUSTERITELEFON && x.MUSTERISIFRE == m.MUSTERISIFRE).FirstOrDefault();
            if (kontrol != null)
            {
                Session["kullaniniAd"] = kontrol.MUSTERIADSOYAD;
                Session["girisYapanKullaniciId"] = kontrol.MUSTERIID;

                if (kontrol.YETKILIMI == true)
                {

                    if (benihatirla=="true")
                    {
                        FormsAuthentication.SetAuthCookie(kontrol.MUSTERITELEFON, true);
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(kontrol.MUSTERITELEFON, false);
                    }
                   
                    return RedirectToAction("Index", "Deneme");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(kontrol.MUSTERITELEFON, false);
                    return RedirectToAction("Index","Kullanici");

                }

            }
            else
            {
                ViewBag.uyarı = "Kullanıcı Kaydı Bulunamadı!";
                return View("loginPage");
            }

        }
        public ActionResult logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return View("loginPage");
        }

        public ActionResult RegisterPage()
        {
            return View();
        }
        public ActionResult ForgotPasswordPage()
        {
            return View();
        }
    }
}