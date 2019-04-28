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
    public class MenuController : BaseController
    {

        /// <summary>
        /// 依赖注入
        /// </summary>
        public MenuManagerApp App { get; set; }


        #region 0.0展示菜单
        // GET: Administrator/Menu
        public ActionResult Index()
        {
            return View();
        }
        #endregion


        #region //1.0 加载数据表格

        public string Load()
        {




            QueryFlowSchemeListReq request = new QueryFlowSchemeListReq { };


            int page = 1;
            int limit = 10;


            Int32.TryParse(Request["page"] ?? "1",out page);
            Int32.TryParse(Request["limit"] ?? "1", out limit);


            var key = Request["key"];
          

            request.page =page;
            request.limit = limit;
            request.key = key;


            var data = App.Load(request);

            return JsonHelper.Instance.Serialize(data);
            /// <summary>
            /// 加载列表
            /// </summary>
            //public string Load([FromUri]QueryFlowSchemeListReq request)
            //{
                
            //}
        }

        #endregion


        #region //2.0 添加导航栏目
        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]
        public string Add(Menu model)
        {
            try
            {

                if (model == null)
                {
                    Result.Code = 1;
                    Result.Message = "参数格式错误";
                       
                }


              //  if( model.IsOnNav)


                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = Guid.NewGuid().ToString("N");
                }

                if (string.IsNullOrEmpty(model.Styles))
                {
                    model.Styles = string.Empty;
                }

                if (string.IsNullOrEmpty(model.Href))
                {
                    model.Href = string.Empty;
                }
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                App.Add(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        #endregion



        #region //3.0 修改导航栏目
        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]
        public string Update(Menu model)
        {
            try
            {

                if (model == null)
                {
                    Result.Code = 1;
                    Result.Message = "参数格式错误";

                }


                //  if( model.IsOnNav)


                //if (string.IsNullOrEmpty(model.Id))
                //{
                //    model.Id = Guid.NewGuid().ToString("N");
                //}

                if (string.IsNullOrEmpty(model.Styles))
                {
                    model.Styles = string.Empty;
                }

                if (string.IsNullOrEmpty(model.Href))
                {
                    model.Href = string.Empty;
                }
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                App.Update(model);
              
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
        #endregion





        public ActionResult Create()
        {
            return View();
        }


        #region //4.0 删除导航栏

        /// <summary>
        /// 删除导航栏
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
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

        #endregion



        #region 5.0 配置显示模块

        public ActionResult SetModel()
        {
            return View();
        }


        #endregion


        #region 6.0 批量获取菜单

        #endregion
    }
}