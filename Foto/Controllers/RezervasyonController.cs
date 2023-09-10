using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Foto.Models;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Foto.Controllers
{
    [Authorize(Roles = "admin")]
    public class RezervasyonController : Controller
    {
        // GET: Rezervasyon
        FotoDbEntities db = new FotoDbEntities();
        Class1 cs = new Class1();
        public ActionResult Index()
        {
            var rEZERVASYON = db.REZERVASYON.Include(r => r.MUSTERILER);

            return View();
        }
        public ActionResult Resimler(int? id)
        {
            var musteri = db.MUSTERILER.Find(id);
            // ID'ye göre müşteriye ait resimleri seçelim
            var images = db.RESIMLER.Where(i => i.MUSTERIID == id).ToList();
            return View(images);
        }
        public ActionResult ResimSil(int? id, RESIMLER r)
        {
            var sil = db.RESIMLER.Find(id);
            ViewBag.musid = sil.MUSTERIID;

            // Resmin fiziksel konumunu elde et ve çift ters slaşları tek ters slaşa dönüştür
            var resimYolu = Path.Combine(Server.MapPath("~" + sil.RESIMYOL));
            resimYolu = resimYolu.Replace("\\\\", "\\");

            // Fiziksel konumdaki dosyayı sil
            if (System.IO.File.Exists(resimYolu))
            {
                System.IO.File.Delete(resimYolu);
            }

            // Veritabanından resmi sil
            db.RESIMLER.Remove(sil);
            db.SaveChanges();

            return RedirectToAction("Resimler", "Rezervasyon", new { id = ViewBag.musid });
        }
        
       
        [HttpPost]
        public ActionResult ResimYukle(REZERVASYON R, HttpPostedFileBase[] files)
        {

            foreach (var file in files)
            {
             
                if (file != null && file.ContentLength > 0)
                {
                    var dosyaadi = Path.GetFileName(file.FileName);
                    var uzanti = Path.GetExtension(file.FileName);

                    // Kullanıcının klasör yolunu oluşturma
                    var klasorAdi = R.MUSTERIID.ToString();
                    var klasorYolu = Path.Combine(Server.MapPath("~/img"), klasorAdi);
                    if (!Directory.Exists(klasorYolu))
                        Directory.CreateDirectory(klasorYolu);

                    var yol = "/img/" + klasorAdi + "/" + dosyaadi;

                    // Küçültülmüş resmi filigran ekleyerek kaydetme
                    var filigranliYol = "/img/" + klasorAdi + "/filigranli_" + dosyaadi;
                    var filigranliKayitYolu = Path.Combine(klasorYolu, "filigranli_" + dosyaadi);
                    ResizeAndAddWatermark(file.InputStream, filigranliKayitYolu, 800, 600, "FOTO İPEK");

                    // Veritabanına kaydetme
                    var resim = new RESIMLER();
                    resim.MUSTERIID = R.MUSTERIID;
                    resim.RESIMYOL = filigranliYol;
                    resim.REZERVEID = R.REZERVEID;
                    db.RESIMLER.Add(resim);
 
                }
              
            }

            db.SaveChanges();
            return RedirectToAction("Resimler", "Rezervasyon", new { id = R.MUSTERIID });
        }

        public void ResizeAndAddWatermark(Stream imageStream, string outputPath, int width, int height, string watermarkText)
        {
            using (var image = Image.FromStream(imageStream))
            {
                // Orijinal resmin boyutunu al
                int originalWidth = image.Width;
                int originalHeight = image.Height;

                // Boyutları yeniden boyutlandır
                int newWidth;
                int newHeight;
                if (originalWidth > originalHeight)
                {
                    newWidth = width;
                    newHeight = (int)(((float)originalHeight / originalWidth) * width);
                }
                else
                {
                    newWidth = (int)(((float)originalWidth / originalHeight) * height);
                    newHeight = height;
                }

                // Yeniden boyutlandırılmış resmi oluştur
                using (var resizedImage = new Bitmap(newWidth, newHeight))
                using (var graphics = Graphics.FromImage(resizedImage))
                using (var font = new Font("Arial", 48)) // Filigranın font büyüklüğünü ayarlayabilirsiniz
                using (var brush = new SolidBrush(Color.FromArgb(128, 255, 255, 255))) // Filigranın arkaplan rengi
                {
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);

                    var textSize = graphics.MeasureString(watermarkText, font);
                    var watermarkPosition = new PointF((newWidth - textSize.Width) / 2, (newHeight - textSize.Height) / 2); // Filigranın konumu
                    graphics.DrawString(watermarkText, font, brush, watermarkPosition);

                    resizedImage.Save(outputPath, ImageFormat.Jpeg); // Resim formatını ayarlayabilirsiniz
                }
            }
        }
        [HttpPost]
        public ActionResult Kaydet(int[] extralar, int[] sozlesmeler, REZERVASYON R, MUSTERILER M, MUSTERISOZLESMELER MS, MUSTERIEXTRALAR ME, Events events)
        {
            Random rnd = new Random();
            int sayi = rnd.Next(100000, 999999);

            M.MUSTERISIFRE = sayi;
            M.YETKILIMI = false;
            db.MUSTERILER.Add(M);

            for (int i = 0; i < sozlesmeler.Length; i++)
            {
                MS.MUSTERIID = M.MUSTERIID;
                MS.SOZLESMEIDLER = sozlesmeler[i];
                db.MUSTERISOZLESMELER.Add(MS);
                db.SaveChanges();

            }
            if (extralar != null)
            {
                for (int i = 0; i < extralar.Length; i++)
                {
                    ME.MUSTERIID = M.MUSTERIID;
                    ME.EXTRAIDLER = extralar[i];
                    db.MUSTERIEXTRALAR.Add(ME);
                    db.SaveChanges();
                }
            }

            events.color = "#298DDC";

            events.title = M.MUSTERIADSOYAD + "\n(" + R.BASLANGICSAAT + " " + R.BITISAAT + " " + R.CEKIMYERI + ")";
            events.start = R.REZERVETARIH;
            db.Events.Add(events);
            db.SaveChanges();
            ViewBag.eventid = events.İd;


            R.EVENTS = events.İd;
            R.MUSTERIID = M.MUSTERIID;
            R.MUSTERIEXTID = M.MUSTERIID;
            R.MUSTERISOZID = M.MUSTERIID;

            if (R.REZERVERESIM != null)
            {
                if (Request.Files.Count > 0)
                {

                    string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                    string uzanti = Path.GetExtension(Request.Files[0].FileName);
                    string yol = "/img/" + dosyaadi;

                    R.REZERVERESIM = yol;
                    string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(Request.Files[0].FileName));
                    Request.Files[0].SaveAs(path);

                }
            }

            db.REZERVASYON.Add(R);

            db.SaveChanges();
            ViewBag.rezerveid = R.REZERVEID;
            if (ViewBag.rezerveid != null)
            {
                var id = db.Events.Find(ViewBag.eventid);
                events.url = "Rezervasyon/guncelle/" + ViewBag.rezerveid;

                db.SaveChanges();
            }
            //diziyi birleştirmek için bu yapıyı kullandım !
            //R.EXTRALARID = string.Join(",", Extralar);
            //R.SOZLESMELERID = string.Join(",", sozlesmeler);
            return RedirectToAction("RezerveAnasayfa", "Home");

        }

        /*  var rEZERVASYON = db.REZERVASYON.Include(r => r.Events1).Include(r => r.MUSTERILER);
          return View(rEZERVASYON.ToList());*/
        public ActionResult Guncelle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            REZERVASYON rEZERVASYON = db.REZERVASYON.Find(id);
            //program idyi burda alıyorum ve cshtmle taşıyorum
            if (ViewBag.programid != null)
            {
                ViewBag.programid = rEZERVASYON.PROGRAM1.PROGRAMID;

            }

            var musteriId = rEZERVASYON.MUSTERIID;
            if (rEZERVASYON == null)
            {
                return HttpNotFound();
            }

            var sozidler = (from ms in db.MUSTERISOZLESMELER
                            join s in db.SOZLESMELER on ms.SOZLESMEIDLER equals s.SOZLESMEID
                            where ms.MUSTERIID == musteriId
                            select new { value = s.SOZLESMEID }).ToList();

            ViewBag.sozlesmeidler = sozidler;

            var extraidler = (from ms in db.MUSTERIEXTRALAR
                              join s in db.EXTRALAR on ms.EXTRAIDLER equals s.EXTRAID
                              where ms.MUSTERIID == musteriId
                              select new { value = s.EXTRAID }).ToList();
            ViewBag.extraidler = extraidler;
            return View(rEZERVASYON);
        }
        [HttpPost]
        public ActionResult Guncelle(int?[] sozlesmeler, int?[] extralar, REZERVASYON R, MUSTERILER M)
        {
            var gncl = db.REZERVASYON.Find(R.REZERVEID);
            gncl.BASLANGICSAAT = R.BASLANGICSAAT;
            gncl.BITISAAT = R.BITISAAT;
            gncl.CEKIMYERI = R.CEKIMYERI;
            gncl.DAMATAD = R.DAMATAD;
            gncl.DAMATEMAIL = R.DAMATEMAIL;
            gncl.DAMATTEL = R.DAMATTEL;
            gncl.DURUM = R.DURUM;
            gncl.EVLILIKTARIHI = R.EVLILIKTARIHI;
            gncl.EXTRAFIYAT = R.EXTRAFIYAT;
            gncl.FIYAT = R.FIYAT;
            gncl.GELINAD = R.GELINAD;
            gncl.GELINEMAIL = R.GELINEMAIL;
            gncl.GELINTEL = R.GELINTEL;
            gncl.GENELTOPLAM = R.GENELTOPLAM;
            gncl.ISKONTO = R.ISKONTO;
            gncl.NOTLAR = R.NOTLAR;
            gncl.REZERVETARIH = R.REZERVETARIH;
            gncl.SOZLESMEFIYAT = R.SOZLESMEFIYAT;

            var kayitlar = db.MUSTERISOZLESMELER.Where(m => m.MUSTERIID == R.MUSTERIID).ToList();

            for (int i = kayitlar.Count - 1; i >= 0; i--)
            {
                if (sozlesmeler.Contains(kayitlar[i].SOZLESMEIDLER))
                {
                    kayitlar[i].SOZLESMEIDLER = sozlesmeler[kayitlar.IndexOf(kayitlar[i])];
                }
                else
                {
                    db.MUSTERISOZLESMELER.Remove(kayitlar[i]);
                }
            }
            var events = db.Events.Find(R.EVENTS);
            events.title = M.MUSTERIADSOYAD + "\n(" + R.BASLANGICSAAT + " " + R.BITISAAT + " " + R.CEKIMYERI + ")";
            events.start = R.REZERVETARIH;
            // Mevcut kayıtlar arasında olmayan yeni sözleşmeleri ekle
            var yeniSozlesmeler = sozlesmeler.Except(kayitlar.Select(k => k.SOZLESMEIDLER)).ToList();

            foreach (var yeniSozlesme in yeniSozlesmeler)
            {
                var yeniKayit = new MUSTERISOZLESMELER
                {
                    MUSTERIID = R.MUSTERIID,
                    SOZLESMEIDLER = yeniSozlesme
                };
                db.MUSTERISOZLESMELER.Add(yeniKayit);
            }

            var kayitlar1 = db.MUSTERIEXTRALAR.Where(m => m.MUSTERIID == R.MUSTERIID).ToList();

            for (int i = kayitlar1.Count - 1; i >= 0; i--)
            {
                if (extralar.Contains(kayitlar1[i].EXTRAIDLER))
                {
                    kayitlar1[i].EXTRAIDLER = extralar[kayitlar1.IndexOf(kayitlar1[i])];
                }
                else
                {
                    db.MUSTERIEXTRALAR.Remove(kayitlar1[i]);
                }
            }

            // Mevcut kayıtlar arasında olmayan yeni sözleşmeleri ekle
            var yeniSozlesmeler1 = extralar.Except(kayitlar1.Select(k => k.EXTRAIDLER)).ToList();

            foreach (var yeniSozlesme in yeniSozlesmeler1)
            {
                var yeniKayit = new MUSTERIEXTRALAR
                {
                    MUSTERIID = R.MUSTERIID,
                    EXTRAIDLER = yeniSozlesme
                };
                db.MUSTERIEXTRALAR.Add(yeniKayit);
            }

            if (R.REZERVERESIM != null)
            {
                if (Request.Files.Count > 0)
                {

                    string dosyaadi = Path.GetFileName(Request.Files[0].FileName);
                    string uzanti = Path.GetExtension(Request.Files[0].FileName);
                    string yol = "/img/" + dosyaadi;

                    gncl.REZERVERESIM = yol;
                    string path = Path.Combine(Server.MapPath("~/img"), Path.GetFileName(Request.Files[0].FileName));
                    Request.Files[0].SaveAs(path);

                }
            }

            db.SaveChanges();
            return RedirectToAction("RezerveAnasayfa", "Home");
        }
        /*  var sozlesmeler = (from ms in db.MUSTERISOZLESMELER
        join s in db.SOZLESMELER on ms.SOZLESMEIDLER equals s.SOZLESMEID
        where ms.MUSTERIID == id
        select new 
                               {
                                   value = ms.SOZLESMEIDLER,

                               }).ToList();

            return Json(sozlesmeler, JsonRequestBehavior.AllowGet);
        */
        public JsonResult detayGetir(int p)
        {

            var detay = (from x in db.SOZDETAY
                         join y in db.SOZLESMELER on x.SOZLESMELER.SOZLESMEID equals y.SOZLESMEID
                         where x.SOZLESMELER.SOZLESMEID == p
                         select new
                         {
                             text = x.DETAYAD,
                             Value = x.DETAYID.ToString(),
                             Valuee = x.SOZLESMELER.FIYAT
                         }).ToList();

            return Json(detay, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult ResimYukle(REZERVASYON R, HttpPostedFileBase[] files)
        //{
        //    foreach (var file in files)
        //    {
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            var dosyaadi = Path.GetFileName(file.FileName);
        //            var uzanti = Path.GetExtension(file.FileName);

        //            // Kullanıcının klasör yolunu oluşturma
        //            var klasorAdi = R.MUSTERIID.ToString();
        //            var klasorYolu = Path.Combine(Server.MapPath("~/img"), klasorAdi);
        //            if (!Directory.Exists(klasorYolu))
        //                Directory.CreateDirectory(klasorYolu);

        //            var yol = "/img/" + klasorAdi + "/" + dosyaadi;

        //            // Küçültülmüş resmi filigran ekleyerek kaydetme
        //            var filigranliYol = "/img/" + klasorAdi + "/filigranli_" + dosyaadi;
        //            var filigranliKayitYolu = Path.Combine(klasorYolu, "filigranli_" + dosyaadi);
        //            ResizeAndAddWatermark(file.InputStream, filigranliKayitYolu, 800, 600, "Filigran Metni");

        //            // Veritabanına kaydetme
        //            var resim = new RESIMLER();
        //            resim.MUSTERIID = R.MUSTERIID;
        //            resim.RESIMYOL = filigranliYol;
        //            resim.REZERVEID = R.REZERVEID;
        //            db.RESIMLER.Add(resim);
        //        }
        //    }

        //    db.SaveChanges();
        //    return RedirectToAction("Resimler", "Rezervasyon", new { id = R.MUSTERIID });
        //}

        //public void ResizeAndAddWatermark(Stream imageStream, string outputPath, int width, int height, string watermarkText)
        //{
        //    using (var image = Image.FromStream(imageStream))
        //    using (var resizedImage = new Bitmap(width, height))
        //    using (var graphics = Graphics.FromImage(resizedImage))
        //    using (var font = new Font("Arial", 48)) // Filigranın font büyüklüğünü ayarlayabilirsiniz
        //    using (var brush = new SolidBrush(Color.FromArgb(128, 255, 255, 255))) // Filigranın arkaplan rengi
        //    {
        //        graphics.DrawImage(image, 0, 0, width, height);

        //        var textSize = graphics.MeasureString(watermarkText, font);
        //        var watermarkPosition = new PointF((width - textSize.Width) / 2, (height - textSize.Height) / 2); // Filigranın konumu
        //        graphics.DrawString(watermarkText, font, brush, watermarkPosition);

        //        resizedImage.Save(outputPath, ImageFormat.Jpeg); // Resim formatını ayarlayabilirsiniz
        //    }
        //}
        // Bu güncellenmiş kod, her kullanıcı için ayrı bir klasör oluşturacak ve resimleri
        //  bu klasörlerin içine kaydedecektir.Örneğin, bir kullanıcının ID'si 
        //   123 ise, resimler "/img/123/" klasörüne kaydedilecektir.
      //  Aşağıda her bir adımın açıklaması bulunmaktadır:

        // foreach döngüsü, yüklenen her bir dosya için işlemleri gerçekleştirir.
          //     Dosya varsa ve içeriği boş değilse, işlemlere devam edilir.
         //    Path.GetFileName yöntemiyle dosyanın adı alınır.
          //   Path.GetExtension yöntemiyle dosyanın uzantısı alınır.
         //    Kullanıcının klasör yolunu oluşturmak için kullanıcının ID'sini alır ve bir klasör adı olarak kullanır.
          //  Path.Combine yöntemiyle kullanıcının klasör yolunu tamamlar.
          //  Klasör henüz mevcut değilse, Directory.CreateDirectory yöntemiyle klasör oluşturulur.
           // Dosyanın hedef yolunu belirlemek için kullanıcının klasör yolunu ve dosya adını birleştirir.
              //    Küçültülmüş resmin yolunu ve kayıt yolunu belirlemek için yeni bir değişken kullanılır.
          //  ResizeAndAddWatermark yöntemi, dosya akışını, hedef kayıt yolunu, hedef genişliği, yüksekliği ve filigran metnini alarak resmi küçültür ve filigran ekler.
          //Veritabanına kaydetmek için bir RESIMLER nesnesi ol
    }
}