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
    using System;
    using System.Collections.Generic;
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

    public class SSOController : BaseController
    {
        #region members

        private IUserService _userService;
        private IMenuItemService _menuItemService;
        private ICurrentSession _currentSession;

        #endregion

        #region ctors

        public SSOController()
            : this(null, null, null)
        {
        }

        public SSOController(IUserService userService, IMenuItemService menuItemService,
            ICurrentSession currentSession)
        {
            _userService = userService ?? new UserService();
            _menuItemService = menuItemService ?? new MenuItemService();
            _currentSession = currentSession ?? new CurrentSession();
        }

        #endregion

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string userId, string onUserId, string time)
        {
            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onUserId = Cryptography.CfsDeCrypt(onUserId.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));

            var expiration = DateTime.Now - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= 10)
            {
                #region do SSO check-in

                var criteria = new UserSSOCheckInUserCriteria
                        {
                            UserId = Convert.ToInt32(userId),
                            SiteId = _siteId,
                            CheckOnUserId = Convert.ToInt32(onUserId),
                            OnUserId = 0,
                            IP = _currentContext.IP,
                            NetSessionId = Session.SessionID,
                            LoginDate = DateTime.Now
                        };

                _userService.UserSSOCheckIn(ref criteria);

                #endregion

                if (criteria.OnUserId != 0)
                {
                    var user = _userService.GetById(criteria.UserId) as User;

                    #region saves user session

                    _currentSession.UserId = user.Id;
                    _currentSession.Username = user.Username;
                    _currentSession.OnUserId = criteria.OnUserId;
                    _currentSession.SessionId = Session.SessionID.ToString();
                    _currentSession.Level = _userService.GetUserSiteLevel(user.Id, this._siteId);

                    if (this._siteId != 0)
                    {
                        _currentSession.Menu =
                            _menuItemService.GetByUserAndSiteIncludeGroup(user.Id, _siteId,
                                _environment, true) as IList<MenuItem>;
                    }

                    #endregion

                    #region creates auth info

                    CreateAuthInfo(user);

                    #endregion

                    return Redirect(FormsAuthentication.DefaultUrl);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        private void CreateAuthInfo(User user)
        {
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
                FormsAuthentication.FormsCookiePath);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = FormsAuthentication.FormsCookiePath,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }
    }
}