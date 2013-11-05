// --------------------------------------------------------------------------
// <copyright file="ExchangeRateModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Models
{
    /// <summary>
    /// ExchangeRate Model.
    /// </summary>
    public class ExchangeRateModel
    {
        public int ExchangeId { get; set; }

        public string Currency { get; set; }

        public int MultipliedBy { get; set; }

        public double? ActualRate { get; set; }

        public double? MinBet { get; set; }

        public double? MinBetLive { get; set; }

        public string Description { get; set; }
    }
}
