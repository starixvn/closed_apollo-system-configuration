// --------------------------------------------------------------------------
// <copyright file="IGroupRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.1">Created</change>
// </history>

namespace Fanex.NAP.Core.Repository.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Criteria.Group;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Group repository interface.
    /// </summary>
    public interface IGroupRepository : IRepository<IGroup>
    {
        ICommandResult Insert(ref InsertGroupCriteria criteria, ICriteria logCriteria);

        ICommandResult Update(ICriteria criteria, ICriteria logCriteria);

        ICommandResult Delete(ICriteria criteria, ICriteria logCriteria);

        IEnumerable<GroupPolicy> GetGroupPolicy(ICriteria criteria);
    }
}