using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foto.Models;

namespace Foto.Controllers
{
    [Authorize(Roles = "admin")]
    public class CalendarController : Controller
    {
        FotoDbEntities db = new FotoDbEntities();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult EtkinlikleriGetir()
        {
            db.Configuration.ProxyCreationEnabled = false;
            var rezervasyonlar = db.Events.ToList();
            return Json(rezervasyonlar, JsonRequestBehavior.AllowGet);
        }
    }
}