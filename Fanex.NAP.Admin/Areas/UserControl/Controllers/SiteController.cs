// --------------------------------------------------------------------------
// <copyright file="SiteController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Created</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Fanex.Logging;

    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Site;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Site controller.
    /// </summary>
    [NapSuperAuth]
    public class SiteController : BaseController
    {
        #region Members

        private readonly ISiteService _siteService;
        private readonly IUserService _userService;

        #endregion

        #region Constructors

        public SiteController()
            : this(null, null)
        {
        }

        public SiteController(ISiteService siteService = null, IUserService userService = null)
        {
            _siteService = siteService ?? new SiteService { ServiceContext = ServiceContext };
            _userService = userService ?? new UserService { ServiceContext = ServiceContext };
        }

        #endregion

        #region Manage

        /// <summary>
        /// Manage site.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Read)]
        public ActionResult Manage(GetAllSiteCriteria criteria)
        {
            try
            {
                int policy = _userService.GetUserSiteFunctionKeyPolicy(
                                                CurrentSession.UserId,
                                                this.SiteId,
                                                FunctionKeys.ManageSite.ToString());

                ViewBag.Site = _siteService.GetByUserAndFunctionKeyIncludeGroup(
                                                        CurrentSession.UserId,
                                                        FunctionKeys.ManageUser.ToString(),
                                                        this.Environment,
                                                        policy: Constant.Policy.Read).Select(s => s.Id).ToList();

                var hasManageUser = _userService.GetUserSiteFunctionKeyPolicy(
                   CurrentSession.UserId,
                   SiteId,
                   FunctionKeys.ManageUser.ToString()) >= 1 ? true : false;

                ViewFlags.HasEditRight = policy >= (int)Constant.Policy.Host;
                ViewFlags.HasManageUserRight = hasManageUser;

                ViewBag.SiteList = _siteService.GetAllWithNumOfUsers(this.Environment).OrderBy(s => s.Id).ToList();
                ViewBag.SiteUrlList = _siteService.GetAllSiteUrl(this.Environment).ToList();
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        #endregion

        #region Add New

        /// <summary>
        /// Add new site.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult AddNew()
        {
            return View(new InsertSiteCriteria
            {
                Environment = this.Environment,
                Urls = new List<SiteUrl> { new SiteUrl { IsActive = true, IsDefault = true } }
            });
        }

        /// <summary>
        /// Add new site.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(InsertSiteCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                // check url
                if (!ReferenceEquals(criteria.Urls, null))
                {
                    foreach (var itemUrl in criteria.Urls)
                    {
                        bool isuri = Uri.IsWellFormedUriString(itemUrl.Url, UriKind.Absolute);
                        if (!isuri)
                        {
                            ModelState.AddModelError("SiteUrls", string.Format(ViewResources.InvalidURLValidationMsg, criteria.SiteUrls));
                            return View(criteria);
                        }
                    }
                }

                // checks SiteName duplication
                string siteName = criteria.SiteName = criteria.SiteName.Trim();
                var site = _siteService.GetByName(siteName);

                if (site != null)
                {
                    ModelState.AddModelError("SiteName", string.Format(ViewResources.DuplicateMsg, siteName));

                    return View(criteria);
                }

                // checks url duplication
                criteria.Urls = criteria.Urls.Where(x => !string.IsNullOrWhiteSpace(x.Url)).ToList();

                if (criteria.Urls != null)
                {
                    int distinctCount = criteria.Urls.Select(x => x.Url).Distinct().Count();

                    if (distinctCount != criteria.Urls.Count)
                    {
                        ModelState.AddModelError("Urls", ViewResources.The_Urls_must_be_unique);

                        return View(criteria);
                    }
                }

                // add new site
                criteria.Environment = this.Environment;

                var result = _siteService.Insert(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            return View(criteria);
        }

        #endregion

        #region Edit

        /// <summary>
        /// Update site.
        /// </summary>
        /// <param name="id">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int id)
        {
            var criteria = new UpdateSiteCriteria();

            try
            {
                var site = _siteService.GetById(id);

                if (site == null)
                {
                    return RedirectToAction("NotFound", "Common");
                }

                criteria.SiteId = site.Id;
                criteria.SiteName = site.Name;
                criteria.Description = site.Description;
                criteria.IsActive = site.IsActive;
                criteria.Environment = site.Environment;
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        /// <summary>
        /// Update site.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult Edit(UpdateSiteCriteria criteria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // checks SiteName duplication
                    string siteName = criteria.SiteName = criteria.SiteName.Trim();
                    var site = _siteService.GetByName(siteName);

                    if (site != null && site.Id != criteria.SiteId)
                    {
                        ModelState.AddModelError("SiteName", string.Format(ViewResources.DuplicateMsg, siteName));

                        return View(criteria);
                    }

                    // updates site
                    var result = _siteService.Update(criteria);

                    if (result != null && result.IsSuccessful)
                    {
                        ViewBag.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        #endregion

        /// <summary>
        /// Update site URL.
        /// </summary>
        /// <param name="id">The site URL ID.</param>
        /// <returns>Action result.</returns>
        #region SiteUrl
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Read)]
        public ActionResult EditUrl(int id)
        {
            try
            {
                var site = _siteService.GetById(id);

                if (site == null)
                {
                    return HttpNotFound();
                }

                ViewBag.SiteId = site.Id;
                ViewBag.SiteName = site.Name;
                ViewBag.Urls = _siteService.GetSiteUrlBySiteId(id, this.Environment).ToList();
                ViewFlags.SetEditRight(_userService.GetUserSiteFunctionKeyPolicy(
                                                                        CurrentSession.UserId,
                                                                        this.SiteId,
                                                                        FunctionKeys.ManageSite.ToString()));
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View();
        }

        /// <summary>
        /// Update site URL.
        /// </summary>
        /// <param name="id">The site URL ID.</param>
        /// <param name="url">The site URL.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult UpdateSiteUrl(int id, string url, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                var siteUrl = _siteService.GetSiteUrlById(id);

                if (siteUrl != null)
                {
                    url = url.Trim();

                    string result = Constant.OK;

                    // Check URL
                    bool isuri = Uri.IsWellFormedUriString(url, UriKind.Absolute);
                    if (!isuri)
                    {
                        result = "invalid";
                    }

                    if (isuri)
                    {
                        // Check URL duplication
                        var altUrls = _siteService.GetSiteUrlBySiteId(siteUrl.Site.Id, siteUrl.Environment);

                        if (altUrls.Where(x => x.SiteUrlId != id && x.Url == url).Any())
                        {
                            result = "duplicate";
                        }

                        // Update
                        if (result == Constant.OK)
                        {
                            _siteService.UpdateSiteUrl(new UpdateSiteUrlCriteria
                            {
                                SiteUrlId = id,
                                SiteId = siteUrl.Site.Id,
                                Url = url,
                                Environment = siteUrl.Environment,
                                IsActive = isActive,
                                IsDefault = siteUrl.IsDefault
                            });
                        }
                    }

                    return Json(result);
                }
            }

            return HttpNotFound();
        }

        /// <summary>
        /// Set default site URL.
        /// </summary>
        /// <param name="id">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult SetDefaultSiteUrl(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var siteUrl = _siteService.GetSiteUrlById(id);

                if (siteUrl != null)
                {
                    _siteService.UpdateDefaultSiteUrl(new UpdateDefaultSiteUrlCriteria
                    {
                        SiteUrlId = id,
                        SiteId = siteUrl.Site.Id,
                        Environment = this.Environment
                    });

                    return Json(Constant.OK);
                }
            }

            return HttpNotFound();
        }

        /// <summary>
        /// Add site URL.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <param name="url">The site URL.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult AddSiteUrl(int siteId, string url, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                var site = _siteService.GetById(siteId);

                if (site != null)
                {
                    url = url.Trim();

                    string result = Constant.OK;

                    // check url
                    bool isuri = Uri.IsWellFormedUriString(url, UriKind.Absolute);
                    if (!isuri)
                    {
                        result = "invalid";
                    }

                    if (isuri)
                    {
                        // checks url duplication
                        var siteUrls = _siteService.GetSiteUrlBySiteId(site.Id, this.Environment);

                        if (siteUrls.Where(x => x.Url == url).Any())
                        {
                            result = "duplicate";
                        }

                        // add site url
                        if (result == Constant.OK)
                        {
                            var criteria = new InsertSiteUrlCriteria
                            {
                                SiteId = siteId,
                                Url = url,
                                Environment = this.Environment,
                                IsActive = isActive,
                                IsDefault = false
                            };

                            var cmdResult = _siteService.InsertSiteUrl(criteria);

                            if (cmdResult != null && cmdResult.IsSuccessful)
                            {
                                result = criteria.SiteUrlId.ToString();
                            }
                            else
                            {
                                result = "error";
                            }
                        }
                    }

                    return Json(result);
                }
            }

            return HttpNotFound();
        }

        #endregion

        #region Activation

        /// <summary>
        /// Update site status.
        /// </summary>
        /// <param name="id">The site ID.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult Activate(int id, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    var result = _siteService.UpdateStatus(new UpdateStatusSiteCriteria
                    {
                        SiteId = id,
                        IsActive = isActive
                    });

                    if (result != null && result.IsSuccessful)
                    {
                        return Json(Constant.OK);
                    }
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            return new EmptyResult();
        }

        #endregion
    }
}