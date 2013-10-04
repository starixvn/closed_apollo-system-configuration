// --------------------------------------------------------------------------
// <copyright file="GetBySiteMenuItemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MenuItem
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get by site
    /// </summary>
    public class GetBySiteMenuItemCriteria : CriteriaBase
    {
        [Required]
        [Display(Name = "Site", ResourceType = typeof(ViewResources))]
        public int SiteId { get; set; }

        [Display(Name = "Environment", ResourceType = typeof(ViewResources))]
        public string Environment { get; set; }

        public bool? IsActive { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_MenuItem_GetBySite.</returns>
        public override string GetSettingKey()
        {
            return "NAP_MenuItem_GetBySite";
        }

        /// <summary>
        /// Check data's constrains
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
