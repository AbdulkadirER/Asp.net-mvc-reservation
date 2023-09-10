using Foto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Foto.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        // GET: Home
        FotoDbEntities db = new FotoDbEntities();
        [HttpGet]
        public ActionResult RezerveAnasayfa()
        {
            var rezervasyonlar = db.REZERVASYON.ToList();
            return View(rezervasyonlar.ToList());
        }
        public ActionResult SayfaYok()
        {
            return View();
        }
        public ActionResult Empty()
        {
            return View();
        }
        public ActionResult Chart()
        {
            return View();
        }
        public ActionResult Table()
        {
            return View();
        }
        public ActionResult IsEkle()
        {
            return View();
        }
    }
}