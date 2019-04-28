using HttpClientLib;


//using ModelManager.UI.Models.ViewModel.ClientRequest;
//using ModelManager.UI.Models.ViewModel.ServerResponse;
using Newtonsoft.Json;
using QS.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCView.Core.Util;
using XCView.Core.Extensions;
using System.Threading.Tasks;
using ModelManager.Domain;
using ModelManager.BLL.UserBLL;
using WxApp.Core;
using ModelManager.Domain.ViewModel.ServerResponse;
using ModelManager.Domain.ViewModel.ClientRequest;
using ModelManager.BLL.CategoryBLL;
using QS.Common.Common;

namespace ModelManager.UI.Controllers
{


    /// <summary>
    /// 测试调试 控制器
    /// </summary>
    public class TestController : BaseController
    {

        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHelper _webHelper;



        public TestController(IUserService userService, IWebHelper webHelper,ICategoryService categoryService)
        {
            _userService = userService;
            _webHelper = webHelper;
            _categoryService = categoryService;
        }
        

        ///
        /// GET: /Test/AddUser
        public ActionResult AddUser(FormCollection form)
        {


            var name = form["name"];
            var nickname = form["nickname"];
            var password = form["password"];
            var phone = form["phone"];
            var username = form["username"];



            AddUserRequest request = new AddUserRequest()
            {
                ID = Guid.NewGuid().ToString("N"),
                NAME = name,
                NickName = nickname,
                OpenID = string.Empty,
                PASSWORD = password,
                phone = phone,
                HeadImgUrl = string.Empty,
                Username = username,
            };

            var result = _userService.AddUserData(request);

            return JResult(result.Result, result.Code);
        }


