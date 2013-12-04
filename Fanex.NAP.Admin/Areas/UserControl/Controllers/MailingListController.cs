// --------------------------------------------------------------------------
// <copyright file="MailingListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.26">Created</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Logging;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Function;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// MailingList Controller.
    /// </summary>
    public class MailingListController : BaseController
    {
        private readonly IMailingListService _mailingListService;
        private readonly IFunctionService _functionService;
        private readonly ISiteService _siteService;
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        
        public MailingListController()
            : this(null, null, null, null, null)
        {
        }

        public MailingListController(
            IFunctionService functionService, 
            ISiteService siteService, 
            IUserService userService, 
            IGroupService groupService, 
            IMailingListService mailingListService)
        {
            _mailingListService = mailingListService ?? new MailingListService { ServiceContext = ServiceContext };
            _functionService = functionService ?? new FunctionService { ServiceContext = ServiceContext };
            _siteService = siteService ?? new SiteService { ServiceContext = ServiceContext };
            _userService = userService ?? new UserService { ServiceContext = ServiceContext };
            _groupService = groupService ?? new GroupService { ServiceContext = ServiceContext };
        }

        /// <summary>
        /// Action to Activate and Deactivate Mailing List with mailing list id.
        /// </summary>
        /// <param name="id">Mailing List Id.</param>
        /// <param name="active">Activate is true or false.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult Activate(int id, bool active)
        {
            if (Request.IsAjaxRequest())
            {
                var result = _mailingListService.Update(new UpdateStatusMailingListCriteria
                {
                    ListId = id,
                    IsActive = active,
                    ModifiedBy = CurrentSession.UserId,
                    ModifiedDate = DateTime.Now
                });

                if (result != null && result.IsSuccessful)
                {
                    return Json(Constant.OK);
                }
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Action to set internal and unset internal for Mailing List with mailing list id.
        /// </summary>
        /// <param name="id">Mailing List Id.</param>
        /// <param name="isInternal">Internal is true or false.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult Internal(int id, bool isInternal)
        {
            if (Request.IsAjaxRequest())
            {
                var result = _mailingListService.Update(new UpdateIsInternalMailingListCriteria
                {
                    ListId = id,
                    IsInternal = isInternal,
                    ModifiedBy = CurrentSession.UserId,
                    ModifiedDate = DateTime.Now
                });

                if (result != null && result.IsSuccessful)
                {
                    return Json(Constant.OK);
                }
            }

            return new EmptyResult();
        }

        /// <summary>
        /// Action to view list Mailing List in a site with get method.
        /// </summary>
        /// <param name="siteId">Site ID which mailing list is showed follow it.</param>
        /// <returns>View Result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Read)]
        public ActionResult Manage(int? siteId)
        {
            GetMailingListByUserAndSiteIncludeGroupCriteria criteria = null;

            var hasEditRight = false;
            var isITGroup = CheckIsPortalAminOrITGroup();

            criteria = new GetMailingListByUserAndSiteIncludeGroupCriteria
            {
                UserId = CurrentSession.UserId,
                SiteId = siteId ?? this.SiteId,
                Environment = this.Environment,
                IsInternal = isITGroup
            };
                
            var mailingList = _mailingListService.GetBySite(criteria);
            IList<SelectListItem> siteItemList = new List<SelectListItem>();

            GetSitesAndPolicyForManageAction(criteria, ref hasEditRight, ref siteItemList);

            ViewBag.IsPortalAdmin = this.UserIsPortalAdmin;
            ViewBag.MailingList = mailingList;
            ViewBag.IsITGroup = isITGroup;
            ViewBag.ViewFlags.HasEditRight = hasEditRight;
            ViewBag.SiteItemList = siteItemList;

            return View(criteria);
        }

        /// <summary>
        /// Action to view list Mailing List in a site with post method.
        /// </summary>
        /// <param name="criteria">Criteria to query to DB.</param>
        /// <returns>View Result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Read)]
        public ActionResult Manage(GetMailingListByUserAndSiteIncludeGroupCriteria criteria)
        {
            var hasEditRight = false;
            var isITGroup = CheckIsPortalAminOrITGroup();

            criteria.UserId = CurrentSession.UserId;
            criteria.IsInternal = isITGroup;

            var mailingList = _mailingListService.GetBySite(criteria);
            IList<SelectListItem> siteItemList = new List<SelectListItem>();

            GetSitesAndPolicyForManageAction(criteria, ref hasEditRight, ref siteItemList);

            ViewBag.IsPortalAdmin = this.UserIsPortalAdmin;
            ViewBag.MailingList = mailingList;
            ViewBag.IsITGroup = isITGroup;
            ViewBag.ViewFlags.HasEditRight = hasEditRight;
            ViewBag.SiteItemList = siteItemList;

            return View(criteria);
        }

        /// <summary>
        /// Action to Add new Mailing List with get method.
        /// </summary>
        /// <param name="siteId">Site ID default.</param>
        /// <returns>View Result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(int? siteId = null)
        {
            var criteria = new InsertMailingListCriteria
            {
                SiteId = (!siteId.HasValue || siteId.Value == 0) ? this.SiteId : siteId.Value,
                Environment = this.Environment,
                IsInternal = false,
                BCCList = string.Empty
            };

            var siteItemList = GetSiteList(criteria.SiteId, criteria.Environment);
            var functionItemList = GetFunctionList(criteria.SiteId, criteria.Environment);
            var mailFunctionItemList = GetMailFunctionList(criteria.Environment);

            ViewBag.FunctionList = functionItemList;
            ViewBag.MailFunctionList = mailFunctionItemList;
            ViewBag.SiteItemList = siteItemList;
            ViewBag.IsITGroup = CheckIsPortalAminOrITGroup();

            return View(criteria);
        }

        /// <summary>
        /// Action to Add new Mailing List with post method.
        /// </summary>
        /// <param name="criteria">Criteria is contains value insert Mailing List.</param>
        /// <returns>View Result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(InsertMailingListCriteria criteria)
        {
            var siteItemList = GetSiteList(criteria.SiteId, criteria.Environment);
            var functionItemList = GetFunctionList(criteria.SiteId, criteria.Environment);
            var mailFunctionItemList = GetMailFunctionList(criteria.Environment);

            ViewBag.FunctionList = functionItemList;
            ViewBag.MailFunctionList = mailFunctionItemList;
            ViewBag.SiteItemList = siteItemList;
            ViewBag.IsITGroup = CheckIsPortalAminOrITGroup();

            if (ModelState.IsValid)
            {
                if (!CheckInvalidMailingListForAddNew(criteria))
                {
                    return View(criteria);
                }

                criteria.CreatedBy = this.CurrentSession.UserId;
                criteria.CreatedDate = DateTime.Now;
                criteria.ToList = RemoveDuplicatedEmail(criteria.ToList);
                criteria.CCList = RemoveDuplicatedEmail(criteria.CCList);
                criteria.BCCList = RemoveDuplicatedEmail(criteria.BCCList);

                var result = _mailingListService.Insert(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            return View(criteria);
        }

        /// <summary>
        /// Action to Edit Mailing List with get method.
        /// </summary>
        /// <param name="id">Mailing List Id.</param>
        /// <returns>View Result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int id)
        {
            var criteria = new UpdateMailingListCriteria();
            var siteName = string.Empty;
            var isITGroup = CheckIsPortalAminOrITGroup();
            var mailingList = _mailingListService.GetById(id);

            if (ReferenceEquals(mailingList, null))
            {
                return RedirectToAction("Manage");
            }

            criteria.ListId = mailingList.Id;
            criteria.ListKey = mailingList.Key;
            criteria.ListName = mailingList.Name;
            criteria.Description = mailingList.Description;
            criteria.SiteId = mailingList.Site.Id;
            criteria.SiteName = mailingList.Site.Name;
            criteria.Environment = mailingList.Environment;
            criteria.ListFunctionId = mailingList.MailFunction.Id;
            criteria.FunctionId = mailingList.Function.Id;
            criteria.ToList = mailingList.ToList;
            criteria.CCList = mailingList.CCList;
            criteria.BCCList = mailingList.BCCList;
            criteria.IsInternal = mailingList.IsInternal;
            criteria.IsActive = mailingList.IsActive;

            var functionItemList = GetFunctionList(criteria.SiteId, criteria.Environment);
            var mailFunctionItemList = GetMailFunctionList(criteria.Environment);

            ViewBag.MailFunctionList = mailFunctionItemList;
            ViewBag.FunctionList = functionItemList;
            ViewBag.IsITGroup = isITGroup;

            return View(criteria);
        }

        /// <summary>
        /// Action to Edit Mailing List with post method.
        /// </summary>
        /// <param name="criteria">Criteria is contains value to update Mailing List.</param>
        /// <returns>View Result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult Edit(UpdateMailingListCriteria criteria)
        {
            var siteName = string.Empty;
            var isITGroup = CheckIsPortalAminOrITGroup();

            ViewBag.IsITGroup = isITGroup;

            var functionItemList = GetFunctionList(criteria.SiteId, criteria.Environment);
            var mailFunctionItemList = GetMailFunctionList(criteria.Environment);

            ViewBag.MailFunctionList = mailFunctionItemList;
            ViewBag.FunctionList = functionItemList;
            ViewBag.IsITGroup = isITGroup;
                
            if (ModelState.IsValid)
            {
                if (!CheckInvalidMailingListForEdit(criteria))
                {
                    return View(criteria);
                }

                criteria.ModifiedBy = this.CurrentSession.UserId;
                criteria.ModifiedDate = DateTime.Now;
                criteria.ToList = RemoveDuplicatedEmail(criteria.ToList);
                criteria.CCList = RemoveDuplicatedEmail(criteria.CCList);
                criteria.BCCList = RemoveDuplicatedEmail(criteria.BCCList);

                var result = _mailingListService.Update(criteria);

                if (result != null && result.IsSuccessful)
                {
                    ViewBag.Success = true;
                }
            }

            return View(criteria);
        }

        /// <summary>
        /// Action to get function by site and environment.
        /// </summary>
        /// <param name="siteId">Site Id which function is in it.</param>
        /// <param name="environment">Environment is UAT or LIVE.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetFunctionBySite(int siteId, string environment = Constant.UAT)
        {
            if (Request.IsAjaxRequest())
            {
                List<Function> functionList = null;

                if (this.UserIsPortalAdmin)
                {
                    functionList = (List<Function>)_functionService.GetBySite(siteId, environment, true);
                }
                else
                {
                    var functionPolicyList = _functionService.GetByUserAndSiteIncludeGroup(this.CurrentSession.UserId, siteId, environment);
                    functionPolicyList = functionPolicyList.Where(t => t.Policy >= (byte)Constant.Policy.Host);
                    functionList = functionPolicyList.Select(t => t.Function).ToList();
                }

                return Json(functionList);
            }

            return Json(null);
        }

        /// <summary>
        /// Action to get mail function by environment.
        /// </summary>
        /// <param name="environment">Environment is UAT or LIVE.</param>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        public JsonResult GetMailFunctionByEnvironment(string environment = Constant.UAT)
        {
            if (Request.IsAjaxRequest())
            {
                var criteria = new GetByParentKeyAndSiteCriteria
                {
                    SiteId = this.SiteId,
                    ParentKey = FunctionKeys.MailingList,
                    Environment = environment,
                    IsActive = true,
                    RoleId = (byte)Constant.Policy.Host,
                    UserId = this.CurrentSession.UserId
                };

                var functionList = _functionService.GetByParentKeyAndSite(criteria).ToList();

                return Json(functionList);
            }

            return Json(null);
        }

        /// <summary>
        /// Action to get mail function by environment.
        /// </summary>
        /// <param name="listId">Id of current Mailing List.</param>
        /// <param name="listName">Mailing List Name which key general from it.</param>
        /// <returns>JSON Result (List key).</returns>
        [HttpPost]
        public JsonResult GetMailingListKey(int listId, string listName)
        {
            var tempMailingList = _mailingListService.GetById(listId);

            if (ReferenceEquals(tempMailingList, null))
            {
                return Json(-1);
            }

            //// checks Mailing List Name duplication
            var mailingList = _mailingListService.GetByName(listName);

            if (mailingList != null && mailingList.Id != tempMailingList.Id)
            {
                return Json(0);
            }

            //// checks Function Key duplication

            string tempMailingListKey = listName;
            var listKey = GeneralKeyFromName(tempMailingListKey);

            mailingList = _mailingListService.GetByKey(listKey);

            if (mailingList != null && mailingList.Id != tempMailingList.Id)
            {
                return Json(-2);
            }

            return Json(listKey);
        }

        /// <summary>
        /// Check User is portal admin or it group.
        /// </summary>
        /// <returns>Is True or False.</returns>
        [NonAction]
        private bool CheckIsPortalAminOrITGroup()
        {
            var userIsPortalAdminOrIT = this.UserIsPortalAdmin;
            if (!userIsPortalAdminOrIT)
            {
                var group = this._groupService.GetByUser(this.CurrentSession.UserId);
                userIsPortalAdminOrIT = group != null && group.Name.Equals(Constant.IT);
            }

            return userIsPortalAdminOrIT;
        }

        /// <summary>
        /// Get list sites and policy for view Manage.
        /// </summary>
        /// <param name="criteria">GetMailingListBySiteCriteria input value to get.</param>
        /// <param name="hasEditRight">Return role is edit or no.</param>
        /// <param name="siteItemList">Return list sites.</param>
        [NonAction]
        private void GetSitesAndPolicyForManageAction(
            GetMailingListByUserAndSiteIncludeGroupCriteria criteria, 
            ref bool hasEditRight, 
            ref IList<SelectListItem> siteItemList)
        {
            if (this.UserIsPortalAdmin)
            {
                siteItemList = _siteService.GetAllSelectListItem(criteria.Environment, true, criteria.SiteId);
                hasEditRight = true;
            }
            else
            {
                var sites = _siteService.GetSiteLevelByUser(this.CurrentSession.UserId, this.Environment);
                if (sites != null)
                {
                    siteItemList = sites.Select(s => new SelectListItem
                    {
                        Text = s.Site.Name,
                        Value = s.Site.Id.ToString(),
                    }).ToList();
                }

                int policy = _userService.GetUserSiteFunctionKeyPolicy(
                        CurrentSession.UserId,
                        this.SiteId,
                        FunctionKeys.ManageMailingList.ToString());

                hasEditRight = policy >= (int)Constant.Policy.Host;
            }
        }

        /// <summary>
        /// Get list Site for Dropdown List in view.
        /// </summary>
        /// <param name="siteId">Input siteId.</param>
        /// <param name="environment">Input environment.</param>
        /// <param name="selectedSiteId">Input Selected site.</param>
        /// <returns>Return a site list.</returns>
        [NonAction]
        private IList<SelectListItem> GetSiteList(int siteId, string environment, int selectedSiteId = 0)
        {
            IList<SelectListItem> siteItemList = null;

            if (this.UserIsPortalAdmin)
            {
                siteItemList = _siteService.GetAllSelectListItem(environment, isActive: true, selectedSiteId: siteId);
            }
            else
            {
                var sites = _siteService.GetSiteLevelByUser(this.CurrentSession.UserId, this.Environment);
                if (sites != null)
                {
                    siteItemList = sites.Select(s => new SelectListItem
                    {
                        Text = s.Site.Name, 
                        Value = s.Site.Id.ToString(),
                        Selected = siteId == s.Site.Id
                    }).ToList();
                }
            }

            return siteItemList;
        }

        /// <summary>
        /// Get list Function for Dropdown List in view.
        /// </summary>
        /// <param name="siteId">Input siteId.</param>
        /// <param name="environment">Input environment.</param>
        /// <param name="selectedFunctionId">Input Selected function.</param>
        /// <returns>Return a function list.</returns>
        private IList<SelectListItem> GetFunctionList(int siteId, string environment, int selectedFunctionId = 0)
        {
            IList<SelectListItem> functionItemList = null;

            if (this.UserIsPortalAdmin)
            {
                functionItemList = _functionService.GetSelectListItemBySite(siteId, environment, isActive: true);
            }
            else
            {
                var functionList = _functionService.GetByUserAndSiteIncludeGroup(this.CurrentSession.UserId, siteId, environment);
                if (functionList != null)
                {
                    functionList = functionList.Where(t => t.Policy >= (byte)Constant.Policy.Host);
                    functionItemList = functionList.Select(s => new SelectListItem
                    {
                        Text = s.Function.Name,
                        Value = s.Function.Id.ToString(),
                        Selected = selectedFunctionId == s.Function.Id
                    }).ToList();
                }
            }

            return functionItemList;
        }

        /// <summary>
        /// Get list Mailing List Function for Dropdown List in view.
        /// </summary>
        /// <param name="environment">Input environment.</param>
        /// <returns>Return a mail function list.</returns>
        private IList<SelectListItem> GetMailFunctionList(string environment)
        {
            IList<SelectListItem> mailFunctionItemList = null;

            GetByParentKeyAndSiteCriteria getByParentKeyAndSiteCriteria = new GetByParentKeyAndSiteCriteria
            {
                SiteId = this.SiteId,
                ParentKey = FunctionKeys.MailingList,
                Environment = environment,
                IsActive = true,
            };

            if (!this.UserIsPortalAdmin)
            {
                getByParentKeyAndSiteCriteria.RoleId = (byte)Constant.Policy.Host;
                getByParentKeyAndSiteCriteria.UserId = this.CurrentSession.UserId;
            }

            var mailFunctionList = _functionService.GetByParentKeyAndSite(getByParentKeyAndSiteCriteria).ToList();
            if (mailFunctionList != null)
            {
                mailFunctionItemList = mailFunctionList.Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                }).ToList();
            }

            return mailFunctionItemList;
        }

        /// <summary>
        /// Get a key from a name.
        /// </summary>
        /// <param name="listName">Mailing List Name.</param>
        /// <returns>A Mailing List Key.</returns>
        [NonAction]
        private string GeneralKeyFromName(string listName)
        {
            var strListKeyModify = new StringBuilder();
            var listKey = listName.Replace(Constant.SPACE, string.Empty);

            for (int i = 0; i < listKey.Length; i++)
            {
                char item = listKey[i];
                if (char.IsLetterOrDigit(item))
                {
                    strListKeyModify.Append(item);
                }
                else
                {
                    strListKeyModify.Append("_");
                }
            }

            return strListKeyModify.ToString();
        }

        /// <summary>
        /// Check Invalid MailingList Object in case Add New.
        /// </summary>
        /// <param name="criteria">InsertMailingListCriteria all values need check.</param>
        /// <returns>Is Invalid or no.</returns>
        [NonAction]
        private bool CheckInvalidMailingListForAddNew(InsertMailingListCriteria criteria)
        {
            var isValid = true;
            //// checks duplicate mailing list name
            string listName = criteria.ListName = criteria.ListName != null ? criteria.ListName.Trim() : string.Empty;
            var mailingList = _mailingListService.GetByName(listName);
            if (string.IsNullOrWhiteSpace(criteria.ListName))
            {
                ModelState.AddModelError("ListName", string.Format(ViewResources.Field_Required, listName));
                isValid = false;
            }
            else
            {
                if (mailingList != null)
                {
                    ModelState.AddModelError("ListName", string.Format(ViewResources.DuplicateMsg, listName));
                    isValid = false;
                }
                else
                {
                    //// checks duplicate mailing list key
                    var listKey = GeneralKeyFromName(criteria.ListName);
                    criteria.ListKey = listKey;
                    mailingList = _mailingListService.GetByKey(listKey);

                    if (mailingList != null)
                    {
                        ModelState.AddModelError("ListKey", string.Format(ViewResources.DuplicateMsg, listKey));
                        isValid = false;
                    }
                }
            }
            
            if (criteria.ListFunctionId == 0)
            {
                ModelState.AddModelError("ListFunctionId", string.Format(ViewResources.Field_Required, ViewResources.ListFunction));
                isValid = false;
            }

            if (criteria.FunctionId == 0)
            {
                ModelState.AddModelError("FunctionId", string.Format(ViewResources.Field_Required, ViewResources.Function));
                isValid = false;
            }

            isValid = CheckIsValidForAllEmails(criteria.ToList, criteria.CCList, criteria.BCCList) && isValid;

            return isValid;
        }

        /// <summary>
        /// Check Invalid MailingList Object in case Edit.
        /// </summary>
        /// <param name="criteria">UpdateMailingListCriteria all values need check.</param>
        /// <returns>Is Invalid or no.</returns>
        [NonAction]
        private bool CheckInvalidMailingListForEdit(UpdateMailingListCriteria criteria)
        {
            var isValid = true;
            //// checks duplicate mailing list name
            string listName = criteria.ListName = criteria.ListName != null ? criteria.ListName.Trim() : string.Empty;

            if (string.IsNullOrWhiteSpace(criteria.ListName))
            {
                ModelState.AddModelError("ListName", string.Format(ViewResources.Field_Required, listName));
                isValid = false;
            }
            else
            {
                var mailingList = _mailingListService.GetByName(listName);
                var listKey = criteria.ListKey = GeneralKeyFromName(criteria.ListName);
                if (mailingList != null && mailingList.Id != criteria.ListId)
                {
                    ModelState.AddModelError("ListName", string.Format(ViewResources.DuplicateMsg, listName));
                    isValid = false;
                }
                else
                {
                    //// checks duplicate function key
                    mailingList = _mailingListService.GetByKey(listKey);

                    if (mailingList != null && mailingList.Id != criteria.ListId)
                    {
                        ModelState.AddModelError("ListKey", string.Format(ViewResources.DuplicateMsg, listKey));
                        isValid = false;
                    }
                }
            }

            if (criteria.ListFunctionId == 0)
            {
                ModelState.AddModelError("ListFunctionId", string.Format(ViewResources.Field_Required, ViewResources.ListFunction));
                isValid = false;
            }

            if (criteria.FunctionId == 0)
            {
                ModelState.AddModelError("FunctionId", string.Format(ViewResources.Field_Required, ViewResources.Function));
                isValid = false;
            }

            isValid = CheckIsValidForAllEmails(criteria.ToList, criteria.CCList, criteria.BCCList) && isValid;

            return isValid;
        }

        /// <summary>
        /// Check IsValid For All Emails. 
        /// </summary>
        /// <param name="emailToList">To email list.</param>
        /// <param name="emailCCList">CC email list.</param>
        /// <param name="emailBCCList">BCC email list.</param>
        /// <returns>Return IsValid or no.</returns>
        private bool CheckIsValidForAllEmails(string emailToList, string emailCCList, string emailBCCList)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(emailToList))
            {
                ModelState.AddModelError("ToList", string.Format(ViewResources.Field_Required, ViewResources.To));
                isValid = false;
            }

            if (!EmailService.IsValidEmailList(emailToList))
            {
                ModelState.AddModelError("ToList", string.Format(ViewResources.EmailFormatValidationMsg, ViewResources.To));
                isValid = false;
            }

            if (!EmailService.IsValidEmailList(emailCCList))
            {
                ModelState.AddModelError("CCList", string.Format(ViewResources.EmailFormatValidationMsg, ViewResources.Cc));
                isValid = false;
            }

            if (!EmailService.IsValidEmailList(emailBCCList))
            {
                ModelState.AddModelError("BCCList", string.Format(ViewResources.EmailFormatValidationMsg, ViewResources.Bcc));
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// Function to remove Duplicated email.
        /// </summary>
        /// <param name="emails">Input List emails.</param>
        /// <returns>Return List emails.</returns>
        private string RemoveDuplicatedEmail(string emails)
        {
            var emailList = new List<string>();
            if (!string.IsNullOrWhiteSpace(emails))
            {
                var arrEmail = emails.Split(';');
                foreach (var email in arrEmail)
                {
                    if (!emailList.Contains(email.Trim().ToLower()))
                    {
                        emailList.Add(email.Trim().ToLower());
                    }
                }
            }

            var temp = emailList.ToArray();
            return string.Join(";", temp);
        }
    }
}
