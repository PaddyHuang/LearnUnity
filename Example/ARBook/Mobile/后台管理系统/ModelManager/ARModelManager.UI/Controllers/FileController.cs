using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Official.Migrate.Areas.Administrator.Controllers
{
    public class FileController : BaseController
    {
        // GET: Administrator/File
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult UpLoad()
        {
            return Json(true,JsonRequestBehavior.AllowGet);
        }


        public ActionResult Process()
        {

            var action = Request["action"];
            Console.WriteLine(string.Format("{0}", action));
            switch (action)
            {
                case "config": //配置 

                   return RedirectToAction("ProccessConfig");

                   // break;
                case "uploadimage"://上传图片
                    break;
                case "uploadscrawl":
                    break;
                case "uploadvideo":
                    break;
                case "uploadfile":
                    break;
                case "listimage":
                    break;
                case "listfile":
                    break;
                case "catchimage":
                    break;
            }
            return View();
        }



        public ActionResult ProccessConfig()
        {
          var jsonresult =  JsonConvert.SerializeObject(Config.Items);

           // var data =Json( Config.Items);


         //   return Content(data);
            return Json(JsonConvert.SerializeObject(Config.Items), JsonRequestBehavior.AllowGet);
        }


        public ActionResult uploadimage()
        {

            UploadImageResult result = new UploadImageResult() {
                state= "SUCCESS",
                url= "upload/demo.jpg",
                title="demo.jpg",
                original= "demo.jpg"

            };


            

            return Json(result,JsonRequestBehavior.AllowGet);
        }
    }
}