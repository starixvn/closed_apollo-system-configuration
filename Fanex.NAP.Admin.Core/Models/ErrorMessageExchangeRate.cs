// --------------------------------------------------------------------------
// <copyright file="ErrorMessageExchangeRate.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.8.22">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Models
{
    /// <summary>
    /// ErrorMessage ExchangeRate.
    /// </summary>
    public class ErrorMessageExchangeRate
    {
        public int ErrorCode { get; set; }

        public string MsgCurrency { get; set; }

        public string MsgMultipliedBy { get; set; }

        public string MsgActualRate { get; set; }

        public string MsgMinBet { get; set; }

        public string MsgMinBetLive { get; set; }
    }
}
