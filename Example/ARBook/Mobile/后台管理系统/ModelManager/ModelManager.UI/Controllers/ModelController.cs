using ModelManager.UI.Models.BLL;
using ModelManager.UI.Models.Common;
//using ModelManager.UI.Models.SystemCode;
//using ModelManager.UI.Models.ViewModel.ClientRequest;
//using ModelManager.UI.Models.ViewModel.ServerResponse;
using ModelManager.UI.Models.ViewModel.TestModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Mvc;
using XCView.Core.Util;
using XCView.Core.Extensions;
using XCView.Core.ThirdPartyReference;
using QS.Common.Export;
using ModelManager.BLL.VRModelBLL;
using WxApp.Core;
using ModelManager.Domain.ViewModel.ClientRequest;
using ModelManager.Domain.ViewModel.ServerResponse;
using ModelManager.Domain;
using ModelManager.Domain.SystemCode;
using ModelManager.BLL.CategoryBLL;
using ModelManager.Domain.Entitys;
using Qiniu.RS;
using QS.Common;
using ModelManager.BLL.OpLogBLL;
using ModelManager.BLL.SearchKeyLogBLL;
using ModelManager.Domain.Entitys.SearchKeyword;
using System.Threading.Tasks;
using ModelManager.Domain.Entitys.AllLog;


namespace ModelManager.UI.Controllers
{
    public class ModelController : BaseController
    {



        private readonly IVRModelService _vrModelService;
        private readonly IWebHelper _webHelper;
        private readonly ICategoryService _categoryService;
        private readonly IOpLogBLL _opLogBLL;
        private readonly ISearchKeywordBLL _searchKeywordBLL;
        public ModelController(IVRModelService vrmodelservice, IWebHelper webhelper, ICategoryService categoryService, IOpLogBLL opLogBLL, ISearchKeywordBLL searchKeywordBLL)
        {
            _vrModelService = vrmodelservice;
            _webHelper = webhelper;
            _categoryService = categoryService;
            _opLogBLL = opLogBLL;
            _searchKeywordBLL = searchKeywordBLL;
 
        }

