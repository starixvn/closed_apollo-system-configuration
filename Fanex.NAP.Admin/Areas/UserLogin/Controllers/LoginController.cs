// --------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Login controller.
    /// </summary>
    public class LoginController : BaseController
    {
        #region members

        private IUserService _userService;
        private IGroupService _groupService;
        private ISiteService _siteService;
        private IMenuItemService _menuItemService;
        private ICurrentSession _currentSession;
        private IMaintenanceService _maintenanceService;

        #endregion

        #region ctors

        public LoginController()
            : this(null, null, null, null, null, null)
        {
        }

        public LoginController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession,
            IMaintenanceService maintenanceService)
        {
            _userService = userService ?? new UserService();
            _groupService = groupService ?? new GroupService();
            _siteService = siteService ?? new SiteService();
            _menuItemService = menuItemService ?? new MenuItemService();
            this._currentSession = currentSession ?? new CurrentSession();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        #endregion

        /// <summary>
        /// Index page.
        /// </summary>
        /// <returns>Action result.</returns>
        #region sign in
        [HttpGet]
        public ActionResult Index()
        {
            if (!NapSettings.Settings.IsNapHub && !EnableDirectLogin)
            {
                // if login-directly feature is disabled then redirect users to NAP Hub
                return Redirect(NapSettings.Settings.HubUrl);
            }

            if (_maintenanceService.GetDowntimeInfo().IsDowntime)
            {
                return RedirectToAction("Offline", "Common");
            }

            return View();
        }

        /// <summary>
        /// Index page.
        /// </summary>
        /// <param name="username">The login username.</param>
        /// <param name="password">The login password.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string username, string password)
        {
            var user = _userService.Authenticate(username, password) as User;

            if (!ReferenceEquals(user, null))
            {
                var group = user.Group == null ? null : _groupService.GetById(user.Group.Id);
                var siteLevelList = _siteService.GetSiteLevelByUser(user.Id, Environment); // gets all sites of logging-in user

                if ((group == null || group.IsActive) && siteLevelList.Where(x => x.IsActive).Any())
                {
                    // allows user login

                    // check-in user
                    var criteria = new UserCheckInUserCriteria
                    {
                        UserId = user.Id,
                        SiteId = this.SiteId,
                        NetSessionId = Session.SessionID,
                        Ip = CurrentContext.IP,
                        LoginDate = DateTime.Now,
                        BrowserName = CurrentContext.BrowserName,
                        CountryCode = CurrentContext.CountryCode,
                        Domain = CurrentContext.DomainName,
                        Platform = CurrentContext.Platform
                    };

                    _userService.UserCheckIn(ref criteria);

                    // saves user session
                    this._currentSession.Level = _userService.GetUserSiteLevel(user.Id, this.SiteId);
                    this._currentSession.UserId = user.Id;
                    this._currentSession.Username = username;
                    this._currentSession.OnUserId = criteria.OnUserId;
                    this._currentSession.SessionId = Session.SessionID;

                    if (!ReferenceEquals(user.Group, null))
                    {
                        _currentSession.GroupId = user.Group.Id;
                        _currentSession.GroupName = user.Group.Name;
                    }

                    // current site is not NAP Hub
                    if (this.SiteId != 0)
                    {
                        _currentSession.Menu =
                            (IList<MenuItem>)_menuItemService.GetByUserAndSiteIncludeGroup(user.Id, SiteId, Environment, true);
                    }

                    CreateAuthInfo(user);

                    // redirects user if the user login directly, not through NAP Hub
                    if (this.EnableDirectLogin)
                    {
                        // checks if logging-in user is active on this site
                        if (siteLevelList.Where(x => x.Site.Id == this.SiteId && x.IsActive).Any())
                        {
                            return Redirect(FormsAuthentication.DefaultUrl);
                        }

                        DoSignOut();
                        ViewBag.Message = ViewResources.Your_account_was_suspended;
                    }
                    else
                    {
                        // user has access to multiple sites
                        if (siteLevelList.Where(s => s.IsActive).Count() > 1)
                        {
                            return RedirectToAction("Site", "UserSite");
                        }

                        // user has access to one site
                        var site = siteLevelList.Where(s => s.IsActive).First().Site;

                        var listSiteUrl = _siteService.GetSiteUrlBySiteId(site.Id, this.Environment).Where(u => u.IsActive).ToList();

                        if (listSiteUrl.Count() > 1)
                        {
                            return RedirectToAction("Site", "UserSite");
                        }

                        return RedirectToAction("Forward", "UserSite", new { id = listSiteUrl.First().SiteUrlId });
                    }
                }
                else if (siteLevelList.Any())
                {
                    // user account was suspended on all sites
                    ViewBag.Message = ViewResources.Your_account_was_suspended;
                }
                else
                {
                    // logging-in user belongs to no site
                    ViewBag.Message = ViewResources.You_have_no_authorization;
                }
            }
            else
            {
                ViewBag.Message = ViewResources.Invalid_Username_or_Password;
            }

            return View();
        }

        #endregion

        /// <summary>
        /// User log out.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult LogOut()
        {
            DoSignOut();

            return Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// User re-check login.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ReCheckLogin()
        {
            var maintenanceInfo = _maintenanceService.GetDowntimeInfo();

            if (maintenanceInfo.IsDowntime)
            {
                return Json("-1");
            }

            // check if not nap hub
            if (this.SiteId != 0)
            {
                var siteStatus = _siteService.GetStatusById(this.SiteId);

                if (!siteStatus)
                {
                    return Json("1");
                }
            }

            var criteria = new UserReCheckInUserCriteria();
            criteria.OnUserId = Convert.ToInt32(this._currentSession.OnUserId);
            _userService.ReUserCheckIn(ref criteria);

            return Json(criteria.IsKickedOut ? "1" : "0");
        }

        #region helpers

        /// <summary>
        /// Create authorize information.
        /// </summary>
        /// <param name="user">The login user.</param>
        private void CreateAuthInfo(User user)
        {
            string domainPath = CurrentContext.GetCurrentDomain().Path;
            var authInfo = new AuthInfo();

            authInfo.UserId = user.Id;
            authInfo.OnUserId = this._currentSession.OnUserId;

            var authTicket = new FormsAuthenticationTicket(
                1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                domainPath);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = domainPath,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }

        private void DoSignOut()
        {
            var criteria = new LogOutUserCriteria
            {
                OnUserId = this._currentSession.OnUserId
            };

            _userService.UserLogOut(criteria);
            FormsAuthentication.SignOut();
            this._currentSession.ClearAll();
        }

        #endregion
    }
}