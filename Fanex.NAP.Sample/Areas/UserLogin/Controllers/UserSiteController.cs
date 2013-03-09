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
    using System.Web;
    using System.Web.Mvc;

    using Fanex.Common;

    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    public class UserSiteController : BaseController
    {
        #region members

        private ISiteService _siteService = new SiteService();
        private ICurrentSession _currentSession = new CurrentSession();

        #endregion

        #region ctors

        public UserSiteController() : this(null, null) { }

        public UserSiteController(ISiteService siteService, ICurrentSession currentSession)
        {
            this._siteService = this._siteService ?? new SiteService();
            this._currentSession = this._currentSession ?? new CurrentSession();
        }

        #endregion

        #region actions

        public ActionResult Site()
        {
            if (HttpContext.User.Identity.IsAuthenticated && _currentSession.UserId > 0)
            {
                var SiteUrlList = _siteService.GetSiteUrlByUser(_currentSession.UserId, this._environment);

                ViewBag.OnUserId = _currentSession.OnUserId; // for re-checkin

                return View(SiteUrlList);
            }

            return RedirectToAction("logout", "login");
        }

        [HttpGet]
        public ActionResult Forward(string id)
        {
            int siteUrlId = Convert.ToInt32(id);
            var siteUrl = _siteService.GetSiteUrlById(siteUrlId);

            return new RedirectResult(
                string.Format("{0}/userlogin/sso?userid={1}&onuserid={2}&time={3}",
                    siteUrl.Url.Trim(),
                    Cryptography.CfsEnCrypt(_currentSession.UserId.ToString()),
                    Cryptography.CfsEnCrypt(_currentSession.OnUserId.ToString()),
                    Cryptography.CfsEnCrypt(DateTime.Now.ToString("G"))));
        }

        #endregion
    }
}