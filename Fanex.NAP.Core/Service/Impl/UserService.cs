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

    /// <summary>
    /// User service.
    /// </summary>
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
            this._userRepository = repository ?? new UserRepository();
            _traceService = traceService ?? new TraceService();
        }

        /// <summary>
        /// Get user by ID.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>The user object.</returns>
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
                    return this._userRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The user object.</returns>
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
                    return this._userRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Check if username exist.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>True : exist, false : not.</returns>
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
                    return this._userRepository.CheckUsernameExist(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        /// <summary>
        /// Get all user.
        /// </summary>
        /// <returns>The list of user object.</returns>
        public IEnumerable<IUser> GetAll()
        {
            try
            {
                var criteria = new GetAllUserCriteria();

                if (criteria.IsValid())
                {
                    return this._userRepository.Fetch(criteria).OrderBy(u => u.Username);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get user by site.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>The list of user object.</returns>
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
                    return this._userRepository.Fetch(criteria).OrderBy(u => u.Username);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get user by group and site.
        /// </summary>
        /// <param name="groupId">The group ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>The list of user object.</returns>
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
                    return this._userRepository.Fetch(criteria).OrderBy(u => u.Username);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Search user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="groupId">The group ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>List of user level.</returns>
        public IEnumerable<UserLevel> SearchUser(string username, int groupId = 0, int siteId = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get user level by username group and site.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalRows">Total number of records.</param>
        /// <param name="username">The username.</param>
        /// <param name="groupId">The group ID.</param>
        /// <param name="listSite">List of site select list item.</param>
        /// <returns>List of user level.</returns>
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
                    userList = this._userRepository.Search(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        /// <summary>
        /// Search user.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalRows">Total number of records.</param>
        /// <param name="username">The username.</param>
        /// <param name="groupId">The group ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>List of user level.</returns>
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
                    userList = this._userRepository.Search(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        /// <summary>
        /// Search user exclude function.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalRows">Total number of records.</param>
        /// <param name="username">The username.</param>
        /// <param name="groupId">The group ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>List of user level.</returns>
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
                    userList = this._userRepository.Search(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        /// <summary>
        /// Search user exclude function.
        /// </summary>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalRows">Total number of records.</param>
        /// <param name="username">The username.</param>
        /// <param name="groupId">The group ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="functionId">The function ID.</param>
        /// <returns>List of user level.</returns>
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
                    userList = this._userRepository.Search(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        /// <summary>
        /// Get user site level.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>User site level.</returns>
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
                    return this._userRepository.Count(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return -1;
        }

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="user">The user object.</param>
        /// <returns>The command result.</returns>
        public ICommandResult ResetPassword(IUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert user.
        /// </summary>
        /// <param name="user">The user object.</param>
        /// <returns>The command result.</returns>
        public ICommandResult Insert(IUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public ICommandResult Insert(InsertCustCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._userRepository.Insert(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="email">The email address.</param>
        /// <param name="displayName">The display name.</param>
        /// <returns>The command result.</returns>
        public ICommandResult Update(int userId, string email, string displayName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="user">The user object.</param>
        /// <returns>The command result.</returns>
        public ICommandResult Update(IUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Change user site status.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>The command result.</returns>
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
                    return this._userRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Assign site to user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public ICommandResult AssignSite(AssignSiteUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._userRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Assign permission to user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public ICommandResult AssignFunction(AssignFunctionUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._userRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public ICommandResult ChangePassword(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Login on user.
        /// </summary>
        /// <param name="netSessionId">The net session ID.</param>
        /// <param name="serverId">The server ID.</param>
        /// <returns>The command result.</returns>
        public ICommandResult LoginOnUser(string netSessionId, int serverId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Log out user.
        /// </summary>
        /// <returns>The command result.</returns>
        public ICommandResult Logout()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Re-login user.
        /// </summary>
        /// <returns>The command result.</returns>
        public ICommandResult ReLogin()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write log user login.
        /// </summary>
        /// <param name="loginIp">Login IP address.</param>
        /// <param name="domainName">Domain name.</param>
        /// <param name="logData">Login log data.</param>
        /// <param name="browserName">Browser name.</param>
        /// <param name="platform">Platform name.</param>
        /// <param name="countryCode">Country code.</param>
        /// <param name="duration">Log duration.</param>
        /// <returns>The command result.</returns>
        public ICommandResult WriteLogUserLogin(
            string loginIp, 
            string domainName, 
            string logData, 
            string browserName, 
            string platform, 
            string countryCode, 
            int duration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check in user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void UserCheckIn(ref UserCheckInUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    this._userRepository.UserCheckIn(ref criteria);

                    _traceService.InsertLogUserLogin(
                        criteria.UserId,
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

        /// <summary>
        /// Re-check in user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void ReUserCheckIn(ref UserReCheckInUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    this._userRepository.UserReCheckIn(ref criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Log out user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void UserLogOut(LogOutUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    this._userRepository.UserLogOut(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// SSO check in user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void UserSSOCheckIn(ref UserSSOCheckInUserCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    this._userRepository.UserSSOCheckIn(ref criteria);

                    _traceService.InsertLogUserLogin(
                        criteria.UserId,
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

        /// <summary>
        /// Get user function policy.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="functionId">The function ID.</param>
        /// <returns>User function policy.</returns>
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
                    return this._userRepository.Count(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return -1;
        }

        /// <summary>
        /// Get user function policy by site and function key.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="functionKey">The function key.</param>
        /// <returns>User site function key policy.</returns>
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
                    return this._userRepository.Count(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return -1;
        }

        /// <summary>
        /// Search user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="totalRows">Total number of records.</param>
        /// <returns>The list of user object.</returns>
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
                    userList = this._userRepository.Fetch(criteria, out totalRows);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return userList;
        }

        /// <summary>
        /// Insert or update user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The command result.</returns>
        public ICommandResult InsertOrUpdate(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                if (criteria.IsValid() && logCriteria.IsValid())
                {
                    return this._userRepository.Update(criteria, logCriteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get user policy by function and site.
        /// </summary>
        /// <param name="functionId">The function ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <returns>List of user policy.</returns>
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
                    return this._userRepository.GetUserPolicy(criteria).OrderBy(u => u.User.Username);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Get user list site function key policy.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="functionKey">The function key.</param>
        /// <param name="listSite">List of site select list item.</param>
        public Hashtable GetUserListSiteFunctionKeyPolicy(
            int userId, 
            string functionKey, 
            List<SelectListItem> listSite = null)
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
                    return this._userRepository.GetUserListSiteFunctionKeyPolicy(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        /// <summary>
        /// Check if have permission user site function key.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="functionKey">The function key.</param>
        /// <param name="policy">The function policy.</param>
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

        /// <summary>
        /// Get user SSO time.
        /// </summary>
        public string UserSSOGetTime()
        {
            try
            {
                var criteria = new UserSSOGetTimeUserCriteria
                {
                    Time = DateTime.Now
                };

                return this._userRepository.UserSSOGetTime(criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return DateTime.Now.ToString();
        }
    }
}