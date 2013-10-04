// --------------------------------------------------------------------------
// <copyright file="Orderable.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// <remarks>ref to Orchard</remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Orderable<T>
    {
        private IEnumerable<T> _queryable;

        public Orderable(IEnumerable<T> enumerable)
        {
            this._queryable = enumerable;
        }

        public IEnumerable<T> Queryable
        {
            get { return this._queryable; }
        }

        public Orderable<T> Asc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            this._queryable = this._queryable
                .AsQueryable<T>()
                .OrderBy(keySelector);
            return this;
        }

        public Orderable<T> Asc<TKey1, TKey2>(
            Expression<Func<T, TKey1>> keySelector1,
            Expression<Func<T, TKey2>> keySelector2)
        {
            this._queryable = this._queryable
                .AsQueryable<T>()
                .OrderBy(keySelector1)
                .OrderBy(keySelector2);
            return this;
        }

        public Orderable<T> Asc<TKey1, TKey2, TKey3>(
            Expression<Func<T, TKey1>> keySelector1,
            Expression<Func<T, TKey2>> keySelector2,
            Expression<Func<T, TKey3>> keySelector3)
        {
            this._queryable = this._queryable
                .AsQueryable<T>()
                .OrderBy(keySelector1)
                .OrderBy(keySelector2)
                .OrderBy(keySelector3);
            return this;
        }

        public Orderable<T> Desc<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            this._queryable = this._queryable
                .AsQueryable<T>()
                .OrderByDescending(keySelector);
            return this;
        }

        public Orderable<T> Desc<TKey1, TKey2>(
            Expression<Func<T, TKey1>> keySelector1,
            Expression<Func<T, TKey2>> keySelector2)
        {
            this._queryable = this._queryable
                .AsQueryable<T>()
                .OrderByDescending(keySelector1)
                .OrderByDescending(keySelector2);
            return this;
        }

        public Orderable<T> Desc<TKey1, TKey2, TKey3>(
            Expression<Func<T, TKey1>> keySelector1,
            Expression<Func<T, TKey2>> keySelector2,
            Expression<Func<T, TKey3>> keySelector3)
        {
            this._queryable = this._queryable
                .AsQueryable<T>()
                .OrderByDescending(keySelector1)
                .OrderByDescending(keySelector2)
                .OrderByDescending(keySelector3);
            return this;
        }
    }
}
