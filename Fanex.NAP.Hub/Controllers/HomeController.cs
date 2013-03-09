// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.10.29">Created</change>
// </history>

namespace Fanex.NAP.Hub.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return base.RedirectToAction("Index", "Login", new { Area = "UserLogin" });
        }
    }
}
