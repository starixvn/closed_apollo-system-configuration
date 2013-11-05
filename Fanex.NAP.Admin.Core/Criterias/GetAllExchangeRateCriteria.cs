// --------------------------------------------------------------------------
// <copyright file="GetAllExchangeRateCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// GetAllExchangeRate Criteria.
    /// </summary>
    public class GetAllExchangeRateCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Age_Exchange_GetAll";
        }
    }
}