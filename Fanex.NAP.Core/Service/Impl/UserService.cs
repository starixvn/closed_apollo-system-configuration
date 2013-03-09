// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Impl;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITraceService _traceService;

        public UserService()
            : this(null, null)
        {
        }

        public UserService(IUserRepository repository = null, ITraceService traceService = null)
        {
            _userRepository = repository ?? new UserRepository();
            _traceService = traceService ?? new TraceService();
        }

        public IUser GetById(int id)
        {
            Check.Argument.IsNotNegativeOrZero(id, "id");

            try
            {
                var criteria = new GetByIdUserCriteria
                {
                    UserId = id
                };

                if (criteria.IsValid())
                {
                    return _userRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IUser GetByUsername(string username)
        {
            Check.Argument.IsNotEmpty(username, "username");

            try
            {
                var criteria = new GetByUsernameUserCriteria
                {
                    Username = username
                };

                if (criteria.IsValid())
                {
                    return _userRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public bool CheckUsernameExist(string username)
        {
            Check.Argument.IsNotEmpty(username, "username");

            try
            {
                var criteria = new CheckUsernameExistUserCriteria
                {
                    Username = username
                };

                if (criteria.IsValid())
                {
                    return _userRepository.CheckUsernameExist(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        public IEnumerable<IUser> GetAll()
        {
            try
            {
                var criteria = new GetAllUserCriteria();

                if (criteria.IsValid())
                {
                    return _userRepository.Fetch(criteria).OrderBy(u => u.Username);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IUser> GetBySite(int siteId = 0, bool? isActive = null)
        {
            try
            {
                var criteria = new GetBySiteUserCriteria
                {
                    SiteId = siteId,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return _userRepository.Fetch(criteria).OrderBy(u => u.Username);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IUser> GetByGroupAndSite(int groupId = 0, int siteId = 0, bool? isActive = null)
        {
            try
            {
                var criteria = new GetByGroupAndSiteUserCriteria
                {
                    GroupId = groupId,
                    SiteId = siteId,
                    IsActive = isActive
                };

                if (criteria.IsValid())
                {
                    return _userRepository.Fetch(criteria).OrderBy(u => u.Username);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<UserLevel> SearchUser(string username, int groupId = 0, int siteId = 0)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UserLevel> GetAllByUsernameGroupSite(
            int pageIndex,
            int pageSize,
            out int totalRows,
            string username,
            int groupId = 0,
            List<SelectListItem> listSite = null)
        {
            IQueryable<UserLevel> userList = null;
            totalRows = 0;

            try
            {
                var criteria = new GetAllByUsernameGroupSiteUserCriteria
                {
                    Username = username,
                    GroupId = groupId,
                    ListSite = listSite,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                if (criteria.IsValid())
                {
                    userList = _userRepository.Search(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        public IQueryable<UserLevel> SearchUser(
            int pageIndex,
            int pageSize,
            out int totalRows,
            string username,
            int groupId = 0,
            int siteId = 0)
        {
            IQueryable<UserLevel> userList = null;
            totalRows = 0;

            try
            {
                var criteria = new SearchUserCriteria
                {
                    Username = username,
                    GroupId = groupId,
                    SiteId = siteId,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                if (criteria.IsValid())
                {
                    userList = _userRepository.Search(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        public IQueryable<UserLevel> SearchUserExcludeFunction(
            int pageIndex,
            int pageSize,
            out int totalRows,
            string username,
            int groupId = 0,
            int siteId = 0)
        {
            IQueryable<UserLevel> userList = null;
            totalRows = 0;

            try
            {
                var criteria = new SearchUserCriteria
                {
                    Username = username,
                    GroupId = groupId,
                    SiteId = siteId,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                if (criteria.IsValid())
                {
                    userList = _userRepository.Search(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        public IQueryable<UserLevel> SearchUserExcludeFunction(
            int pageIndex,
            int pageSize,
            out int totalRows,
            string username,
            int groupId = 0,
            int siteId = 0,
            int functionId = 0)
        {
            IQueryable<UserLevel> userList = null;
            totalRows = 0;

            try
            {
                var criteria = new SearchUserExcludeFunctionCriteria
                {
                    Username = username,
                    GroupId = groupId,
                    SiteId = siteId,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    FunctionId = functionId
                };

                if (criteria.IsValid())
                {
                    userList = _userRepository.Search(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        public int GetUserSiteLevel(int userId, int siteId)
        {
            try
            {
                var criteria = new GetUserSiteLevelUserCriteria
                {
                    UserId = userId,
                    SiteId = siteId
                };

                if (criteria.IsValid())
                {
                    return _userRepository.Count(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return -1;
        }

        public ICommandResult ResetPassword(IUser user)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(IUser user)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(InsertCustCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _userRepository.Insert(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult Update(int userId, string email, string displayName)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(IUser user)
        {
            throw new NotImplementedException();
        }

        public ICommandResult ChangeUserSiteStatus(int userId, int siteId, bool isActive)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteId");
            Check.Argument.IsNotNull(isActive, "isActive");

            try
            {
                var criteria = new ChangeUserSiteStatusUserCriteria
                {
                    UserId = userId,
                    SiteId = siteId,
                    IsActive = isActive
                };

                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _userRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult AssignSite(AssignSiteUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _userRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult AssignFunction(AssignFunctionUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _userRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult ChangePassword(ICriteria user)
        {
            throw new NotImplementedException();
        }

        public ICommandResult LoginOnUser(string netSessionId, int serverId)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Logout()
        {
            throw new NotImplementedException();
        }

        public ICommandResult ReLogin()
        {
            throw new NotImplementedException();
        }

        public ICommandResult WriteLogUserLogin(string loginIp, string domainName, string logData, string browserName, string platform, string countryCode, int duration)
        {
            throw new NotImplementedException();
        }

        public void UserCheckIn(ref UserCheckInUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    _userRepository.UserCheckIn(ref criteria);

                    _traceService.InsertLogUserLogin(criteria.UserId,
                        criteria.SiteId,
                        criteria.Ip,
                        criteria.LoginDate,
                        criteria.Domain,
                        criteria.BrowserName,
                        criteria.Platform,
                        criteria.CountryCode,
                        criteria.Doing,
                        criteria.Duration);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void ReUserCheckIn(ref UserReCheckInUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    _userRepository.UserReCheckIn(ref criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void UserLogOut(LogOutUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    _userRepository.UserLogOut(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public void UserSSOCheckIn(ref UserSSOCheckInUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    _userRepository.UserSSOCheckIn(ref criteria);

                    _traceService.InsertLogUserLogin(criteria.UserId,
                       criteria.SiteId,
                       criteria.IP,
                       criteria.LoginDate,
                       criteria.Domain,
                       criteria.BrowserName,
                       criteria.Platform,
                       criteria.CountryCode,
                       criteria.Doing,
                       criteria.Duration);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public int GetUserFunctionPolicy(int userId, int functionId)
        {
            try
            {
                var criteria = new GetUserFunctionPolicyUserCriteria
                {
                    UserId = userId,
                    FunctionId = functionId
                };

                if (criteria.IsValid())
                {
                    return _userRepository.Count(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return -1;
        }

        public int GetUserSiteFunctionKeyPolicy(int userId, int siteId, string functionKey)
        {
            try
            {
                var criteria = new GetUserSiteFunctionKeyPolicyUserCriteria
                {
                    UserId = userId,
                    SiteId = siteId,
                    FunctionKey = functionKey
                };

                if (criteria.IsValid())
                {
                    return _userRepository.Count(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return -1;
        }

        public IQueryable<IUser> SearchByUsername(string username, int pageIndex, int pageSize, out int totalRows)
        {
            IQueryable<IUser> userList = null;
            totalRows = 0;

            try
            {
                var criteria = new SearchByUsernameUserCriteria
                {
                    Username = username,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                if (criteria.IsValid())
                {
                    userList = _userRepository.Fetch(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        public ICommandResult InsertOrUpdate(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return _userRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<UserPolicy> GetByFunctionAndSite(int functionId, int siteId)
        {
            Check.Argument.IsNotNegativeOrZero(functionId, "functionid");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteid");

            try
            {
                var criteria = new GetByFunctionAndSiteUserCriteria
                {
                    FunctionId = functionId,
                    SiteId = siteId
                };

                if (criteria.IsValid())
                {
                    return _userRepository.GetUserPolicy(criteria).OrderBy(u => u.User.Username);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public Hashtable GetUserListSiteFunctionKeyPolicy(int userId, string functionKey, List<SelectListItem> listSite = null)
        {
            try
            {
                var criteria = new GetUserListSiteFunctionKeyPolicyUserCriteria
                {
                    UserId = userId,
                    ListSite = listSite,
                    FunctionKey = functionKey
                };

                if (criteria.IsValid())
                {
                    return _userRepository.GetUserListSiteFunctionKeyPolicy(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public bool HasPermissionUserSiteFunctionKey(int userId, int siteId, string functionKey, Constant.Policy policy)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            Check.Argument.IsNotNegativeOrZero(siteId, "siteid");
            Check.Argument.IsNotEmpty(functionKey, "functionKey");

            try
            {
                int tempPolicy = this.GetUserSiteFunctionKeyPolicy(userId, siteId, functionKey);
                if (tempPolicy >= (int)policy)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        public string UserSSOGetTime()
        {
            try
            {
                var criteria = new UserSSOGetTimeUserCriteria
                {
                    Time = DateTime.Now
                };

                return _userRepository.UserSSOGetTime(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return DateTime.Now.ToString();
        }
    }
}