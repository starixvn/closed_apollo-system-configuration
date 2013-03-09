// --------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IUserService : IBaseService
    {
        IUser GetById(int id);

        IUser GetByUsername(string username);

        bool CheckUsernameExist(string username);

        IEnumerable<IUser> GetAll();

        IEnumerable<UserPolicy> GetByFunctionAndSite(int functionId, int siteId);

        IEnumerable<IUser> GetBySite(int siteId = 0, bool? isActive = null);

        IEnumerable<IUser> GetByGroupAndSite(int groupId = 0, int siteId = 0, bool? isActive = null);

        IEnumerable<UserLevel> SearchUser(string username, int groupId = 0, int siteId = 0);

        IQueryable<UserLevel> GetAllByUsernameGroupSite(
           int pageIndex,
           int pageSize,
           out int totalRows,
           string username,
           int groupId = 0,
           List<SelectListItem> listSite = null);

        IQueryable<UserLevel> SearchUser(
            int pageIndex,
            int pageSize,
            out int totalRows,
            string username,
            int groupId = 0,
            int siteId = 0);

        /// <summary>
        /// Searches users by username, group, site, but excludes ones who was assigned
        /// the function (functionId) already
        /// </summary>
        IQueryable<UserLevel> SearchUserExcludeFunction(
            int pageIndex,
            int pageSize,
            out int totalRows,
            string username,
            int groupId = 0,
            int siteId = 0,
            int functionId = 0);

        IQueryable<IUser> SearchByUsername(string username, int pageIndex, int pageSize, out int totalRows);

        int GetUserSiteLevel(int userId, int siteId);

        int GetUserFunctionPolicy(int userId, int functionId);

        int GetUserSiteFunctionKeyPolicy(int userId, int siteId, string functionKey);

        bool HasPermissionUserSiteFunctionKey(int userId, int siteId, string functionKey, Constant.Policy policy);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="functionKey">Function key</param>
        /// <param name="listSite">List of site</param>
        /// <returns>key is site id and value is policy is host</returns>
        Hashtable GetUserListSiteFunctionKeyPolicy(int userId, string functionKey, List<SelectListItem> listSite = null);

        ICommandResult ResetPassword(IUser user);

        ICommandResult Insert(IUser user);

        ICommandResult Insert(InsertCustCriteria criteria);

        ICommandResult Update(int userId, string email, string displayName);

        ICommandResult Update(IUser user);

        ICommandResult ChangeUserSiteStatus(int userId, int siteId, bool isActive);

        /// <summary>
        /// Modify user site
        /// </summary>
        /// <param name="site">UserId and List(SiteLevel)</param>
        /// <returns></returns>
        ICommandResult AssignSite(AssignSiteUserCriteria site);

        /// <summary>
        /// Modify user function
        /// </summary>
        /// <param name="permision">UserId and List(FunctionPolicy)</param>
        /// <returns>Result</returns>
        ICommandResult AssignFunction(AssignFunctionUserCriteria function);

        ICommandResult ChangePassword(ICriteria user);

        ICommandResult LoginOnUser(string netSessionId, int serverId);

        ICommandResult Logout();

        ICommandResult ReLogin();

        ICommandResult WriteLogUserLogin(
            string loginIp,
            string domainName,
            string logData,
            string browserName,
            string platform,
            string countryCode,
            int duration);

        void UserCheckIn(ref UserCheckInUserCriteria criteria);

        void ReUserCheckIn(ref UserReCheckInUserCriteria criteria);

        void UserLogOut(LogOutUserCriteria criteria);

        void UserSSOCheckIn(ref UserSSOCheckInUserCriteria criteria);

        ICommandResult InsertOrUpdate(ICriteria criteria);

        string UserSSOGetTime();
    }
}
