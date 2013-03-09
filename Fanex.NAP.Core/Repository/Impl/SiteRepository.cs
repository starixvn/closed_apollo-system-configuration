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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Data;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Criteria.Site;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class SiteRepository : RepositoryBase<ISite>, ISiteRepository
    {
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

        public override int Count(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

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

        public ICommandResult Delete(ICriteria criteria, ICriteria logCriteria)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(ISite entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(ISite entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Delete(ISite entity)
        {
            throw new NotImplementedException();
        }
    }
}
