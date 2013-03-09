// --------------------------------------------------------------------------
// <copyright file="SiteService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
//   <change who="Lewis.Le" date="2012.10.5">
//      Added GetSiteUrlByUser
//      Renamed GetByUser to GetSiteLevelByUser
//   </change>
//   <change who="Lewis.Le" date="2012.10.8">Added GetSiteUrlById</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Caching;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Function;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Criteria.Site;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Impl;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class SiteService : BaseService, ISiteService
    {
        #region members

        private readonly ISiteRepository _siteRepository;
        private readonly IFunctionRepository _functionRepository;
        private readonly ICache _cache;

        #endregion

        #region ctors

        public SiteService()
            : this(null, null, null)
        {
        }

        public SiteService(ISiteRepository siteRepository, IFunctionRepository functionRepository, ICache cache)
        {
            _siteRepository = siteRepository ?? new SiteRepository();
            _functionRepository = functionRepository ?? new FunctionRepository();
            _cache = cache ?? CacheService.CreateInstance(CacheType.MemoryCache);
        }

        #endregion

        #region get

        public ISite GetById(int id)
        {
            // check parammeters
            Check.Argument.IsNotNegativeOrZero(id, "id");

            try
            {
                var criteria = new GetByIdSiteCriteria
                {
                    SiteId = id
                };

                if (criteria.IsValid())
                {
                    return _siteRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ISite GetByName(string name)
        {
            // check parammeters
            Check.Argument.IsNotEmpty(name, "name");

            try
            {
                var criteria = new GetByNameSiteCriteria
                {
                    SiteName = name
                };

                if (criteria.IsValid())
                {
                    return _siteRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public bool GetStatusById(int id)
        {
            // check parammeters
            Check.Argument.IsNotNegativeOrZero(id, "id");

            try
            {
                bool isActive = false;
                string cacheKey = CacheKey.CacheSiteStatus + id.ToString();

                if (_cache.Contains(cacheKey))
                {
                    isActive = _cache.Get<bool>(cacheKey);
                }
                else
                {
                    var criteria = new GetByIdSiteCriteria
                    {
                        SiteId = id
                    };

                    if (criteria.IsValid())
                    {
                        ISite site = _siteRepository.Get(criteria);

                        if (site != null)
                        {
                            _cache.Set(CacheKey.CacheSiteStatus, site.IsActive, 3600);
                            isActive = site.IsActive;
                        }
                    }

                    return isActive;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        public IEnumerable<SiteLevel> GetSiteLevelByUser(int userId, string env = Constant.UAT, bool? isActive = null)
        {
            // check parammeters
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByUserSiteCriteria
                {
                    UserId = userId,
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return _siteRepository.FetchSiteLevel(criteria).OrderBy(sl => sl.Site.Name);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<ISite> GetAll(string env = Constant.UAT, bool? isActive = null)
        {
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetAllSiteCriteria
                {
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return _siteRepository.Fetch(criteria).OrderBy(s => s.Name);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IList<SelectListItem> GetAllSelectListItem(
            string env = Constant.UAT,
            bool? isActive = null,
            int selectedSiteId = 0)
        {
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetAllSiteCriteria
                {
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    IEnumerable<ISite> siteList = _siteRepository.Fetch(criteria).OrderBy(s => s.Name);

                    if (siteList != null)
                    {
                        return siteList.Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString(),
                                Selected = s.Id == selectedSiteId
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<ISite> GetAllWithNumOfUsers(string env = Constant.UAT, bool? isActive = null)
        {
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetAllSiteWithNumOfUserCriteria
                {
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return _siteRepository.Fetch(criteria).OrderBy(s => s.Name);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<ISite> GetByUserAndFunctionKeyIncludeGroup(
            int userId,
            string functionKey,
            string env = Constant.UAT,
            Constant.Policy policy = Constant.Policy.Read)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(userId, "UserId");
            Check.Argument.IsNotEmpty(functionKey, "FunctionKey");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByUserAndFunctionKeyIncludeGroupFunctionCriteria
                {
                    UserId = userId,
                    FunctionKey = functionKey,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    IEnumerable<FunctionPolicy> functionPolicyList = _functionRepository.FetchFunctionPolicy(criteria).OrderBy(s => s.Site.Name);

                    if (functionPolicyList != null)
                    {
                        return functionPolicyList.Where(f => f.Policy >= (int)policy)
                            .Select(s => new Site
                            {
                                Id = s.Site.Id,
                                Name = s.Site.Name
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IList<SelectListItem> GetSelectListItemByUserAndFunctionKeyIncludeGroup(
            int userId,
            string functionKey,
            string env = Constant.UAT,
            Constant.Policy policy = Constant.Policy.Read,
            int selectedSiteId = 0)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(userId, "UserId");
            Check.Argument.IsNotEmpty(functionKey, "FunctionKey");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByUserAndFunctionKeyIncludeGroupFunctionCriteria
                {
                    UserId = userId,
                    FunctionKey = functionKey,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    IEnumerable<FunctionPolicy> functionPolicyList = _functionRepository.FetchFunctionPolicy(criteria).OrderBy(s => s.Site.Name);

                    if (functionPolicyList != null)
                    {
                        return functionPolicyList.Where(f => f.Policy >= (int)policy)
                            .Select(s => new SelectListItem
                            {
                                Text = s.Site.Name,
                                Value = s.Site.Id.ToString(),
                                Selected = s.Site.Id == selectedSiteId
                            }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion

        #region insert/update

        public ICommandResult Insert(InsertSiteCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _siteRepository.Insert(ref criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult Update(UpdateSiteCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _siteRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult UpdateStatus(UpdateStatusSiteCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _siteRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion

        #region SiteLevel services

        public SiteLevel GetUserSiteLevel(int userId, int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");

            try
            {
                var criteria = new GetUserSiteLevelSiteCriteria
                {
                    UserId = userId,
                    SiteId = siteId
                };

                if (criteria.IsValid())
                {
                    return _siteRepository.FetchSiteLevel(criteria).FirstOrDefault<SiteLevel>();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion

        #region SiteUrl services

        public IEnumerable<SiteUrl> GetAllSiteUrl(string env)
        {
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetAllSiteUrlCriteria
                {
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return _siteRepository.FetchSiteUrl(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<SiteUrl> GetSiteUrlByUser(int userId, string env)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotEmpty(env, "Environment");

            var result = new List<SiteUrl>();

            try
            {
                var criteria = new GetSiteUrlByUserCriteria
                {
                    UserId = userId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    result = _siteRepository.FetchSiteUrl(criteria).OrderBy(su => su.Site.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        public IEnumerable<SiteUrl> GetSiteUrlBySiteId(int siteId, string env)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            var result = new List<SiteUrl>();

            try
            {
                var criteria = new GetSiteUrlBySiteIdCriteria
                {
                    SiteId = siteId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    result = _siteRepository.FetchSiteUrl(criteria).OrderBy(su => su.Site.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        /// <summary>
        /// Gets a SiteUrl by its SiteUrlId
        /// </summary>
        /// <param name="siteUrlId">Site URL ID</param>
        /// <returns>Site URL</returns>
        public SiteUrl GetSiteUrlById(int siteUrlId)
        {
            Check.Argument.IsNotNegativeOrZero(siteUrlId, "siteUrlId");

            SiteUrl result = null;

            try
            {
                var criteria = new GetSiteUrlByIdCriteria
                {
                    SiteUrlId = siteUrlId
                };

                if (criteria.IsValid())
                {
                    result = _siteRepository.GetSiteUrl(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        public ICommandResult UpdateSiteUrl(UpdateSiteUrlCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _siteRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult UpdateDefaultSiteUrl(UpdateDefaultSiteUrlCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _siteRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult InsertSiteUrl(InsertSiteUrlCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _siteRepository.InsertSiteUrl(ref criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion
    }
}