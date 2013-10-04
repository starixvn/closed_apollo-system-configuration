// --------------------------------------------------------------------------
// <copyright file="UserSSOCheckInUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.27">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SSO check in.
    /// </summary>
    public class UserSSOCheckInUserCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SiteId { get; set; }

        public string NetSessionId { get; set; }

        public int CheckOnUserId { get; set; }

        public int OnUserId { get; set; }

        public string IP { get; set; }

        public DateTime LoginDate { get; set; }

        [SpParam(Ignored = true)]
        public string Doing { get; set; }

        [SpParam(Ignored = true)]
        public string Domain { get; set; }

        [SpParam(Ignored = true)]
        public string BrowserName { get; set; }

        [SpParam(Ignored = true)]
        public string Platform { get; set; }

        [SpParam(Ignored = true)]
        public string CountryCode { get; set; }

        [SpParam(Ignored = true)]
        public int Duration { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_SSOCheckIn";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}