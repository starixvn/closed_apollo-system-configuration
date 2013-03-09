// --------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
// </history>

namespace Fanex.NAP.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Repository;
    using Fanex.NAP.Framework.Data;
    using Fanex.NAP.Core;
    using Fanex.NAP.Core.Repository;

    public class UserRepository : IUserRepository
    {
        public ICommandResult Insert(IUser entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(IUser entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Delete(IUser entity)
        {
            throw new NotImplementedException();
        }

        public IUser Get(int id)
        {
            throw new NotImplementedException();
        }

        public IUser Get(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public int Count(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUser> Fetch()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUser> Fetch(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IUser> Fetch(ICriteria criteria, Action<Orderable<IUser>> order)
        {
            throw new NotImplementedException();
        }
    }
}
