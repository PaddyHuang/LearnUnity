using Autofac;
using Autofac.Integration.Mvc;
using ModelManager.BLL.ARModelBLL;
using ModelManager.BLL.CategoryBLL;
using ModelManager.BLL.CRM;
using ModelManager.BLL.GroupModelBLL;
using ModelManager.BLL.MenuBLL;
using ModelManager.BLL.OpLogBLL;
using ModelManager.BLL.PermissionBLL;
using ModelManager.BLL.RoleBLL;
using ModelManager.BLL.SearchKeyLogBLL;
using ModelManager.BLL.UserBLL;
using ModelManager.BLL.VRModelBLL;
using ModelManager.Respository.AllRespository;
using ModelManager.Respository.AllRespository.ARModelDAL;
using ModelManager.Respository.AllRespository.CategoryDAL;
using ModelManager.Respository.AllRespository.CRM;
using ModelManager.Respository.AllRespository.SearchLog;
using ModelManager.Respository.AllRespository.VRModelDAL;
using ModelManager.Respository.Interface;
using ModelManager.UI.Models.CorsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WxApp.Core;
using WxApp.Core.Fakes;

namespace ModelManager.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
          //  GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsMessageHandler());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.RemoveAt(0);
            //  EngineContext.Initialize(false);
           AutofacIOC();


          // var logger = EngineContext.Current.Resolve<ILogger>();
        }


        /// <summary>
        /// 使用Autofac 进行依赖注入
        /// </summary>
        protected static void AutofacIOC()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers. (MvcApplication is the name of
            // the class in Global.asax.)
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();



            //HTTP context and other related stuff
            builder.Register(c =>
                //register FakeHttpContext when HttpContext is not available
                HttpContext.Current != null ?
                (new HttpContextWrapper(HttpContext.Current) as HttpContextBase) :
                (new FakeHttpContext("~/") as HttpContextBase))
                .As<HttpContextBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Request)
                .As<HttpRequestBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Response)
                .As<HttpResponseBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Server)
                .As<HttpServerUtilityBase>()
                .InstancePerLifetimeScope();
            builder.Register(c => c.Resolve<HttpContextBase>().Session)
                .As<HttpSessionStateBase>()
                .InstancePerLifetimeScope();

            //web helper
            builder.RegisterType<WebHelper>().As<IWebHelper>().InstancePerLifetimeScope();



            #region 依赖注入数据层
            builder.RegisterType<ARModelRespository>().As<IARModelRespository>().InstancePerLifetimeScope();
            builder.RegisterType<ModelRespository>().As<IModelRespository>().InstancePerLifetimeScope();
            builder.RegisterType<MenuRespository>().As<IMenuRespository>().InstancePerLifetimeScope();
            builder.RegisterType<GroupRespository>().As<IGroupRespository>().InstancePerLifetimeScope();
            builder.RegisterType<OpLogRespository>().As<IOpLogRespository>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionRespository>().As<IPermissionRespository>().InstancePerLifetimeScope();
            builder.RegisterType<RoleRespository>().As<IRoleRespository>().InstancePerLifetimeScope();
            builder.RegisterType<UserDataRespository>().As<IUserDataRespository>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryRespository>().As<ICategoryRespository>().InstancePerLifetimeScope();
            builder.RegisterType<WeChatOrgRespository>().As<IWeChatOrgRespository>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerBasisRespository>().As<ITCustomerBasis>().InstancePerLifetimeScope();
            builder.RegisterType<WechatinfoRespository>().As<IWechatinfoRespository>().InstancePerLifetimeScope();
            builder.RegisterType<OpLogRespository>().As<IOpLogRespository>().InstancePerLifetimeScope();
            builder.RegisterType<SearchKeywordLogRespository>().As<ISearchKeywordLogRespository>().InstancePerLifetimeScope();

            #endregion


            #region 依赖注入服务层
            builder.RegisterType<ARModelService>().As<IARModelService>().InstancePerLifetimeScope();
            builder.RegisterType<VRModelService>().As<IVRModelService>().InstancePerLifetimeScope();
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();
            builder.RegisterType<GroupService>().As<IGroupService>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();        
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<WeChatOrgService>().As<IWeChatOrgService>().InstancePerLifetimeScope();
            builder.RegisterType<WxMonitorService>().As<IWxMonitorService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerBasicService>().As<ICustomerBasicService>().InstancePerLifetimeScope();
            builder.RegisterType<OpLogBLL>().As<IOpLogBLL>().InstancePerLifetimeScope();
            builder.RegisterType<SearchKeywordBLL>().As<ISearchKeywordBLL>().InstancePerLifetimeScope();


            #endregion

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            // OPTIONAL: Enable action method parameter injection (RARE).
           // builder.InjectActionInvoker();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

           // container.re
        }
    }
}
