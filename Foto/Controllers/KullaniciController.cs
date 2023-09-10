using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foto.Models;

namespace Foto.Controllers
{
    //[Authorize(Roles ="users")]
    //[Authorize(Roles ="admin")]
    [Authorize]
    public class KullaniciController : Controller
    {
        // GET: Kullanici
        FotoDbEntities db = new FotoDbEntities();
        public ActionResult Index()
        {
            int kullaniciId = Convert.ToInt32(Session["girisYapanKullaniciId"]);
            var sorgu = db.RESIMLER.Where(x => x.MUSTERIID == kullaniciId).ToList();
            ViewBag.resimler = sorgu;
            return View();
        }
    }
}