using Infrastructure;
using Infrastructure.Cache;
using OpenAuth.App;
using OpenAuth.App.Response;
using OpenAuth.App.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Official.Migrate.Areas.Administrator.Controllers.api
{
    /// <summary>
    ///  sso验证
    /// <para>其他站点通过后台Post来认证</para>
    /// <para>或使用静态类OpenAuth.App.SSO.AuthUtil访问</para>
    /// </summary>
    public class CheckController : Controller
    {
        public AuthorizeApp _app { get; set; }
        private ObjCacheProvider<UserAuthSession> _objCacheProvider = new ObjCacheProvider<UserAuthSession>();

        /// <summary>
        /// 检验token是否有效
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="requestid">备用参数.</param>
        [System.Web.Mvc.HttpGet]
        public string GetStatus(string token, string requestid = "")
        {
            var result = new Response<bool>();
            try
            {
                result.Result = _objCacheProvider.GetCache(token) != null;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return JsonHelper.Instance.Serialize(result);
        }

        /// <summary>
        /// 根据token获取用户及用户可访问的所有资源
        /// </summary>
        /// <param name="token"></param>
        /// <param name="requestid">备用参数.</param>
        [System.Web.Mvc.HttpGet]
        public string GetUser(string token, string requestid = "")
        {
            var result = new Response<UserWithAccessedCtrls>();
            try
            {
                var user = _objCacheProvider.GetCache(token);
                if (user != null)
                {
                    result.Result = _app.GetAccessedControls(user.Account);
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return JsonHelper.Instance.Serialize( result);

        }

        /// <summary>
        /// 根据token获取用户名称
        /// </summary>
        /// <param name="token"></param>
        /// <param name="requestid">备用参数.</param>
        [System.Web.Mvc.HttpGet]
        public string GetUserName(string token, string requestid = "")
        {
            var result = new Response<string>();
            try
            {
                var user = _objCacheProvider.GetCache(token);
                if (user != null)
                {
                    result.Result = user.Account;
                }
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return JsonHelper.Instance.Serialize(result);
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="request">登录参数</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string Login(PassportLoginRequest request)
        {
            var result = new LoginResult();
            try
            {

                XCView.Core.Util.LogHelper.WriteInfo("111111111111111");

                if (request == null)
                {
                    XCView.Core.Util.LogHelper.WriteInfo("参数为空");
                }
                result = SSOAuthUtil.Parse(request);

                XCView.Core.Util.LogHelper.WriteInfo("22222222222222");
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return JsonHelper.Instance.Serialize( result);
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="requestid">备用参数.</param>
        [System.Web.Mvc.HttpPost]
        public bool Logout(string token, string requestid = "")
        {
            try
            {
                _objCacheProvider.Remove(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}