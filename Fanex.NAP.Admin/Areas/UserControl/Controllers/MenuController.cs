// --------------------------------------------------------------------------
// <copyright file="MenuController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.9.11">Created</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Fanex.Logging;

    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.MenuItem;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Menu controller.
    /// </summary>
    [NapSuperAuth]
    public class MenuController : BaseController
    {
        #region Properties

        private readonly IMenuItemService _menuItemService;
        private readonly ISiteService _siteService;
        private readonly IFunctionService _functionService;
        private readonly IUserService _userService;

        #endregion

        #region Contructors

        public MenuController()
            : this(null, null, null, null)
        {
        }

        public MenuController(
            IMenuItemService menuitemService,
            ISiteService siteService,
            IFunctionService functionService,
            IUserService userService)
        {
            _menuItemService = menuitemService ?? new MenuItemService { ServiceContext = this.ServiceContext };
            _siteService = siteService ?? new SiteService { ServiceContext = this.ServiceContext };
            this._functionService = functionService ?? new FunctionService { ServiceContext = this.ServiceContext };
            _userService = userService ?? new UserService { ServiceContext = this.ServiceContext };
        }

        #endregion

        #region Manage Menu item

        /// <summary>
        /// Manage menu.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMenu, Policy = Constant.Policy.Read)]
        public ActionResult Manage(int? siteId)
        {
            var criteria = new GetBySiteMenuItemCriteria
            {
                SiteId = siteId ?? this.SiteId,
                Environment = this.Environment
            };

            try
            {
                if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
                {
                    ViewBag.Site = _siteService.GetAllSelectListItem(criteria.Environment, true, criteria.SiteId);
                    ViewBag.ViewFlags.HasEditRight = true;
                }
                else
                {
                    ViewBag.Site =
                        _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                            CurrentSession.UserId,
                                            FunctionKeys.ManageMenu.ToString(),
                                            criteria.Environment,
                                            policy: Constant.Policy.Read,
                                            selectedSiteId: criteria.SiteId);

                    // get user policy
                    int policy = _userService.GetUserSiteFunctionKeyPolicy(
                                                    CurrentSession.UserId,
                                                    criteria.SiteId,
                                                    FunctionKeys.ManageMenu.ToString());

                    int policyCurrentSite = _userService.GetUserSiteFunctionKeyPolicy(
                                                    CurrentSession.UserId,
                                                    this.SiteId,
                                                    FunctionKeys.ManageMenu.ToString());

                    ViewBag.ViewFlags.HasEditRight = policy >= (int)Constant.Policy.Host
                                                        && policyCurrentSite >= (int)Constant.Policy.Host;
                }

                ViewBag.MenuItemList = _menuItemService.GetBySiteInHierarchy(criteria.SiteId, criteria.Environment);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        /// <summary>
        /// Manage menu.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMenu, Policy = Constant.Policy.Read)]
        public ActionResult Manage(GetBySiteMenuItemCriteria criteria)
        {
            try
            {
                if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
                {
                    ViewBag.Site = _siteService.GetAllSelectListItem(criteria.Environment, true, criteria.SiteId);
                    ViewBag.ViewFlags.HasEditRight = true;
                }
                else
                {
                    ViewBag.Site =
                        _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                            CurrentSession.UserId,
                                            FunctionKeys.ManageMenu.ToString(),
                                            criteria.Environment,
                                            policy: Constant.Policy.Read,
                                            selectedSiteId: criteria.SiteId);

                    // get user policy
                    int policy = _userService.GetUserSiteFunctionKeyPolicy(
                                                    CurrentSession.UserId,
                                                    criteria.SiteId,
                                                    FunctionKeys.ManageMenu.ToString());

                    int policyCurrentSite = _userService.GetUserSiteFunctionKeyPolicy(
                                                    CurrentSession.UserId,
                                                    this.SiteId,
                                                    FunctionKeys.ManageMenu.ToString());

                    ViewBag.ViewFlags.HasEditRight = policy >= (int)Constant.Policy.Host
                                                        && policyCurrentSite >= (int)Constant.Policy.Host;
                }

                ViewBag.MenuItemList = _menuItemService.GetBySiteInHierarchy(criteria.SiteId, criteria.Environment);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        #endregion

        #region Add New Menu

        /// <summary>
        /// Add new menu item.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMenu, Policy = Constant.Policy.Host)]
        public ActionResult AddNewMenuItem(int? siteId = null)
        {
            var criteria = new InsertMenuItemCriteria
            {
                Environment = this.Environment,
                Policy = (int)Constant.Policy.Read,
                SiteId = siteId ?? this.SiteId
            };

            ViewBag.Parent = _menuItemService.GetSelectListItemBySite(criteria.SiteId, this.Environment, isActive: true);
            ViewBag.Function = this._functionService.GetSelectListItemBySite(criteria.SiteId, this.Environment, isActive: true);
            if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
            {
                ViewBag.Site = _siteService.GetAllSelectListItem(criteria.Environment, true, criteria.SiteId);
            }
            else
            {
                ViewBag.Site =
                    _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                        CurrentSession.UserId,
                                        FunctionKeys.ManageMenu.ToString(),
                                        criteria.Environment,
                                        policy: Constant.Policy.Host,
                                        selectedSiteId: criteria.SiteId);
            }

            return View(criteria);
        }

        /// <summary>
        /// Add new menu item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMenu, Policy = Constant.Policy.Host)]
        public ActionResult AddNewMenuItem(InsertMenuItemCriteria criteria)
        {
            try
            {
                ViewBag.Parent = _menuItemService.GetAllSelectListItem(this.Environment, isActive: true);
                ViewBag.Function = this._functionService.GetSelectListItemBySite(criteria.SiteId, this.Environment, isActive: true);
                if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
                {
                    ViewBag.Site = _siteService.GetAllSelectListItem(criteria.Environment, true, criteria.SiteId);
                }
                else
                {
                    ViewBag.Site =
                        _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                            CurrentSession.UserId,
                                            FunctionKeys.ManageMenu.ToString(),
                                            criteria.Environment,
                                            policy: Constant.Policy.Host,
                                            selectedSiteId: criteria.SiteId);
                }

                if (ModelState.IsValid)
                {
                    // Add new
                    criteria.CreatedBy = CurrentSession.UserId;
                    criteria.CreatedDate = DateTime.Now;
                    criteria.ParentId = criteria.ParentId == 0 ? null : criteria.ParentId;
                    criteria.ParentName = criteria.ParentId == 0 || criteria.ParentId == null ? null
                        : criteria.ParentName;

                    var result = _menuItemService.Insert(criteria);

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

        #region Edit Menu Item

        /// <summary>
        /// Update menu item.
        /// </summary>
        /// <param name="menuItemId">Menu item ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMenu, Policy = Constant.Policy.Host)]
        public ActionResult EditMenuItem(int menuItemId)
        {
            var criteria = new UpdateMenuItemCriteria();

            try
            {
                var menuItem = (MenuItem)_menuItemService.GetById(menuItemId);

                if (menuItem == null)
                {
                    return RedirectToAction("Manage");
                }

                criteria.MenuItemId = menuItem.Id;
                criteria.MenuItemName = menuItem.Name;
                criteria.Description = menuItem.Description;
                criteria.DisplayOrder = menuItem.DisplayOrder;
                criteria.FunctionId = menuItem.Function.Id;
                criteria.IsActive = menuItem.IsActive;
                criteria.ParentId = menuItem.Parent != null ? menuItem.Parent.Id : 0;
                criteria.ParentName = menuItem.Parent != null ? menuItem.Parent.Name : string.Empty;
                criteria.Policy = menuItem.Policy;
                criteria.SiteId = menuItem.Site.Id;
                criteria.SiteName = menuItem.Site.Name;
                criteria.Url = menuItem.Url;
                criteria.Environment = menuItem.Environment;
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            ViewBag.Function = this._functionService.GetSelectListItemBySite(
                                                        criteria.SiteId,
                                                        criteria.Environment,
                                                        isActive: true,
                                                        selectedFunctionId: criteria.FunctionId);

            if (criteria.ParentId == null)
            {
                ViewBag.Parent = _menuItemService.GetParentSelectListItemById(id: criteria.MenuItemId, env: criteria.Environment);
            }
            else
            {
                ViewBag.Parent = _menuItemService.GetParentSelectListItemById(criteria.MenuItemId, (int)criteria.ParentId, criteria.Environment);
            }

            return View(criteria);
        }

        /// <summary>
        /// Update menu item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMenu, Policy = Constant.Policy.Host)]
        public ActionResult EditMenuItem(UpdateMenuItemCriteria criteria)
        {
            try
            {
                ViewBag.Function = this._functionService.GetSelectListItemBySite(
                                                            criteria.SiteId,
                                                            criteria.Environment,
                                                            isActive: true,
                                                            selectedFunctionId: criteria.FunctionId);

                if (criteria.ParentId == null)
                {
                    ViewBag.Parent = _menuItemService.GetParentSelectListItemById(criteria.MenuItemId);
                }
                else
                {
                    ViewBag.Parent = _menuItemService.GetParentSelectListItemById(criteria.MenuItemId, (int)criteria.ParentId);
                }

                if (ModelState.IsValid)
                {
                    // Update menu item
                    criteria.ModifierId = CurrentSession.UserId;
                    criteria.ModifiedDate = DateTime.Now;
                    criteria.ParentId = criteria.ParentId == 0 ? null : criteria.ParentId;
                    criteria.ParentName = criteria.ParentId == 0 || criteria.ParentId == null ? null
                        : criteria.ParentName;

                    var result = _menuItemService.Update(criteria);

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

        #region AjaxRequest

        /// <summary>
        /// Get list menu item by site id.
        /// </summary>
        /// <param name="siteId">Site ID.</param>
        /// <param name="menuItemId">Menu item ID.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMenu, Policy = Constant.Policy.Host)]
        public ActionResult ListMenuItemBySite(int? siteId, int? menuItemId, string environment = EmptyString)
        {
            if (Request.IsAjaxRequest())
            {
                List<IMenuItem> menuItemList = new List<IMenuItem>();

                environment = !string.IsNullOrEmpty(environment) ? environment : this.Environment;

                try
                {
                    menuItemList.Add(new MenuItem { Id = 0, Name = ViewResources.None });
                    if (menuItemId != null)
                    {
                        menuItemList.AddRange(_menuItemService.GetParentById(
                                        (int)menuItemId,
                                        env: environment).ToList());
                    }
                    else
                    {
                        menuItemList.AddRange(_menuItemService.GetBySite((int)siteId, environment).ToList());
                    }
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }

                return Json(menuItemList);
            }

            return Json(new List<IMenuItem>());
        }

        /// <summary>
        /// Get list function by site id.
        /// </summary>
        /// <param name="siteId">Site ID.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMenu, Policy = Constant.Policy.Host)]
        public ActionResult ListFunctionBySite(int siteId, string environment = EmptyString)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    return Json(this._functionService.GetBySite(
                                        siteId,
                                        !string.IsNullOrEmpty(environment) ? environment : this.Environment,
                                        isActive: true));
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            return Json(null);
        }

        /// <summary>
        /// Update status menu.
        /// </summary>
        /// <param name="id">Menu item ID.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMenu, Policy = Constant.Policy.Host)]
        public ActionResult Activate(int id, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    var result = _menuItemService.UpdateStatus(new UpdateStatusMenuItemCriteria
                    {
                        IsActive = isActive,
                        MenuItemId = id,
                        ModifiedBy = CurrentSession.UserId,
                        ModifiedDate = DateTime.Now
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