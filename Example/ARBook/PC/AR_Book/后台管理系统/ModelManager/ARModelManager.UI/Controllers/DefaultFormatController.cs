
using Infrastructure.SystemCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCView.Core.Extensions;
namespace ARModelManager.UI.Controllers
{
    public class DefaultFormatController : Controller
    {
        //
        // GET: /Base/

        protected JsonResult JResult(ErrorCode code)
        {
            //return Json(new
            //{
            //    Code = code,
            //    ErrorDesc = code.GetDescription()
            //}, JsonRequestBehavior.AllowGet);
            return Json(new
            {
                Code = code,
                ErrorDesc = code.GetDescription()
            }, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JResult<T>(ErrorCode code, T model = default(T))
        {
            if (code == ErrorCode.sys_success)
            {
                return Json(new
                {
                    Code = code,
                    Result = model
                }, JsonRequestBehavior.AllowGet);//zhangh update
            }
            else
            {
                return JResult(code);
            }
        }

        protected JsonResult JResult<T>(T model, ErrorCode code = ErrorCode.sys_fail)
        {
            if (model == null) return JResult<T>(code);
            return JResult<T>(ErrorCode.sys_success, model);
        }

        protected JsonResult JResult<T>(T model, int count)
        {
            return Json(new
            {
                Code = ErrorCode.sys_success,
                Count = count,
                Result = model
            }, JsonRequestBehavior.AllowGet);
        }

        protected JsonResult JResult(string data, ErrorCode code = ErrorCode.sys_fail)
        {
            if (data.IsNullOrEmpty()) return JResult<string>(code);
            return JResult<string>(ErrorCode.sys_success, data);
        }

        protected JsonResult JResult(int data, ErrorCode code = ErrorCode.sys_fail)
        {
            if (data == 0) return JResult<int>(code);
            return JResult<int>(ErrorCode.sys_success, data);
        }

        protected JsonResult JResult<T>(T model, int count, ErrorCode code = ErrorCode.sys_success)
        {
            return Json(new
            {
                Code = ErrorCode.sys_success,
                Count = count,
                Result = model
            }, JsonRequestBehavior.AllowGet);
        }
    }
}