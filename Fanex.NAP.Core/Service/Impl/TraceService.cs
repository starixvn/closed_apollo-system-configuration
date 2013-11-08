// --------------------------------------------------------------------------
// <copyright file="TraceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.11.14">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml.Linq;
    using Fanex.NAP.Core.Criteria.Trace;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Impl;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Trace service.
    /// </summary>
    public class TraceService : BaseService, ITraceService
    {
        private readonly ITraceRepository _traceRepository;

        public TraceService()
            : this(null)
        {
        }

        public TraceService(ITraceRepository traceRepository)
        {
            this._traceRepository = traceRepository ?? new TraceRepository();
        }

        /// <summary>
        /// Search execute SP log.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalRows">Total number of records.</param>
        /// <param name="name">The item name.</param>
        /// <param name="username">The username.</param>
        /// <param name="from">From date value.</param>
        /// <param name="to">To date value.</param>
        /// <param name="filterBy">Filter by option.</param>
        /// <returns>List of execute SP log.</returns>
        public IQueryable<LogExecSp> SearchExecSp(
            int pageIndex, 
            int pageSize, 
            out int totalRows, 
            string name, 
            string username, 
            DateTime from, 
            DateTime to, 
            int filterBy = 0)
        {
            IQueryable<LogExecSp> logList = null;
            totalRows = 0;

            try
            {
                var criteria = new GetAllLogCriteria
                {
                    Username = username,
                    Name = name,
                    From = from,
                    To = to,
                    FilterType = filterBy,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                if (criteria.IsValid())
                {
                    logList = this._traceRepository.SearchLogExecSp(criteria, out totalRows).AsQueryable();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return logList;
        }

        /// <summary>
        /// Get execute SP log by ID.
        /// </summary>
        /// <param name="logId">Execute SP log ID.</param>
        /// <returns>Execute SP log.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public LogExecSp GetById(int logId)
        {
            var criteria = new GetByIdLogCriteria
            {
                LogId = logId
            };

            if (criteria.IsValid())
            {
                var logExec = this._traceRepository.GetById(criteria);

                if (!ReferenceEquals(logExec, null))
                {
                    var hashItems = new Hashtable();
                    var hashSubItems = new Hashtable();

                    if (!string.IsNullOrWhiteSpace(logExec.Items))
                    {
                        var xmlItems = XDocument.Parse(logExec.Items);
                        var xmlItemRows = xmlItems.Descendants("row");
                        foreach (var item in xmlItemRows)
                        {
                            var alias = item.Attribute("alias").Value.ToString();
                            if (!hashItems.Contains(alias))
                            {
                                var fieldName = item.Attribute("item").Value.ToString();
                                hashItems.Add(alias, fieldName);
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(logExec.SubItems))
                    {
                        var xmlItems = XDocument.Parse(logExec.SubItems);
                        var xmlItemRows = xmlItems.Descendants("row");
                        foreach (var item in xmlItemRows)
                        {
                            var alias = item.Attribute("alias").Value.ToString();
                            if (!hashSubItems.Contains(alias))
                            {
                                var fieldName = item.Attribute("item").Value.ToString();
                                hashSubItems.Add(alias, fieldName);
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(logExec.Data))
                    {
                        var xmlData = XDocument.Parse(logExec.Data);
                        var xmlElementData = new XElement("ROOT");
                        if (hashItems.Count > 0)
                        {
                            foreach (DictionaryEntry item in hashItems)
                            {
                                var value = xmlData.Root.Attribute(item.Key.ToString()) != null ?
                                    xmlData.Root.Attribute(item.Key.ToString()).Value.ToString() : string.Empty;

                                xmlElementData.SetAttributeValue(item.Value.ToString(), value.ToString());
                            }
                        }

                        if (hashSubItems.Count > 0)
                        {
                            var xmlElementValue = new XElement("ROW");
                            var xmlElementSubData = xmlData.Descendants("z");
                            foreach (var item in xmlElementSubData)
                            {
                                var xmlElementChildTemp = new XElement("ITEM");
                                foreach (DictionaryEntry itemAtt in hashSubItems)
                                {
                                    var value = item.Attribute(itemAtt.Key.ToString()) != null ?
                                        item.Attribute(itemAtt.Key.ToString()).Value.ToString() : string.Empty;

                                    xmlElementChildTemp.SetAttributeValue(itemAtt.Value.ToString(), value);
                                }

                                xmlElementValue.Add(xmlElementChildTemp);
                            }

                            xmlElementData.SetAttributeValue("Value", this.EncodeXMLValue(xmlElementValue.ToString()));
                        }

                        logExec.Data = xmlElementData.ToString();
                    }

                    if (!string.IsNullOrWhiteSpace(logExec.OldData))
                    {
                        var xmlData = XDocument.Parse(logExec.OldData);
                        var xmlElementData = new XElement("ROOT");
                        if (hashItems.Count > 0)
                        {
                            foreach (DictionaryEntry item in hashItems)
                            {
                                var value = xmlData.Root.Attribute(item.Key.ToString()) != null ?
                                    xmlData.Root.Attribute(item.Key.ToString()).Value.ToString() : string.Empty;

                                xmlElementData.SetAttributeValue(item.Value.ToString(), value.ToString());
                            }
                        }

                        if (hashSubItems.Count > 0)
                        {
                            var xmlElementValue = new XElement("ROW");
                            var xmlElementSubData = xmlData.Descendants("z");
                            foreach (var item in xmlElementSubData)
                            {
                                var xmlElementChildTemp = new XElement("ITEM");
                                foreach (DictionaryEntry itemAtt in hashSubItems)
                                {
                                    var value = item.Attribute(itemAtt.Key.ToString()) != null ?
                                        item.Attribute(itemAtt.Key.ToString()).Value.ToString() : string.Empty;

                                    xmlElementChildTemp.SetAttributeValue(itemAtt.Value.ToString(), value);
                                }

                                xmlElementValue.Add(xmlElementChildTemp);
                            }

                            xmlElementData.SetAttributeValue("Value", this.EncodeXMLValue(xmlElementValue.ToString()));
                        }

                        logExec.OldData = xmlElementData.ToString();
                    }
                }

                return logExec;
            }

            return null;
        }

        /// <summary>
        /// Insert user login log.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="ip">IP address.</param>
        /// <param name="date">Log date value.</param>
        /// <param name="domain">Domain name.</param>
        /// <param name="broswser">Browser name.</param>
        /// <param name="platform">Platform name.</param>
        /// <param name="countryCode">Country code.</param>
        /// <param name="doing">Doing action.</param>
        /// <param name="duration">Log duration.</param>
        /// <returns>The command result.</returns>
        public ICommandResult InsertLogUserLogin(
            int userId,
            int siteId,
            string ip,
            DateTime date,
            string domain,
            string broswser,
            string platform,
            string countryCode,
            string doing,
            int duration)
        {
            try
            {
                var criteria = new InsertLogUserLoginCriteria
                {
                    UserId = userId,
                    SiteId = siteId,
                    LogDate = date,
                    DomainName = domain,
                    BrowserName = broswser,
                    Platform = platform,
                    Doing = doing,
                    IP = ip,
                    CountryCode = countryCode,
                    Duration = duration
                };

                if (criteria.IsValid())
                {
                    return this._traceRepository.InsertLog(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Search user login log.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalRows">Total number of records.</param>
        /// <param name="name">The username.</param>
        /// <param name="from">From date value.</param>
        /// <param name="to">To date value.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>List of user login log.</returns>
        public IQueryable<LogUserLogin> SearchUserLogin(
            int pageIndex, 
            int pageSize, 
            out int totalRows, 
            string name, 
            DateTime from, 
            DateTime to, 
            int siteId = 0)
        {
            IQueryable<LogUserLogin> logList = null;
            totalRows = 0;

            try
            {
                var criteria = new GetAllLogUserLoginCriteria
                {
                    Name = name,
                    From = from,
                    To = to,
                    SiteId = siteId,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                if (criteria.IsValid())
                {
                    logList = this._traceRepository.SearchLogUserLogin(criteria, out totalRows).AsQueryable();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return logList;
        }

        /// <summary>
        /// Insert function log.
        /// </summary>
        /// <param name="functionId">The function ID.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="ip">IP address.</param>
        /// <param name="date">Log date value.</param>
        /// <param name="browser">Browser name.</param>
        /// <returns>The command result.</returns>
        public ICommandResult InsertLogFunction(
            int functionId, 
            int userId, 
            int siteId, 
            string param, 
            string ip, 
            DateTime date, 
            string browser)
        {
            try
            {
                var criteria = new InsertLogFunctionCriteria
                {
                    FunctionId = functionId,
                    UserId = userId,
                    SiteId = siteId,
                    LogDate = date,
                    Param = param,
                    BrowserName = browser,
                    IP = ip
                };

                if (criteria.IsValid())
                {
                    return this._traceRepository.InsertLog(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Search function log.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalRows">Total number of records.</param>
        /// <param name="name">The function name.</param>
        /// <param name="from">From date value.</param>
        /// <param name="to">To date value.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>List of function log.</returns>
        public IQueryable<LogFunction> SearchFunction(
            int pageIndex, 
            int pageSize, 
            out int totalRows, 
            string name, 
            DateTime from, 
            DateTime to, 
            int siteId = 0)
        {
            IQueryable<LogFunction> logList = null;
            totalRows = 0;

            try
            {
                var criteria = new GetAllLogFunctionCriteria
                {
                    Name = name,
                    From = from,
                    To = to,
                    SiteId = siteId,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                if (criteria.IsValid())
                {
                    logList = this._traceRepository.SearchLogFunction(criteria, out totalRows).AsQueryable();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return logList;
        }

        /// <summary>
        /// Decode xml of [f_EncodeXMLValue].
        /// </summary>
        /// <param name="value">XML value.</param>
        /// <returns>Encoded XML value.</returns>
        private string EncodeXMLValue(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("&", "&amp;");
                value = value.Replace("<", "&lt;");
                value = value.Replace(">", "&gt;");
                value = value.Replace("\"", "&quot;");
                value = value.Replace("''", "&apos;");

                return value;
            }

            return value;
        }
    }
}