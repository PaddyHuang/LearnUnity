using ARModelManager.UI.Models.ARJsonModel;
using ARModelManager.UI.Models.SystemCode;
using Infrastructure;
using Official.Migrate.Areas.Administrator.Controllers;
using OpenAuth.App;
using OpenAuth.App.Request;
using OpenAuth.Repository.Domain;
using QS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARModelManager.UI.Controllers
{
    /// <summary>
    /// 构件类型管理哦
    /// </summary>
    public class TypeItemController : BaseController
    {


        public  ArModelManagerApp armodelApp { get; set; }

        // GET: TypeItem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {

            ViewBag.ModleID = Guid.NewGuid().ToString("N");

            return View();
        }


        public ActionResult Update()
        {



            RootModel rootmodel = new RootModel();

            var id = Request["id"];

            var model  =  armodelApp.Get(id);

            if (model != null)
            {
                rootmodel = JsonHelper.Instance.Deserialize<RootModel>(model.Resource);

                rootmodel.SerId = model.Sort.ToString();
            }


            ViewBag.ModelID = id;
            ViewBag.ResourceUrl = ConfigUtils.qiniurl;

            if (rootmodel == null)
            {
                rootmodel = new RootModel();
            }
            if (rootmodel.TypeItem != null && rootmodel.TypeItem.Practices != null)
            {

                if (!string.IsNullOrEmpty(rootmodel.TypeItem.Practices.TaskPicName))
                {

                    rootmodel.TypeItem.Practices.TaskPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeItem.Practices.TaskPicName);
                }

                if (!string.IsNullOrEmpty(rootmodel.TypeItem.Practices.AnswerPicName))
                {

                    rootmodel.TypeItem.Practices.AnswerPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeItem.Practices.AnswerPicName);
                }

            }


            return View(rootmodel);
        }

        public string Delete(string ids)
        {

            var id = ids;
            try
            {
               armodelApp.Delete(id);
            }
            catch (Exception ex)
            {
                Result.Code = -1;
                Result.Message = ex.Message;
            }          
            return JsonHelper.Instance.Serialize(Result);
        }



        [ValidateAntiForgeryToken]
        
        [HttpPost]
        public string Save()
        {

            var id = Request["modelID"];
            var abUrl = Request["aburl"];
            var bookID = Request["bookID"];
            var keywords = Request["CNID"];
            var name = Request["Name"];
            var sort = Request["sort"];
            var resource = Request["Resource"];


            RootModel result = null;
            try
            {
                result = JsonHelper.Instance.Deserialize<RootModel>(resource);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message);
            }

                       
            int sortintval = 1;
            int.TryParse(sort,out sortintval);
            ARModel arModel = new ARModel
            {
                Id = id,
                AbUrl = abUrl,
                BookID = bookID,
                Flag = 1,
                Type = (int)ModelTypeCode.WuJian,
                KeyWords = keywords,
                Name = name,
                Resource = resource,
                Sort = sortintval,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = Guid.NewGuid().ToString("N");
                    arModel.Id = id;
                    armodelApp.Add(arModel);
                }
                else
                {
                    var existModel = armodelApp.Get(id);



                    if (existModel == null)
                    {
                        armodelApp.Add(arModel);

                    }
                    else
                    {
                        armodelApp.Update(arModel);
                    }

                    
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message);


                Result.Code = -1;
                Result.Message = ex.Message;
            }
        

           
            
      
            return JsonHelper.Instance.Serialize(Result);
        }


        public string Load()
        {

            var key = Request["keywords"] ?? "";

            var bookID = Request["bookID"] ?? "";
            QueryFlowSchemeListReq qfr = new QueryFlowSchemeListReq {
                orgId = bookID,
                 key =key,
                  TypeCode = (int)ModelTypeCode.WuJian
                  
             
            };

           var result = armodelApp.Load(qfr);

         return   JsonHelper.Instance.Serialize(result);
        }

        public ActionResult GetList()
        {
            return View();
        }
    }

}