using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Foto.Controllers
{
    public class DenemeController : Controller
    {
        // GET: Deneme
        public ActionResult Index()
        {

            return View();
        }

        private static int processedImageCount = 0;

        
        private static List<string> uploadedFilePaths = new List<string>();

        [HttpGet]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    throw new Exception("No file sent.");
                }

               

                //string filename = string.Format("files/{0}_{1}", Guid.NewGuid(), Path.GetFileName(file.FileName));
                //string filepath = Server.MapPath("~/") + filename;

                //file.SaveAs(filepath);

                // Tüm işlemler başarılı oldu, yanıtı döndür
                processedImageCount++;
                //uploadedFilePaths.Add(filepath);

                return Json(new
                {
                    status = "ok",
                    //path = filename,
                    count = processedImageCount
                },JsonRequestBehavior.AllowGet);
              
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Json(new
                {
                    status = "error",
                    message = ex.Message,
                    count = processedImageCount
                },JsonRequestBehavior.AllowGet);
            }
            

        }
    }
}