// --------------------------------------------------------------------------
// <copyright file="RouteConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.10.29">Style Cop</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Area_Start
{
    using System.Web.Mvc;

    /// <summary>
    /// Route config.
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(string areaName, AreaRegistrationContext context)
        {
            context.MapRoute(
                areaName + "_Default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}