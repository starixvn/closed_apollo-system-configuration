// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Base controller.
    /// </summary>
    [NapSuperAuth]
    public class BaseController : Controller
    {
        #region Const
        
        protected const string EmptyString = "";

        protected const int NapAdminId = 1;

        protected readonly string Environment = NapSettings.Settings.Environment;

        protected readonly int SiteId = NapSettings.Settings.SiteId;

        #endregion

        #region Context
        
        protected readonly ICurrentContext CurrentContext;

        protected readonly ICurrentSession CurrentSession;

        protected readonly IServiceContext ServiceContext;

        #endregion

        private ViewFlags _viewFlags;

        private IUserService _userService;

        public BaseController()
        {
            CurrentContext = new CurrentContext();
            ViewBag.Session = CurrentSession = new CurrentSession();
            ViewBag.ServiceContext = ServiceContext = new ServiceContext
            {
                UserId = this.CurrentSession.UserId,
                SiteId = this.SiteId,
                Ip = this.CurrentContext.IP
            };

            ViewFlags = new ViewFlags
            {
                IsLive = Environment.Equals(Constant.LIVE),
                IsNapAdminSite = SiteId.Equals(NapAdminId),
                HasEditRight = false,
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };

            _userService = new UserService { ServiceContext = ServiceContext };
        }

        protected ViewFlags ViewFlags
        {
            get
            {
                return ViewBag.ViewFlags;
            }

            set
            {
                ViewBag.ViewFlags = _viewFlags = value;
            }
        }

        protected bool ThisSiteIsNapAdmin
        {
            get
            {
                return this.SiteId.Equals(NapAdminId);
            }
        }

        protected bool UserIsPortalAdmin
        {
            get
            {
                return CurrentSession.Level.Equals((int)Constant.Level.PortalAdmin);
            }
        }

        protected bool UserIsSiteAdmin
        {
            get
            {
                return CurrentSession.Level.Equals((int)Constant.Level.SiteAdmin);
            }
        }

        [ChildActionOnly]
        public PartialViewResult _Menu()
        {
            ViewBag.Menu = CurrentSession.Menu;

            return PartialView("~/Areas/UserControl/Views/Shared/_Menu.cshtml");
        }

        /// <summary>
        /// Determines whether the specified user has the right on a function.
        /// </summary>
        /// <param name="functionKey">The function key.</param>
        /// <param name="policy">The policy.</param>
        /// <returns>Whether the specified user has the right on a function.</returns>
        public bool HasRight(string functionKey, Constant.Policy policy)
        {
            int functionPolicy = _userService.GetUserSiteFunctionKeyPolicy(CurrentSession.UserId, this.SiteId, functionKey);

            return functionPolicy >= (int)policy;
        }

        /// <summary>
        /// Determines whether the specified user has the right on a function of a site.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <param name="functionKey">The function key.</param>
        /// <param name="policy">The policy.</param>
        /// <returns>Whether the specified user has the right on a function.</returns>
        public bool HasRightOnSite(int siteId, string functionKey, Constant.Policy policy)
        {
            int functionPolicy = _userService.GetUserSiteFunctionKeyPolicy(CurrentSession.UserId, siteId, functionKey);

            return functionPolicy >= (int)policy;
        }

    }
}