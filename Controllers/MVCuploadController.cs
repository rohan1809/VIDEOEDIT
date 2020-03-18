using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Video_Upload_MVC.Models;

namespace Video_Upload_MVC.Controllers
{
    public class MVCuploadController : Controller
    {
        videoEntities db = new videoEntities();
        // GET: MVCupload
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(videofile vd , HttpPostedFileBase videofile)
        {
            if(videofile!=null)
            {
                string filename = Path.GetFileName(videofile.FileName);
                if(videofile.ContentLength<104857600)
                {
                    videofile.SaveAs(Server.MapPath("/Videofiles/"+filename));
                    vd.Vname = filename;
                    vd.Vpath = ("/Videofiles/"+filename);
                    db.videofiles.Add(vd);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Display");
        }
        public ActionResult Display()
        {
            var emp = db.videofiles.ToList();
            return View(emp);
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            videofile vd = db.videofiles.Find(id);
            if (vd == null)
            {
                return HttpNotFound();
            }
            return View(vd);
        }
        [HttpPost]
        public ActionResult Edit(int id,videofile vd, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                vd = db.videofiles.Find(id);
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    if (file.ContentLength < 104857600)
                    {
                       
                        file.SaveAs(Server.MapPath("/Videofiles/"+filename));
                        vd.Vname = filename;
                        vd.Vpath = ("/Videofiles/"+filename);
                        
                        db.Entry(vd).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                   
                }
            }
            return RedirectToAction("Display");
            
        }

    }
}