// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;
    using System.Data.SqlClient;

    /// <summary>
    /// Note: For instructions on enabling IIS6 or IIS7 classic mode, visit http://go.microsoft.com/?LinkId=9394801 .
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }

        /// <summary>
        /// Application the start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DbSettingProviderManager.Current.Start();
        }

        /// <summary>
        /// Application error event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            // Logging
            Exception ex = Server.GetLastError().GetBaseException();

            if (!Request.IsLocal)
            {
                // Remove sensitive log data
                var formVariable = string.Empty;
                int length = Request.Form.Count;

                for (int i = 0; i < length; i++)
                {
                    if (!Request.Form.Keys[i].Equals("password", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i != 0)
                        {
                            formVariable += "&";
                        }
                        formVariable += string.Format("{0}={1}", Request.Form.Keys[i], Request.Form[Request.Form.Keys[i]]);
                    }
                }

                // Get base exception data
                string message = ex.Message +
                    "\r\nType: " + ex.GetType().ToString() +
                    "\r\nSource: " + ex.Source +
                    "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                    "\r\nForm Variable: " + formVariable +
                    "\r\nQuery String: " + Request.QueryString.ToString() +
                    "\r\nTarget Site: " + ex.TargetSite +
                    "\r\nStack Trace: " + ex.StackTrace;

                // Get additional exception data
                if (ex is SqlException)
                {
                    message += "\r\nProcedure: " + ((SqlException)ex).Procedure +
                        "\r\nLine Number: " + ((SqlException)ex).LineNumber +
                        "\r\nServer: " + ((SqlException)ex).Server;
                }

                LogService.Error(message);
            }
        }

        /// <summary>
        /// Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Session_End(object sender, EventArgs e)
        {
            var _currentSession = new CurrentSession();
            var _userService = new UserService();
            var logOutUserCriteria = new LogOutUserCriteria();

            logOutUserCriteria.OnUserId = _currentSession.OnUserId;
            _userService.UserLogOut(logOutUserCriteria);
            _currentSession.ClearAll();
        }

        /// <summary>
        /// Handles the AuthenticateRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity is FormsIdentity
                    && HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    HttpCookie httpCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];

                    if (httpCookie != null)
                    {
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(httpCookie.Value);

                        if (ticket == null || ticket.Expired || string.IsNullOrEmpty(ticket.UserData))
                        {
                            return;
                        }

                        var identity = new FormsIdentity(ticket);
                        var udata = AuthInfo.CreateUserData(ticket.UserData);

                        Context.User = new NapPrincipal(identity, udata);
                    }
                }
            }
        }
    }
}