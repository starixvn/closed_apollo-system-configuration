// --------------------------------------------------------------------------
// <copyright file="AssignSiteUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Assign site.
    /// </summary>
    public class AssignSiteUserCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets user site information.
        /// </summary>
        /// <value>
        /// <ROOT><SiteLevel SiteId="4" Level="3" ></SiteLevel></ROOT>.
        /// </value>
        public string Value { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_AssignSite";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}