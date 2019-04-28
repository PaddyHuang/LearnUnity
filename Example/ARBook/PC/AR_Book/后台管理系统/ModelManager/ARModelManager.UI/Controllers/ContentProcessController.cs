using Official.Migrate.Areas.Administrator.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARModelManager.UI.Controllers
{

   /// <summary>
   /// /流程类
   /// </summary>
    public class ContentProcessController : BaseController
    {
        // GET: ContentProcess
        public ActionResult Index()
        {
            return View();
        }
    }
}