        /// <summary>
        /// 上传数据
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadData()
        {

            byte[] data;

            HttpPostedFileBase postThumb = Request.Files["modelthumb"];
            var id = Request["modelID"];
            var isImg = Request["img"].ToBoolean();

            if (postThumb != null)
            {
                Bitmap bitmap = null;
                try
                {
                    var md5Code = string.Empty;
                    using (MemoryStream target = new MemoryStream())
                    {
                        string name = postThumb.FileName;
                        //  string exts = postFile.FileName.er
                        var contentType = postThumb.ContentType;
                        string str = name;
                        string filename = System.IO.Path.GetFileName(str);
                        string extension = System.IO.Path.GetExtension(str);//扩展名 “.aspx”
                        postThumb.InputStream.CopyTo(target);
                        data = target.ToArray();                                 
                     Dictionary<object,object> objparams =     new Dictionary<object, object>(){                                        
                     };
                     objparams.Add("username", postThumb.FileName.Substring(0, postThumb.FileName.IndexOf('.')));
                     objparams.Add("fileintro", "测试");
                    var result = HttpMethods.HttpMethod.HttpPost(ConfigUtils.StaticService, objparams, data, filename, "myfile");
                    //Task.Factory.StartNew(() =>
                    //{
                    //    try
                    //    {
                    //        bitmap = ImageUtil.ConvertImageFormByte(data, out md5Code) as Bitmap;


                    //      bitmap =  ImageUtil.Crop(bitmap, 0, 0, bitmap.Width,1000);

                    //        ///获取缩略图
                    //        var bitimg = ImageUtil.GetThumbnail(bitmap, 500,367);
                    //        //var detectresult = ImageUtil.ConvertImageToByteArray(bitimg);
                    //        objparams["username"] = id;
                    //        var resultr = HttpMethods.HttpMethod.HttpPost(ConfigUtils.StaticService, objparams, ImageUtil.ConvertImageToByteArray(bitimg), id + ".jpg", "myfile");

                    //        if(bitmap != null)
                    //        {
                    //            bitmap.Dispose();
                    //        }

                    //        if (bitimg != null)
                    //        {
                    //            bitimg.Dispose();
                    //        }
                    //        LogHelper.WriteInfo("上传缩略图结果：" + resultr);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LogHelper.WriteException("上传缩略图失败", ex);
                    //    }
                    //}).ContinueWith(x => {
                    //    if (x != null)
                    //    {
                    //        x.Dispose();
                    //    }                    
                    //});
                       var oresult = JsonConvert.DeserializeObject<UploadResponse>(result);

                        if (oresult != null && oresult.Status.ToLower() == "ok")
                        {
                            oresult.ID = id;
                            return JResult(oresult);
                        }
                        return JResult(result);                   
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

            return JResult(ErrorCode.upload_fail);
        }



        /// <summary>
        /// 上传数据
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadImg()
        {

            byte[] data;

            HttpPostedFileBase postThumb = Request.Files["modelthumb"];
            var id = Request["modelID"];
            var isImg = Request["img"].ToBoolean();

            if (postThumb != null)
            {
                Bitmap bitmap = null;
                try
                {
                    var md5Code = string.Empty;
                    using (MemoryStream target = new MemoryStream())
                    {
                        string name = postThumb.FileName;
                        //  string exts = postFile.FileName.er
                        var contentType = postThumb.ContentType;
                        string str = name;
                        string filename = System.IO.Path.GetFileName(str);
                        string extension = System.IO.Path.GetExtension(str);//扩展名 “.aspx”
                        postThumb.InputStream.CopyTo(target);
                        data = target.ToArray();
                        Dictionary<object, object> objparams = new Dictionary<object, object>()
                        {
                        };

                        var uniquID = Guid.NewGuid().ToString("N");

                        objparams.Add("username", uniquID);
                        objparams.Add("fileintro", "测试");
                        var result = HttpMethods.HttpMethod.HttpPost(ConfigUtils.StaticService, objparams, data, uniquID + extension, "myfile");
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                bitmap = ImageUtil.ConvertImageFormByte(data, out md5Code) as Bitmap;


                                bitmap = ImageUtil.Crop(bitmap, 0, 0, bitmap.Width, 1000);

                                ///获取缩略图
                                var bitimg = ImageUtil.GetThumbnail(bitmap, 500, 367);
                                //var detectresult = ImageUtil.ConvertImageToByteArray(bitimg);
                                objparams["username"] = id;
                                var resultr = HttpMethods.HttpMethod.HttpPost(ConfigUtils.StaticService, objparams, ImageUtil.ConvertImageToByteArray(bitimg), id + ".jpg", "myfile");

                                if (bitmap != null)
                                {
                                    bitmap.Dispose();
                                }

                                if (bitimg != null)
                                {
                                    bitimg.Dispose();
                                }
                                LogHelper.WriteInfo("上传缩略图结果：" + resultr);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.WriteException("上传缩略图失败", ex);
                            }
                        }).ContinueWith(x =>
                        {
                            if (x != null)
                            {
                                x.Dispose();
                            }
                        });
                        var oresult = JsonConvert.DeserializeObject<UploadResponse>(result);

                        if (oresult != null && oresult.Status.ToLower() == "ok")
                        {
                            oresult.ID = id;
                            return JResult(oresult);
                        }
                        return JResult(result);
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
            return JResult(ErrorCode.upload_fail);
        }
        public ActionResult TestPage()
        {




            var categorys = _categoryService.GetAllCategory();
            return View(categorys.Result);
        }



        /// <summary>
        /// 测试搜索框页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TestSearchPage()
        {
            return View();
        }


        /// <summary>
        /// 测试 jquery.redirect.js 框架的实现方法
        /// </summary>
        /// <returns></returns>
        public ActionResult PostJs()
        {


            var id = Request["id"];

            var title = Request["title"];

            var description =  Request["description"];

            var pic =  Request["pic"];

            var bigpic = Request["bigpic"];


            return JResult(true);
        }



        public ActionResult TestWxRedirect()
        {



            ViewBag.Name = Request["name"];

            ViewBag.ID = Request["id"];

            LogHelper.WriteInfo("获取名称" + ViewBag.Name);

            return View();
        }


    }
}