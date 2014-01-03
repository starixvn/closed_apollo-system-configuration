// --------------------------------------------------------------------------
// <copyright file="FunctionController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Function;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Helpers;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Function controller.
    /// </summary>
    [NapSuperAuth]
    public class FunctionController : BaseController
    {
        #region Members

        private readonly IFunctionService _functionService;
        private readonly ISiteService _siteService;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;

        #endregion

        #region Constructors

        public FunctionController()
            : this(null, null, null, null)
        {
        }

        public FunctionController(
            IFunctionService functionService,
            ISiteService siteService,
            IUserService userService,
            IGroupService groupService)
        {
            this._functionService = functionService ?? new FunctionService { ServiceContext = ServiceContext };
            _siteService = siteService ?? new SiteService { ServiceContext = ServiceContext };
            _userService = userService ?? new UserService { ServiceContext = ServiceContext };
            _groupService = groupService ?? new GroupService { ServiceContext = ServiceContext };
        }

        #endregion

        #region Manage

        /// <summary>
        /// Manage function.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public ActionResult Manage(int? siteId)
        {
            var criteria = new GetBySiteFunctionCriteria
            {
                SiteId = siteId ?? this.SiteId,
                Environment = this.Environment
            };

            PrepareManageData(criteria);

            return View(criteria);
        }

        /// <summary>
        /// Manage function.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public ActionResult Manage(GetBySiteFunctionCriteria criteria)
        {
            PrepareManageData(criteria);

            return View(criteria);
        }

        #endregion

        #region Add new

        /// <summary>
        /// Add new function.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(int? siteId = null)
        {
            var criteria = new InsertFunctionCriteria
            {
                SiteId = siteId ?? this.SiteId,
                Environment = this.Environment
            };

            GetSitesAndFunctionsForAddNew(criteria);

            return View(criteria);
        }

        /// <summary>
        /// Add new function.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(InsertFunctionCriteria criteria)
        {
            GetSitesAndFunctionsForAddNew(criteria);

            if (ModelState.IsValid)
            {
                // checks duplicate function name
                criteria.FunctionName = criteria.FunctionName.Trim();
                IFunction function = this._functionService.GetByNameAndSite(criteria.FunctionName, criteria.SiteId);

                if (function != null)
                {
                    ModelState.AddModelError("FunctionName", string.Format(ViewResources.DuplicateMsg, criteria.FunctionName));
                    return View(criteria);
                }

                // checks duplicate function key
                criteria.FunctionKey = GenerateFunctionKey(criteria.FunctionName);
                function = this._functionService.GetByKeyAndSite(criteria.FunctionKey, criteria.SiteId);

                if (function != null)
                {
                    ModelState.AddModelError("FunctionKey", string.Format(ViewResources.DuplicateMsg, criteria.FunctionKey));
                    return View(criteria);
                }

                // adds function
                criteria.ParentId = criteria.ParentId == 0 ? null : criteria.ParentId;
                criteria.ParentName = (criteria.ParentId == null || criteria.ParentId == 0) ? null : criteria.ParentName;
                criteria.CreatedBy = CurrentSession.UserId;
                criteria.CreatedDate = DateTime.Now;

                // inserts function and assigns it to all site-admins of the current site and all portal admins
                ICommandResult result = this._functionService.Insert(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            return View(criteria);
        }

        #endregion

        #region Duplicate Function

        /// <summary>
        /// Duplicate function.
        /// </summary>
        /// <param name="id">The function ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult Duplicate(int id, int? siteId = null)
        {
            IFunction function = this._functionService.GetById(id);

            if (function == null)
            {
                return HttpNotFound();
            }

            ViewBag.OriginFunctionName = function.Name;

            ISite site = _siteService.GetById(function.Site.Id);

            var criteria = new DuplicateFunctionCriteria
            {
                FunctionId = function.Id,
                FunctionName = function.Name + " Copy",
                Description = function.Description,
                SiteId = this.SiteId,
                SiteName = site.Name,
                Readonly = function.Crud == (byte)Constant.Crud.Read,
                ParentId = function.Parent != null ? (int?)function.Parent.Id : null,
                DisplayOrder = function.DisplayOrder,
                IsActive = function.IsActive,
                Environment = function.Environment,
                ReturnSiteId = siteId ?? function.Site.Id
            };

            GetSitesAndFunctionsForAddNew(criteria, getSiteList: false);

            return View(criteria);
        }

        /// <summary>
        /// Duplicate function.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult Duplicate(DuplicateFunctionCriteria criteria)
        {
            IFunction function = this._functionService.GetById(criteria.FunctionId);

            if (function == null)
            {
                return HttpNotFound();
            }

            ViewBag.OriginFunctionName = function.Name;
            criteria.SiteId = function.Site.Id;
            GetSitesAndFunctionsForAddNew(criteria, getSiteList: false);

            if (ModelState.IsValid)
            {
                // checks duplicate function name
                criteria.FunctionName = criteria.FunctionName.Trim();

                if (this._functionService.GetByNameAndSite(criteria.FunctionName, criteria.SiteId) != null)
                {
                    ModelState.AddModelError("FunctionName", string.Format(ViewResources.DuplicateMsg, criteria.FunctionName));
                    return View(criteria);
                }

                // checks duplicate function key
                criteria.FunctionKey = GenerateFunctionKey(criteria.FunctionName);

                if (this._functionService.GetByKeyAndSite(criteria.FunctionKey, criteria.SiteId) != null)
                {
                    ModelState.AddModelError("FunctionKey", string.Format(ViewResources.DuplicateMsg, criteria.FunctionKey));
                    return View(criteria);
                }

                // duplicates function
                criteria.DuplicateFunctionId = function.Id;
                criteria.ParentId = criteria.ParentId == 0 ? null : criteria.ParentId;
                criteria.ParentName = (criteria.ParentId == null || criteria.ParentId == 0) ? null : criteria.ParentName;
                criteria.CreatedBy = CurrentSession.UserId;
                criteria.CreatedDate = DateTime.Now;

                ICommandResult result = this._functionService.Duplicate(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            return View(criteria);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update function.
        /// </summary>
        /// <param name="id">The function ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int id)
        {
            IFunction function = this._functionService.GetById(id);

            if (ReferenceEquals(function, null))
            {
                return RedirectToAction("Manage");
            }

            var criteria = new UpdateFunctionCriteria();

            criteria.FunctionId = function.Id;
            criteria.SiteId = function.Site.Id;
            criteria.SiteName = function.Site.Name;
            criteria.FunctionName = function.Name;
            criteria.FunctionKey = function.Key;
            criteria.Description = function.Description;
            criteria.ParentId = function.Parent == null ? null : (int?)function.Parent.Id;
            criteria.ParentName = function.Parent == null ? null : function.Parent.Name;
            criteria.Readonly = function.Crud == (byte)Constant.Crud.Read;
            criteria.DisplayOrder = function.DisplayOrder;
            criteria.IsActive = function.IsActive;
            criteria.Environment = function.Environment;

            // get parent function list
            IEnumerable<IFunction> functionList = this._functionService.GetBySite(criteria.SiteId, criteria.Environment, true);

            ViewBag.ParentFunctionItemList = functionList
                .Where(f => f.Id != id && (f.Parent == null || f.Parent.Id != id))
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                    Selected = function.Parent != null && s.Id == function.Parent.Id
                }).ToList();

            return View(criteria);
        }

        /// <summary>
        /// Update function.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]        
        public ActionResult Edit(UpdateFunctionCriteria criteria)
        {
            IFunction currentFunction = this._functionService.GetById(criteria.FunctionId);

            if (ReferenceEquals(currentFunction, null))
            {
                return RedirectToAction("Manage");
            }

            IEnumerable<IFunction> functionList = this._functionService.GetBySite(criteria.SiteId, criteria.Environment, true);

            ViewBag.ParentFunctionItemList = functionList.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString(),
                Selected = criteria.ParentId != null && s.Id == criteria.ParentId
            }).ToList();

            if (ModelState.IsValid)
            {
                // checks Function Name duplication
                criteria.FunctionName = criteria.FunctionName.Trim();
                IFunction function = this._functionService.GetByNameAndSite(criteria.FunctionName, criteria.SiteId);

                if (function != null && function.Id != criteria.FunctionId)
                {
                    criteria.FunctionKey = currentFunction.Key;

                    ModelState.AddModelError("FunctionName", string.Format(ViewResources.DuplicateMsg, criteria.FunctionName));

                    return View(criteria);
                }

                // checks Function Key duplication
                if (criteria.FunctionKey != null)
                {
                    criteria.FunctionKey = GenerateFunctionKey(criteria.FunctionName);
                }

                function = this._functionService.GetByKeyAndSite(criteria.FunctionKey, criteria.SiteId);

                if (function != null && function.Id != criteria.FunctionId)
                {
                    ModelState.AddModelError("FunctionKey", string.Format(ViewResources.DuplicateMsg, criteria.FunctionKey));

                    return View(criteria);
                }

                // check for all user,group has permission on this function when CRUD change
                function = this._functionService.GetById(criteria.FunctionId);

                if (ReferenceEquals(function, null))
                {
                    return RedirectToAction("Manage");
                }

                if (criteria.Crud < function.Crud)
                {
                    if (CurrentSession.Level != (int)Constant.Level.SiteAdmin
                        && CurrentSession.Level != (int)Constant.Level.PortalAdmin)
                    {
                        // list user has host permission in this function
                        var listUsers = _userService.GetByFunctionAndSite(function.Id, function.Site.Id)
                                                        .Where(u => u.Policy == (int)Constant.Policy.Host).ToList();

                        // list group has host permission in this function
                        var listGroups = _groupService.GetByFunctionAndSite(function.Id, function.Site.Id)
                                                        .Where(u => u.Policy == (int)Constant.Policy.Host).ToList();

                        if (listUsers.Any() || listGroups.Any())
                        {
                            ModelState.AddModelError("Readonly", ViewResources.Privileged_User_Group_Message);

                            return View(criteria);
                        }
                    }
                }

                // update function
                criteria.ParentId = criteria.ParentId == 0 ? null : criteria.ParentId;
                criteria.ParentName = criteria.ParentId == null || criteria.ParentId == 0 ? null : criteria.ParentName;
                criteria.ModifiedBy = CurrentSession.UserId;
                criteria.ModifiedDate = DateTime.Now;

                ICommandResult result = this._functionService.Update(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            return View(criteria);
        }

        #endregion

        #region Activation

        /// <summary>
        /// Update function status.
        /// </summary>
        /// <param name="id">The function ID.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult Activate(int id, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                var criteria = new UpdateStatusFunctionCriteria
                {
                    FunctionId = id,
                    IsActive = isActive,
                    ModifiedBy = CurrentSession.UserId,
                    ModifiedDate = DateTime.Now
                };

                ICommandResult result = this._functionService.UpdateStatus(criteria);

                if (result != null && result.IsSuccessful)
                {
                    return Json(Constant.OK);
                }
            }

            return new EmptyResult();
        }

        #endregion

        #region Privileged Groups And Users

        /// <summary>
        /// Get privileged group and user.
        /// </summary>
        /// <param name="id">The function ID.</param>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public ActionResult PrivilegedGroupsAndUsers(int id = 0)
        {
            var function = this._functionService.GetById(id);

            if (function == null)
            {
                return HttpNotFound();
            }

            int siteId = function.Site.Id;

            ViewBag.Function = function;
            ViewBag.Users = _userService.GetByFunctionAndSite(id, siteId).ToList();
            ViewBag.Groups = _groupService.GetByFunctionAndSite(id, siteId).ToList();

            // gets logged-in-user privileges
            ViewFlags.HasAssignFunctionToUserRight =
                HasRightOnSite(siteId, FunctionKeys.AssignFunctionToUser, Constant.Policy.Host);
            ViewFlags.HasAssignFunctionToGroupRight =
                HasRightOnSite(siteId, FunctionKeys.AssignFunctionToGroup, Constant.Policy.Host);

            ViewBag.GroupSelectList = _groupService.GetAllSelectListItem();
            ViewBag.SiteId = siteId.ToString();

        return View();
        }

        /// <summary>
        /// Manage privileged user.
        /// </summary>
        /// <param name="id">The page index.</param>
        /// <param name="size">The page size.</param>
        /// <param name="fid">The function ID.</param>
        /// <param name="gid">The group ID.</param>
        /// <param name="q">The username.</param>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToUser, Policy = Constant.Policy.Host)]
        public ActionResult PrivilegedUsers(
            int id = 1,
            int size = 10,
            int fid = 0,
            int gid = 0,
            string q = EmptyString)
        {
            if (Request.IsAjaxRequest())
            {
                int pageIndex = id;
                int pageSize = size;
                int functionId = fid;
                int groupId = gid;
                string username = q.Trim();

                var userList = new PagerList<UserLevel>(pageIndex, pageSize, 0, new List<UserLevel>());
                int totalUsers = 0;

                var function = this._functionService.GetById(functionId);

                if (function == null)
                {
                    return HttpNotFound();
                }

                IQueryable<UserLevel> users = _userService.SearchUserExcludeFunction(
                    pageIndex,
                    pageSize,
                    out totalUsers,
                    username,
                    groupId,
                    function.Site.Id,
                    functionId);

                userList = new PagerList<UserLevel>(pageIndex, pageSize, totalUsers, users);

                // checks currently-logged-in user's policy on the function
                int adminPolicy = _userService.GetUserFunctionPolicy(CurrentSession.UserId, functionId);
                ViewBag.IsReadonlyAssign = adminPolicy < (int)Constant.Policy.Host;

                return View(userList);
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Add privileged user.
        /// </summary>
        /// <param name="id">The function ID.</param>
        /// <param name="readList">List of function which has read permission.</param>
        /// <param name="hostList">List of function which has host permission.</param>
        /// <returns>Result string.</returns>
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToUser, Policy = Constant.Policy.Host)]
        public string AddPrivilegedUsers(int id = 0, string readList = EmptyString, string hostList = EmptyString)
        {
            if (Request.IsAjaxRequest())
            {
                var function = this._functionService.GetById(id);

                if (function == null)
                {
                    throw new ArgumentException(string.Format("Function (id={0}) not found", id));
                }

                var criteria = new AssignFunctionToUsersCriteria
                {
                    FunctionId = id,
                    SiteId = function.Site.Id,
                    UserRead = readList.Split(',').ToList(),
                    UserHost = hostList.Split(',').ToList(),
                    ModifiedBy = CurrentSession.UserId,
                    ModifiedDate = DateTime.Now
                };

                ICommandResult result = this._functionService.AssignFunctionToUsers(criteria);

                if (result != null && result.IsSuccessful)
                {
                    return Constant.OK;
                }
            }

            return ViewResources.Unexpected_Error;
        }

        /// <summary>
        /// Manage privileged group.
        /// </summary>
        /// <param name="fid">The function ID.</param>
        /// <param name="q">The group name.</param>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToGroup, Policy = Constant.Policy.Host)]
        public ActionResult PrivilegedGroups(int fid = 0, string q = EmptyString)
        {
            if (Request.IsAjaxRequest())
            {
                int functionId = fid;
                string groupName = q.Trim();

                IFunction function = this._functionService.GetById(functionId);

                if (function == null)
                {
                    return HttpNotFound();
                }

                IEnumerable<IGroup> group = _groupService.SearchGroupExcludeFunction(groupName, function.Site.Id, fid);

                // checks currently-logged-in user's policy on the function
                int adminPolicy = _userService.GetUserFunctionPolicy(CurrentSession.UserId, fid);
                ViewBag.IsReadonlyAssign = adminPolicy < (int)Constant.Policy.Host;

                return View(group);
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Add privileged group.
        /// </summary>
        /// <param name="id">The function ID.</param>
        /// <param name="readList">List of function which has read permission.</param>
        /// <param name="hostList">List of function which has host permission.</param>
        /// <returns>Result string.</returns>
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToGroup, Policy = Constant.Policy.Host)]
        public string AddPrivilegedGroups(int id = 0, string readList = EmptyString, string hostList = EmptyString)
        {
            if (Request.IsAjaxRequest())
            {
                int functionId = id;
                IFunction function = this._functionService.GetById(functionId);

                if (function == null)
                {
                    throw new ArgumentException(string.Format("Function (id={0}) not found", id));
                }

                var criteria = new AssignFunctionToGroupsCriteria
                {
                    FunctionId = id,
                    SiteId = function.Site.Id,
                    GroupRead = readList.Split(',').ToList(),
                    GroupHost = hostList.Split(',').ToList(),
                    ModifiedBy = CurrentSession.UserId,
                    ModifiedDate = DateTime.Now
                };

                ICommandResult result = this._functionService.AssignFunctionToGroups(criteria);

                if (result != null && result.IsSuccessful)
                {
                    return Constant.OK;
                }
            }

            return ViewResources.Unexpected_Error;
        }

        #endregion

        #region Ajax Request

        /// <summary>
        /// Get function by site.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public JsonResult GetFunctionBySite(int siteId)
        {
            if (Request.IsAjaxRequest())
            {
                List<IFunction> functionList = this._functionService.GetBySite(siteId, this.Environment, true).ToList();

                functionList.Insert(0, new Function { Id = 0, Name = ViewResources.None });

                return Json(functionList);
            }

            return Json(null);
        }

        /// <summary>
        /// Get parent function by environment.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <param name="functionId">The function ID.</param>
        /// <param name="env">The environment.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public JsonResult GetFunctionParentByEnvironment(int siteId = 0, int functionId = 0, string env = Constant.UAT)
        {
            if (Request.IsAjaxRequest())
            {
                if (functionId != 0)
                {
                    IFunction function = this._functionService.GetById(functionId);
                    List<IFunction> functionList = this._functionService.GetBySite(function.Site.Id, env, true).ToList();

                    functionList = 
                        functionList.Where(f => f.Id != functionId && (f.Parent == null || f.Parent.Id != functionId)).ToList();

                    functionList.Insert(0, new Function { Id = 0, Name = ViewResources.None });

                    return Json(functionList);
                }
                else
                {
                    var functionList = this._functionService.GetBySite(siteId, env, true).ToList();

                    functionList.Insert(0, new Function { Id = 0, Name = ViewResources.None });

                    return Json(functionList);
                }
            }

            return Json(null);
        }

        /// <summary>
        /// Get function key.
        /// </summary>
        /// <param name="functionId">The function ID.</param>
        /// <param name="functionName">The function name.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public JsonResult GetFunctionKey(int functionId, string functionName)
        {
            IFunction tempFunction = this._functionService.GetById(functionId);

            if (ReferenceEquals(tempFunction, null))
            {
                return Json(-1);
            }

            // checks Function Name duplication
            IFunction function = this._functionService.GetByNameAndSite(functionName, tempFunction.Site.Id);

            if (function != null && function.Id != tempFunction.Id)
            {
                return Json(0);
            }

            // checks Function Key duplication
            string functionKey = GenerateFunctionKey(functionName);

            function = this._functionService.GetByKeyAndSite(functionKey, tempFunction.Site.Id);

            if (function != null && function.Id != tempFunction.Id)
            {
                return Json(-2);
            }

            return Json(functionKey);
        }
        #endregion

        #region Helpers

        /// <summary>
        /// Insert child function.
        /// </summary>
        /// <param name="orderedFunctionList">Ordered function list.</param>
        /// <param name="functionList">List of function.</param>
        /// <param name="parent">Parent function.</param>
        private void InsertChildFunction(
            ref List<Function> orderedFunctionList, 
            List<Function> functionList, 
            Function parent)
        {
            List<Function> childFunctionList = functionList.Where(f => f.Parent != null && f.Parent.Id == parent.Id)
                .OrderBy(f => f.DisplayOrder).ToList();

            foreach (Function childFunction in childFunctionList)
            {
                orderedFunctionList.Add(childFunction);
                InsertChildFunction(ref orderedFunctionList, functionList, childFunction);
            }
        }

        #endregion

        /// <summary>
        /// Prepare manage data.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        private void PrepareManageData(GetBySiteFunctionCriteria criteria)
        {
            ViewBag.FunctionList = this._functionService.GetBySiteInHierarchy(criteria.SiteId, criteria.Environment);

            if (this.UserIsPortalAdmin) 
            {
                // portal admin
                ViewBag.SiteItemList = _siteService.GetAllSelectListItem(criteria.Environment, true, criteria.SiteId);
                ViewFlags.HasEditRight = true;
            }
            else
            {
                ViewBag.SiteItemList =
                    _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                        CurrentSession.UserId,
                        FunctionKeys.ManageFunction,
                        criteria.Environment,
                        policy: Constant.Policy.Read,
                        selectedSiteId: criteria.SiteId);

                ViewFlags.HasEditRight =
                    HasRight(FunctionKeys.ManageFunction, Constant.Policy.Host)
                    && HasRightOnSite(criteria.SiteId, FunctionKeys.ManageFunction, Constant.Policy.Host);
            }
        }

        /// <summary>
        /// Get site and functions for add new.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="getSiteList">Get site list or not.</param>
        private void GetSitesAndFunctionsForAddNew(dynamic criteria, bool getSiteList = true)
        {
            // gets site list
            if (getSiteList)
            {
                // portal admin
                if (this.UserIsPortalAdmin)
                {
                    ViewBag.SiteItemList = _siteService
                        .GetAllSelectListItem(criteria.Environment, isActive: true, selectedSiteId: criteria.SiteId);
                }
                else
                {
                    ViewBag.SiteItemList =
                        _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                CurrentSession.UserId,
                                FunctionKeys.ManageFunction,
                                criteria.Environment,
                                policy: Constant.Policy.Host,
                                selectedSiteId: criteria.SiteId);
                }
            }

            // gets function list
            if (criteria.ParentId == null)
            {
                ViewBag.FunctionList = 
                    this._functionService.GetSelectListItemBySite(criteria.SiteId, criteria.Environment, isActive: true);
            }
            else
            {
                ViewBag.FunctionList = this._functionService.GetSelectListItemBySite(
                                                criteria.SiteId,
                                                criteria.Environment,
                                                isActive: true,
                                                selectedFunctionId: (int)criteria.ParentId);
            }
        }

        /// <summary>
        /// Generate function key from function name.
        /// </summary>
        /// <param name="functionName">Function name.</param>
        /// <returns>Function key.</returns>
        private string GenerateFunctionKey(string functionName)
        {
            string functionKey = functionName.Replace(Constant.SPACE, string.Empty);

            var sb = new StringBuilder();
            int length = functionKey.Length;

            for (int i = 0; i < length; i++)
            {
                char item = functionKey[i];

                if (char.IsLetterOrDigit(item))
                {
                    sb.Append(item);
                }
                else
                {
                    sb.Append("_");
                }
            }

            return sb.ToString();
        }
    }
}
