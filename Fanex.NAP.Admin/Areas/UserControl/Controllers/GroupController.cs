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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Group;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Data;

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
            _functionService = functionService ?? new FunctionService { ServiceContext = ServiceContext };
            _userService = userService ?? new UserService { ServiceContext = ServiceContext };
        }

        #endregion

        #region Manage

        /// <summary>
        /// Manage group.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Read)]
        public ActionResult Manage()
        {
            ViewFlags.HasEditRight = HasRight(FunctionKeys.ManageGroup, Constant.Policy.Host);
            ViewFlags.HasAssignFunctionRight = HasRight(FunctionKeys.AssignFunctionToGroup, Constant.Policy.Read);
            ViewFlags.HasManageUserRight = HasRight(FunctionKeys.ManageUser, Constant.Policy.Read);

            ViewBag.GroupList = _groupService.GetAllWithNumOfUser(string.Empty, Environment);

            return View(new GetAllWithNumOfUserGroupCriteria());
        }

        /// <summary>
        /// Manage group.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Read)]
        public ActionResult Manage(GetAllWithNumOfUserGroupCriteria criteria)
        {
            criteria.Environment = Environment;

            ViewFlags.HasEditRight = HasRight(FunctionKeys.ManageGroup, Constant.Policy.Host);
            ViewFlags.HasAssignFunctionRight = HasRight(FunctionKeys.AssignFunctionToGroup, Constant.Policy.Read);
            ViewFlags.HasManageUserRight = HasRight(FunctionKeys.ManageUser, Constant.Policy.Read);

            ViewBag.GroupList = _groupService.GetAllWithNumOfUser(criteria.GroupName);

            return View(criteria);
        }

        #endregion

        #region Add New Group

        /// <summary>
        /// Add new group.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult AddNew()
        {
            return View(new InsertGroupCriteria());
        }

        /// <summary>
        /// Add new group.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(InsertGroupCriteria criteria)
        {
            criteria.GroupName = criteria.GroupName.Trim();

            if (ModelState.IsValid)
            {
                // Check name exist
                IGroup group = _groupService.GetByName(criteria.GroupName);

                if (!ReferenceEquals(group, null))
                {
                    ModelState.AddModelError("SiteName", string.Format(ViewResources.DuplicateMsg, criteria.GroupName));

                    return View(criteria);
                }

                // Add group
                criteria.CreatedBy = CurrentSession.UserId;
                criteria.CreatedDate = DateTime.Now;

                ICommandResult result = _groupService.Insert(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            return View(criteria);
        }

        #endregion

        #region Edit Group

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="id">The group ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int id)
        {
            var criteria = new UpdateGroupCriteria();

            IGroup group = _groupService.GetById(id);

            if (ReferenceEquals(group, null))
            {
                return RedirectToAction("Manage");
            }

            criteria.GroupId = group.Id;
            criteria.Description = group.Description;
            criteria.GroupName = group.Name;
            criteria.IsActive = group.IsActive;

            return View(criteria);
        }

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult Edit(UpdateGroupCriteria criteria)
        {
            criteria.GroupName = criteria.GroupName.Trim();

            if (ModelState.IsValid)
            {
                // Check name exist
                IGroup group = _groupService.GetByName(criteria.GroupName);

                if (!ReferenceEquals(group, null))
                {
                    if (group.Id != criteria.GroupId)
                    {
                        ModelState.AddModelError("SiteName", string.Format(ViewResources.DuplicateMsg, criteria.GroupName));

                        return View(criteria);
                    }
                }

                // update function
                criteria.ModifiedBy = CurrentSession.UserId;
                criteria.ModifiedDate = DateTime.Now;

                var result = _groupService.Update(criteria);

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
        /// Update group status.
        /// </summary>
        /// <param name="id">The group ID.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Host)]
        public ActionResult Activate(int id, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                var criteria = new UpdateStatusGroupCriteria
                {
                    GroupId = id,
                    IsActive = isActive,
                    ModifiedBy = CurrentSession.UserId,
                    ModifiedDate = DateTime.Now
                };
                
                ICommandResult result = _groupService.UpdateStatus(criteria);

                if (result != null && result.IsSuccessful)
                {
                    return Json(Constant.OK);
                }
            }

            return new EmptyResult();
        }

        #endregion

        #region Assign Permission

        /// <summary>
        /// Assign group permission.
        /// </summary>
        /// <param name="id">The group ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToGroup, Policy = Constant.Policy.Read)]
        public ActionResult AssignPermission(int id, int siteId = 0)
        {
            var criteria = new AssignFunctionGroupCriteria();

            siteId = siteId == 0 || this.SiteId != NapAdminId ? this.SiteId : siteId;

            IGroup group = _groupService.GetById(id);
            ISite site = _siteService.GetById(siteId);

            if (group == null || site == null)
            {
                return RedirectToAction("Manage");
            }

            criteria.GroupId = group.Id;
            criteria.GroupName = group.Name;
            criteria.SiteId = site.Id;

            ViewFlags.HasAssignFunctionRight =
                HasRightOnSite(criteria.SiteId, FunctionKeys.AssignFunctionToGroup, Constant.Policy.Host);

            if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
            {
                ViewBag.SiteSelectList = _siteService.GetAllSelectListItem(this.Environment, true, criteria.SiteId);
            }
            else
            {
                ViewBag.SiteSelectList = _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                    CurrentSession.UserId,
                    FunctionKeys.AssignFunctionToGroup,
                    this.Environment,
                    policy: Constant.Policy.Read,
                    selectedSiteId: criteria.SiteId);
            }

            ViewBag.FunctionList = this._functionService.GetBySiteInHierarchy(criteria.SiteId, Environment, true);
            ViewBag.GroupFuncPolicyList = 
                this._functionService.GetByGroupAndSite(criteria.GroupId, criteria.SiteId, Environment);
            ViewBag.UserFuncPolicyList =
                this._functionService.GetByUserAndSiteIncludeGroup(CurrentSession.UserId, criteria.SiteId, Environment);
            ViewBag.UserLevel = CurrentSession.Level;

            return View(criteria);
        }

        /// <summary>
        /// Assign group permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToGroup, Policy = Constant.Policy.Host)]
        public ActionResult AssignPermission(AssignFunctionGroupCriteria criteria)
        {
            // check group and site
            IGroup group = _groupService.GetById(criteria.GroupId);
            ISite site = _siteService.GetById(criteria.SiteId);

            if (group == null || site == null)
            {
                return RedirectToAction("Manage");
            }

            // check policy
            int policy = _userService.GetUserSiteFunctionKeyPolicy(
                CurrentSession.UserId,
                criteria.SiteId,
                FunctionKeys.AssignFunctionToGroup);

            if (policy < (int)Constant.Policy.Read)
            {
                return RedirectToAction("Unauthorize", "Common");
            }

            ViewFlags.HasAssignFunctionRight = policy >= (int)Constant.Policy.Host;

            criteria.ModifiedBy = CurrentSession.UserId;
            criteria.ModifiedDate = DateTime.Now;

            // list function policy
            List<IFunction> listFunc = 
                this._functionService.GetBySiteInHierarchy(criteria.SiteId, Environment, true).ToList();

            // list group function policy
            List<FunctionPolicy> listGroupFuncPolicy = 
                this._functionService.GetByGroupAndSite(criteria.GroupId, criteria.SiteId, Environment).ToList();

            // list function will be remove by current user login
            var listGroupFuncRemove = new List<FunctionPolicy>();
            listGroupFuncRemove.AddRange(listGroupFuncPolicy);

            // list function policy of current user login
            List<FunctionPolicy> listCurrentUserFuncPolicy = 
                this._functionService.GetByUserAndSiteIncludeGroup(CurrentSession.UserId, criteria.SiteId, Environment).ToList();

            // check function host,contains in list function
            if (!criteria.FuncHost.IsNullOrDBNull())
            {
                foreach (int itemFunc in criteria.FuncHost)
                {
                    // function find from list function, policy is host
                    IFunction tempFuncHost = listFunc.Where(f => f.Id == itemFunc).FirstOrDefault();

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
                foreach (int itemFunc in criteria.FuncRead)
                {
                    listGroupFuncRemove.Remove(f => f.Function.Id == itemFunc);
                }
            }

            // check permission current user login with group be assign permission
            // check for function policy will be remove ( set policy = 0 )
            listGroupFuncRemove = listGroupFuncRemove.Where(f => f.Policy > 0).ToList();

            foreach (FunctionPolicy itemFunc in listGroupFuncRemove)
            {
                var tempFunc = listCurrentUserFuncPolicy
                    .Where(f =>f.Function.Id == itemFunc.Function.Id && f.Policy >= itemFunc.Policy).FirstOrDefault();

                if (tempFunc.IsNullOrDBNull())
                {
                    throw new Exception();
                }
            }

            // assign permission
            ICommandResult result = _groupService.AssignFunction(criteria);

            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;

                listGroupFuncPolicy = 
                    this._functionService.GetByGroupAndSite(criteria.GroupId, criteria.SiteId, Environment).ToList();
            }

            if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
            {
                ViewBag.SiteSelectList = _siteService.GetAllSelectListItem(this.Environment, true, criteria.SiteId);
            }
            else
            {
                ViewBag.SiteSelectList = _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                                        CurrentSession.UserId,
                                                        FunctionKeys.AssignFunctionToGroup,
                                                        this.Environment,
                                                        policy: Constant.Policy.Read,
                                                        selectedSiteId: criteria.SiteId);
            }

            ViewBag.FunctionList = listFunc;
            ViewBag.GroupFuncPolicyList = listGroupFuncPolicy;
            ViewBag.UserFuncPolicyList = listCurrentUserFuncPolicy;
            ViewBag.UserLevel = CurrentSession.Level;

            return View(criteria);
        }

        #endregion

        #region Group Info

        /// <summary>
        /// Get group information.
        /// </summary>
        /// <param name="id">The group ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageGroup, Policy = Constant.Policy.Read)]
        public ActionResult GroupInfo(int id)
        {
            return View(_groupService.GetById(id));
        }

        #endregion
    }
}