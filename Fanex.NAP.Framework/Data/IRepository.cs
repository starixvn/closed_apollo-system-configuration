// --------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.11">Created</change>
// </history>

namespace Fanex.NAP.Framework.Data
{
    using Fanex.NAP.Framework.Command;

    public interface IRepository<T> : IReadonlyRepository<T>
    {
        ICommandResult Insert(T entity);

        ICommandResult Update(T entity);

        ICommandResult Delete(T entity);
    }
}
