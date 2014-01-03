// --------------------------------------------------------------------------
// <copyright file="SSOController.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// SSO controller.
    /// </summary>
    public class SSOController : BaseController
    {
        private const int ExpiredDuration = 30;

        private IUserService _userService;
        private IMenuItemService _menuItemService;
        private ICurrentSession _currentSession;

        public SSOController()
            : this(null, null, null)
        {
        }

        public SSOController(IUserService userService, IMenuItemService menuItemService, ICurrentSession currentSession)
        {
            _userService = userService ?? new UserService();
            _menuItemService = menuItemService ?? new MenuItemService();
            _currentSession = currentSession ?? new CurrentSession();
        }

        /// <summary>
        /// Index page.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="onuserId">The online user ID.</param>
        /// <param name="time">The SSO time.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string userId, string onuserId, string time)
        {
            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onuserId = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            string currentTime = _userService.UserSSOGetTime();

            TimeSpan expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= ExpiredDuration)
            {
                // SSO check in
                var criteria = new UserSSOCheckInUserCriteria
                {
                    UserId = Convert.ToInt32(userId),
                    SiteId = SiteId,
                    CheckOnUserId = Convert.ToInt32(onuserId),
                    OnUserId = 0,
                    IP = CurrentContext.IP,
                    NetSessionId = Session.SessionID,
                    LoginDate = DateTime.Now,
                    BrowserName = CurrentContext.BrowserName,
                    CountryCode = CurrentContext.CountryCode,
                    Domain = CurrentContext.DomainName,
                    Platform = CurrentContext.Platform
                };

                _userService.UserSSOCheckIn(ref criteria);

                if (criteria.OnUserId != 0)
                {
                    User user = _userService.GetById(criteria.UserId) as User;

                    // Save user session and auth info
                    _currentSession.UserId = user.Id;
                    _currentSession.Username = user.Username;
                    _currentSession.OnUserId = criteria.OnUserId;
                    _currentSession.SessionId = Session.SessionID.ToString();
                    _currentSession.Level = _userService.GetUserSiteLevel(user.Id, this.SiteId);

                    if (!ReferenceEquals(user.Group, null))
                    {
                        _currentSession.GroupId = user.Group.Id;
                        _currentSession.GroupName = user.Group.Name;
                    }

                    if (this.SiteId != 0)
                    {
                        _currentSession.Menu = 
                            _menuItemService.GetByUserAndSiteIncludeGroup(user.Id, SiteId, Environment, true) as IList<MenuItem>;
                    }

                    CreateAuthInfo(user);

                    return Redirect(FormsAuthentication.DefaultUrl);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// Create authorize information.
        /// </summary>
        /// <param name="user">The user object.</param>
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
    }
}