//using Infrastructure;
using Infrastructure;
using Infrastructure.SystemCode;
using Official.Migrate.Areas.Administrator.Controllers;
using Qiniu.RS;
using QS.Common;
using QS.Common.QsQiniu.Http;
using QS.Common.QsQiniu.Storage;
using QS.Common.QsQiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ARModelManager.UI.Controllers.QiniuTest
{
    public class QiniuClientController : BaseController
    {
        //
        // GET: /QiniuClient/
        public ActionResult Index()
        {





            return View();
        }



        public ActionResult CreateDir(HttpPostedFileBase modelthumb)
        {


            var id = Request["modelID"];
            //HttpPostedFileBase postThumb = Request.Files["modelthumb"];

            HttpPostedFileBase postThumb = modelthumb;
            ///如果 上传图片不为空
            if (postThumb != null)
            {
                using (MemoryStream content = new MemoryStream())
                {
                    string name = postThumb.FileName;
                    //  string exts = postFile.FileName.er
                    var contentType = postThumb.ContentType;
                    string str = name;
                    string filename = System.IO.Path.GetFileName(str);
                    string extension = System.IO.Path.GetExtension(str);//扩展名 “.aspx”
                    postThumb.InputStream.CopyTo(content);


                    var client = QiniuClientManager.QiniuClient;
                    //Mac mac = new Mac(AccessKey, SecretKey);
                    // Random rand = new Random();
                    string key = string.Format(@"{0}",filename);
                    //string filePath = LocalFile;
                    PutPolicy putPolicy = new PutPolicy();
                    putPolicy.Scope = ConfigUtils.bucket + ":" + key;
                    putPolicy.SetExpires(3600);
                    //   putPolicy.DeleteAfterDays = 360000;
                    string token = Auth.CreateUploadToken(client, putPolicy.ToJsonString());
                    QS.Common.QsQiniu.Storage.Config config = new QS.Common.QsQiniu.Storage.Config();
                    config.Zone = Zone.ZONE_CN_South;
                    config.UseHttps = true;
                    config.UseCdnDomains = true;
                    config.ChunkSize = ChunkUnit.U512K;
                    ResumableUploader target = new ResumableUploader(config);
                    HttpResult result = target.UploadStream(content, key, token, null);
                    if (result.Code == 200)
                    {

                        // LogHelper.WriteInfo("七牛云上传结果" + JsonConvert.SerializeObject(result));
                        // LogHelper.WriteInfo("上传测试" + ConfigUtils.qiniurl + @"/" + key);
                        return JResult(new
                        {

                            path = key,
                            url = ConfigUtils.qiniurl + @"/" + key

                        });
                    }
                    else
                    {
                       return JResult(ErrorCode.upload_fail);
                    }
                }



            }
            else
            {
                return JResult(ErrorCode.upload_fail);
            }




            return View();
            // return JResult(true);
        }



        /// <summary>
        /// 设置或更新文件生命周期
        /// </summary>
        public ActionResult UpdateLifecycle()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Qiniu.Util.Mac mac = new Qiniu.Util.Mac(ConfigUtils.qiniuak, ConfigUtils.qiniusk);
            string bucket = ConfigUtils.bucket;
            string key = "02da9f8c63c4470c89a8038a33a16403/b02d033f-3cb2-4ed1-9d3e-88ec7e2dc71f.jpg";
            BucketManager bm = new BucketManager(mac);
            // 新的deleteAfterDays，设置为0表示取消lifecycle
            int deleteAfterDays = 0;
            //BucketManager bm = new BucketManager(mac);
            var result = bm.UpdateLifecycle(bucket, key, deleteAfterDays);
            Console.WriteLine(result);



           // return JResult(false);

            return View();

        }


        public ActionResult UploadTest()
        {

            Console.Write(0);

       
            return Json(new
            {
                code =0 ,
                msg=""
                 ,
                data= new {
                src="http://cdn.layui.com/123.jpg"
                          }
        }, JsonRequestBehavior.AllowGet);
        }
    }
}