// --------------------------------------------------------------------------
// <copyright file="InsertExchangeRateCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// InsertExchangeRate Criteria.
    /// </summary>
    public class InsertExchangeRateCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string IP { get; set; }

        public int ExchangeId { get; set; }

        public string Currency { get; set; }

        public string Description { get; set; }

        public double MultipliedBy { get; set; }

        public double ActualRate { get; set; }

        public double MinBet { get; set; }

        public double MinBetLive { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
        {
            var isValid = !string.IsNullOrWhiteSpace(Currency) && ActualRate >= 0 && MinBet >= 0 && MinBetLive >= 0 && MultipliedBy > 0;
            return isValid;
        }

        public override string GetSettingKey()
        {
            return "Age_Exchange_InsertRate";
        }
    }
}