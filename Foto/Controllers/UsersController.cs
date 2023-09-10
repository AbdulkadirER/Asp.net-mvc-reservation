using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foto.Models;
using PagedList;

namespace Foto.Controllers
{
    public class UsersController : Controller
    {
        FotoDbEntities db = new FotoDbEntities();
        // GET: Users
        public ActionResult Admin(int sayfa = 1)
        {
            var adminler = db.MUSTERILER.Where(x => x.YETKILIMI == true).ToList().ToPagedList(sayfa, 20);
            ViewBag.adminler = adminler;
            return View();
        }
        public ActionResult Userk(int sayfa=1)
        {
            var kullanicilar = db.MUSTERILER.Where(x => x.YETKILIMI == false).ToList().ToPagedList(sayfa, 20);
            ViewBag.kullanicilar = kullanicilar;

            return View();
        }
        [HttpPost]
        public ActionResult Guncelle(MUSTERILER M, bool? myCheckbox)
        {
         
            var guncelle = db.MUSTERILER.Find(M.MUSTERIID);
            guncelle.MUSTERIADSOYAD = M.MUSTERIADSOYAD;
            guncelle.MUSTERITELEFON = M.MUSTERITELEFON;
            guncelle.MUSTERISIFRE = M.MUSTERISIFRE;
            guncelle.YETKILIMI = myCheckbox;
            db.SaveChanges();
            return RedirectToAction("Admin");
        }
        [HttpPost]
        public ActionResult Ekle(MUSTERILER M)
        {

            db.MUSTERILER.Add(M);
            db.SaveChanges();

            return RedirectToAction("Admin");
        }
    }
}