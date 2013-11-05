// --------------------------------------------------------------------------
// <copyright file="GetExchangeRateByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetExchangeRateById Criteria.
    /// </summary>
    public class GetExchangeRateByIdCriteria : CriteriaBase
    {
        public int ExchangeId { get; set; }

        public override bool IsValid()
        {
            return ExchangeId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_Exchange_GetById";
        }
    }
}