// --------------------------------------------------------------------------
// <copyright file="MvcApplication.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
//   <change who="Edward.Nguyen" date="2012.8.14">Add GetRepositoryExecLog</change>
// </history>

namespace Fanex.NAP.Hub
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();

            Fanex.Data.Configuration.DbSettingProviderManager.Current.Start();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the exception object.
            Exception exception = Server.GetLastError();
            var code = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;
            if (code != 404)
            {
                if (exception.InnerException != null)
                {
                    Logging.LogService.Error(exception.InnerException.Message, exception.InnerException);
                }
                else
                {
                    Logging.LogService.Error(exception.Message, exception);
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            ICurrentSession _currentSession = new CurrentSession();
            IUserService _userService = new UserService();
            LogOutUserCriteria logOutUserCriteria = new LogOutUserCriteria();
            logOutUserCriteria.OnUserId = _currentSession.OnUserId;
            _userService.UserLogOut(logOutUserCriteria);
            _currentSession.ClearAll();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity
                    && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    string cookie = FormsAuthentication.FormsCookieName;
                    HttpCookie httpCookie = Context.Request.Cookies[cookie];

                    if (httpCookie == null)
                    {                        
                        return;
                    }

                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(httpCookie.Value);
                    if (ticket == null || ticket.Expired)
                    {                     
                        return;
                    }

                    FormsIdentity identity = new FormsIdentity(ticket);
                    if (string.IsNullOrEmpty(ticket.UserData))
                    {                     
                        return;
                    }

                    AuthInfo udata = AuthInfo.CreateUserData(ticket.UserData);
                    NapPrincipal principal = new NapPrincipal(identity, udata);
                    Context.User = principal;                   
                }
            }
        }
    }
}