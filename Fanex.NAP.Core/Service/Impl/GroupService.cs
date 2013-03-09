// --------------------------------------------------------------------------
// <copyright file="GroupService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.15">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Criteria.Group;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Impl;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;
    using Fanex.NAP.Core.Business;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GroupService : BaseService, IGroupService
    {
        private IGroupRepository _groupRepository;

        public GroupService()
            : this(null)
        {
        }

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? new GroupRepository();
        }

        public IGroup GetByUser(int userId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            try
            {
                var criteria = new GetByUserGroupCriteria
                {
                    UserId = userId
                };

                if (criteria.IsValid())
                {
                    return _groupRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IGroup GetById(int id)
        {
            Check.Argument.IsNotNegativeOrZero(id, "id");
            try
            {
                var criteria = new GetByIdGroupCriteria
                {
                    GroupId = id
                };

                if (criteria.IsValid())
                {
                    return _groupRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IGroup GetByName(string name)
        {
            Check.Argument.IsNotEmpty(name, "name");
            try
            {
                var criteria = new GetByNameGroupCriteria
                {
                    GroupName = name
                };

                if (criteria.IsValid())
                {
                    return _groupRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IGroup> GetAll(bool? isActive = null)
        {
            try
            {
                var criteria = new GetAllGroupCriteria
                {
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return _groupRepository.Fetch(criteria).OrderBy(g => g.Name);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IList<SelectListItem> GetAllSelectListItem(bool? isActive = null, int selectedGroupId = 0)
        {
            try
            {
                var criteria = new GetAllGroupCriteria
                {
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    IEnumerable<IGroup> groupList = _groupRepository.Fetch(criteria).OrderBy(g => g.Name);

                    if (groupList != null)
                    {
                        return groupList.Select(g => new SelectListItem
                            {
                                Text = g.Name,
                                Value = g.Id.ToString(),
                                Selected = g.Id == selectedGroupId
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

        public IEnumerable<IGroup> GetAllWithNumOfUser(string name, string env = Constant.UAT, bool? isActive = null)
        {
            try
            {
                var criteria = new GetAllWithNumOfUserGroupCriteria
                {
                    IsActive = isActive,
                    GroupName = name,
                    Environment = env
                };

                if (criteria.IsValid())
                {
                    return _groupRepository.Fetch(criteria).OrderBy(g => g.Name);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult AssignFunction(AssignFunctionGroupCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _groupRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult UpdateStatus(UpdateStatusGroupCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _groupRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult Insert(InsertGroupCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _groupRepository.Insert(ref criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult Update(UpdateGroupCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _groupRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<GroupPolicy> GetByFunctionAndSite(int functionId, int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(functionId, "functionid");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteid");

            try
            {
                var criteria = new GetByFunctionAndSiteGroupCriteria
                {
                    FunctionId = functionId,
                    SiteId = siteId
                };

                if (criteria.IsValid())
                {
                    return _groupRepository.GetGroupPolicy(criteria).OrderBy(g => g.Group.Name);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IGroup> SearchGroupExcludeFunction(string groupName, int siteId = 0, int functionId = 0)
        {
            try
            {
                var criteria = new SearchGroupExcludeFunctionCriteria
                {
                    GroupName = groupName,
                    SiteId = siteId,
                    FunctionId = functionId
                };

                if (criteria.IsValid())
                {
                    return _groupRepository.Fetch(criteria).OrderBy(g => g.Name);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}