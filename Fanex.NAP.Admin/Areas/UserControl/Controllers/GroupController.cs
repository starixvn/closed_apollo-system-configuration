// --------------------------------------------------------------------------
// <copyright file="GroupController.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common.Extensions;
    using Fanex.Logging;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Group;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Group controller.
    /// </summary>
    [NapSuperAuth]
    public class GroupController : BaseController
    {
        #region Members

        private readonly IGroupService _groupService;
        private readonly ISiteService _siteService;
        private readonly IFunctionService _functionService;
        private readonly IUserService _userService;

        #endregion

        #region Ctors

        public GroupController()
            : this(null, null, null, null)
        {
        }

        public GroupController(
            IGroupService groupService,
            ISiteService siteService,
            IFunctionService functionService,
            IUserService userService)
        {
            _groupService = groupService ?? new GroupService { ServiceContext = ServiceContext };
            _siteService = siteService ?? new SiteService { ServiceContext = ServiceContext };
            this._functionService = functionService ?? new FunctionService { ServiceContext = ServiceContext };
            _userService = userService ?? new UserService { ServiceContext = ServiceContext };
        }

        #endregion

        #region Manage

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Read)]
        public ActionResult Manage()
        {
            try
            {
                #region check edit right
                
                int policy = _userService.GetUserSiteFunctionKeyPolicy(
                            CurrentSession.UserId,
                            this.SiteId,
                            FunctionKeys.ManageGroup.ToString());

                ViewFlags.HasEditRight = policy >= (int)Constant.Policy.Host;

                #endregion

                #region check assign-function right
                
                int assignFunctionPolicy = _userService.GetUserSiteFunctionKeyPolicy(
                            CurrentSession.UserId,
                            this.SiteId,
                            FunctionKeys.AssignFunctionToGroup.ToString());

                ViewFlags.HasAssignFunctionRight = assignFunctionPolicy >= (int)Constant.Policy.Read;

                #endregion

                #region check manage-user right

                var hasManageUser = _userService.GetUserSiteFunctionKeyPolicy(
                   CurrentSession.UserId,
                   SiteId,
                   FunctionKeys.ManageUser.ToString()) >= 1 ? true : false;

                ViewFlags.HasManageUserRight = hasManageUser;

                #endregion

                ViewBag.GroupList = _groupService.GetAllWithNumOfUser(string.Empty, Environment);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(new GetAllWithNumOfUserGroupCriteria());
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Read)]
        public ActionResult Manage(GetAllWithNumOfUserGroupCriteria criteria)
        {
            try
            {
                criteria.Environment = Environment;

                #region check edit right

                int policy = _userService.GetUserSiteFunctionKeyPolicy(
                                                CurrentSession.UserId,
                                                this.SiteId,
                                                FunctionKeys.ManageGroup.ToString());

                ViewFlags.HasEditRight = policy >= (int)Constant.Policy.Host;

                #endregion                

                #region check assign-function right

                int assignFunctionPolicy = _userService.GetUserSiteFunctionKeyPolicy(
                                                    CurrentSession.UserId,
                                                    this.SiteId,
                                                    FunctionKeys.AssignFunctionToGroup.ToString());

                ViewFlags.HasAssignFunctionRight = assignFunctionPolicy >= (int)Constant.Policy.Read;

                #endregion

                #region check manage-user right

                var hasManageUser = _userService.GetUserSiteFunctionKeyPolicy(
                           CurrentSession.UserId,
                           SiteId,
                           FunctionKeys.ManageUser.ToString()) >= 1 ? true : false;

                ViewFlags.HasManageUserRight = hasManageUser;

                #endregion

                ViewBag.GroupList = _groupService.GetAllWithNumOfUser(criteria.GroupName);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        #endregion

        #region Add New Group

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult AddNew()
        {
            return View(new InsertGroupCriteria());
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(InsertGroupCriteria criteria)
        {
            try
            {
                criteria.GroupName = criteria.GroupName.Trim();

                if (ModelState.IsValid)
                {
                    #region Check name exist

                    var group = _groupService.GetByName(criteria.GroupName);

                    if (!ReferenceEquals(group, null))
                    {
                        ModelState.AddModelError(
                            "SiteName",
                            string.Format(ViewResources.DuplicateMsg, criteria.GroupName));

                        return View(criteria);
                    }

                    #endregion

                    #region Add group

                    criteria.CreatedBy = CurrentSession.UserId;
                    criteria.CreatedDate = DateTime.Now;

                    var result = _groupService.Insert(criteria);

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

        #region Edit Group

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int id)
        {
            var criteria = new UpdateGroupCriteria();

            try
            {
                var group = _groupService.GetById(id);

                if (ReferenceEquals(group, null))
                {
                    return RedirectToAction("Manage");
                }

                criteria.GroupId = group.Id;
                criteria.Description = group.Description;
                criteria.GroupName = group.Name;
                criteria.IsActive = group.IsActive;
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult Edit(UpdateGroupCriteria criteria)
        {
            try
            {
                criteria.GroupName = criteria.GroupName.Trim();

                if (ModelState.IsValid)
                {
                    #region Check name exist

                    var group = _groupService.GetByName(criteria.GroupName);

                    if (!ReferenceEquals(group, null))
                    {
                        if (group.Id != criteria.GroupId)
                        {
                            ModelState.AddModelError(
                                "SiteName",
                                string.Format(ViewResources.DuplicateMsg, criteria.GroupName));

                            return View(criteria);
                        }
                    }

                    #endregion

                    #region update function

                    criteria.ModifiedBy = CurrentSession.UserId;
                    criteria.ModifiedDate = DateTime.Now;

                    var result = _groupService.Update(criteria);

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

        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult Activate(int id, bool active)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    var result = _groupService.UpdateStatus(new UpdateStatusGroupCriteria
                    {
                        GroupId = id,
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

        #region Assign Permission

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToGroup, Policy = Constant.Policy.Read)]
        public ActionResult AssignPermission(int id, int siteId = 0)
        {
            var criteria = new AssignFunctionGroupCriteria();

            try
            {
                siteId = siteId == 0 || this.SiteId != NapAdminId ? this.SiteId : siteId;

                var group = _groupService.GetById(id);
                var site = _siteService.GetById(siteId);

                if (group == null || site == null)
                {
                    return RedirectToAction("Manage");
                }

                criteria.GroupId = group.Id;
                criteria.GroupName = group.Name;
                criteria.SiteId = site.Id;

                int policy = _userService.GetUserSiteFunctionKeyPolicy(
                                                CurrentSession.UserId,
                                                criteria.SiteId,
                                                FunctionKeys.AssignFunctionToGroup.ToString());

                ViewBag.ViewFlags.HasAssignFunctionRight = policy >= (int)Constant.Policy.Host;

                if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
                {
                    ViewBag.SiteSelectList = _siteService.GetAllSelectListItem(this.Environment, true, criteria.SiteId);
                }
                else
                {
                    ViewBag.SiteSelectList = _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                                        CurrentSession.UserId,
                                                        FunctionKeys.AssignFunctionToGroup.ToString(),
                                                        this.Environment,
                                                        policy: Constant.Policy.Read,
                                                        selectedSiteId: criteria.SiteId);
                }

                ViewBag.FunctionList = this._functionService.GetBySiteInHierarchy(criteria.SiteId, Environment, true);
                ViewBag.GroupFuncPolicyList = this._functionService.GetByGroupAndSite(
                                                        criteria.GroupId,
                                                        criteria.SiteId,
                                                        Environment);
                ViewBag.UserFuncPolicyList =
                    this._functionService.GetByUserAndSiteIncludeGroup(CurrentSession.UserId, criteria.SiteId, Environment);
                ViewBag.UserLevel = CurrentSession.Level;
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToGroup, Policy = Constant.Policy.Host)]
        public ActionResult AssignPermission(AssignFunctionGroupCriteria criteria)
        {
            try
            {
                #region check group and site

                var group = _groupService.GetById(criteria.GroupId);
                var site = _siteService.GetById(criteria.SiteId);

                if (group == null || site == null)
                {
                    return RedirectToAction("Manage");
                }

                #endregion

                #region check policy

                int policy = _userService.GetUserSiteFunctionKeyPolicy(
                                                CurrentSession.UserId,
                                                criteria.SiteId,
                                                FunctionKeys.AssignFunctionToGroup.ToString());

                if (policy < (int)Constant.Policy.Host)
                {
                    return RedirectToAction("Unauthorize", "Common");
                }

                ViewBag.ViewFlags.HasAssignFunctionRight = policy >= (int)Constant.Policy.Host;

                #endregion

                criteria.ModifiedBy = CurrentSession.UserId;
                criteria.ModifiedDate = DateTime.Now;

                // list function policy
                List<IFunction> listFunc = this._functionService.GetBySiteInHierarchy(
                                                                criteria.SiteId,
                                                                Environment,
                                                                true).ToList();

                // list group function policy
                List<FunctionPolicy> listGroupFuncPolicy = this._functionService.GetByGroupAndSite(
                                                                    criteria.GroupId,
                                                                    criteria.SiteId,
                                                                    Environment).ToList();

                // list function will be remove by current user login
                var listGroupFuncRemove = new List<FunctionPolicy>();
                listGroupFuncRemove.AddRange(listGroupFuncPolicy);

                // list function policy of current user login
                List<FunctionPolicy> listCurrentUserFuncPolicy = this._functionService.GetByUserAndSiteIncludeGroup(
                                                                                        CurrentSession.UserId,
                                                                                        criteria.SiteId,
                                                                                        Environment).ToList();

                // check function host,contains in list function
                if (!criteria.FuncHost.IsNullOrDBNull())
                {
                    foreach (var itemFunc in criteria.FuncHost)
                    {
                        // function find from list function, policy is host
                        var tempFuncHost = listFunc.Where(f => f.Id == itemFunc).FirstOrDefault();

                        if (tempFuncHost.IsNullOrDBNull())
                        {
                            throw new Exception();
                        }

                        listGroupFuncRemove.Remove(f => f.Function.Id == itemFunc);
                    }
                }

                // check function read, with current user login have permission
                if (!criteria.FuncRead.IsNullOrDBNull())
                {
                    foreach (var itemFunc in criteria.FuncRead)
                    {
                        listGroupFuncRemove.Remove(f => f.Function.Id == itemFunc);
                    }
                }

                // check permission current user login with group be assign permission
                // check for function policy will be remove ( set policy = 0 )
                listGroupFuncRemove = listGroupFuncRemove.Where(f => f.Policy > 0).ToList();
                foreach (var itemFunc in listGroupFuncRemove)
                {
                    var tempFunc = listCurrentUserFuncPolicy
                                        .Where(f =>
                                            f.Function.Id == itemFunc.Function.Id
                                            && f.Policy >= itemFunc.Policy).FirstOrDefault();

                    if (tempFunc.IsNullOrDBNull())
                    {
                        throw new Exception();
                    }
                }

                // assign permission
                var result = _groupService.AssignFunction(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;

                    listGroupFuncPolicy = this._functionService.GetByGroupAndSite(
                                                                    criteria.GroupId,
                                                                    criteria.SiteId,
                                                                    Environment).ToList();
                }

                if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
                {
                    ViewBag.SiteSelectList = _siteService.GetAllSelectListItem(this.Environment, true, criteria.SiteId);
                }
                else
                {
                    ViewBag.SiteSelectList = _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                                           CurrentSession.UserId,
                                                           FunctionKeys.AssignFunctionToGroup.ToString(),
                                                           this.Environment,
                                                           policy: Constant.Policy.Read,
                                                           selectedSiteId: criteria.SiteId);
                }

                ViewBag.FunctionList = listFunc;

                ViewBag.GroupFuncPolicyList = listGroupFuncPolicy;

                ViewBag.UserFuncPolicyList = listCurrentUserFuncPolicy;

                ViewBag.UserLevel = CurrentSession.Level;
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        #endregion

        #region Group Info

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Read)]
        public ActionResult GroupInfo(int id)
        {
            try
            {
                return View(_groupService.GetById(id));
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }
        }

        #endregion
    }
}