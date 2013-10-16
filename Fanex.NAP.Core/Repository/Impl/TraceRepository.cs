// --------------------------------------------------------------------------
// <copyright file="TraceRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.13">Created</change>
//   <change who="Lewis.Le" date="2012.10.5">Added FetchSiteUrl</change>
// </history>

namespace Fanex.NAP.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Linq;
    using Fanex.Data;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Trace repository.
    /// </summary>
    public class TraceRepository : DynamicRepository, ITraceRepository
    {
        /// <summary>
        /// Search execute SP log.
        /// </summary>
        /// <param name="criteria">The criteria search execute SP log.</param>
        /// <param name="totalRows">Total rows.</param>
        /// <returns>List of execute SP log.</returns>
        public IQueryable<LogExecSp> SearchLogExecSp(ICriteria criteria, out int totalRows)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    Hashtable hash = criteria.GetSpParams();

                    var logList = db.Query<LogExecSp>(hash);

                    totalRows = Convert.ToInt32(hash["totalrows"]);

                    return logList.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets execute SP log by ID.
        /// </summary>
        /// <param name="criteria">The criteria get execute SP log by ID.</param>
        /// <returns>Execute SP log.</returns>
        public LogExecSp GetById(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    Hashtable hash = criteria.GetSpParams();

                    return db.QueryEntity<LogExecSp>(hash);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert log.
        /// </summary>
        /// <param name="criteria">The criteria insert log.</param>
        /// <returns>Command result.</returns>
        public ICommandResult InsertLog(ICriteria criteria)
        {
            try
            {
                // prepare SP parameters
                Hashtable hash = criteria.GetSpParams();

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);

                    return new CommandResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Search user login log.
        /// </summary>
        /// <param name="criteria">The criteria search user login log.</param>
        /// <param name="totalRows">Total rows.</param>
        /// <returns>List of user login log.</returns>
        public IQueryable<LogUserLogin> SearchLogUserLogin(ICriteria criteria, out int totalRows)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    Hashtable hash = criteria.GetSpParams();

                    var logList = db.Query<LogUserLogin, User, Site, LogUserLogin>(
                                    (l, u, s) => { l.User = u; l.Site = s; return l; },
                                    "UserId,SiteId",
                                    hash);

                    totalRows = Convert.ToInt32(hash["totalrows"]);

                    return logList.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Search function log.
        /// </summary>
        /// <param name="criteria">The criteria search function log.</param>
        /// <param name="totalRows">Total rows.</param>
        /// <returns>List of function log.</returns>
        public IQueryable<LogFunction> SearchLogFunction(ICriteria criteria, out int totalRows)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    Hashtable hash = criteria.GetSpParams();

                    var logList = db.Query<LogFunction, Function, User, Site, LogFunction>(
                                    (l, f, u, s) => { l.Function = f; l.User = u; l.Site = s; return l; },
                                    "FunctionId,UserId,SiteId",
                                    hash);

                    totalRows = Convert.ToInt32(hash["totalrows"]);

                    return logList.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
