// --------------------------------------------------------------------------
// <copyright file="IExchangeRateRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Repositories.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Admin.Core.Criterias;
    using Fanex.NAP.Admin.Core.Models;

    public interface IExchangeRateRepository
    {
        IEnumerable<ExchangeRateModel> GetAll(GetAllExchangeRateCriteria criteria);

        ExchangeRateModel GetById(GetExchangeRateByIdCriteria criteria);

        void InsertExchangeRate(ref InsertExchangeRateCriteria criteria);

        void UpdateExchangeRate(ref UpdateExchangeRateCriteria criteria);
    }
}
