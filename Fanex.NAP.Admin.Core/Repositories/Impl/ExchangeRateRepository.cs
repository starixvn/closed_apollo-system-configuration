// --------------------------------------------------------------------------
// <copyright file="ExchangeRateRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Repositories.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.NAP.Admin.Core.Criterias;
    using Fanex.NAP.Admin.Core.Models;
    using Fanex.NAP.Admin.Core.Repositories.Interface;

    /// <summary>
    /// ExchangeRate Repository.
    /// </summary>
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        /// <summary>
        /// Function to get all exchange rate.
        /// </summary>
        /// <param name="criteria">GetAllExchangeRate Criteria.</param>
        /// <returns>List exchange rate.</returns>
        public IEnumerable<ExchangeRateModel> GetAll(GetAllExchangeRateCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.Query<ExchangeRateModel>(criteria.GetSpParams());
                    }
                }

                return new List<ExchangeRateModel>();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Function to get by id.
        /// </summary>
        /// <param name="criteria">GetExchangeRateById Criteria.</param>
        /// <returns>ExchangeRate Model.</returns>
        public ExchangeRateModel GetById(GetExchangeRateByIdCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.QueryEntity<ExchangeRateModel>(criteria.GetSpParams());
                    }
                }

                return null;
            }
            catch 
            { 
                throw; 
            }
        }

        /// <summary>
        /// Function to insert exchange rate.
        /// </summary>
        /// <param name="criteria">InsertExchangeRate Criteria.</param>
        public void InsertExchangeRate(ref InsertExchangeRateCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    Hashtable parameters = criteria.GetSpParams();

                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        db.ExecuteNonQuery(parameters);
                    }

                    criteria.ExchangeId = Convert.ToInt32(parameters["exchangeid"]);
                    criteria.ErrCode = Convert.ToInt32(parameters["errcode"]);
                    criteria.ErrMsg = parameters["errmsg"].ToString();
                }
            }
            catch 
            { 
                throw; 
            }
        }

        /// <summary>
        /// Function to update exchange rate.
        /// </summary>
        /// <param name="criteria">UpdateExchangeRate Criteria.</param>
        public void UpdateExchangeRate(ref UpdateExchangeRateCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    Hashtable parameters = criteria.GetSpParams();

                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        db.ExecuteNonQuery(parameters);
                    }

                    criteria.ErrCode = Convert.ToInt32(parameters["errcode"]);
                    criteria.ErrMsg = parameters["errmsg"].ToString();
                }
            }
            catch 
            { 
                throw; 
            }
        }
    }
}
