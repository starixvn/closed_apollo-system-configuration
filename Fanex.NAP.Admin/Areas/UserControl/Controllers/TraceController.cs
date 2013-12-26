// --------------------------------------------------------------------------
// <copyright file="TraceController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.11.13">Created</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using Fanex.NAP.Core.Criteria.Trace;
    using Fanex.NAP.Core.Helpers;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Trace controller.
    /// </summary>
    public class TraceController : BaseController
    {
        private readonly ITraceService _traceService;
        private readonly IFunctionService _functionService;
        private readonly ISiteService _siteService;

        public TraceController()
            : this(null, null, null)
        {
        }

        public TraceController(ITraceService traceService, IFunctionService functionService, ISiteService siteService)
        {
            _traceService = traceService ?? new TraceService();
            _functionService = functionService ?? new FunctionService();
            _siteService = siteService ?? new SiteService();
        }

        #region Log Exec Sp

        /// <summary>
        /// Manage execute SP log.
        /// </summary>
        /// <param name="id">The page index.</param>
        /// <param name="size">The page size.</param>
        /// <param name="q">The item name.</param>
        /// <param name="username">The username.</param>
        /// <param name="from">From date value.</param>
        /// <param name="to">To date value.</param>
        /// <param name="filterBy">Filter by option.</param>
        /// <returns>Action result.</returns>
        [NapAuth(FunctionKey = FunctionKeys.ExecSpLog)]
        [HttpGet]
        public ActionResult LogExecSp(
            int id = 1,
            int size = 20,
            string q = EmptyString,
            string username = EmptyString,
            DateTime? from = null,
            DateTime? to = null,
            int filterBy = 1)
        {
            var criteria = new GetAllLogCriteria
            {
                PageIndex = id,
                PageSize = size,
                Name = q,
                Username = username,
                From = from ?? DateTime.Now,
                To = to ?? DateTime.Now.AddDays(1),
                FilterType = filterBy
            };

            int totalLogs = 0;

            criteria.Name = criteria.Name ?? string.Empty;

            IQueryable<LogExecSp> listLog = _traceService.SearchExecSp(
                                                criteria.PageIndex,
                                                criteria.PageSize,
                                                out totalLogs,
                                                criteria.Name,
                                                criteria.Username,
                                                criteria.From,
                                                criteria.To,
                                                criteria.FilterType);
            ViewBag.ListLog = new PagerList<LogExecSp>(criteria.PageIndex, criteria.PageSize, totalLogs, listLog);

            return View(criteria);
        }

        /// <summary>
        /// Manage execute SP log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult LogExecSp(GetAllLogCriteria criteria)
        {
            return RedirectToAction(
             "LogExecSp",
             new
             {
                 id = criteria.PageIndex,
                 size = criteria.PageSize,
                 q = criteria.Name,
                 username = criteria.Username,
                 from = criteria.From.ToString("MMM dd,yyyy"),
                 to = criteria.To.ToString("MMM dd,yyyy"),
                 filterBy = criteria.FilterType
             });
        }

        #endregion

        #region Log User Login

        /// <summary>
        /// Manage user login log.
        /// </summary>
        /// <param name="id">The page index.</param>
        /// <param name="size">The page size.</param>
        /// <param name="q">The username.</param>
        /// <param name="from">From date value.</param>
        /// <param name="to">To date value.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult LogUserLogin(
            int id = 1,
            int size = 20,
            string q = EmptyString,
            DateTime? from = null,
            DateTime? to = null,
            int siteId = 0)
        {
            var criteria = new GetAllLogUserLoginCriteria
            {
                PageIndex = id,
                PageSize = size,
                Name = q,
                From = from ?? DateTime.Now,
                To = to ?? DateTime.Now.AddDays(1),
                SiteId = siteId
            };

            int totalLogs = 0;

            criteria.Name = criteria.Name ?? string.Empty;

            IQueryable<LogUserLogin> listLog = _traceService.SearchUserLogin(
                                                criteria.PageIndex,
                                                criteria.PageSize,
                                                out totalLogs,
                                                criteria.Name,
                                                criteria.From,
                                                criteria.To,
                                                criteria.SiteId);

            var listSite = _siteService.GetAll().ToList();
            listSite.Add(new Site
            {
                Id = 0,
                Name = @ViewResources.NAP_Hub
            });

            ViewBag.ListSite = listSite.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();

            ViewBag.ListLog = new PagerList<LogUserLogin>(criteria.PageIndex, criteria.PageSize, totalLogs, listLog);

            return View(criteria);
        }

        /// <summary>
        /// Manage user login log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult LogUserLogin(GetAllLogUserLoginCriteria criteria)
        {
            return RedirectToAction(
              "LogUserLogin",
              new
              {
                  id = criteria.PageIndex,
                  size = criteria.PageSize,
                  q = criteria.Name,
                  from = criteria.From.ToString("MMM dd,yyyy"),
                  to = criteria.To.ToString("MMM dd,yyyy"),
                  siteId = criteria.SiteId
              });
        }

        #endregion

        #region Log Function

        /// <summary>
        /// Manage function log.
        /// </summary>
        /// <param name="id">The page index.</param>
        /// <param name="size">The page size.</param>
        /// <param name="q">The username.</param>
        /// <param name="from">From date value.</param>
        /// <param name="to">To date value.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult LogFunction(
            int id = 1,
            int size = 20,
            string q = EmptyString,
            DateTime? from = null,
            DateTime? to = null,
            int siteId = 0)
        {
            var criteria = new GetAllLogFunctionCriteria
            {
                PageIndex = id,
                PageSize = size,
                Name = q,
                From = from ?? DateTime.Now,
                To = to ?? DateTime.Now.AddDays(1),
                SiteId = siteId
            };

            int totalLogs = 0;

            criteria.Name = criteria.Name ?? string.Empty;

            IQueryable<LogFunction> listLog = _traceService.SearchFunction(
                                                criteria.PageIndex,
                                                criteria.PageSize,
                                                out totalLogs,
                                                criteria.Name,
                                                criteria.From,
                                                criteria.To,
                                                criteria.SiteId);

            ViewBag.ListSite = _siteService.GetAll().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            }).ToList();

            ViewBag.ListLog = new PagerList<LogFunction>(criteria.PageIndex, criteria.PageSize, totalLogs, listLog);

            return View(criteria);
        }

        /// <summary>
        /// Manage function log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult LogFunction(GetAllLogFunctionCriteria criteria)
        {
            return RedirectToAction(
              "LogFunction",
              new
              {
                  id = criteria.PageIndex,
                  size = criteria.PageSize,
                  q = criteria.Name,
                  from = criteria.From.ToString("MMM dd,yyyy"),
                  to = criteria.To.ToString("MMM dd,yyyy"),
                  siteId = criteria.SiteId
              });
        }

        #endregion

        #region Ajax

        /// <summary>
        /// Get log insert detail.
        /// </summary>
        /// <param name="id">The log ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult GetLogInsertDetail(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var log = _traceService.GetById(id);

                return View(log);
            }

            return View(new LogExecSp());
        }

        #endregion

        #region View log

        /// <summary>
        /// Get user insert log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_User_Insert(LogExecSp log)
        {
            XElement element = XElement.Parse(log.Data);

            if (log.ActionId == (int)LogAction.NAP_User_Insert)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Username,
                    NewValue = element.Attribute("Username").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Group_Name,
                    NewValue = element.Attribute("GroupName").Value
                });

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogInsert.cshtml");
        }

        /// <summary>
        /// Get customer insert log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_Cust_Insert(LogExecSp log)
        {
            XElement element = XElement.Parse(log.Data);

            if (log.ActionId == (int)LogAction.NAP_Cust_Insert)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Username,
                    NewValue = element.Attribute("Username").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Group_Name,
                    NewValue = element.Attribute("GroupName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.First_Name,
                    NewValue = element.Attribute("FirstName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Last_Name,
                    NewValue = element.Attribute("LastName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Email,
                    NewValue = element.Attribute("Email").Value
                });

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogInsert.cshtml");
        }

        /// <summary>
        /// Get function insert log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_Function_Insert(LogExecSp log)
        {
            XElement element = XElement.Parse(log.Data);

            if (log.ActionId == (int)LogAction.NAP_Function_Insert)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Function_Name,
                    NewValue = element.Attribute("FunctionName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Site_Name,
                    NewValue = element.Attribute("SiteName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Function_Key,
                    NewValue = element.Attribute("FunctionKey").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    NewValue = element.Attribute("Description").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Parent,
                    NewValue = element.Attribute("ParentName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.CRUD,
                    NewValue = element.Attribute("CRUD").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Display_Order,
                    NewValue = element.Attribute("DisplayOrder").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Active,
                    NewValue = element.Attribute("IsActive").Value == "1" ? "true" : "false"
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Environment,
                    NewValue = element.Attribute("Environment").Value
                });

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogInsert.cshtml");
        }

        /// <summary>
        /// Get group insert log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_Group_Insert(LogExecSp log)
        {
            XElement element = XElement.Parse(log.Data);

            if (log.ActionId == (int)LogAction.NAP_Group_Insert)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Group_Name,
                    NewValue = element.Attribute("GroupName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    NewValue = element.Attribute("Description").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Active,
                    NewValue = element.Attribute("IsActive").Value == "1" ? "true" : "false"
                });

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogInsert.cshtml");
        }

        /// <summary>
        /// Get menu item insert log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_MenuItem_Insert(LogExecSp log)
        {
            XElement element = XElement.Parse(log.Data);

            if (log.ActionId == (int)LogAction.NAP_MenuItem_Insert)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Menu_Item_Name,
                    NewValue = element.Attribute("MenuItemName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Function_Name,
                    NewValue = element.Attribute("FunctionName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Site_Name,
                    NewValue = element.Attribute("SiteName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    NewValue = element.Attribute("Description").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Parent,
                    NewValue = element.Attribute("ParentName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Policy,
                    NewValue = element.Attribute("Policy").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.URL,
                    NewValue = element.Attribute("URL").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Display_Order,
                    NewValue = element.Attribute("DisplayOrder").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Active,
                    NewValue = element.Attribute("IsActive").Value == "1" ? "true" : "false"
                });

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogInsert.cshtml");
        }

        /// <summary>
        /// Get insert site log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_Site_InsertWithSiteURL(LogExecSp log)
        {
            XElement element = XElement.Parse(log.Data);

            if (log.ActionId == (int)LogAction.NAP_Site_InsertWithSiteURL)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Site_Name,
                    NewValue = element.Attribute("SiteName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    NewValue = element.Attribute("Description").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Environment,
                    NewValue = element.Attribute("Environment").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Active,
                    NewValue = element.Attribute("IsActive").Value == "1" ? "true" : "false"
                });

                string strUrl = element.Attribute("Value").Value.DecodeXMLValue();

                var listSiteUrl = new List<SiteUrl>();
                XDocument documentSiteUrl = XDocument.Parse(strUrl);
                IEnumerable<XElement> siteUrl = documentSiteUrl.Descendants("ITEM");

                foreach (XElement item in siteUrl)
                {
                    var objSiteUrl = new SiteUrl
                    {
                        Url = item.Attribute("URL").Value,
                        Environment = item.Attribute("Environment").Value,
                        IsDefault = item.Attribute("IsDefault").Value == "1" ? true : false,
                        IsActive = item.Attribute("IsActive").Value == "1" ? true : false,
                    };
                    listSiteUrl.Add(objSiteUrl);
                }

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
                ViewBag.SiteUrl = listSiteUrl;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAP_Site_InsertWithSiteURL.cshtml");
        }

        /// <summary>
        /// Get update menu item log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_MenuItem_Update(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_MenuItem_Update)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Menu_Item_Name,
                    OldValue = elementOld.Attribute("MenuItemName").Value,
                    NewValue = elementNew.Attribute("MenuItemName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Function_Name,
                    OldValue = elementOld.Attribute("FunctionName").Value,
                    NewValue = elementNew.Attribute("FunctionName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Site_Name,
                    OldValue = elementOld.Attribute("SiteName").Value,
                    NewValue = elementNew.Attribute("SiteName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    OldValue = elementOld.Attribute("Description").Value,
                    NewValue = elementNew.Attribute("Description").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Parent,
                    OldValue = elementOld.Attribute("ParentName").Value,
                    NewValue = elementNew.Attribute("ParentName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Policy,
                    OldValue = elementOld.Attribute("Policy").Value,
                    NewValue = elementNew.Attribute("Policy").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.URL,
                    OldValue = elementOld.Attribute("URL").Value,
                    NewValue = elementNew.Attribute("URL").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Display_Order,
                    OldValue = elementOld.Attribute("DisplayOrder").Value,
                    NewValue = elementNew.Attribute("DisplayOrder").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Active,
                    OldValue = elementOld.Attribute("IsActive").Value == "1" ? "true" : "false",
                    NewValue = elementNew.Attribute("IsActive").Value == "1" ? "true" : "false"
                });

                ViewBag.IsInsert = false;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update menu item status log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_MenuItem_UpdateStatus(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_MenuItem_UpdateStatus)
            {
                var listLogData = new List<LogData>();

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Status,
                    OldValue = elementOld.Attribute("IsActive").Value == "1" ? "Active" : "Unactive",
                    NewValue = elementNew.Attribute("IsActive").Value == "1" ? "Active" : "Unactive"
                });

                ViewBag.IsInsert = false;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update function log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_Function_Update(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_Function_Update
                || log.ActionId == (int)LogAction.NAP_Function_Duplicate)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Function_Name,
                    OldValue = elementOld.Attribute("FunctionName").Value,
                    NewValue = elementNew.Attribute("FunctionName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Site_Name,
                    OldValue = elementOld.Attribute("SiteName").Value,
                    NewValue = elementNew.Attribute("SiteName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Function_Key,
                    OldValue = elementOld.Attribute("FunctionKey").Value,
                    NewValue = elementNew.Attribute("FunctionKey").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    OldValue = elementOld.Attribute("Description").Value,
                    NewValue = elementNew.Attribute("Description").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Parent,
                    OldValue = elementOld.Attribute("ParentName").Value,
                    NewValue = elementNew.Attribute("ParentName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.CRUD,
                    OldValue = elementOld.Attribute("CRUD").Value,
                    NewValue = elementNew.Attribute("CRUD").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Display_Order,
                    OldValue = elementOld.Attribute("DisplayOrder").Value,
                    NewValue = elementNew.Attribute("DisplayOrder").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Active,
                    OldValue = elementOld.Attribute("IsActive").Value == "1" ? "true" : "false",
                    NewValue = elementNew.Attribute("IsActive").Value == "1" ? "true" : "false"
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Environment,
                    OldValue = elementOld.Attribute("Environment").Value,
                    NewValue = elementNew.Attribute("Environment").Value
                });

                ViewBag.IsInsert = false;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update function status log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_Function_UpdateStatus(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_Function_UpdateStatus)
            {
                var listLogData = new List<LogData>();

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Status,
                    OldValue = elementOld.Attribute("IsActive").Value == "1" ? "Active" : "Unactive",
                    NewValue = elementNew.Attribute("IsActive").Value == "1" ? "Active" : "Unactive"
                });

                ViewBag.IsInsert = false;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update site log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_Site_Update(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_Site_Update)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Site_Name,
                    OldValue = elementOld.Attribute("SiteName").Value,
                    NewValue = elementNew.Attribute("SiteName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    OldValue = elementOld.Attribute("Description").Value,
                    NewValue = elementNew.Attribute("Description").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Environment,
                    OldValue = elementOld.Attribute("Environment").Value,
                    NewValue = elementNew.Attribute("Environment").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Active,
                    OldValue = elementOld.Attribute("IsActive").Value == "1" ? "true" : "false",
                    NewValue = elementNew.Attribute("IsActive").Value == "1" ? "true" : "false"
                });

                ViewBag.IsInsert = false;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update site status log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_Site_UpdateStatus(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_Site_UpdateStatus)
            {
                var listLogData = new List<LogData>();

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Status,
                    OldValue = elementOld.Attribute("IsActive").Value == "1" ? "Active" : "Unactive",
                    NewValue = elementNew.Attribute("IsActive").Value == "1" ? "Active" : "Unactive"
                });

                ViewBag.IsInsert = false;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update group log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_Group_Update(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_Group_Update)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Group_Name,
                    OldValue = elementOld.Attribute("GroupName").Value,
                    NewValue = elementNew.Attribute("GroupName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    OldValue = elementOld.Attribute("Description").Value,
                    NewValue = elementNew.Attribute("Description").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Active,
                    OldValue = elementOld.Attribute("IsActive").Value == "1" ? "true" : "false",
                    NewValue = elementNew.Attribute("IsActive").Value == "1" ? "true" : "false"
                });

                ViewBag.IsInsert = false;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update group status log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_Group_UpdateStatus(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_Group_UpdateStatus)
            {
                var listLogData = new List<LogData>();

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Status,
                    OldValue = elementOld.Attribute("IsActive").Value == "1" ? "Active" : "Unactive",
                    NewValue = elementNew.Attribute("IsActive").Value == "1" ? "Active" : "Unactive"
                });

                ViewBag.IsInsert = false;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update customer log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_Cust_Update(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data ?? "<ROOT/>");
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_Cust_Update)
            {
                var listLogData = new List<LogData>();

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Group_Name,
                    OldValue = elementOld.Attribute("GroupName").Value,
                    NewValue = elementNew.Attribute("GroupName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.First_Name,
                    OldValue = elementOld.Attribute("FirstName").Value,
                    NewValue = elementNew.Attribute("FirstName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Last_Name,
                    OldValue = elementOld.Attribute("LastName").Value,
                    NewValue = elementNew.Attribute("LastName").Value
                });

                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Email,
                    OldValue = elementOld.Attribute("Email").Value,
                    NewValue = elementNew.Attribute("Email").Value
                });

                ViewBag.IsInsert = false;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get site URL log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_Site_Url(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            // NAP_Site_UpdateSiteURL || NAP_Site_InsertSiteURL || NAP_Site_UpdateDefaultSiteURL
            if (log.ActionId == (int)LogAction.NAP_Site_UpdateSiteURL
                || log.ActionId == (int)LogAction.NAP_Site_InsertSiteURL
                || log.ActionId == (int)LogAction.NAP_Site_UpdateDefaultSiteURL)
            {
                // Old data
                string strUrlOld = elementOld.Attribute("Value").Value.DecodeXMLValue();

                var listSiteUrlOld = new List<SiteUrl>();
                XDocument documentSiteUrlOld = XDocument.Parse(strUrlOld);
                var siteUrlOld = documentSiteUrlOld.Descendants("ITEM");

                foreach (var item in siteUrlOld)
                {
                    var objSiteUrl = new SiteUrl
                    {
                        Url = item.Attribute("URL").Value,
                        Environment = item.Attribute("Environment").Value,
                        IsDefault = item.Attribute("IsDefault").Value == "1" ? true : false,
                        IsActive = item.Attribute("IsActive").Value == "1" ? true : false,
                    };
                    listSiteUrlOld.Add(objSiteUrl);
                }

                ViewBag.SiteUrlOld = listSiteUrlOld;

                // New data
                string strUrl = elementNew.Attribute("Value").Value.DecodeXMLValue();

                var listSiteUrl = new List<SiteUrl>();
                XDocument documentSiteUrl = XDocument.Parse(strUrl);
                var siteUrl = documentSiteUrl.Descendants("ITEM");

                foreach (var item in siteUrl)
                {
                    var objSiteUrl = new SiteUrl
                    {
                        Url = item.Attribute("URL").Value,
                        Environment = item.Attribute("Environment").Value,
                        IsDefault = item.Attribute("IsDefault").Value == "1" ? true : false,
                        IsActive = item.Attribute("IsActive").Value == "1" ? true : false,
                    };
                    listSiteUrl.Add(objSiteUrl);
                }

                ViewBag.SiteUrl = listSiteUrl;
                ViewBag.IsSiteUpdate = true;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAP_Site_Url.cshtml");
        }

        /// <summary>
        /// Get assign permission log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_Assign_Permission(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            // NAP_Group_AssignFunction || NAP_User_AssignFunction
            if (log.ActionId == (int)LogAction.NAP_Group_AssignFunction
                || log.ActionId == (int)LogAction.NAP_User_AssignFunction)
            {
                // Get function list
                int siteId = Convert.ToInt32(elementNew.Attribute("SiteId").Value);

                var site = _siteService.GetById(siteId);

                ViewBag.FunctionList = this._functionService.GetBySiteInHierarchy(siteId, Environment, true);

                // Old data
                var listFunctionPolicyOld = new List<FunctionPolicy>();

                if (elementOld.Attribute("Value") != null)
                {
                    string strFunctionPolicy = elementOld.Attribute("Value").Value.DecodeXMLValue();
                    if (!string.IsNullOrWhiteSpace(strFunctionPolicy))
                    {
                        XDocument documentFunctionPolicy = XDocument.Parse(strFunctionPolicy);
                        var functionPolicy = documentFunctionPolicy.Descendants("ITEM");

                        foreach (var item in functionPolicy)
                        {
                            var objFunctionPolicy = new FunctionPolicy
                            {
                                Function = new Function
                                {
                                    Id = Convert.ToInt16(item.Attribute("FunctionId").Value)
                                },
                                Policy = Convert.ToInt32(item.Attribute("Policy").Value)
                            };
                            listFunctionPolicyOld.Add(objFunctionPolicy);
                        }

                        ViewBag.ListOldData = listFunctionPolicyOld;
                    }
                }

                // New data
                if (elementNew.Attribute("Value") != null)
                {
                    string strFunctionPolicy = elementNew.Attribute("Value").Value.DecodeXMLValue();
                    if (!string.IsNullOrWhiteSpace(strFunctionPolicy))
                    {
                        var hashNewPolicy = new Hashtable();

                        XDocument documentFunctionPolicy = XDocument.Parse(strFunctionPolicy);
                        var functionPolicy = documentFunctionPolicy.Descendants("ITEM");

                        foreach (var item in functionPolicy)
                        {
                            var id = Convert.ToInt16(item.Attribute("FunctionId").Value);
                            var policy = Convert.ToInt32(item.Attribute("Policy").Value);

                            if (!hashNewPolicy.Contains(id))
                            {
                                hashNewPolicy.Add(id, policy);
                            }
                        }

                        ViewBag.ListNewData = hashNewPolicy;
                    }
                }

                ViewBag.IsSiteUpdate = true;

                ViewBag.SiteName = site.Name;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAP_Assign_Permission.cshtml");
        }

        #endregion

        /// <summary>
        /// Get assign site log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_Assign_Site(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_User_AssignSite)
            {
                // Old data
                if (elementOld.Attribute("Value") != null
                    && !string.IsNullOrWhiteSpace(elementOld.Attribute("Value").Value))
                {
                    string strUrlOld = elementOld.Attribute("Value").Value.DecodeXMLValue();

                    var listSiteUrlOld = new List<SiteLevel>();
                    XDocument documentSiteUrlOld = XDocument.Parse(strUrlOld);
                    var siteUrlOld = documentSiteUrlOld.Descendants("ITEM");

                    foreach (var item in siteUrlOld)
                    {
                        var objSiteUrl = new SiteLevel
                        {
                            Site = new Site
                            {
                                Id = Convert.ToByte(item.Attribute("SiteId").Value),
                                Name = item.Attribute("SiteName").Value
                            },
                            Level = Convert.ToInt32(item.Attribute("Level").Value),
                            IsActive = item.Attribute("IsActive").Value == "1" ? true : false
                        };
                        listSiteUrlOld.Add(objSiteUrl);
                    }

                    ViewBag.SiteUrlOld = listSiteUrlOld;
                }

                // New data
                if (elementNew.Attribute("Value") != null)
                {
                    string strUrl = elementNew.Attribute("Value").Value.DecodeXMLValue();

                    var listSiteUrl = new List<SiteLevel>();
                    XDocument documentSiteUrl = XDocument.Parse(strUrl);
                    var siteUrl = documentSiteUrl.Descendants("ITEM");
                    foreach (var item in siteUrl)
                    {
                        var objSiteUrl = new SiteLevel
                        {
                            Site = new Site
                            {
                                Id = Convert.ToByte(item.Attribute("SiteId").Value),
                                Name = item.Attribute("SiteName").Value
                            },
                            Level = Convert.ToInt32(item.Attribute("Level").Value),
                            IsActive = item.Attribute("IsActive").Value == "1" ? true : false
                        };
                        listSiteUrl.Add(objSiteUrl);
                    }

                    ViewBag.SiteUrl = listSiteUrl;
                }

                ViewBag.IsSiteUpdate = true;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAP_Assign_Site.cshtml");
        }

        /// <summary>
        /// Get assign permission to users log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_Function_AssignToUsers(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_Function_AssignFunctionToUsers)
            {
                // Old data
                string strUserPolicyOld = elementOld.Attribute("Value").Value.DecodeXMLValue();
                var listUserPolicyOld = new List<UserPolicy>();
                XDocument documentUserPolicyOld = XDocument.Parse(strUserPolicyOld);
                var userPolicyOld = documentUserPolicyOld.Descendants("ITEM");

                foreach (var item in userPolicyOld)
                {
                    var userPolicy = new UserPolicy
                    {
                        User = new User
                        {
                            Username = item.Attribute("UserName").Value
                        },
                        Policy = Convert.ToInt32(item.Attribute("Policy").Value)
                    };
                    listUserPolicyOld.Add(userPolicy);
                }

                ViewBag.UserPolicyOld = listUserPolicyOld;

                // New data
                string strUrl = elementNew.Attribute("Value").Value.DecodeXMLValue();
                var listUserPolicy = new List<UserPolicy>();
                XDocument documentUserPolicy = XDocument.Parse(strUrl);
                var userPolicyNew = documentUserPolicy.Descendants("ITEM");

                foreach (var item in userPolicyNew)
                {
                    var userPolicy = new UserPolicy
                    {
                        User = new User
                        {
                            Username = item.Attribute("UserName").Value
                        },
                        Policy = Convert.ToInt32(item.Attribute("Policy").Value)
                    };
                    listUserPolicy.Add(userPolicy);
                }

                ViewBag.UserPolicyNew = listUserPolicy;
                ViewBag.IsSiteUpdate = true;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAP_Function_AssignToUsers.cshtml");
        }

        /// <summary>
        /// Get assign permission to groups log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_Function_AssignFunctionToGroups(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_Function_AssignFunctionToGroups)
            {
                // Old data
                string strGroupPolicyOld = elementOld.Attribute("Value").Value.DecodeXMLValue();

                var listGroupPolicyOld = new List<GroupPolicy>();
                XDocument documentGroupPolicyOld = XDocument.Parse(strGroupPolicyOld);
                var groupPolicyOld = documentGroupPolicyOld.Descendants("ITEM");

                foreach (var item in groupPolicyOld)
                {
                    var groupPolicy = new GroupPolicy
                    {
                        Group = new Group
                        {
                            Name = item.Attribute("GroupName").Value
                        },
                        Policy = Convert.ToInt32(item.Attribute("Policy").Value)
                    };
                    listGroupPolicyOld.Add(groupPolicy);
                }

                ViewBag.GroupPolicyOld = listGroupPolicyOld;

                // New data
                string strUrl = elementNew.Attribute("Value").Value.DecodeXMLValue();

                var listGroupPolicy = new List<GroupPolicy>();
                XDocument documentGroupPolicy = XDocument.Parse(strUrl);
                var groupPolicyNew = documentGroupPolicy.Descendants("ITEM");

                foreach (var item in groupPolicyNew)
                {
                    var groupPolicy = new GroupPolicy
                    {
                        Group = new Group
                        {
                            Name = item.Attribute("GroupName").Value
                        },
                        Policy = Convert.ToInt32(item.Attribute("Policy").Value)
                    };
                    listGroupPolicy.Add(groupPolicy);
                }

                ViewBag.GroupPolicyNew = listGroupPolicy;
                ViewBag.IsSiteUpdate = true;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAP_Function_AssignToGroups.cshtml");
        }

        #region Mailing list log

        /// <summary>
        /// Get insert mailing list log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_MailingList_Insert(LogExecSp log)
        {
            XElement element = XElement.Parse(log.Data);

            if (log.ActionId == (int)LogAction.NAP_MailingList_Insert)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.List_Key,
                    NewValue = element.Attribute("ListKey").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.List_Name,
                    NewValue = element.Attribute("ListName").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    NewValue = element.Attribute("Description").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Site_Name,
                    NewValue = element.Attribute("SiteName").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Environment,
                    NewValue = element.Attribute("Environment").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.ListFunction,
                    NewValue = element.Attribute("ListFunctionName").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Function,
                    NewValue = element.Attribute("FunctionName").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.To,
                    NewValue = element.Attribute("ToList").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Cc,
                    NewValue = element.Attribute("CcList").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Bcc,
                    NewValue = element.Attribute("BccList").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Internal,
                    NewValue = element.Attribute("IsInternal").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Activate,
                    NewValue = element.Attribute("IsActive").Value
                });

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogInsert.cshtml");
        }

        /// <summary>
        /// Get update mailing list log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public PartialViewResult _NAP_MailingList_Update(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_MailingList_Update)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.List_Key,
                    OldValue = elementOld.Attribute("ListKey").Value,
                    NewValue = elementNew.Attribute("ListKey").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.List_Name,
                    OldValue = elementOld.Attribute("ListName").Value,
                    NewValue = elementNew.Attribute("ListName").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Description,
                    OldValue = elementOld.Attribute("Description").Value,
                    NewValue = elementNew.Attribute("Description").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Site_Name,
                    OldValue = elementOld.Attribute("SiteName").Value,
                    NewValue = elementNew.Attribute("SiteName").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Environment,
                    OldValue = elementOld.Attribute("Environment").Value,
                    NewValue = elementNew.Attribute("Environment").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.ListFunction,
                    OldValue = elementOld.Attribute("ListFunctionName").Value,
                    NewValue = elementNew.Attribute("ListFunctionName").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Function,
                    OldValue = elementOld.Attribute("FunctionName").Value,
                    NewValue = elementNew.Attribute("FunctionName").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.To,
                    OldValue = elementOld.Attribute("ToList").Value,
                    NewValue = elementNew.Attribute("ToList").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Cc,
                    OldValue = elementOld.Attribute("CcList").Value,
                    NewValue = elementNew.Attribute("CcList").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Bcc,
                    OldValue = elementOld.Attribute("BccList").Value,
                    NewValue = elementNew.Attribute("BccList").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Internal,
                    OldValue = elementOld.Attribute("IsInternal").Value,
                    NewValue = elementNew.Attribute("IsInternal").Value
                });
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Activate,
                    OldValue = elementOld.Attribute("IsActive").Value,
                    NewValue = elementNew.Attribute("IsActive").Value
                });

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update mailing list status log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_MailingList_UpdateStatus(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_MailingList_UpdateStatus)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Status,
                    OldValue = elementOld.Attribute("IsActive").Value == "1" ? "Active" : "Unactive",
                    NewValue = elementNew.Attribute("IsActive").Value == "1" ? "Active" : "Unactive"
                });

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        /// <summary>
        /// Get update mailing list internal log detail.
        /// </summary>
        /// <param name="log">Execute SP log.</param>
        /// <returns>Partial view result.</returns>
        public PartialViewResult _NAP_MailingList_UpdateInternal(LogExecSp log)
        {
            XElement elementNew = XElement.Parse(log.Data);
            XElement elementOld = XElement.Parse(log.OldData ?? "<ROOT/>");

            if (log.ActionId == (int)LogAction.NAP_MailingList_UpdateIsInternal)
            {
                var listLogData = new List<LogData>();
                listLogData.Add(new LogData
                {
                    FieldName = ViewResources.Internal,
                    OldValue = elementOld.Attribute("IsInternal").Value == "1" ? "True" : "False",
                    NewValue = elementNew.Attribute("IsInternal").Value == "1" ? "True" : "False"
                });

                ViewBag.IsInsert = true;
                ViewBag.LogData = listLogData;
            }

            return PartialView("~/Areas/UserControl/Views/Trace/_NAPLogUpdate.cshtml");
        }

        #endregion
    }
}
