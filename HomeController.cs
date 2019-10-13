using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JROS.Models;
using JROS.Utils;
using System.IO;
using Rotativa;

namespace JROS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Send_Email()
        {
            return View(new SendEmailViewModel());
        }

        [HttpPost]
        public ActionResult Send_Email(SendEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    String toEmail = model.ToEmail;
                    String subject = model.Subject;
                    String contents = model.Contents;



                    string localPath = Server.MapPath("~/Files/");
                    string fileName = Path.GetFileName(model.File.FileName);

                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    string path = "";

                    if (model.File != null && model.File.ContentLength > 0)
                    {
                        path = Path.Combine(localPath, fileName);
                        model.File.SaveAs(path);
                    }

                    EmailSender es = new EmailSender();
                    es.Send(toEmail, subject, contents, path, model.File);

                    ViewBag.Result = "Email has been send.";

                    ModelState.Clear();

                    return View(new SendEmailViewModel());
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

        Entities context;

        public HomeController()
        {
            context = new Entities();
        }

        public ActionResult GetAll()
        {
            var AllFood = context.MenuSet.ToList();
            return View(AllFood);
        }

        public ActionResult PrintAll()
        {
            var a = new ActionAsPdf("GetAll");
            return a;
        }
    }
}