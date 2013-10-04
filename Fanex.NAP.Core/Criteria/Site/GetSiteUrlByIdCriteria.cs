// --------------------------------------------------------------------------
// <copyright file="GetSiteUrlByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.8">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Site
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get site URL by ID.
    /// </summary>
    public class GetSiteUrlByIdCriteria : CriteriaBase
    {
        public int SiteUrlId { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_Site_GetSiteURLById.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Site_GetSiteURLById";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}