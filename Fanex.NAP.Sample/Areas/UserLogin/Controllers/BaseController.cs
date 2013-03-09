// --------------------------------------------------------------------------
// <copyright file="GroupController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Web.Context;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BaseController : Controller
    {
        protected readonly string _environment = NapSettings.Settings.Environment;

        protected readonly int _siteId = NapSettings.Settings.SiteId;

        protected readonly int _isSiteLogin = NapSettings.Settings.IsSiteLogin;

        protected readonly CurrentContext _currentContext;

        public BaseController()
        {
            this._currentContext = new CurrentContext();
        }

        /// <summary>
        /// Indicates whether the current site allows users login directly.
        /// If "0", users must login through NAP Hub
        /// </summary>
        public int IsSiteLogin
        {
            get
            {
                return this._isSiteLogin;
            }
        }
    }
}