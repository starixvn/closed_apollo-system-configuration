// --------------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
//   <change who="Edward.Nguyen" date="2012.8.14">Add GetRepositoryExecLog</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.Logging;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Base service.
    /// </summary>
    public class BaseService : IBaseService
    {
        private readonly IDynamicRepository _dynamicRepository;

        public BaseService(IDynamicRepository dynamicRepository = null)
        {
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }
        
        public IServiceContext ServiceContext { get; set; }

        /// <summary>
        /// Gets the list data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list data.</returns>
        public virtual IEnumerable<T> GetData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return _dynamicRepository.Fetch<T>(criteria);
            }
            
            return new List<T>();
        }

        /// <summary>
        /// Gets the list of data and total items.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="totalItems">Total items.</param>
        /// <returns>The list of data.</returns>
        public virtual IEnumerable<T> GetData<T>(ICriteria criteria, out int totalItems)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            
            IEnumerable<T> result = null;
            totalItems = 0;

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                result = db.Query<T>(criteria.GetSpParams());

                Hashtable outputParam = db.GetOuputParams();

                if (outputParam.ContainsKey("totalitems"))
                {
                    totalItems = (int)outputParam["totalitems"];
                }
            }

            result = result ?? new List<T>();

            return result;
        }

        /// <summary>
        /// Gets item that are satisfied the passed criteria.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public virtual T GetItem<T>(ICriteria criteria) where T : new()
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return _dynamicRepository.Get<T>(criteria);
            }

            return new T();
        }

        /// <summary>
        /// Handle exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void HandleException(Exception ex)
        {
            LogService.Error(ex.Message, ex);
            throw ex;
        }

        /// <summary>
        /// Get execute store procedure log criteria.
        /// </summary>
        /// <returns>Execute SP log criteria.</returns>
        public LogExecSpCriteria GetLogExecSpCriteria()
        {
            Check.Argument.IsNotNull(this.ServiceContext, "ServiceContext");

            return new LogExecSpCriteria
            {
                CurrentUserId = this.ServiceContext.UserId,
                CurrentSiteId = this.ServiceContext.SiteId,
                LogDate = DateTime.Now,
                LoginIp = this.ServiceContext.Ip
            };
        }
    }
}