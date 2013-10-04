// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.10.29">Style Cop</change>
// </history>

namespace Fanex.NAP.Admin.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.NAP.Admin.Areas.UserControl.Controllers;

    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Response.Cookies.Add(new HttpCookie("menu-id") { Expires = DateTime.Now.AddDays(-1d) });

            return View();
        }
    }
}
