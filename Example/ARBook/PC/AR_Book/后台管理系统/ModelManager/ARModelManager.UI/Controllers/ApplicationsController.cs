using Infrastructure;
using OpenAuth.App;
using OpenAuth.App.Request;
using OpenAuth.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace Official.Migrate.Areas.Administrator.Controllers
{
    public class ApplicationsController : BaseController
    {
        public AppManager App { get; set; }


        public string GetList([FromUri]QueryAppListReq request)
        {
            return JsonHelper.Instance.Serialize(App.GetList(request));
        }

        [System.Web.Mvc.HttpPost]
        public string Delete(string[] ids)
        {
            Response resp = new Response();
            try
            {
                App.Delete(ids);
            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        [System.Web.Mvc.HttpPost]
        public string Add(Application obj)
        {
            Response resp = new Response();
            try
            {
                App.Add(obj);
            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        [System.Web.Mvc.HttpPost]
        public string Update(Application obj)
        {
            Response resp = new Response();
            try
            {
                App.Update(obj);
            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(resp);
        }


    }
}