// --------------------------------------------------------------------------
// <copyright file="IDynamicRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.10.9">Created</change>
// </history>

namespace Fanex.NAP.Framework.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Dynamic repository interface.
    /// </summary>
    public interface IDynamicRepository
    {
        T Get<T>(int id);

        T Get<T>(ICriteria criteria);

        int Count(ICriteria criteria);

        IEnumerable<T> Fetch<T>();

        IEnumerable<T> Fetch<T>(ICriteria criteria);

        IEnumerable<T> Fetch<T>(ICriteria criteria, Action<Orderable<T>> order);

        T Single<T>(dynamic parameters);

        IEnumerable<T> Query<T>(dynamic parameters);

        int Length(dynamic paramaters);
    }
}
