// --------------------------------------------------------------------------
// <copyright file="UserSiteController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// User site controller.
    /// </summary>
    public class UserSiteController : BaseController
    {
        private ISiteService _siteService;
        private ICurrentSession _currentSession;
        private IUserService _userService;

        public UserSiteController()
            : this(null, null, null)
        {
        }

        public UserSiteController(ISiteService siteService, ICurrentSession currentSession, IUserService userService)
        {
            _siteService = siteService ?? new SiteService();
            _currentSession = currentSession ?? new CurrentSession();
            _userService = userService ?? new UserService();
        }

        /// <summary>
        /// View user sites.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult Site()
        {
            if (HttpContext.User.Identity.IsAuthenticated && _currentSession.UserId > 0)
            {
                IEnumerable<SiteUrl> siteUrlList = _siteService.GetSiteUrlByUser(_currentSession.UserId, this.Environment);

                // for re-checkin
                ViewBag.OnUserId = _currentSession.OnUserId; 

                return View(siteUrlList);
            }

            return RedirectToAction("logout", "login");
        }

        /// <summary>
        /// Forward user to a site to SSO.
        /// </summary>
        /// <param name="id">The site URL.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Forward(int id)
        {
            SiteUrl siteUrl = _siteService.GetSiteUrlById(id);

            if (siteUrl == null)
            {
                return HttpNotFound();
            }

            string time = _userService.UserSSOGetTime();

            return new RedirectResult(
                string.Format(
                    "{0}/userlogin/sso?userid={1}&onuserid={2}&time={3}",
                    siteUrl.Url.Trim(),
                    Cryptography.CfsEnCrypt(_currentSession.UserId.ToString()),
                    Cryptography.CfsEnCrypt(_currentSession.OnUserId.ToString()),
                    Cryptography.CfsEnCrypt(time)));
        }
    }
}