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

namespace ARModelManager.UI.ARbook
{


    /// <summary>
    /// 流程类型
    /// </summary>
    public class ProcessItemController : BaseController
    {
        public ArModelManagerApp armodelApp { get; set; }

        // GET: /TypeItem/Index
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



            ARModelProcess rootmodel = new ARModelProcess();

            var id = Request["id"];

            var model = armodelApp.Get(id);

            if (model != null)
            {
                rootmodel = JsonHelper.Instance.Deserialize<ARModelProcess>(model.Resource);

                rootmodel.SerId = model.Sort.ToString();
            }


            if (rootmodel == null)
            {
                rootmodel = new ARModelProcess();
            }


            if (rootmodel.TypeProcess != null && rootmodel.TypeProcess.Practices != null )
            {

                if (!string.IsNullOrEmpty(rootmodel.TypeProcess.Practices.TaskPicName))
                {

                    rootmodel.TypeProcess.Practices.TaskPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeProcess.Practices.TaskPicName);
                }

                if (!string.IsNullOrEmpty(rootmodel.TypeProcess.Practices.AnswerPicName))
                {

                    rootmodel.TypeProcess.Practices.AnswerPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeProcess.Practices.AnswerPicName);
                }

            }

            ViewBag.ModleID = id;
            ViewBag.ResourceUrl = ConfigUtils.qiniurl;
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


            ARModelProcess result = null;
            try
            {
                result = JsonHelper.Instance.Deserialize<ARModelProcess>(resource);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex.Message);
            }


            int sortintval = 1;
            int.TryParse(sort, out sortintval);
            ARModel arModel = new ARModel
            {
                Id = id,
                AbUrl = abUrl,
                BookID = bookID,
                Flag = 1,
                Type = (int)ModelTypeCode.Process,
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

                        arModel.Id = existModel.Id;
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


            var keywords = Request["keywords"];

            var bid = Request["bookID"];


            QueryFlowSchemeListReq qfr = new QueryFlowSchemeListReq
            {
                orgId = bid,
                key = keywords,
                 TypeCode = (int)ModelTypeCode.Process


            };

            var result = armodelApp.Load(qfr);

            return JsonHelper.Instance.Serialize(result);
        }

        public ActionResult GetList()
        {
            return View();
        }


    }
}