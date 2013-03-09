// --------------------------------------------------------------------------
// <copyright file="IReadonlyRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.11">Created</change>
// </history>

namespace Fanex.NAP.Framework.Data
{
    using System;
    using System.Collections.Generic;

    public interface IReadonlyRepository<T> 
    {
        T Get(int id);

        T Get(ICriteria criteria);

        int Count(ICriteria criteria);

        IEnumerable<T> Fetch();

        IEnumerable<T> Fetch(ICriteria criteria);
        
        IEnumerable<T> Fetch(ICriteria criteria, Action<Orderable<T>> order);

        T Single(dynamic parameters);

        IEnumerable<T> Query(dynamic parameters);

        int Length(dynamic paramaters);
    }
}
