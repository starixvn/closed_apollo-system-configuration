// --------------------------------------------------------------------------
// <copyright file="UpdateDefaultSiteUrlCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.11">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Site
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update default site URL
    /// </summary>
    public class UpdateDefaultSiteUrlCriteria : CriteriaBase
    {
        public int SiteUrlId { get; set; }

        public int SiteId { get; set; }

        public string Environment { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_Site_UpdateDefaultSiteURL</returns>
        public override string GetSettingKey()
        {
            return "NAP_Site_UpdateDefaultSiteURL";
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