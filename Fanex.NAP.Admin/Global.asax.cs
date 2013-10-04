// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.10.29">Style Cop</change>
// </history>

namespace Fanex.NAP.Admin
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Fanex.Data.Configuration;
    using Fanex.NAP.Admin.App_Start;

    /// <summary>
    /// MVC application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbSettingProviderManager.Current.Start();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            AuthConfig.SignOut();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            AuthConfig.RegisterAuth(Context);
        }
    }
}