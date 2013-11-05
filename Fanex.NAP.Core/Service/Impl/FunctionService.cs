// --------------------------------------------------------------------------
// <copyright file="FunctionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
//   <change who="Edward.Nguyen" date="2012.8.7">Implemented</change>
//   <change who="Lewis.Le" date="2012.11.01">Added ICommandResult AssignFunction(AssignFunctionToUsersCriteria criteria)</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Function;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Impl;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Function service.
    /// </summary>
    public class FunctionService : BaseService, IFunctionService
    {
        private readonly IFunctionRepository _functionRepository;

        #region Ctors

        public FunctionService() : this(null)
        {
        }

        public FunctionService(IFunctionRepository functionRepository = null)
        {
            this._functionRepository = functionRepository ?? new FunctionRepository();
        }

        #endregion

        #region Gets Function

        /// <summary>
        /// Get function by ID.
        /// </summary>
        public IFunction GetById(int id)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(id, "Id");

            try
            {
                var criteria = new GetByIdFunctionCriteria
                {
                    FunctionId = id
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get function by name and site.
        /// </summary>
        public IFunction GetByNameAndSite(string name, int siteId)
        {
            // check parameters
            Check.Argument.IsNotEmpty(name, "Name");
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");

            try
            {
                var criteria = new GetByNameAndSiteFunctionCriteria
                {
                    FunctionName = name,
                    SiteId = siteId
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get function by key and site.
        /// </summary>
        public IFunction GetByKeyAndSite(string key, int siteId)
        {
            // check parameters
            Check.Argument.IsNotEmpty(key, "Key");
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");

            try
            {
                var criteria = new GetByKeyAndSiteFunctionCriteria
                {
                    FunctionKey = key,
                    SiteId = siteId
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion

        #region Get Functions

        /// <summary>
        /// Get all function.
        /// </summary>
        public IEnumerable<IFunction> GetAll(string env = Constant.UAT, bool? isActive = null)
        {
            // check parameters
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetAllFunctionCriteria
                {
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get all function and group by site.
        /// </summary>
        public Dictionary<ISite, IEnumerable<IFunction>> GetAllAndGroupBySite(
            string env = Constant.UAT,
            bool? isActive = null)
        {
            // check parameters
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetAllFunctionCriteria
                {
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IFunction> functionList = this._functionRepository.Fetch(criteria);

                    var result = new Dictionary<ISite, IEnumerable<IFunction>>();

                    var siteList = functionList.GroupBy(f => f.Site.Id)
                        .Select(s => new Site
                        {
                            Id = s.Key,
                            Name = s.Where(f => f.Site.Id == s.Key).FirstOrDefault().Site.Name
                        }).ToList();

                    foreach (var site in siteList)
                    {
                        var subFunctionList = functionList.Where(f => f.Site.Id == site.Id).ToList();

                        result.Add(site, subFunctionList);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get function by site.
        /// </summary>
        public IEnumerable<IFunction> GetBySite(int siteId, string env = Constant.UAT, bool? isActive = null)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetBySiteFunctionCriteria
                {
                    SiteId = siteId,
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get select list item by site.
        /// </summary>
        public IList<SelectListItem> GetSelectListItemBySite(
            int siteId,
            string env = Constant.UAT,
            bool? isActive = null, 
            int selectedFunctionId = 0)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetBySiteFunctionCriteria
                {
                    SiteId = siteId,
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IFunction> functionList = this._functionRepository.Fetch(criteria);

                    if (functionList != null)
                    {
                        return functionList.Select(s => new SelectListItem
                        {
                            Text = s.Name,
                            Value = s.Id.ToString(),
                            Selected = s.Id == selectedFunctionId
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

        /// <summary>
        /// Get function by site in hierarchy.
        /// </summary>
        public IEnumerable<IFunction> GetBySiteInHierarchy(int siteId, string env = Constant.UAT, bool? isActive = null)
        {
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetBySiteFunctionCriteria
                {
                    SiteId = siteId,
                    Environment = env,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IFunction> functionList = this._functionRepository.Fetch(criteria).OrderBy(x => x.DisplayOrder);

                    return SortFunctionsInHierarchy(functionList);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get parent function by ID.
        /// </summary>
        public IEnumerable<IFunction> GetParentById(int id)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(id, "Id");

            try
            {
                var criteria = new GetParentByIdFunctionCriteria
                {
                    FunctionId = id
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Search function.
        /// </summary>
        public IEnumerable<IFunction> Search(
            string functionName, 
            int siteId, 
            string env = Constant.UAT,
            bool? isActive = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFunction> GetByParentKeyAndSite(GetByParentKeyAndSiteCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(criteria.SiteId, "SiteId");
            Check.Argument.IsNotEmpty(criteria.ParentKey, "ParentKey");
            Check.Argument.IsNotEmpty(criteria.Environment, "Environment");

            try
            {
                if (criteria.IsValid())
                {
                    return _functionRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion

        #region Gets Function Policy

        /// <summary>
        /// Get user function policy.
        /// </summary>
        public IEnumerable<FunctionPolicy> GetByUser(int userId, string env = Constant.UAT)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(userId, "UserId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByUserFunctionCriteria
                {
                    UserId = userId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.FetchFullFunctionPolicy(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get function policy by user and group by site.
        /// </summary>
        public Dictionary<ISite, IEnumerable<FunctionPolicy>> GetByUserAndGroupBySite(
            int userId,
            string env = Constant.UAT)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(userId, "UserId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByUserFunctionCriteria
                {
                    UserId = userId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    IEnumerable<FunctionPolicy> functionPolicyList = this._functionRepository.FetchFullFunctionPolicy(criteria);

                    var result = new Dictionary<ISite, IEnumerable<FunctionPolicy>>();

                    var siteList = functionPolicyList.GroupBy(f => f.Site.Id)
                        .Select(s => new Site
                        {
                            Id = s.Key,
                            Name = s.Where(f => f.Site.Id == s.Key).FirstOrDefault().Site.Name
                        }).ToList();

                    foreach (var site in siteList)
                    {
                        var subFunctionPolicyList = functionPolicyList.Where(f => f.Site.Id == site.Id).ToList();

                        result.Add(site, subFunctionPolicyList);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get function policy by group.
        /// </summary>
        public IEnumerable<FunctionPolicy> GetByGroup(int groupId, string env = Constant.UAT)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(groupId, "GroupId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByGroupFunctionCriteria
                {
                    GroupId = groupId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.FetchFullFunctionPolicy(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get function policy by group and site.
        /// </summary>
        public IEnumerable<FunctionPolicy> GetByGroupAndSite(int groupId, int siteId, string env = Constant.UAT)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(groupId, "GroupId");
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByGroupAndSiteFunctionCriteria
                {
                    GroupId = groupId,
                    SiteId = siteId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.FetchFullFunctionPolicy(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
        
        /// <summary>
        /// Get function policy by group and group by site.
        /// </summary>
        public Dictionary<ISite, IEnumerable<FunctionPolicy>> GetByGroupAndGroupBySite(
            int groupId,
            string env = Constant.UAT)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(groupId, "GroupId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByGroupFunctionCriteria
                {
                    GroupId = groupId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    IEnumerable<FunctionPolicy> functionPolicyList = this._functionRepository.FetchFullFunctionPolicy(criteria);

                    var result = new Dictionary<ISite, IEnumerable<FunctionPolicy>>();

                    var siteList = functionPolicyList.GroupBy(f => f.Site.Id)
                        .Select(s => new Site
                        {
                            Id = s.Key,
                            Name = s.Where(f => f.Site.Id == s.Key).FirstOrDefault().Site.Name
                        }).ToList();

                    foreach (var site in siteList)
                    {
                        var subFunctionPolicyList = functionPolicyList.Where(f => f.Site.Id == site.Id).ToList();

                        result.Add(site, subFunctionPolicyList);
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get function policy by user site.
        /// </summary>
        public IEnumerable<FunctionPolicy> GetByUserAndSite(int userId, int siteId, string env = Constant.UAT)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(userId, "UserId");
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByUserAndSiteFunctionCriteria
                {
                    UserId = userId,
                    SiteId = siteId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.FetchFullFunctionPolicy(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get function policy by user and site include group.
        /// </summary>
        public IEnumerable<FunctionPolicy> GetByUserAndSiteIncludeGroup(
            int userId,
            int siteId,
            string env = Constant.UAT)
        {
            // check parameters
            Check.Argument.IsNotNegativeOrZero(userId, "UserId");
            Check.Argument.IsNotNegativeOrZero(siteId, "SiteId");
            Check.Argument.IsNotEmpty(env, "Environment");

            try
            {
                var criteria = new GetByUserAndSiteIncludeGroupFunctionCriteria
                {
                    UserId = userId,
                    SiteId = siteId,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return this._functionRepository.FetchFunctionPolicy(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get function policy by user and function key include group.
        /// </summary>
        public IEnumerable<FunctionPolicy> GetByUserAndFunctionKeyIncludeGroup(
            int userId,
            string functionKey,
            string env = Constant.UAT)
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
                    return this._functionRepository.FetchFunctionPolicy(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Check user function policy.
        /// </summary>
        public bool CheckUserFunctionPolicy(int userId, int functionId, int policy)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Duplicate function.
        /// </summary>
        public ICommandResult Duplicate(DuplicateFunctionCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                    if (criteria.IsValid() && logCriteria.IsValid())
                    {
                        return this._functionRepository.Duplicate(ref criteria, logCriteria);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Insert function.
        /// </summary>
        public ICommandResult Insert(InsertFunctionCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                    if (criteria.IsValid() && logCriteria.IsValid())
                    {
                        return this._functionRepository.Insert(ref criteria, logCriteria);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Update function.
        /// </summary>
        public ICommandResult Update(UpdateFunctionCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                    if (criteria.IsValid() && logCriteria.IsValid())
                    {
                        return this._functionRepository.Update(criteria, logCriteria);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Update function status.
        /// </summary>
        public ICommandResult UpdateStatus(UpdateStatusFunctionCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                    if (criteria.IsValid() && logCriteria.IsValid())
                    {
                        return this._functionRepository.Update(criteria, logCriteria);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Assign user permission.
        /// </summary>
        public ICommandResult AssignFunctionToUsers(AssignFunctionToUsersCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._functionRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Assign group permission.
        /// </summary>
        public ICommandResult AssignFunctionToGroups(AssignFunctionToGroupsCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._functionRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Sort function in hierarchy.
        /// </summary>
        private IEnumerable<IFunction> SortFunctionsInHierarchy(IEnumerable<IFunction> functionList, int parentId = 0)
        {
            var sortedFunctionList = new List<IFunction>();

            foreach (var function in functionList)
            {
                // top functions || sub functions
                if ((function.Parent == null && parentId == 0)
                    || (function.Parent != null && function.Parent.Id == parentId))
                {
                    sortedFunctionList.Add(function);
                    sortedFunctionList.AddRange(SortFunctionsInHierarchy(functionList, function.Id));
                }
            }

            return sortedFunctionList;
        }
    }
}