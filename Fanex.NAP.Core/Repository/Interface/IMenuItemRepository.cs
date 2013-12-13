// --------------------------------------------------------------------------
// <copyright file="IMenuItemRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.1">Created</change>
// </history>

namespace Fanex.NAP.Core.Repository.Interface
{
    using Fanex.NAP.Core.Criteria.MenuItem;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Framework.Data;

    public interface IMenuItemRepository : IRepository<IMenuItem>
    {
        ICommandResult Insert(ref InsertMenuItemCriteria criteria, ICriteria logCriteria);

        ICommandResult Update(ICriteria criteria, ICriteria logCriteria);

        ICommandResult Delete(ICriteria criteria, ICriteria logCriteria);
    }
}