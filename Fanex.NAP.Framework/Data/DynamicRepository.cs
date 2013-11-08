// --------------------------------------------------------------------------
// <copyright file="DynamicRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.10.9">Created</change>
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

    /// <summary>
    /// Dynamic repository.
    /// </summary>
    public class DynamicRepository : IDynamicRepository
    {
        public virtual T Get<T>(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get object information.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returned object.</returns>
        public virtual T Get<T>(ICriteria criteria)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    T entity = objectDb.QueryEntity<T>(criteria.GetSpParams());
                    return entity;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Count number of item.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Number of object.</returns>
        public virtual int Count(ICriteria criteria)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    int count = Convert.ToInt32(objectDb.ExecuteScalar(criteria.GetSpParams()));

                    return count;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <returns>List of object.</returns>
        public IEnumerable<T> Fetch<T>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetch object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of object.</returns>
        public virtual IEnumerable<T> Fetch<T>(ICriteria criteria)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    IEnumerable<T> entityList = objectDb.Query<T>(criteria.GetSpParams());

                    return entityList;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <param name="order">Order type.</param>
        /// <returns>List of object.</returns>
        public virtual IEnumerable<T> Fetch<T>(ICriteria criteria, Action<Orderable<T>> order)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    IEnumerable<T> entityList = objectDb.Query<T>(criteria.GetSpParams());

                    if (entityList != null && entityList.Any())
                    {
                        var orderable = new Orderable<T>(entityList);
                    }

                    return entityList;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get single object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="parameters">Dynamic parameters.</param>
        /// <returns>Returned object.</returns>
        public virtual T Single<T>(dynamic parameters)
        {
            try
            {
                var hash = this.ConvertDynamicParamsToHashtable(parameters);
                var settingKey = this.GetSettingKeyFromDynamicParam(parameters);

                using (var objectDb = new ObjectDb(settingKey))
                {
                    var entity = objectDb.QueryEntity<T>(null, hash);
                    return entity;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Query object.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="parameters">Dynamic parameters.</param>
        /// <returns>List of object.</returns>
        public virtual IEnumerable<T> Query<T>(dynamic parameters)
        {
            try
            {
                var hash = this.ConvertDynamicParamsToHashtable(parameters);
                var settingKey = this.GetSettingKeyFromDynamicParam(parameters);

                using (var objectDb = new ObjectDb(settingKey))
                {
                    var entityList = objectDb.Query<T>(null, hash);
                    return entityList;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get number of records.
        /// </summary>
        /// <param name="parameters">Dynamic parameters.</param>
        /// <returns>Number of records.</returns>
        public virtual int Length(dynamic parameters)
        {
            try
            {
                var hash = this.ConvertDynamicParamsToHashtable(parameters);
                var settingKey = this.GetSettingKeyFromDynamicParam(parameters);

                using (var objectDb = new ObjectDb(settingKey))
                {
                    return Convert.ToInt32(objectDb.ExecuteScalar(hash));
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Check setting key whether exist in the parameter collection or not.
        /// </summary>
        /// <param name="parameters">Input parameters.</param>
        protected void CheckSettingKeyExist(dynamic parameters)
        {
            Check.Argument.IsNotNull(parameters, "parameters");

            Type type = parameters.GetType();
            var propInfo = type.GetProperty("SettingKey");

            if (propInfo == null)
            {
                throw new NullReferenceException("Input paramater does not contain the expected property SettingKey");
            }
        }

        /// <summary>
        /// Convert dynamic parameter to hash table.
        /// </summary>
        /// <param name="parameters">Dynamic parameters.</param>
        /// <returns>Returned hash table.</returns>
        protected Hashtable ConvertDynamicParamsToHashtable(dynamic parameters)
        {
            this.CheckSettingKeyExist(parameters);

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

        /// <summary>
        /// Add dynamic parameter to hash table.
        /// </summary>
        /// <param name="parameters">Dynamic parameters.</param>
        /// <returns>Returned hash table.</returns>
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

        /// <summary>
        /// Get setting key from dynamic parameter.
        /// </summary>
        /// <param name="parameters">Dynamic parameters.</param>
        /// <returns>Setting key.</returns>
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
    }
}
