// --------------------------------------------------------------------------
// <copyright file="GetByGroupAndSiteUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get by group and site.
    /// </summary>
    public class GetByGroupAndSiteUserCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public int SiteId { get; set; }

        public bool? IsActive { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_GetByGroupAndSite";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}