// --------------------------------------------------------------------------
// <copyright file="SiteRepository.cs" company="Nexcel Solutions Vietnam">
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
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Criteria.Site;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Site repository.
    /// </summary>
    public class SiteRepository : RepositoryBase<ISite>, ISiteRepository
    {
        /// <summary>
        /// Gets site.
        /// </summary>
        /// <param name="criteria">The criteria get site.</param>
        /// <returns>The site object.</returns>
        public override ISite Get(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<Site>(criteria.GetSpParams());
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets site URL.
        /// </summary>
        /// <param name="criteria">The criteria get site URL.</param>
        /// <returns>The site URL.</returns>
        public SiteUrl GetSiteUrl(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<SiteUrl, Site, SiteUrl>(
                        (su, s) =>
                        {
                            su.Site = s;
                            return su;
                        },
                        "Id",
                        criteria.GetSpParams());
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Count site.
        /// </summary>
        /// <param name="criteria">The criteria count site.</param>
        /// <returns>Number of site.</returns>
        public override int Count(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetch site.
        /// </summary>
        /// <param name="criteria">The criteria fetch site.</param>
        /// <returns>List of site.</returns>
        public override IEnumerable<ISite> Fetch(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<Site>(criteria.GetSpParams()).OrderBy(s => s.Name).ToList<Site>();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch site level.
        /// </summary>
        /// <param name="criteria">The criteria fetch site level.</param>
        /// <returns>List of site level.</returns>
        public IEnumerable<SiteLevel> FetchSiteLevel(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<SiteLevel, Site, User, User, SiteLevel>(
                         (sl, s, cu, mu) =>
                         {
                             sl.Site = s;
                             sl.Creator = cu;
                             sl.Modifier = mu;
                             return sl;
                         },
                         "SiteId,CreatorId,ModifierId",
                         criteria.GetSpParams()).OrderBy(s => s.Site.Name).ToList<SiteLevel>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fetch site URL.
        /// </summary>
        /// <param name="criteria">The criteria fetch site URL.</param>
        /// <returns>List of site URL.</returns>
        public IEnumerable<SiteUrl> FetchSiteUrl(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<SiteUrl, Site, SiteUrl>(
                         (su, s) =>
                         {
                             su.Site = s;
                             return su;
                         },
                         "Id",
                         criteria.GetSpParams()).OrderBy(s => s.Site.Name).ToList<SiteUrl>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert site.
        /// </summary>
        /// <param name="criteria">The criteria insert site.</param>
        /// <param name="logCriteria">The log criteria insert site.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Insert(ref InsertSiteCriteria criteria, ICriteria logCriteria)
        {
            try
            {
                // prepare SP parameters
                Hashtable hash = criteria.GetSpParams();

                hash = logCriteria.AddSpParams(hash);
                hash = (new ResultCriteria()).AddSpParams(hash);

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);

                    criteria.SiteId = Convert.ToInt32(db.GetParameterValue("SiteId"));

                    return GetCommandResult(hash);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert site URL.
        /// </summary>
        /// <param name="criteria">The criteria insert site URL.</param>
        /// <param name="logCriteria">The log criteria insert site URL.</param>
        /// <returns>Command result.</returns>
        public ICommandResult InsertSiteUrl(ref InsertSiteUrlCriteria criteria, ICriteria logCriteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();

                hash = logCriteria.AddSpParams(hash);
                hash = (new ResultCriteria()).AddSpParams(hash);

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);

                    criteria.SiteUrlId = Convert.ToInt32(db.GetParameterValue("SiteUrlId"));

                    return GetCommandResult(hash);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update site.
        /// </summary>
        /// <param name="criteria">The criteria update site.</param>
        /// <param name="logCriteria">The log criteria update site.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Update(ICriteria criteria, ICriteria logCriteria)
        {
            try
            {
                // prepare SP parameters
                Hashtable hash = criteria.GetSpParams();

                hash = logCriteria.AddSpParams(hash);
                hash = (new ResultCriteria()).AddSpParams(hash);

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);

                    return GetCommandResult(hash);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete site.
        /// </summary>
        /// <param name="criteria">The criteria delete site.</param>
        /// <param name="logCriteria">The log criteria delete site.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Delete(ICriteria criteria, ICriteria logCriteria)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert site.
        /// </summary>
        /// <param name="entity">The site entity.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Insert(ISite entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update site.
        /// </summary>
        /// <param name="entity">The site entity.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Update(ISite entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete site.
        /// </summary>
        /// <param name="entity">The site entity.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Delete(ISite entity)
        {
            throw new NotImplementedException();
        }
    }
}
