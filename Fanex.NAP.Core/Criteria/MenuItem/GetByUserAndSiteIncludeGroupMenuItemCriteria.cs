// --------------------------------------------------------------------------
// <copyright file="GetByUserAndSiteIncludeGroupMenuItemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MenuItem
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get by user and site
    /// </summary>
    public class GetByUserAndSiteIncludeGroupMenuItemCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SiteId { get; set; }

        public string Environment { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_MenuItem_GetByUserAndSiteIncludeGroup</returns>
        public override string GetSettingKey()
        {
            return "NAP_MenuItem_GetByUserAndSiteIncludeGroup";
        }

        /// <summary>
        /// Check data's constrains
        /// </summary>
        /// <returns>True: valid; False: invalid</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
