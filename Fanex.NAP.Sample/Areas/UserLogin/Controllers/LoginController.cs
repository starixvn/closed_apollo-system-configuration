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
    using System.Linq;
    using Fanex.Common.Extensions;
    using Fanex.Common;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.Logging;

    public class LoginController : BaseController
    {
        #region members

        private IUserService _userService;
        private ISiteService _siteService;
        private IMenuItemService _menuItemService;
        private ICurrentSession _currentSession;

        #endregion

        #region ctors

        public LoginController()
            : this(null, null, null, null) { }

        public LoginController(IUserService userService, ISiteService siteService,
            IMenuItemService menuItemService, ICurrentSession currentSession)
        {
            _userService = userService ?? new UserService();
            _siteService = siteService ?? new SiteService();
            _menuItemService = menuItemService ?? new MenuItemService();
            _currentSession = currentSession ?? new CurrentSession();
        }

        #endregion

        #region helpers

        private void CreateUserMenu(User user)
        {
            _currentSession.MenuItemList = (IList<MenuItem>)_menuItemService.GetByUserAndSiteIncludeGroup(user.Id,
                _siteId, _environment, true);
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

        #endregion

        #region sign in
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (!username.IsNullOrTrimEmpty() && !password.IsNullOrTrimEmpty())
            {
                var user = _userService.GetByUsername(username.Trim()) as User;

                try
                {
                    if (user != null)
                    {
                        password = Cryptography.CfsEnCode(password);

                        if (password == user.Password)
                        {
                            var siteLevelList = _siteService.GetSiteLevelByUser(user.Id, _environment); // gets all sites of logging-in user

                            if (siteLevelList.Where(x => x.IsActive).Count() > 0)
                            {
                                #region allows user login

                                #region check-in user
                                var criteria = new UserCheckInUserCriteria
                                {
                                    UserId = user.Id,
                                    SiteId = this._siteId,
                                    NetSessionId = Session.SessionID,
                                    Ip = _currentContext.IP,
                                    LoginDate = DateTime.Now
                                };

                                _userService.UserCheckIn(ref criteria);
                                #endregion

                                #region saves user session
                                _currentSession.Level = _userService.GetUserSiteLevel(user.Id, this._siteId);
                                _currentSession.UserId = user.Id;
                                _currentSession.Username = username;
                                _currentSession.OnUserId = criteria.OnUserId;
                                _currentSession.SessionId = Session.SessionID.ToString();

                                if (this._siteId != 0) // current site is not NAP Hub
                                {
                                    CreateUserMenu(user);
                                }

                                CreateAuthInfo(user);
                                #endregion

                                #region redirects user
                                if (this.EnableDirectLogin) // if the user login directly, not through NAP Hub
                                {
                                    #region checks if logging-in user is active on this site
                                    bool userIsActiveOnThisSite = false;

                                    foreach (var siteLevel in siteLevelList)
                                    {
                                        if (siteLevel.Site.Id == this._siteId && siteLevel.IsActive)
                                        {
                                            userIsActiveOnThisSite = true;
                                        }
                                    }
                                    #endregion

                                    if (userIsActiveOnThisSite)
                                    {
                                        return Redirect(FormsAuthentication.DefaultUrl);
                                    }
                                    else
                                    {
                                        DoSignOut();
                                        ViewBag.Message = ViewResources.Your_account_was_suspended;
                                    }
                                }
                                else
                                {
                                    if (siteLevelList.Count() > 1) // user has access to multiple sites
                                    {
                                        return RedirectToAction("Site", "UserSite");
                                    }

                                    return RedirectToAction("GoToSite", "UserSite", new { id = siteLevelList.First().Site.Id });
                                }
                                #endregion

                                #endregion
                            }
                            else
                            {
                                if (siteLevelList.Count() > 0) // user account was suspended on all sites
                                {
                                    ViewBag.Message = ViewResources.Your_account_was_suspended;
                                }
                                else // logging-in user belongs to no site
                                {
                                    ViewBag.Message = ViewResources.You_have_no_authorization;
                                }
                            }
                        }
                        else // wrong password
                        {
                            ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                        }
                    }
                    else // user doesn't exist
                    {
                        ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    }
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }
            else
            {
                ViewBag.Message = "Invalid input!";
            }
            return View();
        }

        #endregion

        #region sign out

        public ActionResult LogOut()
        {
            DoSignOut();

            return Redirect(FormsAuthentication.LoginUrl);
        }

        private void DoSignOut()
        {
            _userService.UserLogOut(new LogOutUserCriteria
            {
                OnUserId = _currentSession.OnUserId
            });
            FormsAuthentication.SignOut();
            _currentSession.ClearAll();
        }

        #endregion

        #region re check in

        [HttpPost]
        public ActionResult ReCheckLogin()
        {
            var siteStatus = _siteService.GetStatusById(this._siteId);

            if (!siteStatus)
            {
                return Json("1");
            }

            UserReCheckInUserCriteria criteria = new UserReCheckInUserCriteria();
            criteria.OnUserId = Convert.ToInt32(_currentSession.OnUserId);
            _userService.ReUserCheckIn(ref criteria);

            return Json(criteria.IsKickedOut ? "1" : "0");
        }

        #endregion
    }
}