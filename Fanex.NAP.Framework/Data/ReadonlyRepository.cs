// --------------------------------------------------------------------------
// <copyright file="ReadonlyRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.18">Created</change>
//   <change who="Anders.Le" date="2012.8.13">Add Single, Query, Length method</change>
//   <change who="Anders.Le" date="2012.10.9">Re-Implement  by inherit from DynamicRepository</change>
// </history>

namespace Fanex.NAP.Framework.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Fanex.Common.Helper;
    using Fanex.Data;

    public class ReadonlyRepository<T> : DynamicRepository, IReadonlyRepository<T>
    {
        public virtual T Get(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get single record set and map to data transfer object
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>object of T</returns>
        public virtual T Get(ICriteria criteria)
        {
            return base.Get<T>(criteria);
        }

        /// <summary>
        /// Get total element matching the criteria
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>value of integer</returns>
        public override int Count(ICriteria criteria)
        {
            return base.Count(criteria);
        }

        public virtual IEnumerable<T> Fetch()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable of T</returns>
        public virtual IEnumerable<T> Fetch(ICriteria criteria)
        {
            return base.Fetch<T>(criteria);
        }        

        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="order">The order.</param>
        /// <returns>IEnumerable of T</returns>
        public virtual IEnumerable<T> Fetch(ICriteria criteria, Action<Orderable<T>> order)
        {
            return base.Fetch<T>(criteria, order);
        }
                
        public virtual T Single(dynamic parameters)
        {
            return base.Single<T>((object)parameters);
        }

        public virtual IEnumerable<T> Query(dynamic parameters)
        {
            return base.Query<T>((object)parameters);
        }

        public override int Length(dynamic parameters)
        {
            return base.Length((object)parameters);
        }

        /*
        /// <summary>
        /// Check setting key whether exist in the paramater or not
        /// </summary>
        /// <param name="parameters"></param>
        protected void CheckSettingKeyExist(dynamic parameters)
        {
            Check.Argument.IsNotNull(parameters, "parameters");

            Type type = parameters.GetType();
            var propInfo = type.GetProperty("SettingKey");

            if (propInfo == null)
                throw new NullReferenceException("Input paramater does not contain the expected property SettingKey");
        }

        protected Hashtable ConvertDynamicParamsToHashtable(dynamic parameters)
        {
            CheckSettingKeyExist(parameters);

            Type type = parameters.GetType();
            var hash = new Hashtable();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (!propertyInfo.Name.Equals("SettingKey"))
                {
                    hash.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(parameters, null));
                }
            }

            return hash;
        }

        protected Hashtable AddDynamicParamsToHashtable(Hashtable hash, dynamic parameters)
        {
            Type type = parameters.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (!propertyInfo.Name.Equals("SettingKey"))
                {
                    hash.Add(propertyInfo.Name.ToLower(), propertyInfo.GetValue(parameters, null));
                }
            }

            return hash;
        }

        protected string GetSettingKeyFromDynamicParam(dynamic parameters)
        {
            Type type = parameters.GetType();

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.Name.Equals("SettingKey"))
                {
                    return propertyInfo.GetValue(parameters, null);
                }
            }

            return string.Empty;
        }
        */
    }
}
