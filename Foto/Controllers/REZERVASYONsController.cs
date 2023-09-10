using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Foto.Models;

namespace Foto.Controllers
{
    [Authorize(Roles = "admin")]
    public class REZERVASYONsController : Controller
    {
        private FotoDbEntities db = new FotoDbEntities();

        // GET: REZERVASYONs
        public ActionResult Index()
        {
            var rEZERVASYON = db.REZERVASYON.Include(r => r.MUSTERILER);
            return View(rEZERVASYON.ToList());
        }

        // GET: REZERVASYONs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REZERVASYON rEZERVASYON = db.REZERVASYON.Find(id);
            if (rEZERVASYON == null)
            {
                return HttpNotFound();
            }
            return View(rEZERVASYON);
        }

        // GET: REZERVASYONs/Create
        public ActionResult Create()
        {
            ViewBag.EVENTS = new SelectList(db.Events, "İd", "title");
            ViewBag.MUSTERIID = new SelectList(db.MUSTERILER, "MUSTERIID", "MUSTERIADSOYAD");
            return View();
        }

        // POST: REZERVASYONs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "REZERVEID,MUSTERIID,REZERVETARIH,BASLANGICSAAT,BITISAAT,PROGRAM,DURUM,DAMATAD,GELINAD,DAMATTEL,GELINTEL,DAMATEMAIL,GELINEMAIL,EVLILIKTARIHI,MUSTERISOZID,MUSTERIEXTID,SOZLESMEFIYAT,EXTRAFIYAT,ISKONTO,FIYAT,GENELTOPLAM,NOTLAR,CEKIMYERI,EVENTS,REZERVERESIM")] REZERVASYON rEZERVASYON)
        {
            if (ModelState.IsValid)
            {
                db.REZERVASYON.Add(rEZERVASYON);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EVENTS = new SelectList(db.Events, "İd", "title", rEZERVASYON.EVENTS);
            ViewBag.MUSTERIID = new SelectList(db.MUSTERILER, "MUSTERIID", "MUSTERIADSOYAD", rEZERVASYON.MUSTERIID);
            return View(rEZERVASYON);
        }

        // GET: REZERVASYONs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REZERVASYON rEZERVASYON = db.REZERVASYON.Find(id);
            if (rEZERVASYON == null)
            {
                return HttpNotFound();
            }
            ViewBag.EVENTS = new SelectList(db.Events, "İd", "title", rEZERVASYON.EVENTS);
            ViewBag.MUSTERIID = new SelectList(db.MUSTERILER, "MUSTERIID", "MUSTERIADSOYAD", rEZERVASYON.MUSTERIID);
            return View(rEZERVASYON);
        }

        // POST: REZERVASYONs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "REZERVEID,MUSTERIID,REZERVETARIH,BASLANGICSAAT,BITISAAT,PROGRAM,DURUM,DAMATAD,GELINAD,DAMATTEL,GELINTEL,DAMATEMAIL,GELINEMAIL,EVLILIKTARIHI,MUSTERISOZID,MUSTERIEXTID,SOZLESMEFIYAT,EXTRAFIYAT,ISKONTO,FIYAT,GENELTOPLAM,NOTLAR,CEKIMYERI,EVENTS,REZERVERESIM")] REZERVASYON rEZERVASYON)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rEZERVASYON).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EVENTS = new SelectList(db.Events, "İd", "title", rEZERVASYON.EVENTS);
            ViewBag.MUSTERIID = new SelectList(db.MUSTERILER, "MUSTERIID", "MUSTERIADSOYAD", rEZERVASYON.MUSTERIID);
            return View(rEZERVASYON);
        }

        // GET: REZERVASYONs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REZERVASYON rEZERVASYON = db.REZERVASYON.Find(id);
            if (rEZERVASYON == null)
            {
                return HttpNotFound();
            }
            return View(rEZERVASYON);
        }

        // POST: REZERVASYONs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            REZERVASYON rEZERVASYON = db.REZERVASYON.Find(id);
            db.REZERVASYON.Remove(rEZERVASYON);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
