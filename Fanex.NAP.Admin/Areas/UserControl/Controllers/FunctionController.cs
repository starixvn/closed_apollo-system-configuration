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

    using Fanex.Logging;

    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Function;
    using Fanex.NAP.Core.Helpers;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
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
            _functionService = functionService ?? new FunctionService { ServiceContext = ServiceContext };
            _siteService = siteService ?? new SiteService { ServiceContext = ServiceContext };
            _userService = userService ?? new UserService { ServiceContext = ServiceContext };
            _groupService = groupService ?? new GroupService { ServiceContext = ServiceContext };
        }

        #endregion

        #region Manage

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public ActionResult Manage(int? siteId)
        {
            var criteria = new GetBySiteFunctionCriteria
            {
                SiteId = siteId ?? this.SiteId,
                Environment = this.Environment
            };

            try
            {
                PrepareManageData(criteria);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public ActionResult Manage(GetBySiteFunctionCriteria criteria)
        {
            try
            {
                PrepareManageData(criteria);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        private void PrepareManageData(GetBySiteFunctionCriteria criteria)
        {
            ViewBag.FunctionList = _functionService.GetBySiteInHierarchy(criteria.SiteId, criteria.Environment);

            if (this.UserIsPortalAdmin) // portal admin
            {
                ViewBag.SiteItemList = _siteService.GetAllSelectListItem(criteria.Environment, true, criteria.SiteId);
                ViewBag.ViewFlags.HasEditRight = true;
            }
            else
            {
                ViewBag.SiteItemList =
                    _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                        CurrentSession.UserId,
                        FunctionKeys.ManageFunction.ToString(),
                        criteria.Environment,
                        policy: Constant.Policy.Read,
                        selectedSiteId: criteria.SiteId);

                int policyCurrentSite = _userService.GetUserSiteFunctionKeyPolicy(
                        CurrentSession.UserId,
                        this.SiteId,
                        FunctionKeys.ManageFunction.ToString());

                int policy = _userService.GetUserSiteFunctionKeyPolicy(
                        CurrentSession.UserId,
                        criteria.SiteId,
                        FunctionKeys.ManageFunction.ToString());

                ViewBag.ViewFlags.HasEditRight = policy >= (int)Constant.Policy.Host
                                                    && policyCurrentSite >= (int)Constant.Policy.Host;
            }
        }

        #endregion

        #region Add new

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(int? siteId = null)
        {
            var criteria = new InsertFunctionCriteria
            {
                SiteId = siteId ?? this.SiteId,
                Environment = this.Environment
            };

            try
            {
                GetSitesAndFunctionsForAddNew(criteria);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(InsertFunctionCriteria criteria)
        {
            try
            {
                GetSitesAndFunctionsForAddNew(criteria);

                if (ModelState.IsValid)
                {
                    #region checks duplicate function name

                    string functionName = criteria.FunctionName = criteria.FunctionName.Trim();
                    var function = _functionService.GetByNameAndSite(functionName, criteria.SiteId);

                    if (function != null)
                    {
                        ModelState.AddModelError("FunctionName",
                            string.Format(ViewResources.DuplicateMsg, functionName));

                        return View(criteria);
                    }

                    #endregion

                    #region checks duplicate function key

                    string functionKey = criteria.FunctionKey =
                        criteria.FunctionName.Replace(Constant.SPACE, string.Empty);

                    #region replace char not letter or digit of function key

                    StringBuilder strFunctionKeyModify = new StringBuilder();

                    for (int i = 0; i < functionKey.Length; i++)
                    {
                        char item = functionKey[i];
                        if (char.IsLetterOrDigit(item))
                        {
                            strFunctionKeyModify.Append(item);
                        }
                        else
                        {
                            strFunctionKeyModify.Append("_");
                        }
                    }

                    criteria.FunctionKey = strFunctionKeyModify.ToString();
                    functionKey = strFunctionKeyModify.ToString();

                    #endregion

                    function = _functionService.GetByKeyAndSite(functionKey, criteria.SiteId);

                    if (function != null)
                    {
                        ModelState.AddModelError("FunctionKey",
                            string.Format(ViewResources.DuplicateMsg, functionKey));

                        return View(criteria);
                    }

                    #endregion

                    #region adds function

                    criteria.ParentId = criteria.ParentId == 0 ? null : criteria.ParentId;
                    criteria.ParentName = (criteria.ParentId == null || criteria.ParentId == 0) ? null : criteria.ParentName;
                    criteria.CreatedBy = CurrentSession.UserId;
                    criteria.CreatedDate = DateTime.Now;

                    // inserts function and assigns it to all site-admins of the current site and all portal admins
                    var result = _functionService.Insert(criteria);

                    if (result != null && result.IsSuccessful)
                    {
                        ViewBag.Success = true;
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        private void GetSitesAndFunctionsForAddNew(dynamic criteria, bool getSiteList = true)
        {
            #region gets site list
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
                                FunctionKeys.ManageFunction.ToString(),
                                criteria.Environment,
                                policy: Constant.Policy.Host,
                                selectedSiteId: criteria.SiteId);
                }
            }
            #endregion

            #region gets function list
            if (criteria.ParentId == null)
            {
                ViewBag.FunctionList = _functionService.GetSelectListItemBySite(
                                                criteria.SiteId,
                                                criteria.Environment,
                                                isActive: true);
            }
            else
            {
                ViewBag.FunctionList = _functionService.GetSelectListItemBySite(
                                                criteria.SiteId,
                                                criteria.Environment,
                                                isActive: true,
                                                selectedFunctionId: (int)criteria.ParentId);
            }
            #endregion
        }

        #endregion

        #region Duplicate Function

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult Duplicate(int id, int? siteId = null)
        {
            int functionId = id; // id of the function-to-duplicate
            var function = _functionService.GetById(functionId);
            var site = _siteService.GetById(function.Site.Id);

            if (function == null)
            {
                return HttpNotFound();
            }

            ViewBag.OriginFunctionName = function.Name;

            #region builds criteria
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
            #endregion

            try
            {
                GetSitesAndFunctionsForAddNew(criteria, getSiteList: false);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult Duplicate(DuplicateFunctionCriteria criteria)
        {
            try
            {
                var function = _functionService.GetById(criteria.FunctionId);

                if (function == null)
                {
                    return HttpNotFound();
                }

                ViewBag.OriginFunctionName = function.Name;
                criteria.SiteId = function.Site.Id;
                GetSitesAndFunctionsForAddNew(criteria, getSiteList: false);

                if (ModelState.IsValid)
                {
                    #region checks duplicate function name

                    string functionName = criteria.FunctionName = criteria.FunctionName.Trim();

                    if (_functionService.GetByNameAndSite(functionName, criteria.SiteId) != null)
                    {
                        ModelState.AddModelError("FunctionName",
                            string.Format(ViewResources.DuplicateMsg, functionName));

                        return View(criteria);
                    }

                    #endregion

                    #region checks duplicate function key

                    string functionKey = criteria.FunctionKey =
                        criteria.FunctionName.Replace(Constant.SPACE, string.Empty);

                    if (_functionService.GetByKeyAndSite(functionKey, criteria.SiteId) != null)
                    {
                        ModelState.AddModelError("FunctionKey",
                            string.Format(ViewResources.DuplicateMsg, functionKey));

                        return View(criteria);
                    }

                    #endregion

                    #region duplicates function

                    criteria.DuplicateFunctionId = function.Id;
                    criteria.ParentId = criteria.ParentId == 0 ? null : criteria.ParentId;
                    criteria.ParentName = (criteria.ParentId == null || criteria.ParentId == 0) ? null : criteria.ParentName;
                    criteria.CreatedBy = CurrentSession.UserId;
                    criteria.CreatedDate = DateTime.Now;

                    var result = _functionService.Duplicate(criteria);

                    if (result != null && result.IsSuccessful)
                    {
                        ViewBag.Success = true;
                    }

                    #endregion
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

        #region Update

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int id)
        {
            var criteria = new UpdateFunctionCriteria();

            try
            {
                var function = _functionService.GetById(id);

                if (ReferenceEquals(function, null))
                {
                    return RedirectToAction("Manage");
                }

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
                var functionList = _functionService.GetBySite(criteria.SiteId, criteria.Environment, true);

                ViewBag.ParentFunctionItemList = functionList
                    .Where(f => f.Id != id && (f.Parent == null || f.Parent.Id != id))
                    .Select(s => new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                        Selected = function.Parent != null && s.Id == function.Parent.Id
                    }).ToList();
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult Edit(UpdateFunctionCriteria criteria)
        {
            try
            {
                var currentFunction = _functionService.GetById(criteria.FunctionId);

                if (ReferenceEquals(currentFunction, null))
                {
                    return RedirectToAction("Manage");
                }

                var functionList = _functionService.GetBySite(criteria.SiteId, criteria.Environment, true);

                ViewBag.ParentFunctionItemList = functionList.Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                    Selected = criteria.ParentId != null && s.Id == criteria.ParentId
                }).ToList();

                if (ModelState.IsValid)
                {
                    #region checks Function Name duplication

                    string functionName = criteria.FunctionName = criteria.FunctionName.Trim();
                    var function = _functionService.GetByNameAndSite(functionName, criteria.SiteId);

                    if (function != null && function.Id != criteria.FunctionId)
                    {
                        criteria.FunctionKey = currentFunction.Key;

                        ModelState.AddModelError("FunctionName", string.Format(ViewResources.DuplicateMsg, functionName));

                        return View(criteria);
                    }

                    #endregion

                    #region checks Function Key duplication

                    string tempFunctionKey = criteria.FunctionKey != null ? criteria.FunctionKey : criteria.FunctionName;

                    string functionKey = criteria.FunctionKey = tempFunctionKey.Replace(Constant.SPACE, string.Empty);

                    #region replace char not letter or digit of function key

                    StringBuilder strFunctionKeyModify = new StringBuilder();

                    for (int i = 0; i < functionKey.Length; i++)
                    {
                        char item = functionKey[i];
                        if (char.IsLetterOrDigit(item))
                        {
                            strFunctionKeyModify.Append(item);
                        }
                        else
                        {
                            strFunctionKeyModify.Append("_");
                        }
                    }

                    criteria.FunctionKey = strFunctionKeyModify.ToString();
                    functionKey = strFunctionKeyModify.ToString();

                    #endregion

                    function = _functionService.GetByKeyAndSite(functionKey, criteria.SiteId);

                    if (function != null && function.Id != criteria.FunctionId)
                    {
                        ModelState.AddModelError("FunctionKey", string.Format(ViewResources.DuplicateMsg, functionKey));

                        return View(criteria);
                    }

                    #endregion

                    #region check for all user,group has permission on this function when CRUD change

                    function = _functionService.GetById(criteria.FunctionId);

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

                            if (listUsers.Count > 0 || listGroups.Count > 0)
                            {
                                ModelState.AddModelError("Readonly", ViewResources.Privileged_User_Group_Message);

                                return View(criteria);
                            }
                        }
                    }
                    #endregion

                    #region update function

                    criteria.ParentId = criteria.ParentId == 0 ? null : criteria.ParentId;
                    criteria.ParentName = criteria.ParentId == null || criteria.ParentId == 0 ? null :
                        criteria.ParentName;
                    criteria.ModifiedBy = CurrentSession.UserId;
                    criteria.ModifiedDate = DateTime.Now;

                    var result = _functionService.Update(criteria);

                    if (result != null && result.IsSuccessful)
                    {
                        ViewBag.Success = true;
                    }

                    #endregion
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

        #region Activation

        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public ActionResult Activate(int id, bool active)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    var result = _functionService.UpdateStatus(new UpdateStatusFunctionCriteria
                    {
                        FunctionId = id,
                        IsActive = active,
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

        #region Privileged Groups And Users

        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public ActionResult PrivilegedGroupsAndUsers(int id = 0)
        {
            int functionId = id;
            int siteId = 0;

            try
            {
                var function = _functionService.GetById(functionId);

                if (function == null)
                {
                    return HttpNotFound();
                }

                siteId = function.Site.Id;

                ViewBag.Function = function;
                ViewBag.Users = _userService.GetByFunctionAndSite(functionId, siteId).ToList();
                ViewBag.Groups = _groupService.GetByFunctionAndSite(functionId, siteId).ToList();

                #region gets logged-in-user privileges

                ViewFlags.HasAssignFunctionToUserRight = _userService.GetUserSiteFunctionKeyPolicy(
                            CurrentSession.UserId,
                            function.Site.Id,
                            FunctionKeys.AssignFunctionToUser.ToString()) >= (int)Constant.Policy.Host;

                ViewFlags.HasAssignFunctionToGroupRight = _userService.GetUserSiteFunctionKeyPolicy(
                    CurrentSession.UserId,
                    function.Site.Id,
                    FunctionKeys.AssignFunctionToGroup.ToString()) >= (int)Constant.Policy.Host;

                #endregion

                ViewBag.GroupSelectList = _groupService.GetAllSelectListItem();

                ViewBag.SiteId = siteId.ToString();
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View();
        }

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

                PagerList<UserLevel> userList = new PagerList<UserLevel>(pageIndex, pageSize, 0, new List<UserLevel>());
                int totalUsers = 0;

                try
                {
                    var function = _functionService.GetById(functionId);

                    if (function == null)
                    {
                        return HttpNotFound();
                    }

                    var users = _userService.SearchUserExcludeFunction(
                        pageIndex,
                        pageSize,
                        out totalUsers,
                        username,
                        groupId,
                        function.Site.Id,
                        functionId);

                    userList = new PagerList<UserLevel>(pageIndex, pageSize, totalUsers, users);

                    // checks currently-logged-in user's policy on the function
                    var adminPolicy = _userService.GetUserFunctionPolicy(CurrentSession.UserId, functionId);
                    ViewBag.IsReadonlyAssign = adminPolicy < (int)Constant.Policy.Host;

                    return View(userList);
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToUser, Policy = Constant.Policy.Host)]
        public string AddPrivilegedUsers(
            int id = 0,
            string readList = EmptyString,
            string hostList = EmptyString)
        {
            if (Request.IsAjaxRequest())
            {
                int functionId = id;
                string userIdList = readList.Trim();
                string hostIdList = hostList.Trim();
                var function = _functionService.GetById(functionId);

                if (function == null)
                {
                    throw new ArgumentException(string.Format("Function (id={0}) not found", id));
                }

                try
                {
                    var criteria = new AssignFunctionToUsersCriteria
                    {
                        FunctionId = id,
                        SiteId = function.Site.Id,
                        UserRead = readList.Split(',').ToList(),
                        UserHost = hostList.Split(',').ToList(),
                        ModifiedBy = CurrentSession.UserId,
                        ModifiedDate = DateTime.Now
                    };

                    var result = _functionService.AssignFunctionToUsers(criteria);

                    if (result != null && result.IsSuccessful)
                    {
                        return Constant.OK;
                    }
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            throw new Exception(ViewResources.Unexpected_Error);
        }

        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToGroup, Policy = Constant.Policy.Host)]
        public ActionResult PrivilegedGroups(int fid = 0, string q = EmptyString)
        {
            if (Request.IsAjaxRequest())
            {
                int functionId = fid;
                string groupName = q.Trim();

                try
                {
                    var function = _functionService.GetById(functionId);

                    if (function == null)
                    {
                        return HttpNotFound();
                    }

                    var group = _groupService.SearchGroupExcludeFunction(groupName, function.Site.Id, fid);

                    // checks currently-logged-in user's policy on the function
                    var adminPolicy = _userService.GetUserFunctionPolicy(CurrentSession.UserId, fid);
                    ViewBag.IsReadonlyAssign = adminPolicy < (int)Constant.Policy.Host;

                    return View(group);
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToGroup, Policy = Constant.Policy.Host)]
        public string AddPrivilegedGroups(
            int id = 0,
            string readList = EmptyString,
            string hostList = EmptyString)
        {
            if (Request.IsAjaxRequest())
            {
                int functionId = id;
                string groupIdList = readList.Trim();
                string hostIdList = hostList.Trim();
                var function = _functionService.GetById(functionId);

                if (function == null)
                {
                    throw new ArgumentException(string.Format("Function (id={0}) not found", id));
                }

                try
                {
                    var criteria = new AssignFunctionToGroupsCriteria
                    {
                        FunctionId = id,
                        SiteId = function.Site.Id,
                        GroupRead = readList.Split(',').ToList(),
                        GroupHost = hostList.Split(',').ToList(),
                        ModifiedBy = CurrentSession.UserId,
                        ModifiedDate = DateTime.Now
                    };

                    var result = _functionService.AssignFunctionToGroups(criteria);

                    if (result != null && result.IsSuccessful)
                    {
                        return Constant.OK;
                    }
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            throw new Exception(ViewResources.Unexpected_Error);
        }

        #endregion

        #region Ajax Request

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public JsonResult GetFunctionBySite(int siteId)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    var functionList = _functionService.GetBySite(siteId, this.Environment, true).ToList();

                    functionList.Insert(0, new Function { Id = 0, Name = ViewResources.None });

                    return Json(functionList);
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            return Json(null);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public JsonResult GetFunctionParentByEnvironment(int siteId = 0, int functionId = 0, string environment = Constant.UAT)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    if (functionId != 0)
                    {
                        var function = _functionService.GetById(functionId);

                        var functionList = _functionService.GetBySite(function.Site.Id, environment, true).ToList();

                        functionList = functionList.Where(f => f.Id != functionId && (f.Parent == null
                                                    || f.Parent.Id != functionId)).ToList();

                        functionList.Insert(0, new Function { Id = 0, Name = ViewResources.None });

                        return Json(functionList);
                    }
                    else
                    {
                        var functionList = _functionService.GetBySite(siteId, environment, true).ToList();

                        functionList.Insert(0, new Function { Id = 0, Name = ViewResources.None });

                        return Json(functionList);
                    }
                }
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            return Json(null);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public JsonResult GetFunctionKey(int functionId, string functionName)
        {
            var tempFunction = _functionService.GetById(functionId);

            if (ReferenceEquals(tempFunction, null))
            {
                return Json(-1);
            }

            #region checks Function Name duplication

            var function = _functionService.GetByNameAndSite(functionName, tempFunction.Site.Id);

            if (function != null && function.Id != tempFunction.Id)
            {
                return Json(0);
            }

            #endregion

            #region checks Function Key duplication

            string tempFunctionKey = functionName;

            string functionKey = tempFunctionKey.Replace(Constant.SPACE, string.Empty);

            #region replace char not letter or digit of function key

            StringBuilder strFunctionKeyModify = new StringBuilder();

            for (int i = 0; i < functionKey.Length; i++)
            {
                char item = functionKey[i];
                if (char.IsLetterOrDigit(item))
                {
                    strFunctionKeyModify.Append(item);
                }
                else
                {
                    strFunctionKeyModify.Append("_");
                }
            }

            functionKey = strFunctionKeyModify.ToString();

            #endregion

            function = _functionService.GetByKeyAndSite(functionKey, tempFunction.Site.Id);

            if (function != null && function.Id != tempFunction.Id)
            {
                return Json(-2);
            }

            #endregion

            return Json(strFunctionKeyModify.ToString());
        }
        #endregion

        #region Helpers

        private void InsertChildFunction(ref List<Function> orderedFunctionList, List<Function> functionList, Function parent)
        {
            var childFunctionList = functionList.Where(f => f.Parent != null && f.Parent.Id == parent.Id)
                .OrderBy(f => f.DisplayOrder).ToList();

            foreach (var childFunction in childFunctionList)
            {
                orderedFunctionList.Add(childFunction);
                InsertChildFunction(ref orderedFunctionList, functionList, childFunction);
            }
        }

        #endregion
    }
}
