// --------------------------------------------------------------------------
// <copyright file="InsertLogUserLoginCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.11.19">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Trace
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Insert log user login.
    /// </summary>
    public class InsertLogUserLoginCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SiteId { get; set; }

        public string IP { get; set; }

        public DateTime LogDate { get; set; }

        public string Doing { get; set; }

        public string DomainName { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public int Duration { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "NAP_LogUserLogin_Insert";
        }
    }
}