        //
        // GET: /Model/
        public ActionResult ShowList()
        {
            return View();
        }
        /// <summary>
        /// 客户端 根据指令 调用模型集合  根据前端的要求 把JSON 数据格式
         /// </summary>
        /// <returns></returns>
        public ActionResult GetModelList()
        {

            var page = Request["page"].ToInt32(1);
            var limit = Request["limit"].ToInt32(20);
            var name = Request["name"];
            var content = Request["keyword"]??"";
            var url = Request["murl"];
            var pic = Request["pic"];
            var searchType = Request["searchtype"];
            var openID = Request["openID"]??"";
            var userID = Request["userID"];

            PageRequest requset = new PageRequest(){
            PageIndex  = page,
             PageSize = limit
            };
     
            var result = _vrModelService.GetModelByPage(requset, name, content, url, pic);


            if (result.Success)
            {
                if(!string.IsNullOrEmpty(userID) || !string.IsNullOrEmpty(openID))
                ///将查询关键字 持久化到 数据库
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        SearchKeyLog skl = new SearchKeyLog
                        {
                            ID = Guid.NewGuid().ToString("N"),
                            CreateTime = DateTime.Now,
                            Keyword = content+"&" +name,
                            OpenID = openID??"",
                            UserID = userID??"",
                            Result = "成功",
                            SearchMethod = searchType,
                            SourceMedia = "",
                            UpdateTime = DateTime.Now
                        };

                        var logresult = _searchKeywordBLL.AddSearchKeywordLog(skl);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteException("删除目录失败", ex);
                    }


                }).ContinueWith(x =>
                {

                    if (x != null)
                    {
                        x.Dispose();
                    }

                });



            }
            else
            {

                if (!string.IsNullOrEmpty(userID) || !string.IsNullOrEmpty(openID))
                ///将查询关键字 持久化到 数据库
                Task.Factory.StartNew(() =>
                {
                    try
                    {

                        SearchKeyLog skl = new SearchKeyLog
                        {
                            ID = Guid.NewGuid().ToString("N"),
                            CreateTime = DateTime.Now,
                            Keyword = content + "&" + name,
                            OpenID = openID ?? "",
                            UserID = userID ?? "",
                            Result = "查询失败",
                            SearchMethod = searchType,
                            SourceMedia = "",
                            UpdateTime = DateTime.Now


                        };

                        var logresult = _searchKeywordBLL.AddSearchKeywordLog(skl);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteException("删除目录失败", ex);
                    }


                }).ContinueWith(x =>
                {

                    if (x != null)
                    {
                        x.Dispose();
                    }

                });
            }
        //  var str = result.Result.ToJson();
          return Content(result.Result.data.ToJson());
          //return Json(str, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取模型集合
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUIModelList()
        {

            var page = Request["page"].ToInt32(1);
            var limit = Request["limit"].ToInt32(20);
            var name = (Request["name"]??"").Trim();
            var content = (Request["keyword"]??"").Trim();
            var url = Request["murl"];
            var pic = Request["pic"];
            PageRequest requset = new PageRequest()
            {
                PageIndex = page,
                PageSize = limit
            };       
            var result = _vrModelService.GetModelByPage(requset, name, content, url, pic);
          //  LogHelper.WriteInfo(string.Format("关键字【{0}】 查找结果：",name) +result.Result.ToJson());
            return Json(result.Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelRealImg()
        {
            return JResult(false);
        }

        public ActionResult ShowModelList()
        {
           var name = Request["name"];
           var cate = Request["cate"];
           ViewBag.Keyword = name??"";
           ViewBag.Cate = cate ?? ""; 
            return View();
        }
        /// <summary>‘
        /// 搜索模型
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetUISearchModelList()
        {
            var name = (Request["name"] ?? "").Trim();
            var cateName = (Request["cate"] ?? "").Trim();
            if (!cateName.IsNullOrEmpty())
            {
               var result = _vrModelService.GetModelByCate(cateName);
               return JResult(result.Result);
            }
            else
            {
                var result = _vrModelService.GetModelBySearch(name);
                return JResult(result.Result);
            }                   
        }


        /// <summary>
        /// 搜索模型
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUISearchBadModelList()
        {
            var name = (Request["name"] ?? "").Trim();
            var cateName = (Request["cate"] ?? "").Trim();
            if (!cateName.IsNullOrEmpty())
            {
                var result = _vrModelService.GetModelByCate(cateName);

                if (result.Result != null)
                {
                    result.Result = result.Result.Where(x => string.IsNullOrEmpty(x.RealImg) && (string.IsNullOrEmpty(x.BinModelcontentPath) || string.IsNullOrEmpty(x.ModelcontentPath))).ToList();
                }

                return JResult(result.Result);
            }
            else
            {
                var result = _vrModelService.GetModelBySearch(name);
                return JResult(result.Result);
            }        
        }


        /// <summary>
        /// /删除无用的 模型
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteBadModel()
        {


            var cateName = (Request["cate"] ?? "").Trim();
            if (!cateName.IsNullOrEmpty())
            {
                var result = _vrModelService.DeleteBadModelByCate(cateName);

                if (result.Result != null)
                {
                    result.Result = result.Result.Where(x => string.IsNullOrEmpty(x.RealImg) && (string.IsNullOrEmpty(x.BinModelcontentPath) || string.IsNullOrEmpty(x.ModelcontentPath))).ToList();
                }

                return JResult(result.Result);
            }

            return View();
        }



        /// <summary>
        /// 设置或更新文件生命周期
        /// </summary>
        public ActionResult UpdateLifecycle()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            _vrModelService.UpdateLifecycleRealImg();
            return JResult(true);
        }


        /// <summary>
        /// PHP界面获取模型信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModel()
        {
            var id = Request["id"];            
           // var result = ModelServiceUtil.GetModelByID(id);
            var result = _vrModelService.GetModelByID(id);
            DataTableSource<ModelResponse> source = new DataTableSource<ModelResponse>();
            if (result.Result != null &&result.Success)
            {
                source.code = "0";

                source.msg = "获取成功";
                source.data.Add(result.Result);
                source.count = 1;
               var str = source.ToJson();
               return Content(source.data.ToJson());
            }
            else
            {
                source.code = result.Code.ToString();
                source.msg = result.Code.GetDescription();
                source.count = 0;
                var str = source.ToJson();
                return Content(source.data.ToJson());
            }                     
           // return Json(source, JsonRequestBehavior.AllowGet);
   
        }




        /// <summary>
        /// 根据模型名称
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModelByName()
        {
            var page = Request["page"].ToInt32(1);
            var limit = Request["limit"].ToInt32(20);
            var name = Request["name"];
            var content = Request["keyword"];
            var url = Request["murl"];
            var pic = Request["pic"];
            PageRequest requset = new PageRequest()
            {
                PageIndex = page,
                PageSize = limit
            };
            if (name.IsNullOrEmpty())
            {
                return JResult(ErrorCode.sys_param_format_error);
            }
            var result = _vrModelService.GetModelByPage(requset, name, content, url, pic);
            if (result == null || result.Result == null || result.Result.data == null || result.Result.data.Count == 0)
            {
                return JResult(ErrorCode.model_noexist);
            }
            var curresult = result.Result.data.FirstOrDefault(x => name.Equals(x.Name));
            if (curresult == null)
            {
                return JResult(ErrorCode.model_noexist);
            }
            var cate = _categoryService.GetTopCate(curresult.Name);


            if (cate != null && cate.Result != null)
            {
               // LogHelper.WriteInfo("获取到模型》》》》》" + cate.Result.IsShowThumb);
                curresult.IsShowThumb = cate.Result.IsShowThumb;
                // curresult.
            }
            return JResult(curresult);
        }




        /// <summary>
        /// 根据模型名称
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModelByID()
        {


            var id = Request["id"];
            //var id = Request["id"];



         //   LogHelper.WriteInfo(string.Format("根据ID【{0}】获取模型", id));

            if (id.IsNullOrEmpty())
            {
                return JResult(ErrorCode.sys_param_format_error);
            }



            // var result = ModelServiceUtil.GetModelByID(id);

            var result = _vrModelService.GetModelByID(id);
        
  
            var curresult = result.Result;
            if (curresult == null)
            {
                return JResult(ErrorCode.model_noexist);
            }

            var cate = _categoryService.GetTopCate(curresult.Name);


            if (cate != null && cate.Result != null )
            {


              //  LogHelper.WriteInfo("获取到模型》》》》》" + cate.Result.IsShowThumb);

                curresult.IsShowThumb = cate.Result.IsShowThumb;
               // curresult.
            }


            return JResult(curresult);
        }

       /// <summary>
       /// 获取所有模型关键字
       /// </summary>
       /// <returns></returns>
        public ActionResult GetAllKeywords()
        {


           var result = _vrModelService.GetAllKeyWords();

           return Json(string.Join(",", result.Result), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取单个模型
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUIModel()
        {

            var id = Request["id"];

           // var result = ModelServiceUtil.GetModelByID(id);
            var result = _vrModelService.GetModelByID(id);
            

            DataTableSource<ModelResponse> source = new DataTableSource<ModelResponse>();



            if (result.Result != null && result.Success)
            {
                source.code = "0";

                source.msg = "获取成功";
                source.data.Add(result.Result);
                source.count = 1;
              //  var str = result.Result.ToJson().Replace('[', '{').Replace(']', '}');
                return Json(source, JsonRequestBehavior.AllowGet);
            }
            else
            {
                source.code = result.Code.ToString();
                source.msg = result.Code.GetDescription();

              //  var str = result.Result.ToJson().Replace('[', '{').Replace(']', '}');
                source.count = 0;
            }
            return Json(source, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            ViewBag.ID = Guid.NewGuid().ToString("N");
            return View();
        }
        /// <summary>
        /// 模型详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            var id = Request["ID"];


            

          //  var result = ModelServiceUtil.GetModelByID(id);

            var result = _vrModelService.GetModelByID(id);
            if (result.Result == null)
            {
                return RedirectToAction("ShowList", "Model");
            }

            else
            {
                return View(result.Result);
            }





           // return View();


        }
        /// <summary>
        /// 删除模型信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete()
        {

            var id = Request["ID"];

          // var result =  ModelServiceUtil.DeleteQsModel(id);

           var result = _vrModelService.DeleteQsModel(id);
           if (result.Success)
           {
               return JResult(result.Result);
           }
           else
           {
               if (result.Result)
               {
                   return JResult(result.Result, result.Code);
               }
               else
               {
                   return JResult(result.Code);
               }
           }

            
        }
        /// <summary>
        /// 保存模型
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
       [ValidateInput(false)]
        public ActionResult SaveModel(FormCollection form)
        {


            var modelID = form["modelid"];
            var name = form["name"];
            var modelNum = form["modelnum"];
           var modelkey = form["modelkeyword"];
            var imgid = form["imgid"];
            var modelcontent = form["modelcontent"];
          //  var modelcontentpath = form["modelcontentpath"];
            var modelbincontentpath = form["binmodeldatael"];
            var modelurl = form["modelurl"];
            var sort = form["sort"];
            var flag = form["flag"];
            var realimg = form["realimg"];
         
            var desc = form["desccontent"];
            var keywords = form["keywords"];
            var parentID = form["parentID"];

            if (string.IsNullOrEmpty(keywords))
            {
                keywords = modelkey;
            }
            if (desc != null)
            {
                desc = desc.Trim();
            }

            if (realimg != null)
            {
                realimg = realimg.TrimEnd(',');
            }

     
            ModelRequest request = new ModelRequest
            {
                 ID = modelID,
                 Name =name,
                 ModelNum = modelNum,
                 KeyWords = keywords,
                 Flag = flag,
                 ModelcontentPath = modelcontent,
                 ModelBinContentPath = modelbincontentpath,
                 ThumbUrl = imgid,
                 Desc =  desc,
                 RealImg = realimg,
                 Url = modelurl,        
                  ParentID = parentID,
                 CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                 UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")                                  
            };
            if ("on".Equals(flag))
            {

                request.IsFee = 1;

            }


            var result = _vrModelService.AddQsModel(request);


          if (result.Success)
          {
              return JResult(Guid.NewGuid().ToString("N"));
          }

          else
          {
              return JResult(result.Code);
          }
           // return JResult("OK");
        }
        /// <summary>
        /// 更新模型
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
       public ActionResult UpdateModel(FormCollection form)
       {
           var modelID = form["modelid"];
           var parentID = form["parentID"];
           var name = form["name"];
           var modelNum = form["modelnum"];
           var modelkey = form["modelkeyword"];
           var imgid = form["imgid"];
           var modelcontent = form["modelcontent"];
           //  var modelcontentpath = form["modelcontentpath"];
           var modelbincontentpath = form["binmodeldatael"];
           var modelurl = form["modelurl"];
           var sort = form["sort"];
           var flag = form["flag"];
           var desc = form["desccontent"];
           var keywords = form["keywords"];
           var realimg = form["realimg"];
           if (string.IsNullOrEmpty(keywords))
           {
               keywords = modelkey;
           }


           if (desc != null)
           {
               desc = desc.Trim();
           }


           if (!string.IsNullOrEmpty(realimg))
           {

               realimg = realimg.TrimEnd(',');
           }

           ModelRequest request = new ModelRequest
           {
               ID = modelID,
               Name = name,
               ModelNum = modelNum,
               KeyWords = keywords,
               Flag = flag,
                ParentID = parentID,
               ModelcontentPath = modelcontent,
               ModelBinContentPath = modelbincontentpath,
               ThumbUrl = imgid,
                RealImg = realimg, 

               Desc = desc,
               Url = modelurl,        
               UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
           };

           if ("on".Equals(flag))
           {
               request.IsFee = (int)FeeCode.NeedPay;
           }


           var result = _vrModelService.UpdateQsModel(request);

           if (result.Success)
           {
               return JResult(result.Result);
           }

           else
           {
               return JResult(result.Code);
           }

          // return View("Detail",result.Result);
       }
        /// <summary>
        ///  上传文件
        /// </summary>
        /// <returns></returns>
        public ActionResult UpLoad()
        {
            HttpPostedFileBase postFile = Request.Files["modeldata"];
            var modelID = Request["modelID"];

            byte[] data = null;


            if (postFile != null)
            {


         
                try
                {
                    using (MemoryStream target = new MemoryStream())
                    {
                        string name = postFile.FileName;
                      //  string exts = postFile.FileName.er
                        var contentType = postFile.ContentType;
                        string str = name;
                        string filename = System.IO.Path.GetFileName(str);
                        string extension = System.IO.Path.GetExtension(str);//扩展名 “.aspx”

                        postFile.InputStream.CopyTo(target);
                        data = target.ToArray();
                        string md5Code = string.Empty;
                         md5Code =ImageUtil.RenderMD5Code(data);
                        var text = Encoding.Default.GetString(data);
                        if (!Directory.Exists(string.Format(@"{0}Temp\\ModelData", System.AppDomain.CurrentDomain.BaseDirectory)))
                        {
                            Directory.CreateDirectory(string.Format(@"{0}Temp\\ModelData", System.AppDomain.CurrentDomain.BaseDirectory));
                        }
                        var result = FileUtil.Write(string.Format(@"{0}Temp\\ModelData\\{1}{2}", System.AppDomain.CurrentDomain.BaseDirectory, modelID, extension), text);
                        return JResult(new {

                                id = modelID,
                                exts = extension,
                                path = string.Format("http://{0}/Temp/Photo/{1}{2}", GlobalData.Instance.Authority, modelID,extension),
                                content = text
                        
                        });
                    }
                }
                catch (Exception ex)
                {


                    LogHelper.WriteException("图片格式不正确", ex);
                    return JResult("图片格式不正确", ErrorCode.sys_param_format_error);


                }
            }




          //  FileUtil.Write("", "");


            HttpPostedFileBase postThumb = Request.Files["modelthumb"];


            if (postThumb != null)
            {
                Bitmap bitmap = null;
                try
                {
                    var md5Code = string.Empty;

                    using (MemoryStream target = new MemoryStream())
                    {
                        postThumb.InputStream.CopyTo(target);
                        data = target.ToArray();

                        bitmap = ImageUtil.ConvertImageFormByte(data, out md5Code) as Bitmap;

                        if (!Directory.Exists(string.Format(@"{0}Temp\\Photo", System.AppDomain.CurrentDomain.BaseDirectory)))
                        {
                            Directory.CreateDirectory(string.Format(@"{0}Temp\\Photo", System.AppDomain.CurrentDomain.BaseDirectory));
                        }
                        bitmap.Save(string.Format(@"{0}Temp\\Photo\\{1}.png", System.AppDomain.CurrentDomain.BaseDirectory, md5Code));
                        bitmap.Dispose();

                       
                        return JResult(new { 
                            id = md5Code,
                            url = string.Format("http://{0}/Temp/Photo/{1}.png", GlobalData.Instance.Authority, md5Code)
                        });
                    }
                }
                catch (Exception ex)
                {


                    LogHelper.WriteException("图片格式不正确", ex);
                    return JResult("图片格式不正确", ErrorCode.sys_param_format_error);
                }
                finally
                {
                    if (bitmap != null)
                    {
                        bitmap.Dispose();
                    }
                }

             
              

            }

            return JResult("");
        }
        /// <summary>
        /// 测试 demo 
        /// </summary>
        /// <returns></returns>
        public ActionResult Test()
        {


            DataTableSource<TestData> datas = new DataTableSource<TestData>()
            {
                code = "0",
                count = 2,
                data = new List<TestData>() { 
                  new  TestData{
                       	id=10000,
			            username="user-0",
			            sex="女",
			            city="城市-0",
			            sign="签名-0",
			            experience="",
			            logins="24",
			            wealth=82830700,
			            classify="作家",
			            score=57
                  },
                  new TestData{
                        id=10001,
			            username="user-0",
			            sex="女",
			            city="城市-0",
			            sign="签名-0",
			            experience="",
			            logins="24",
			            wealth=82830700,
			            classify="作家",
			            score=57
                  }
                },
                 msg="获取成功"


            };



            return Json(datas, JsonRequestBehavior.AllowGet);


             



            //return View();
        }
        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns></returns>
        public ActionResult TestAPI()
        {

            string testData = @"门柱：采用角钢骨架，外包薄铁皮，基础 1.2 ×1.2m，基础埋深应根据现场地质情况自行确定，采用 C20砼浇筑；门柱截面尺寸为800×800 (1000×1000)mm，门柱地面以上高度5m。2.门梁：采用L40×40×2角钢骨架制作，外包薄铁皮 ，梁高1000(1200)mm，宽度 800(1000)mm，长度为门柱外边各挑出600mm，即门楣长度L=门的净空宽度＋2×门柱宽度＋2×600mm；门梁钢骨架中预埋20cm、φ40钢管，露出表面5cm。位于门梁、轴线上各预埋一根，用于插放彩旗。3.门扇：门扇框采用钢管焊制而成，外包薄铁板，门轴预埋铁件与门柱钢架焊接，并具有足够的锚固力";

         //   DataTableSource<ModelResponse> source = new DataTableSource<ModelResponse>();
            DataTableSource<ModelResponse> datas = new DataTableSource<ModelResponse>()
            {
                code = "0",
                count = 2,
                data = new List<ModelResponse>() { 
                  new  ModelResponse{
                       	     ID = Guid.NewGuid().ToString("N"),
                             BinModelcontentPath = string.Empty,
                             CreateTime = DateTime.Now.ToShortDateString(),
                             Desc = HttpUtility.UrlEncode(testData,Encoding.UTF8),
                             Flag = 0,
                             ImgUrl ="test",
                             Keywords = "",
                             ModelContent = "",
                             ModelcontentPath = "",
                             ModelNum = "",
                             Name = "测试模型",
                             RealImg = "",
                             ThumbUrl = string.Empty,
                             UpdateTime = DateTime.Now.ToShortDateString(),
                             Url = string.Empty
             
                  }
                 
                },
                msg = "获取成功"


            };


            return Content(datas.data.ToJson());
           // return JResult(datas.data);






            //return View();
        }
        /// <summary>
        /// 测试AR课本
        /// </summary>
        /// <returns></returns>
        public ActionResult TestARData()
        {
            ARModelData armd = new ARModelData()
            {
                ModelNum = "12344",
                ModelTag = new ModelTag
                {
                    ModelBar = new ModelBar
                    {
                        content = "",
                        direction = new Direction
                        {
                             startNode = new Node{
                              x = 1,
                               y= 2,
                                z = 4
                             },
                            endNode = new Node
                            {
                                x = 12,
                                y = 13,
                                z = 14
                            },
                        },
                        position = new Node {

                            x = 12,
                            y = 13,
                            z = 14
                        }
                         
                         
                    }
                },
                 


            };


           

            return Json(armd, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///  导入模型
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportModel()
        {
            List<ImportResult<ModelRequest>> importResult = new List<ImportResult<ModelRequest>>();
            List<ModelRequest> templateList = new List<ModelRequest>();
            var fileUpload = Request.Files["fileUpload"];
            try
            {
                var fileName = string.Empty;
                if (!".xls,.xlsx".Contains(Path.GetExtension(fileUpload.FileName)))
                {
                    return JResult("数据格式不正确");             
                }
                else
                {
                    fileName = Path.GetTempFileName();
                    fileUpload.SaveAs(fileName);
                    var datatable = ExcelRenderer.ImportExcelFile(fileName, fileUpload.FileName);
                    if (datatable != null && datatable.Rows.Count > 0)
                    {


                        var cates = _categoryService.GetAllCategory();

                        foreach (System.Data.DataRow dr in datatable.Rows)
                        {
                            ModelRequest te = new ModelRequest();
                            if (datatable.Columns.Count < 4)
                            {
                                continue;
                            }
                            te.Name = dr[0].ToString();                     
                            te.ModelNum = dr[1].ToString();                        
                            te.KeyWords = dr[2].ToString();                        
                            te.Desc = dr[3].ToString();
                            if (string.IsNullOrEmpty(te.Name) && string.IsNullOrEmpty(te.ModelNum) && string.IsNullOrEmpty(te.KeyWords) && string.IsNullOrEmpty(te.Desc))
                            {
                                continue;
                            }
                            if (string.IsNullOrEmpty(te.Name))
                            {
                                ImportResult<ModelRequest> model = new ImportResult<ModelRequest>();
                                model.Msg = "模型名字不能为空";
                                model.Data = te;
                                importResult.Add(model);
                                continue;
                            }



                            if (cates.Result != null && cates.Result.Count > 0)
                            {
                                var curcate = cates.Result.FirstOrDefault(x => x.Name.Equals(te.Name));

                                if (curcate != null)
                                {
                                    te.ParentID = curcate.ID;
                                }
                            }
                            //if (string.IsNullOrEmpty(te.ModelNum))
                            //{
                            //    ImportResult<ModelRequest> model = new ImportResult<ModelRequest>();
                            //    model.Msg = "模型编号不能为空";
                            //    model.Data = te;
                            //    importResult.Add(model);
                            //    continue;
                            //}
                            //if (string.IsNullOrEmpty(te.KeyWords))
                            //{
                            //    ImportResult<ModelRequest> model = new ImportResult<ModelRequest>();
                            //    model.Msg = "模型关键字不能为空";
                            //    model.Data = te;
                            //    importResult.Add(model);
                            //    continue;
                            //}
                            //if (string.IsNullOrEmpty(te.Desc))
                            //{
                            //    ImportResult<ModelRequest> model = new ImportResult<ModelRequest>();
                            //    model.Msg = "模型描述不能为空";
                            //    model.Data = te;
                            //    importResult.Add(model);
                            //    continue;
                            //}
                            te.ID = Guid.NewGuid().ToString("N");
                            te.Flag = ((int)FlagCode.Normal).ToString();
                            te.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            te.UpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            if (_vrModelService.AddQsModel(te).Success)
                            {
                                templateList.Add(te);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(te.Desc))
                                {
                                    ImportResult<ModelRequest> model = new ImportResult<ModelRequest>();
                                    model.Msg = "添加到库失败";
                                    model.Data = te;
                                    importResult.Add(model);
                                    continue;
                                }
                            }                                              
                        }
                        return JResult(importResult);

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException("Upload(FormCollection collection)", ex);
                return JResult(importResult,ErrorCode.sys_fail);
            }

            return JResult(importResult);

        }

        /// <summary>
        /// 导入系统
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            return View();
        }


      //  public ActionResult Read

	}
}