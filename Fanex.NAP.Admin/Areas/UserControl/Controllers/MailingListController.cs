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
    using System.Web;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.Logging;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Model;
    using System.Text;

    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Core.Helpers;
    using Fanex.NAP.Core.Model.ValueObject;


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
                ViewBag.SiteItemList = _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                            CurrentSession.UserId,
                            FunctionKeys.ManageMailingList.ToString(),
                            criteria.Environment,
                            policy: Constant.Policy.Host,
                            selectedSiteId: criteria.SiteId);
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

            var userIsPortalAdminOrIT = this.UserIsPortalAdmin;
            if (!userIsPortalAdminOrIT)
            {
                var group = this._groupService.GetByUser(this.CurrentSession.UserId);
                userIsPortalAdminOrIT = (group != null && group.Name.Equals(Constant.IT));
            }

            ViewBag.IsITGroup = userIsPortalAdminOrIT;

            //FunctionKeys.MailingList;
            //var functionName = string.Empty;
            //var function = _functionService.GetByKeyAndSite(ApolloFunctionKeys.ExchangeRate, this.SiteId);
            //var mailingList = new MailingListModel();

            //if (function != null)
            //{
            //    functionName = function.Name;
            //    var criteria = new GetMailingListByFunctionCriteria { FunctionId = function.Id };
            //    mailingList = _mailingListService.GetByFunction(criteria);
            //}

            //var user = _userService.GetById(this.CurrentSession.UserId);
            //var isAllowShowBCCGroup = false;
            //if (user != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(_allowListShowBCC))
            //    {
            //        var arrGroup = _allowListShowBCC.Split(',');
            //        int pos = Array.IndexOf(arrGroup, user.Group.Id.ToString());
            //        if (pos > -1)
            //        {
            //            isAllowShowBCCGroup = true;
            //        }
            //    }
            //}

            //int policy = _userService.GetUserSiteFunctionKeyPolicy(
            //            CurrentSession.UserId,
            //            this.SiteId,
            //            ApolloFunctionKeys.MailingListExchangeRate.ToString());

            //ViewBag.IsAllowShowBCCGroup = isAllowShowBCCGroup;
            //ViewBag.FunctionName = functionName;
            //ViewBag.HasAllowEdit = policy >= (int)Constant.Policy.Host;

            return View(criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
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
                    ViewBag.SiteItemList = _siteService.GetSelectListItemByUserAndFunctionKeyIncludeGroup(
                                CurrentSession.UserId,
                                FunctionKeys.ManageMailingList.ToString(),
                                criteria.Environment,
                                policy: Constant.Policy.Host,
                                selectedSiteId: criteria.SiteId);
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

                var userIsPortalAdminOrIT = this.UserIsPortalAdmin;
                if (!userIsPortalAdminOrIT)
                {
                    var group = this._groupService.GetByUser(this.CurrentSession.UserId);
                    userIsPortalAdminOrIT = (group != null && group.Name.Equals(Constant.IT));
                }

                ViewBag.IsITGroup = userIsPortalAdminOrIT;

                //GetSitesAndFunctionsForAddNew(criteria);

                if (ModelState.IsValid)
                {
                    #region checks duplicate mailing list name

                    string listName = criteria.ListName = criteria.ListName.Trim();
                    var function = _functionService.GetByNameAndSite(listName, criteria.SiteId);

                    if (function != null)
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

                    function = _functionService.GetByKeyAndSite(listKey, criteria.SiteId);

                    if (function != null)
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
        [NapAuth(FunctionKey = FunctionKeys.ManageFunction, Policy = Constant.Policy.Host)]
        public JsonResult GetMailFunctionByEnvironment(int siteId = 0, string environment = Constant.UAT)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    var functionList = _functionService.GetByParentKeyAndSite(siteId, FunctionKeys.MailingList, environment, isActive: true).ToList();

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
    }
}
