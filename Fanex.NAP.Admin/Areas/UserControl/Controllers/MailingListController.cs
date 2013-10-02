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
    using Fanex.NAP.Core.Criteria.MailingList;
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

        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult Activate(int id, bool active)
        {
            if (Request.IsAjaxRequest())
            {
                try
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
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            return new EmptyResult();
        }

        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult Internal(int id, bool isInternal)
        {
            if (Request.IsAjaxRequest())
            {
                try
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
                catch (Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                    throw ex;
                }
            }

            return new EmptyResult();
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Read)]
        public ActionResult Manage(int? siteId)
        {
            GetMailingListBySiteCriteria criteria = null;

            try
            {
                var hasEditRight = false;
                var isITGroup = CheckIsPortalAminOrITGroup();
                criteria = new GetMailingListBySiteCriteria
                {
                    SiteId = siteId ?? this.SiteId,
                    Environment = this.Environment,
                    IsActive = null,
                    IsInternal = isITGroup,
                    SiteMailingList = this.SiteId,
                    ParentFunctionKey = FunctionKeys.MailingList.ToString(),
                    UserId = this.CurrentSession.UserId
                };
                
                var mailingList = _mailingListService.GetBySite(criteria);
                IList<SelectListItem> siteItemList = new List<SelectListItem>();

                GetSitesAndPolicyForManageAction(criteria, ref hasEditRight, ref siteItemList);

                ViewBag.MailingList = mailingList;
                ViewBag.IsITGroup = isITGroup;
                ViewBag.ViewFlags.HasEditRight = hasEditRight;
                ViewBag.SiteItemList = siteItemList;
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Read)]
        public ActionResult Manage(GetMailingListBySiteCriteria criteria)
        {
            try
            {
                var hasEditRight = false;
                var isITGroup = CheckIsPortalAminOrITGroup();
                criteria.IsInternal = isITGroup;
                criteria.Environment = this.Environment;
                criteria.SiteMailingList = this.SiteId;
                criteria.ParentFunctionKey = FunctionKeys.MailingList.ToString();
                criteria.UserId = this.CurrentSession.UserId;

                var mailingList = _mailingListService.GetBySite(criteria);
                IList<SelectListItem> siteItemList = new List<SelectListItem>();

                GetSitesAndPolicyForManageAction(criteria, ref hasEditRight, ref siteItemList);

                ViewBag.MailingList = mailingList;
                ViewBag.IsITGroup = isITGroup;
                ViewBag.ViewFlags.HasEditRight = hasEditRight;
                ViewBag.SiteItemList = siteItemList;
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(int? siteId = null)
        {
            var criteria = new InsertMailingListCriteria
            {
                SiteId = (!siteId.HasValue || siteId.Value == 0) ? this.SiteId : siteId.Value,
                Environment = this.Environment,
                IsInternal = false
            };

            try
            {
                IList<SelectListItem> functionItemList = new List<SelectListItem>();
                IList<SelectListItem> mailFunctionItemList = new List<SelectListItem>();
                IList<SelectListItem> siteItemList = new List<SelectListItem>();

                GetSiteAndFunctionForAddNew(criteria, ref functionItemList, ref siteItemList, ref mailFunctionItemList);

                ViewBag.FunctionList = functionItemList;
                ViewBag.MailFunctionList = mailFunctionItemList;
                ViewBag.SiteItemList = siteItemList;
                ViewBag.IsITGroup = CheckIsPortalAminOrITGroup();
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(InsertMailingListCriteria criteria)
        {
            try
            {
                IList<SelectListItem> functionItemList = new List<SelectListItem>();
                IList<SelectListItem> mailFunctionItemList = new List<SelectListItem>();
                IList<SelectListItem> siteItemList = new List<SelectListItem>();

                GetSiteAndFunctionForAddNew(criteria, ref functionItemList, ref siteItemList, ref mailFunctionItemList);

                ViewBag.FunctionList = functionItemList;
                ViewBag.MailFunctionList = mailFunctionItemList;
                ViewBag.SiteItemList = siteItemList;
                ViewBag.IsITGroup = CheckIsPortalAminOrITGroup();

                if (ModelState.IsValid)
                {
                    if (CheckInvalidMailingListForAddNew(criteria))
                    {
                        return View(criteria);
                    }

                    criteria.CreatedBy = this.CurrentSession.UserId;
                    criteria.CreatedDate = DateTime.Now;
                    var result = _mailingListService.Insert(criteria);

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

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int id)
        {
            var criteria = new UpdateMailingListCriteria();

            try
            {
                IList<SelectListItem> functionItemList = new List<SelectListItem>();
                IList<SelectListItem> mailFunctionItemList = new List<SelectListItem>();

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
                criteria.Environment = mailingList.Environment;
                criteria.ListFunctionId = mailingList.MailFunction.Id;
                criteria.FunctionId = mailingList.Function.Id;
                criteria.ToList = mailingList.ToList;
                criteria.CCList = mailingList.CCList;
                criteria.BCCList = mailingList.BCCList;
                criteria.IsInternal = mailingList.IsInternal;
                criteria.IsActive = mailingList.IsActive;
                ViewBag.SiteName = mailingList.Site.Name;

                GetFunctionForEdit(criteria, ref functionItemList, ref mailFunctionItemList);

                ViewBag.MailFunctionList = mailFunctionItemList;
                ViewBag.FunctionList = functionItemList;
                ViewBag.IsITGroup = CheckIsPortalAminOrITGroup();
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw ex;
            }

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult Edit(UpdateMailingListCriteria criteria)
        {
            try
            {
                IList<SelectListItem> functionItemList = new List<SelectListItem>();
                IList<SelectListItem> mailFunctionItemList = new List<SelectListItem>();

                GetFunctionForEdit(criteria, ref functionItemList, ref mailFunctionItemList);

                ViewBag.MailFunctionList = mailFunctionItemList;
                ViewBag.FunctionList = functionItemList;
                ViewBag.IsITGroup = CheckIsPortalAminOrITGroup();

                if (ModelState.IsValid)
                {
                    if (CheckInvalidMailingListForEdit(criteria))
                    {
                        return View(criteria);
                    }

                    criteria.ModifiedBy = this.CurrentSession.UserId;
                    criteria.ModifiedDate = DateTime.Now;

                    var result = _mailingListService.Update(criteria);

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

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public JsonResult GetFunctionBySite(int siteId, string environment = Constant.UAT)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    var functionList = _functionService.GetBySite(siteId, environment, true).ToList();

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
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Read)]
        public JsonResult GetMailFunctionByEnvironment(string environment = Constant.UAT)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    var functionList = _functionService.GetByParentKeyAndSite(this.SiteId, FunctionKeys.MailingList, environment, isActive: true).ToList();

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
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
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

        [NonAction]
        private void GetSitesAndPolicyForManageAction(GetMailingListBySiteCriteria criteria, ref bool hasEditRight, ref IList<SelectListItem> siteItemList)
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

        [NonAction]
        private void GetSiteAndFunctionForAddNew(
            InsertMailingListCriteria criteria, 
            ref IList<SelectListItem> functionItemList, 
            ref IList<SelectListItem> siteItemList, 
            ref IList<SelectListItem> mailFunctionItemList)
        {
            functionItemList = _functionService.GetSelectListItemBySite(criteria.SiteId, criteria.Environment, isActive: true);
            //// portal admin
            if (this.UserIsPortalAdmin)
            {
                siteItemList = _siteService.GetAllSelectListItem(criteria.Environment, isActive: true, selectedSiteId: criteria.SiteId);
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
                        Selected = criteria.SiteId == s.Site.Id
                    }).ToList();
                }
            }

            var mailFunctionList = _functionService.GetByParentKeyAndSite(this.SiteId, FunctionKeys.MailingList, criteria.Environment, isActive: true);

            if (mailFunctionList != null)
            {
                mailFunctionItemList = mailFunctionList.Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                }).ToList();
            }
        }

        [NonAction]
        private string GeneralKeyFromName(string keyName)
        {
            var strListKeyModify = new StringBuilder();
            var listKey = keyName.Replace(Constant.SPACE, string.Empty);

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

        [NonAction]
        private bool CheckInvalidMailingListForAddNew(InsertMailingListCriteria criteria)
        {
            var isValid = true;
            //// checks duplicate mailing list name
            var listName = criteria.ListName = criteria.ListName.Trim();
            var mailingList = _mailingListService.GetByName(listName);

            if (mailingList != null)
            {
                ModelState.AddModelError("ListName", string.Format(ViewResources.DuplicateMsg, listName));
                return false;
            }

            //// checks duplicate mailing list key
            var listKey = GeneralKeyFromName(criteria.ListName);
            criteria.ListKey = listKey;
            mailingList = _mailingListService.GetByKey(listKey);

            if (mailingList != null)
            {
                ModelState.AddModelError("ListKey", string.Format(ViewResources.DuplicateMsg, listKey));
                return false;
            }

            return isValid;
        }

        [NonAction]
        private bool CheckInvalidMailingListForEdit(UpdateMailingListCriteria criteria)
        {
            var isValid = true;

            //// checks duplicate mailing list name
            string listName = criteria.ListName = criteria.ListName.Trim();
            var mailingList = _mailingListService.GetByName(listName);

            if (mailingList != null && mailingList.Id != criteria.ListId)
            {
                ModelState.AddModelError("ListName", string.Format(ViewResources.DuplicateMsg, listName));

                return false;
            }

            //// checks duplicate function key
            var listKey = GeneralKeyFromName(criteria.ListName);
            criteria.ListKey = listKey;

            mailingList = _mailingListService.GetByKey(listKey);

            if (mailingList != null && mailingList.Id != criteria.ListId)
            {
                ModelState.AddModelError("ListKey", string.Format(ViewResources.DuplicateMsg, listKey));

                return false;
            }

            ViewBag.SiteName = mailingList.Site.Name;

            return isValid;
        }

        [NonAction]
        private void GetFunctionForEdit(UpdateMailingListCriteria criteria, ref IList<SelectListItem> functionItemList, ref IList<SelectListItem> mailFunctionItemList)
        {
            var mailFunctionList = _functionService.GetByParentKeyAndSite(criteria.SiteId, FunctionKeys.MailingList, criteria.Environment, isActive: true);

            if (mailFunctionList != null)
            {
                mailFunctionItemList = mailFunctionList.Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                }).ToList();
            }

            functionItemList = _functionService.GetSelectListItemBySite(criteria.SiteId, criteria.Environment, isActive: true);
        }
    }
}
