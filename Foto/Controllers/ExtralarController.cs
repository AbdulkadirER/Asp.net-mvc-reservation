using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foto.Models;

namespace Foto.Controllers
{
    [Authorize(Roles = "admin")]
    public class ExtralarController : Controller
    {
        // GET: Extralar
        FotoDbEntities db = new FotoDbEntities();
        public ActionResult Extra()
        {
            var extralar = db.EXTRALAR.ToList();
            return View(extralar);
        }
     
       
        public ActionResult SIL(int ID)
        {
            var remo = db.EXTRALAR.Find(ID);
            db.EXTRALAR.Remove(remo);
            db.SaveChanges();

            return RedirectToAction("Extra");
        }

        [HttpPost]
        public ActionResult Guncelle(EXTRALAR p1)
        {

            var guncelle = db.EXTRALAR.Find(p1.EXTRAID);
            guncelle.AD = p1.AD;
            guncelle.FIYAT = p1.FIYAT;
            guncelle.FIYATTIPI = p1.FIYATTIPI;
            db.SaveChanges();

            return RedirectToAction("Extra");
        }
        [HttpPost]
        public ActionResult Ekle(EXTRALAR p1)
        {
            
            db.EXTRALAR.Add(p1);
            db.SaveChanges();

            return RedirectToAction("Extra");
        }

    }
}