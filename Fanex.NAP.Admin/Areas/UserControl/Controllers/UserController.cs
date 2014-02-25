// --------------------------------------------------------------------------
// <copyright file="UserController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Created</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Helpers;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// User controller.
    /// </summary>
    [NapSuperAuth]
    public class UserController : BaseController
    {
        #region Member

        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly ISiteService _siteService;
        private readonly IFunctionService _functionService;

        #endregion

        #region Ctor

        public UserController()
            : this(null, null, null, null)
        {
        }

        public UserController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            IFunctionService functionService)
        {
            _userService = userService ?? new UserService { ServiceContext = ServiceContext };
            _groupService = groupService ?? new GroupService { ServiceContext = ServiceContext };
            _siteService = siteService ?? new SiteService { ServiceContext = ServiceContext };
            this._functionService = functionService ?? new FunctionService { ServiceContext = ServiceContext };
        }
        #endregion

        #region Manage

        /// <summary>
        /// Manage user.
        /// </summary>
        /// <param name="id">The page index.</param>
        /// <param name="size">The page size.</param>
        /// <param name="q">The username.</param>
        /// <param name="groupId">The group ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Read)]
        public ActionResult Manage(
            int id = 1,
            int size = 20,
            string q = EmptyString,
            int groupId = 0,
            int siteId = -1)
        {
            // declares paging params
            int pageIndex = id;
            int pageSize = size;
            string username = ViewBag.Username = q.Trim();

            // if dont specify a given site
            if (siteId < 0)
            {
                // not specify all site 
                if (siteId != Constant.SearchAllSite)
                {
                    // gets data of current site
                    siteId = this.SiteId;
                }
            }

            // detects user rights on page
            // permission function manage user
            int assignFunctionPolicyCurrentSite = 0;

            int selectedSiteId = siteId == 0 || siteId == Constant.SearchAllSite ? this.SiteId : siteId;

            ViewFlags.HasEditRight =
                HasRight(FunctionKeys.ManageUser, Constant.Policy.Host)
                && HasRightOnSite(selectedSiteId, FunctionKeys.ManageUser, Constant.Policy.Host);

            ViewFlags.HasAssignSiteRight =
                HasRight(FunctionKeys.AssignSiteToUser, Constant.Policy.Read)
                && HasRightOnSite(selectedSiteId, FunctionKeys.AssignSiteToUser, Constant.Policy.Read);

            // if users choose a specific site (siteId == 0 means users choose [No Site])
            bool isSpecificSiteChosen = siteId != 0;

            // show level column
            ViewFlags.IsShowLevelColumn = isSpecificSiteChosen;

            // is portal admin in NAP
            ViewBag.IsPortalAdmin = _userService.GetUserSiteLevel(CurrentSession.UserId, NapAdminId)
                                                == (int)Constant.Level.PortalAdmin;

            // detects "Assign Function" right
            if (isSpecificSiteChosen)
            {
                ViewFlags.HasAssignFunctionRight =
                    HasRight(FunctionKeys.AssignFunctionToUser, Constant.Policy.Read)
                    && HasRightOnSite(siteId, FunctionKeys.AssignFunctionToUser, Constant.Policy.Read);
            }

            var listSite = new List<SelectListItem>();

            if (ThisSiteIsNapAdmin)
            {
                // gets data for dropdownlist of sites
                var siteSelectList = new List<SelectListItem>();

                listSite = UserIsPortalAdmin
                ? _siteService.GetAllSelectListItem(this.Environment, true, siteId).ToList()
                : _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                        CurrentSession.UserId,
                                        FunctionKeys.ManageUser,
                                        this.Environment,
                                        policy: Constant.Policy.Read,
                                        selectedSiteId: siteId).ToList();

                siteSelectList.Add(new SelectListItem 
                { 
                    Text = ViewResources.All_Site, 
                    Value = Constant.SearchAllSite.ToString() 
                });

                siteSelectList.AddRange(listSite);

                siteSelectList.Add(new SelectListItem 
                { 
                    Text = ViewResources.No_Site, 
                    Value = "0" 
                });

                ViewBag.SiteSelectList = siteSelectList;
            }

            // gets data for dropdownlist of groups
            ViewBag.GroupSelectList = _groupService.GetAllSelectListItem(selectedGroupId: groupId);

            // gets ViewBag data
            ViewBag.SiteId = siteId;
            ViewBag.UserId = CurrentSession.UserId;
            ViewBag.ReturnGroupId = groupId;

            // gets level of currently-login-user
            if (isSpecificSiteChosen)
            {
                ViewBag.Level = _userService.GetUserSiteLevel(CurrentSession.UserId, siteId);
            }

            int totalUsers = 0;

            IQueryable<UserLevel> users = null;

            if (siteId == Constant.SearchAllSite)
            {
                users = _userService.GetAllByUsernameGroupSite(
                    pageIndex,
                    pageSize,
                    out totalUsers,
                    username,
                    groupId,
                    listSite);

                var hashManageUserPolicy = _userService.GetUserListSiteFunctionKeyPolicy(
                                                            CurrentSession.UserId,
                                                            FunctionKeys.ManageUser,
                                                            listSite);

                var hashAssignFunctionPolicy = _userService.GetUserListSiteFunctionKeyPolicy(
                                                                CurrentSession.UserId,
                                                                FunctionKeys.AssignFunctionToUser,
                                                                listSite);

                var hashAssignSite = _userService.GetUserListSiteFunctionKeyPolicy(
                                                                CurrentSession.UserId,
                                                                FunctionKeys.AssignSiteToUser,
                                                                listSite);

                var listSiteLevel = _siteService.GetSiteLevelByUser(CurrentSession.UserId, Environment, true).ToList();

                ViewBag.HashManageUser = hashManageUserPolicy;
                ViewBag.HashAssignFunction = hashAssignFunctionPolicy;
                ViewBag.HashAssignSite = hashAssignSite;
                ViewBag.ListSiteLevel = listSiteLevel;
                ViewBag.ViewFlagsCurrentSite = new ViewFlags
                {
                    HasManageUserRight = HasRight(FunctionKeys.ManageUser, Constant.Policy.Host),
                    HasAssignFunctionRight = assignFunctionPolicyCurrentSite >= (int)Constant.Policy.Read,
                    HasAssignSiteRight = HasRight(FunctionKeys.AssignSiteToUser, Constant.Policy.Read)
                };

                foreach (DictionaryEntry item in hashAssignFunctionPolicy)
                {
                    if ((int)item.Value >= (int)Constant.Policy.Read)
                    {
                        ViewFlags.IsShowActionColumn = true;
                        break;
                    }
                }

                foreach (DictionaryEntry item in hashAssignSite)
                {
                    if ((int)item.Value >= (int)Constant.Policy.Read)
                    {
                        ViewFlags.IsShowActionColumn = true;
                        break;
                    }
                }

                foreach (DictionaryEntry item in hashManageUserPolicy)
                {
                    if ((int)item.Value >= (int)Constant.Policy.Host)
                    {
                        ViewFlags.IsShowActionColumn = true;
                        break;
                    }
                }
            }
            else
            {
                users = _userService.SearchUser(pageIndex, pageSize, out totalUsers, username, groupId, siteId);
                ViewFlags.IsShowActionColumn = ViewFlags.HasEditRight || ViewFlags.HasAssignSiteRight
                    || ViewFlags.HasAssignFunctionRight;
            }

            return View(new PagerList<UserLevel>(pageIndex, pageSize, totalUsers, users));
        }

        /// <summary>
        /// Manage user.
        /// </summary>
        /// <param name="size">The page size.</param>
        /// <param name="q">The username.</param>
        /// <param name="groupId">The group ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Read)]
        public ActionResult Manage(int size = 20, string q = EmptyString, int groupId = 0, int siteId = -1)
        {
            return RedirectToAction(
                "Manage",
                new
                {
                    id = 1,
                    size = size,
                    q = q,
                    groupId = groupId,
                    siteId = siteId
                });
        }

        #endregion

        #region Assign Site

        /// <summary>
        /// Assign site.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="groupId">The group ID.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.AssignSiteToUser, Policy = Constant.Policy.Read)]
        public ActionResult AssignSite(int id, int? siteId, int? groupId, int pageIndex = 1)
        {
            // back to manage with current selected site and page index
            ViewBag.SiteId = siteId ?? this.SiteId;
            ViewBag.PageIndex = pageIndex;
            ViewBag.ReturnGroupId = groupId ?? 0;

            var siteList = new List<ISite>();
            var editableSiteList = new List<ISite>();
            var siteLevelList = new List<SiteLevel>();
            var currentUserSiteLevelList = new List<SiteLevel>();

            if (UserIsPortalAdmin)
            {
                siteList = _siteService.GetAll(this.Environment, true).ToList();
                currentUserSiteLevelList = _siteService.GetSiteLevelByUser(CurrentSession.UserId, this.Environment, null).ToList();
                ViewFlags.HasAssignSiteReadRight = true;
                ViewFlags.HasAssignSiteRight = true;
            }
            else
            {
                siteList = _siteService.GetByUserAndFunctionKeyIncludeGroup(
                                                CurrentSession.UserId,
                                                FunctionKeys.AssignSiteToUser,
                                                this.Environment,
                                                policy: Constant.Policy.Read).ToList();

                ViewFlags.HasAssignSiteReadRight = HasRight(FunctionKeys.AssignSiteToUser, Constant.Policy.Read);
                ViewFlags.HasAssignSiteRight = HasRight(FunctionKeys.AssignSiteToUser, Constant.Policy.Host);

                if (ViewFlags.HasAssignSiteRight)
                {
                    editableSiteList = _siteService.GetByUserAndFunctionKeyIncludeGroup(
                                                            CurrentSession.UserId,
                                                            FunctionKeys.AssignSiteToUser,
                                                            this.Environment,
                                                            policy: Constant.Policy.Host).ToList();
                }

                currentUserSiteLevelList = _siteService.GetSiteLevelByUser(CurrentSession.UserId, this.Environment, null).ToList();
            }

            siteLevelList = _siteService.GetSiteLevelByUser(id, this.Environment, null).ToList();

            ViewBag.SiteList = siteList;                                    // list of site which current user has Assign Site To User permission
            ViewBag.CurrentUserSiteLevelList = currentUserSiteLevelList;    // current login site level list
            ViewBag.SiteLevelList = siteLevelList;                          // selected or un-selected user site level list
            ViewBag.EditableSiteList = editableSiteList;                    // list of site which current user has Assign Site To User HOST permission
            ViewBag.Level = CurrentSession.Level;                           // current login user level
            ViewBag.UserId = id;                                            // selected user ID
            ViewBag.Username = _userService.GetById(id).Username;           // selected username
            ViewBag.IsSelfEdit = CurrentSession.UserId == id;               // self edit or not
            ViewBag.CurrentSiteId = this.SiteId;                            // current site ID
            ViewBag.IsPortalAdmin = UserIsPortalAdmin;

            return View();
        }

        #endregion

        #region Get Available Sites for User

        /// <summary>
        /// Get available sites for user.
        /// </summary>
        /// <param name="userId">The user identity.</param>
        /// <param name="username">The username.</param>
        /// <param name="siteId">The site identity.</param>
        /// <param name="groupId">The group identity.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="level">The level of current login user.</param>
        /// <returns>The list sites available for user.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.AssignSiteToUser, Policy = Constant.Policy.Host)]
        public ActionResult GetAvailableSitesForUser(
            int userId,
            string username,
            int? siteId,
            int? groupId,
            int pageIndex,
            int level)
        {
            var availableSite = new List<ISite>();
            var hostRightSite = new List<ISite>();
            var currentUserSiteLevelList = new List<SiteLevel>();
            var liveSitesOnly = this.Environment.Equals(Constant.UAT) ? false : true;

            if (CurrentSession.UserId != userId)
            {
                availableSite = _siteService.GetAvailableSitesForUser(userId, liveSitesOnly).ToList();
                currentUserSiteLevelList = _siteService.GetSiteLevelByUser(CurrentSession.UserId, this.Environment, true).ToList();
                bool isHasAssignRight = HasRight(FunctionKeys.AssignSiteToUser, Constant.Policy.Host);

                if (isHasAssignRight)
                {
                    hostRightSite = _siteService.GetByUserAndFunctionKeyIncludeGroup(
                                                            CurrentSession.UserId,
                                                            FunctionKeys.AssignSiteToUser,
                                                            this.Environment,
                                                            policy: Constant.Policy.Host).ToList();
                }
            }

            ViewBag.AvailableSitesForUser = availableSite;
            ViewBag.CurrentUserSiteLevelList = currentUserSiteLevelList;
            ViewBag.HostRightSite = hostRightSite;
            ViewBag.Username = username;
            ViewBag.UserId = userId;
            ViewBag.SiteId = siteId;
            ViewBag.GroupId = groupId;
            ViewBag.PageIndex = pageIndex;
            ViewBag.Level = level;

            return View("~/Areas/UserControl/Views/User/AssignNewSite.cshtml");
        }

        #endregion Get Available Sites for User

        #region User Info

        /// <summary>
        /// View user information.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Read)]
        public ActionResult UserInfo(int id)
        {
            return View(_userService.GetById(id));
        }

        #endregion

        #region Add Existing User

        /// <summary>
        /// Add existing user.
        /// </summary>
        /// <param name="id">The page index.</param>
        /// <param name="size">The page size.</param>
        /// <param name="q">The username.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="returnPageIndex">Return page index.</param>
        /// <param name="groupId">The group ID.</param>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Host)]
        public ActionResult AddExisting(
            int id = 1,
            int size = 20,
            string q = EmptyString,
            int? siteId = null,
            int? returnPageIndex = null,
            int? groupId = null)
        {
            int pageIndex = id;
            int pageSize = size;
            string username = q.Trim();

            PagerList<IUser> userList = null;
            int totalUsers = 0;

            if (username.Length > 0)
            {
                var users = _userService.SearchByUsername(username, pageIndex, pageSize, out totalUsers);

                userList = new PagerList<IUser>(pageIndex, pageSize, totalUsers, users);
                ViewBag.Groups = _groupService.GetAllSelectListItem();
                ViewBag.Username = username;

                if (Request.IsAjaxRequest())
                {
                    return PartialView("AddExisting_UserList", userList);
                }
            }

            // return current state page manage user
            ViewBag.GroupId = groupId;
            ViewBag.SiteId = siteId;
            ViewBag.PageIndex = returnPageIndex;

            return View(userList);
        }

        #endregion

        #region Add New User

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="groupId">The group ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(int? siteId = null, int pageIndex = 1, int groupId = 0)
        {
            var criteria = new InsertCustCriteria
            {
                SiteId = siteId ?? this.SiteId,
                PageIndex = pageIndex,
                RerturnGroupId = groupId
            };

            ViewBag.Groups = _groupService.GetAllSelectListItem();

            return View(criteria);
        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(InsertCustCriteria criteria)
        {
            ViewBag.Groups = _groupService.GetAllSelectListItem(selectedGroupId: criteria.GroupId);

            if (ModelState.IsValid)
            {
                // check username duplication
                if (_userService.CheckUsernameExist(criteria.Username))
                {
                    ModelState.AddModelError("Username", string.Format(ViewResources.DuplicateMsg, criteria.Username));
                    return View(criteria);
                }
                    
                // add new user
                criteria.UserPwd = Cryptography.CfsEnCode(criteria.UserPwd);
                criteria.FirstName = criteria.FirstName ?? string.Empty;
                criteria.LastName = criteria.LastName ?? string.Empty;
                criteria.Email = criteria.Email ?? string.Empty;

                // assign all functions to the new user if he is site admin or portal admin
                ICommandResult result = _userService.Insert(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                    ViewBag.UserId = criteria.CustId;
                }
            }

            return View(criteria);
        }

        #endregion

        #region Edit User

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="returnGroupId">Return group ID.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int id, int? siteId, int? returnGroupId, int pageIndex = 1)
        {
            var criteria = new UpdateCustCriteria
            {
                SiteId = siteId ?? this.SiteId,
                PageIndex = pageIndex,
                ReturnGroupId = returnGroupId ?? 0
            };

            IUser user = _userService.GetById(id);

            if (user == null)
            {
                return RedirectToAction("Manage");
            }

            criteria.CustId = user.Id;
            criteria.FirstName = user.FirstName;
            criteria.LastName = user.LastName;
            criteria.Email = user.Email;
            criteria.Username = user.Username;
            criteria.GroupId = user.Group != null ? user.Group.Id : 0;

            if (user.Group == null)
            {
                ViewBag.Groups = _groupService.GetAllSelectListItem();
            }
            else
            {
                ViewBag.Groups = _groupService.GetAllSelectListItem(selectedGroupId: user.Group.Id);
            }

            return View(criteria);
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Host)]
        public ActionResult Edit(UpdateCustCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.FirstName = criteria.FirstName ?? string.Empty;
                criteria.LastName = criteria.LastName ?? string.Empty;
                criteria.Email = criteria.Email ?? string.Empty;

                ICommandResult result = _userService.InsertOrUpdate(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            ViewBag.Groups = _groupService.GetAllSelectListItem(selectedGroupId: criteria.GroupId);

            return View(criteria);
        }

        #endregion

        #region Reset Password

        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="groupId">The group ID.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Host)]
        public ActionResult ResetPassword(int id, int? siteId, int? groupId, int pageIndex = 1)
        {
            var criteria = new ResetPasswordCustCriteria
            {
                SiteId = siteId ?? this.SiteId,
                PageIndex = pageIndex,
                ReturnGroupId = groupId ?? 0
            };

            IUser user = _userService.GetById(id);

            if (user == null)
            {
                return RedirectToAction("Manage");
            }

            criteria.CustId = user.Id;
            criteria.Username = user.Username;
            criteria.UserPwd = string.Empty;
            criteria.ConfirmUserPwd = string.Empty;

            return View(criteria);
        }

        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Host)]
        public ActionResult ResetPassword(ResetPasswordCustCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                IUser user = _userService.GetById(criteria.CustId);

                if (user == null)
                {
                    return RedirectToAction("Manage");
                }

                // check password confirmation
                if (!criteria.UserPwd.Equals(criteria.ConfirmUserPwd))
                {
                    ModelState.AddModelError("Password", ViewResources.WrongPasswordConfirmation);

                    return View(criteria);
                }

                criteria.UserPwd = Cryptography.CfsEnCode(criteria.UserPwd);

                ICommandResult result = _userService.InsertOrUpdate(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            return View(criteria);
        }

        #endregion

        #region Change Password

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new UpdatePasswordCustCriteria { Username = CurrentSession.Username });
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ChangePassword(UpdatePasswordCustCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetById(CurrentSession.UserId);
                string oldPwd = Cryptography.CfsEnCode(criteria.OldUserPwd);

                if (oldPwd.Equals(user.Password))
                {
                    criteria.CustId = user.Id;
                    criteria.UserPwd = Cryptography.CfsEnCode(criteria.UserPwd);

                    ICommandResult result = _userService.InsertOrUpdate(criteria);

                    if (result != null && result.IsSuccessful)
                    {
                        ViewBag.Success = true;
                    }
                }
                else
                {
                    ModelState.AddModelError("OldUserPwd", ViewResources.Wrong_password);
                }
            }

            return View(criteria);
        }

        #endregion

        #region Assign Permission

        /// <summary>
        /// Assign user permission.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="returnSiteId">Return site ID.</param>
        /// <param name="groupId">The group ID.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToUser, Policy = Constant.Policy.Read)]
        public ActionResult AssignPermission(
            int id, 
            int siteId = 0, 
            int pageIndex = 1, 
            int returnSiteId = 0, 
            int groupId = 0)
        {
            var criteria = new AssignFunctionUserCriteria
            {
                ReturnSiteId = returnSiteId != 0 ? returnSiteId : siteId,
                PageIndex = pageIndex,
                GroupId = groupId
            };

            if (siteId == 0)
            {
                siteId = this.SiteId;
            }

            IUser user = _userService.GetById(id);
            ISite site = _siteService.GetById(siteId);

            if (ReferenceEquals(user, null) || ReferenceEquals(site, null))
            {
                return HttpNotFound();
            }

            criteria.UserId = id;
            criteria.Username = user.Username;
            criteria.SiteId = siteId;

            if (!ReferenceEquals(user.Group, null))
            {
                ViewBag.GroupFuncPolicyList = 
                    this._functionService.GetByGroupAndSite(user.Group.Id, criteria.SiteId, this.Environment);
            }

            ViewFlags.HasAssignFunctionRight =
                id != CurrentSession.UserId
                && HasRightOnSite(criteria.SiteId, FunctionKeys.AssignFunctionToUser, Constant.Policy.Host);

            if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
            {
                ViewBag.Sites = _siteService.GetAllSelectListItem(this.Environment, true, criteria.SiteId);
            }
            else
            {
                ViewBag.Sites = _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                                    CurrentSession.UserId,
                                                    FunctionKeys.AssignFunctionToUser,
                                                    this.Environment,
                                                    policy: Constant.Policy.Read,
                                                    selectedSiteId: criteria.SiteId);
            }

            ViewBag.FunctionList = _functionService.GetBySiteInHierarchy(criteria.SiteId, Environment, true).ToList();
            ViewBag.UserFuncPolicyList = 
                _functionService.GetByUserAndSite(criteria.UserId, criteria.SiteId, this.Environment);
            ViewBag.CurrentUserFuncPolicyList =
                this._functionService.GetByUserAndSiteIncludeGroup(CurrentSession.UserId, criteria.SiteId, Environment);
            ViewBag.AdminLevel = CurrentSession.Level;
            ViewBag.UserLevel = _userService.GetUserSiteLevel(user.Id, site.Id);

            return View(criteria);
        }

        /// <summary>
        /// Assign user permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.AssignFunctionToUser, Policy = Constant.Policy.Host)]
        public ActionResult AssignPermission(AssignFunctionUserCriteria criteria)
        {
            // check user and site
            IUser user = _userService.GetById(criteria.UserId);
            ISite site = _siteService.GetById(criteria.SiteId);

            if (user == null || site == null)
            {
                return RedirectToAction("Manage");
            }

            // check policy
            int policy = _userService.GetUserSiteFunctionKeyPolicy(
                CurrentSession.UserId,
                criteria.SiteId,
                FunctionKeys.AssignFunctionToUser);

            if (policy < (int)Constant.Policy.Read)
            {
                return RedirectToAction("Unauthorize", "Common");
            }

            ViewFlags.HasAssignFunctionRight = policy >= (int)Constant.Policy.Host;

            criteria.Username = user.Username;
            criteria.ModifiedBy = CurrentSession.UserId;
            criteria.ModifiedDate = DateTime.Now;

            // list function policy
            List<IFunction> listFunc = 
                this._functionService.GetBySiteInHierarchy(criteria.SiteId, Environment, true).ToList().ToList();

            // list user function policy
            List<FunctionPolicy> listUserFuncPolicy = 
                this._functionService.GetByUserAndSite(criteria.UserId, criteria.SiteId, this.Environment).ToList();

            // list function will be remove by current user login
            var listUserFuncRemove = new List<FunctionPolicy>();
            listUserFuncRemove.AddRange(listUserFuncPolicy);

            // list current user login function policy
            List<FunctionPolicy> listCurrentUserFuncPolicy = 
                _functionService.GetByUserAndSiteIncludeGroup(CurrentSession.UserId, criteria.SiteId, Environment).ToList();

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

                    // remove all function host
                    listUserFuncRemove.Remove(f => f.Function.Id == itemFunc);
                }
            }

            // check function read, with current user login have permission
            if (!criteria.FuncRead.IsNullOrDBNull())
            {
                foreach (int itemFunc in criteria.FuncRead)
                {
                    listUserFuncRemove.Remove(f => f.Function.Id == itemFunc);
                }
            }

            // check permission current user login with user be assign permission
            // check for function policy will be remove ( set policy = 0 )
            listUserFuncRemove = listUserFuncRemove.Where(f => f.Policy > 0).ToList();
            foreach (FunctionPolicy itemFunc in listUserFuncRemove)
            {
                FunctionPolicy tempFunc = listCurrentUserFuncPolicy
                    .Where(f => f.Function.Id == itemFunc.Function.Id && f.Policy >= itemFunc.Policy).FirstOrDefault();

                if (tempFunc.IsNullOrDBNull())
                {
                    throw new Exception();
                }
            }

            ICommandResult result = _userService.AssignFunction(criteria);

            if (result != null && result.IsSuccessful)
            {
                ViewBag.Success = true;

                listUserFuncPolicy = 
                    this._functionService.GetByUserAndSite(criteria.UserId, criteria.SiteId, this.Environment).ToList();
            }

            if (!ReferenceEquals(user.Group, null))
            {
                ViewBag.GroupFuncPolicyList = 
                    this._functionService.GetByGroupAndSite(user.Group.Id, criteria.SiteId, this.Environment);
            }

            if (CurrentSession.Level == (int)Constant.Level.PortalAdmin)
            {
                ViewBag.Sites = _siteService.GetAllSelectListItem(this.Environment, true, criteria.SiteId);
            }
            else
            {
                ViewBag.Sites = _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                                    CurrentSession.UserId,
                                                    FunctionKeys.AssignFunctionToUser,
                                                    this.Environment,
                                                    policy: Constant.Policy.Read,
                                                    selectedSiteId: criteria.SiteId);
            }

            ViewBag.FunctionList = listFunc;
            ViewBag.UserFuncPolicyList = listUserFuncPolicy;
            ViewBag.CurrentUserFuncPolicyList = listCurrentUserFuncPolicy;
            ViewBag.AdminLevel = CurrentSession.Level;
            ViewBag.UserLevel = _userService.GetUserSiteLevel(user.Id, site.Id);

            return View(criteria);
        }

        /// <summary>
        /// Assign site to user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="level">The level of user.</param>
        /// <param name="isActive">The boolean to identify the user active or not.</param>
        /// <returns>Result string.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.AssignSiteToUser, Policy = Constant.Policy.Host)]
        public string AssignSiteToUser(int userId, int siteId, int level, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                var criteria = new AssignSiteUserCriteria
                {
                    CreatedBy = CurrentSession.UserId,
                    CreatedDate = DateTime.Now,
                    UserId = userId,
                    SiteId = siteId,
                    Level = level,
                    IsActive = isActive
                };

                ICommandResult result = _userService.AssignSite(criteria);

                if (result != null && result.IsSuccessful)
                {
                    return Constant.OK;
                }
            }

            return Constant.ERROR;
        }

        #endregion

        #region Update Site To User
        /// <summary>
        /// Update information of site to user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="level">The level.</param>
        /// <param name="isActive">The is active.</param>
        /// <returns>The result string.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.AssignSiteToUser, Policy = Constant.Policy.Host)]
        public string UpdateSiteToUser(int userId, int siteId, int level, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                var criteria = new UpdateSiteToUserCriteria
                {
                    ModifiedBy = CurrentSession.UserId,
                    ModifiedDate = DateTime.Now,
                    UserId = userId,
                    SiteId = siteId,
                    Level = level,
                    IsActive = isActive
                };

                ICommandResult result = _userService.UpdateSite(criteria);

                if (result != null && result.IsSuccessful)
                {
                    return Constant.OK;
                }
            }

            return Constant.ERROR;
        }

        #endregion Update Site To User

        #region Profile

        /// <summary>
        /// View user profile.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult UserProfile()
        {
            var criteria = new UpdateCustCriteria();
            var user = _userService.GetById(CurrentSession.UserId);

            criteria.CustId = user.Id;
            criteria.Username = user.Username;
            criteria.FirstName = user.FirstName;
            criteria.LastName = user.LastName;
            criteria.Email = user.Email;
            criteria.GroupId = user.Group != null ? user.Group.Id : 0;

            return View(criteria);
        }

        /// <summary>
        /// View user profile.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult UserProfile(UpdateCustCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.CustId = CurrentSession.UserId;
                criteria.Username = CurrentSession.Username;
                criteria.FirstName = criteria.FirstName ?? string.Empty;
                criteria.LastName = criteria.LastName ?? string.Empty;
                criteria.Email = criteria.Email ?? string.Empty;

                ICommandResult result = _userService.InsertOrUpdate(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            return View(criteria);
        }

        #endregion

        #region Ajax

        /// <summary>
        /// View user list.
        /// </summary>
        /// <param name="groupId">The group ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Read)]
        public ActionResult UserList(int groupId, int siteId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                return View(_userService.GetByGroupAndSite(groupId, siteId));
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Get user site.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Read)]
        public ActionResult GetUserSite(int userId)
        {
            if (Request.IsAjaxRequest())
            {
                IEnumerable<SiteLevel> listSiteLive = _siteService.GetSiteLevelByUser(userId, this.Environment);

                return Json(listSiteLive);
            }

            return Json(null);
        }

        /// <summary>
        /// Get user function policy.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult GetUserFunctionPolicy(int userId)
        {
            if (Request.IsAjaxRequest())
            {
                IEnumerable<FunctionPolicy> listFunctionPolicy = this._functionService.GetByUser(userId, this.Environment);

                return Json(listFunctionPolicy);
            }

            return Json(null);
        }

        /// <summary>
        /// Get group function policy.
        /// </summary>
        /// <param name="groupId">The group ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult GetGroupFunctionPolicy(int groupId)
        {
            if (Request.IsAjaxRequest())
            {
                if (groupId != 0)
                {
                    IEnumerable<FunctionPolicy> listFunctionPolicy = this._functionService.GetByGroup(groupId, this.Environment);

                    return Json(listFunctionPolicy);
                }
            }

            return Json(null);
        }

        /// <summary>
        /// Add user to group.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="groupId">The group ID.</param>
        /// <returns>Result string.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Host)]
        public string AddUserToGroup(int userId, int groupId)
        {
            if (Request.IsAjaxRequest() && userId > 0 && groupId > 0)
            {
                var criteria = new InsertUserCriteria
                {
                    UserId = userId,
                    GroupId = groupId,
                    CreatedBy = CurrentSession.UserId,
                    CreatedDate = DateTime.Now
                };

                IUser user = _userService.GetById(userId);

                if (user != null)
                {
                    throw new Exception(string.Format("User {0} (id: {1}) existed in NAP!", user.Username, user.Id));
                }

                ICommandResult result = _userService.InsertOrUpdate(criteria);

                if (result != null && result.IsSuccessful)
                {
                    return Constant.OK;
                }
            }

            return Constant.ERROR;
        }

        /// <summary>
        /// Get user by group.
        /// </summary>
        /// <param name="groupId">The group ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageUser, Policy = Constant.Policy.Read)]
        public ActionResult GetUserByGroup(int groupId)
        {
            if (Request.IsAjaxRequest())
            {
                IEnumerable<IUser> listUser = _userService.GetByGroupAndSite(groupId);

                return Json(listUser);
            }

            return Json(null);
        }

        #endregion

        #region Private method

        /// <summary>
        /// Get data for assign permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        private void GetDataForAssignPermission(AssignFunctionUserCriteria criteria)
        {
            ViewFlags.HasAssignFunctionRight = 
                HasRightOnSite(criteria.SiteId, FunctionKeys.AssignFunctionToUser, Constant.Policy.Host);

            if (this.UserIsPortalAdmin)
            {
                ViewBag.Sites = 
                    _siteService.GetAllSelectListItem(this.Environment, isActive: true, selectedSiteId: criteria.SiteId);
            }
            else
            {
                ViewBag.Sites = _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                                    CurrentSession.UserId,
                                                    FunctionKeys.AssignFunctionToUser,
                                                    this.Environment,
                                                    policy: Constant.Policy.Read,
                                                    selectedSiteId: criteria.SiteId);
            }

            ViewBag.FunctionList = _functionService.GetBySiteInHierarchy(criteria.SiteId, Environment, true).ToList();
            ViewBag.UserFuncPolicyList = 
                _functionService.GetByUserAndSiteIncludeGroup(criteria.UserId, criteria.SiteId, this.Environment);

            ViewBag.CurrentUserFuncPolicyList =
                this._functionService.GetByUserAndSiteIncludeGroup(CurrentSession.UserId, criteria.SiteId, Environment);
        }

        #endregion
    }
}
