using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foto.Models;
using System.Data.Entity;

namespace Foto.Controllers
{
    [Authorize(Roles = "admin")]
    public class SozlesmeController : Controller
    {
        // GET: Sozlesme
        FotoDbEntities db = new FotoDbEntities();
        public ActionResult Sozle()
        {  
           
            var sozlesme = db.SOZDETAY.ToList();
            return View(sozlesme.ToList());
           
        }


        public ActionResult SIL(int ID)
        {
            var sozsil = db.SOZLESMELER.Find(ID);
            var detaysil = (from sil in db.SOZDETAY where sil.SOZLESMEID == ID select sil).FirstOrDefault();
            db.SOZLESMELER.Remove(sozsil);
            db.SOZDETAY.Remove(detaysil);
            db.SaveChanges();

            return RedirectToAction("Sozle");
        }

        [HttpPost]
        public ActionResult Update(SOZDETAY s1,SOZLESMELER s2)
        {
            var detay = (from x in db.SOZDETAY where s1.SOZLESMEID == x.SOZLESMEID select x).FirstOrDefault();
            detay.DETAYAD = s1.DETAYAD;
            var guncelle = db.SOZLESMELER.Find(s2.SOZLESMEID);
            guncelle.AD = s2.AD;
            guncelle.FIYAT = s2.FIYAT;



            //guncelle.DETAY = s1.DETAY.Replace("\n","<br>");
            db.SaveChanges();

            return RedirectToAction("Sozle");
        }
        [HttpPost]
        public ActionResult Ekle(SOZLESMELER p1, SOZDETAY s1)
        {
            SOZDETAY dty = new SOZDETAY();
            SOZLESMELER soz = new SOZLESMELER();

            soz.AD = p1.AD;
            soz.FIYAT = p1.FIYAT; 
            db.SOZLESMELER.Add(soz);
            dty.DETAYAD = s1.DETAYAD;
            dty.SOZLESMEID = p1.SOZLESMEID;
            dty.DETAYAD = s1.DETAYAD;
            db.SOZDETAY.Add(dty);
            db.SaveChanges();

            return RedirectToAction("Sozle");
        }

    }
}