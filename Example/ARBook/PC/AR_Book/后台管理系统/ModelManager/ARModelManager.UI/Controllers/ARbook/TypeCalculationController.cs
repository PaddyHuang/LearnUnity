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
    /// 计算类型模型
    /// </summary>
    public class TypeCalculationController : BaseController
    {

        public ArModelManagerApp armodelApp { get; set; }
        // GET: TypeCalculation

        /// <summary>
        /// 展示信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {


            ViewBag.ModleID = Guid.NewGuid().ToString("N");

            return View();
        }

        /// <summary>
        /// 更新界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Update()
        {
            RootARCaculate rootmodel = new RootARCaculate();
            var id = Request["id"];
            var model = armodelApp.Get(id);

            if (model != null)
            {
                rootmodel = JsonHelper.Instance.Deserialize<RootARCaculate>(model.Resource);

                rootmodel.SerId = model.Sort.ToString();
            }
            ViewBag.ModleID = id;
            ViewBag.ResourceUrl = ConfigUtils.qiniurl;
            if (rootmodel == null)
            {
                rootmodel = new RootARCaculate();
            }
            if (rootmodel.TypeCalculation != null && rootmodel.TypeCalculation.Practices != null)
            {

                if (!string.IsNullOrEmpty(rootmodel.TypeCalculation.Practices.TaskPicName))
                {

                    rootmodel.TypeCalculation.Practices.TaskPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeCalculation.Practices.TaskPicName);
                }

                if (!string.IsNullOrEmpty(rootmodel.TypeCalculation.Practices.AnswerPicName))
                {

                    rootmodel.TypeCalculation.Practices.AnswerPicUrl = string.Format("{0}/{1}", ConfigUtils.qiniurl, rootmodel.TypeCalculation.Practices.AnswerPicName);
                }

            }


            return View(rootmodel);
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <returns></returns>
        public string Save()
        {


            var id = Request["modelID"];
            var abUrl = Request["aburl"];
            var bookID = Request["bookID"];
            var keywords = Request["CNID"];
            var name = Request["Name"];
            var sort = Request["sort"];
            var resource = Request["Resource"];


            RootARCaculate result = null;
            try
            {
                result = JsonHelper.Instance.Deserialize<RootARCaculate>(resource);
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
                Type = (int)ModelTypeCode.Cacul,
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

            ///查询关键字
            var key = Request["keywords"]??"";

            var bookID = Request["bookID"]??"";

            QueryFlowSchemeListReq qfr = new QueryFlowSchemeListReq
            {
                orgId = bookID,
                key = key,
                TypeCode = (int)ModelTypeCode.Cacul

            };

            var result = armodelApp.Load(qfr);

            return JsonHelper.Instance.Serialize(result);
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
    }
}