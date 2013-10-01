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
            var isITGroup = CheckIsPortalAminOrITGroup();
            var criteria = new GetMailingListBySiteCriteria
            {
                SiteId = siteId ?? 0,
                Environment = this.Environment,
                IsActive = true,
                IsInternal = isITGroup,
                SiteMailingList = this.SiteId,
                ParentFunctionKey = FunctionKeys.MailingList.ToString(),
                UserId = this.CurrentSession.UserId
            };

            try
            {
                IList<SelectListItem> siteItemList = new List<SelectListItem>();

                if (this.UserIsPortalAdmin)
                {
                    siteItemList = _siteService.GetAllSelectListItem(criteria.Environment, true, 0);
                    ViewBag.ViewFlags.HasEditRight = true;
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

                    ViewBag.ViewFlags.HasEditRight = policy >= (int)Constant.Policy.Host;
                }

                siteItemList.Insert(0, new SelectListItem { Value = "0", Text = ViewResources.All_Site });

                ViewBag.SiteItemList = siteItemList;
                ViewBag.IsITGroup = isITGroup;

                // PrepareManageData(criteria);
                var mailingList = _mailingListService.GetBySite(criteria);

                ViewBag.MailingList = mailingList;
                
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
            var isITGroup = CheckIsPortalAminOrITGroup();
            criteria.IsInternal = isITGroup;
            criteria.Environment = this.Environment;
            criteria.IsActive = true;
            criteria.SiteMailingList = this.SiteId;
            criteria.ParentFunctionKey = FunctionKeys.MailingList.ToString();
            criteria.UserId = this.CurrentSession.UserId;
            
            try
            {
                IList<SelectListItem> siteItemList = new List<SelectListItem>();

                if (this.UserIsPortalAdmin)
                {
                    siteItemList = _siteService.GetAllSelectListItem(criteria.Environment, true, 0);
                    ViewBag.ViewFlags.HasEditRight = true;
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

                    ViewBag.ViewFlags.HasEditRight = policy >= (int)Constant.Policy.Host;
                }

                siteItemList.Insert(0, new SelectListItem { Value = "0", Text = ViewResources.All_Site });
                ViewBag.SiteItemList = siteItemList;
                ViewBag.IsITGroup = isITGroup;

                // PrepareManageData(criteria);
                var mailingList = _mailingListService.GetBySite(criteria);

                ViewBag.MailingList = mailingList;

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
                SiteId = siteId ?? this.SiteId,
                Environment = this.Environment,
                IsInternal = false
            };

            // portal admin
            if (this.UserIsPortalAdmin)
            {
                ViewBag.SiteItemList = _siteService.GetAllSelectListItem(criteria.Environment, isActive: true, selectedSiteId: criteria.SiteId);
            }
            else
            {
                var sites = _siteService.GetSiteLevelByUser(this.CurrentSession.UserId, this.Environment);
                if (sites != null)
                {
                    ViewBag.SiteItemList = sites.Select(s => new SelectListItem
                    {
                        Text = s.Site.Name,
                        Value = s.Site.Id.ToString(),
                    }).ToList();
                }
            }

            var mailFunctionList = _functionService.GetByParentKeyAndSite(criteria.SiteId, FunctionKeys.MailingList, criteria.Environment, isActive: true);

            if (mailFunctionList != null)
            {
                ViewBag.MailFunctionList = mailFunctionList.Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                }).ToList();
            }

            ViewBag.FunctionList = _functionService.GetSelectListItemBySite(criteria.SiteId, criteria.Environment, isActive: true);
            ViewBag.IsITGroup = CheckIsPortalAminOrITGroup();

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult AddNew(InsertMailingListCriteria criteria)
        {
            try
            {
                if (this.UserIsPortalAdmin)
                {
                    ViewBag.SiteItemList = _siteService.GetAllSelectListItem(criteria.Environment, isActive: true, selectedSiteId: criteria.SiteId);
                }
                else
                {
                    var sites = _siteService.GetSiteLevelByUser(this.CurrentSession.UserId, this.Environment);
                    if (sites != null)
                    {
                        ViewBag.SiteItemList = sites.Select(s => new SelectListItem
                        {
                            Text = s.Site.Name,
                            Value = s.Site.Id.ToString(),
                        }).ToList();
                    }
                }

                var mailFunctionList = _functionService.GetByParentKeyAndSite(criteria.SiteId, FunctionKeys.MailingList, criteria.Environment, isActive: true);

                if (mailFunctionList != null)
                {
                    ViewBag.MailFunctionList = mailFunctionList.Select(s => new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                    }).ToList();
                }

                ViewBag.FunctionList = _functionService.GetSelectListItemBySite(criteria.SiteId, criteria.Environment, isActive: true);

                ViewBag.IsITGroup = CheckIsPortalAminOrITGroup();

                //GetSitesAndFunctionsForAddNew(criteria);

                if (ModelState.IsValid)
                {
                    #region checks duplicate mailing list name

                    string listName = criteria.ListName = criteria.ListName.Trim();
                    var mailingList = _mailingListService.GetByName(listName);

                    if (mailingList != null)
                    {
                        ModelState.AddModelError("ListName",
                            string.Format(ViewResources.DuplicateMsg, listName));

                        return View(criteria);
                    }

                    #endregion

                    #region checks duplicate function key

                    string listKey = criteria.ListKey = criteria.ListName.Replace(Constant.SPACE, string.Empty);

                    #region replace char not letter or digit of mailing list key

                    StringBuilder strListKeyModify = new StringBuilder();

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

                    criteria.ListKey = strListKeyModify.ToString();
                    listKey = strListKeyModify.ToString();

                    #endregion

                    mailingList = _mailingListService.GetByKey(listKey);

                    if (mailingList != null)
                    {
                        ModelState.AddModelError("ListKey",
                            string.Format(ViewResources.DuplicateMsg, listKey));

                        return View(criteria);
                    }

                    #endregion

                    #region adds mailing list

                    criteria.CreatedBy = this.CurrentSession.UserId;
                    criteria.CreatedDate = DateTime.Now;
                    var result = _mailingListService.Insert(criteria);

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

        [HttpGet]
        [NapAuth(FunctionKey = FunctionKeys.ManageMailingList, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int id)
        {
            var criteria = new UpdateMailingListCriteria();

            try
            {
                var mailingList = _mailingListService.GetById(id);

                if (ReferenceEquals(mailingList, null))
                {
                    return RedirectToAction("Manage");
                }

                criteria.ListId = mailingList.Id;
                criteria.ListKey = mailingList.Key;
                criteria.ListName = mailingList.Name;
                criteria.Description = mailingList.Description;
                criteria.SiteId = mailingList.SiteId;
                criteria.Environment = mailingList.Environment;
                criteria.ListFunctionId = mailingList.MailFunction.Id;
                criteria.FunctionId = mailingList.Function.Id;
                criteria.ToList = mailingList.ToList;
                criteria.CCList = mailingList.CCList;
                criteria.BCCList = mailingList.BCCList;
                criteria.IsInternal = mailingList.IsInternal;
                criteria.IsActive = mailingList.IsActive;

                var mailFunctionList = _functionService.GetByParentKeyAndSite(criteria.SiteId, FunctionKeys.MailingList, criteria.Environment, isActive: true);

                if (mailFunctionList != null)
                {
                    ViewBag.MailFunctionList = mailFunctionList.Select(s => new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                    }).ToList();
                }

                ViewBag.FunctionList = _functionService.GetSelectListItemBySite(criteria.SiteId, criteria.Environment, isActive: true);
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
                var mailFunctionList = _functionService.GetByParentKeyAndSite(criteria.SiteId, FunctionKeys.MailingList, criteria.Environment, isActive: true);

                if (mailFunctionList != null)
                {
                    ViewBag.MailFunctionList = mailFunctionList.Select(s => new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                    }).ToList();
                }

                ViewBag.FunctionList = _functionService.GetSelectListItemBySite(criteria.SiteId, criteria.Environment, isActive: true);

                ViewBag.IsITGroup = CheckIsPortalAminOrITGroup();

                if (ModelState.IsValid)
                {
                    #region checks duplicate mailing list name

                    string listName = criteria.ListName = criteria.ListName.Trim();
                    var mailingList = _mailingListService.GetByName(listName);

                    if (mailingList != null && mailingList.Id != criteria.ListId)
                    {
                        ModelState.AddModelError("ListName",
                            string.Format(ViewResources.DuplicateMsg, listName));

                        return View(criteria);
                    }

                    #endregion

                    #region checks duplicate function key

                    string listKey = criteria.ListKey = criteria.ListName.Replace(Constant.SPACE, string.Empty);

                    #region replace char not letter or digit of mailing list key

                    StringBuilder strListKeyModify = new StringBuilder();

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

                    criteria.ListKey = strListKeyModify.ToString();
                    listKey = strListKeyModify.ToString();

                    #endregion

                    mailingList = _mailingListService.GetByKey(listKey);

                    if (mailingList != null && mailingList.Id != criteria.ListId)
                    {
                        ModelState.AddModelError("ListKey",
                            string.Format(ViewResources.DuplicateMsg, listKey));

                        return View(criteria);
                    }

                    #endregion

                    #region adds mailing list

                    criteria.ModifiedBy = this.CurrentSession.UserId;
                    criteria.ModifiedDate = DateTime.Now;
                    var result = _mailingListService.Update(criteria);

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

        [NonAction]
        private bool CheckIsPortalAminOrITGroup()
        {
            var userIsPortalAdminOrIT = this.UserIsPortalAdmin;
            if (!userIsPortalAdminOrIT)
            {
                var group = this._groupService.GetByUser(this.CurrentSession.UserId);
                userIsPortalAdminOrIT = (group != null && group.Name.Equals(Constant.IT));
            }

            return userIsPortalAdminOrIT;
        }
    }
}
