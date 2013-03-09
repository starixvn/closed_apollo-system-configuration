// --------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.1">Created</change>
// </history>

namespace Fanex.NAP.Core.Repository.Interface
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IUserRepository : IRepository<IUser>
    {
        bool CheckUsernameExist(ICriteria criteria);

        IQueryable<IUser> Fetch(ICriteria criteria, out int totalRows);

        IEnumerable<UserLevel> Search(ICriteria criteria);

        IQueryable<UserLevel> Search(ICriteria criteria, out int totalRows);

        IEnumerable<UserPolicy> GetUserPolicy(ICriteria criteria);

        ICommandResult Insert(InsertCustCriteria criteria, ICriteria logCriteria);

        ICommandResult Update(ICriteria criteria, ICriteria logCriteria);

        void UserCheckIn(ref UserCheckInUserCriteria criteria);

        void UserSSOCheckIn(ref UserSSOCheckInUserCriteria criteria);

        void UserReCheckIn(ref UserReCheckInUserCriteria criteria);

        void UserLogOut(LogOutUserCriteria criteria);

        Hashtable GetUserListSiteFunctionKeyPolicy(ICriteria criteria);

        string UserSSOGetTime(ICriteria criteria);
    }
}