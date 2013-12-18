// --------------------------------------------------------------------------
// <copyright file="InsertSiteUrlCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.11">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Site
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Insert site URL.
    /// </summary>
    public class InsertSiteUrlCriteria : CriteriaBase
    {
        public int SiteUrlId { get; set; }

        [Required]
        public int SiteId { get; set; }

        [Display(Name = "URL", ResourceType = typeof(ViewResources))]
        [StringLength(100, MinimumLength = 0,
            ErrorMessageResourceName = "InputLengthValidationMsg", ErrorMessageResourceType = typeof(ViewResources))]
        public string Url { get; set; }

        [Required]
        [Display(Name = "Environment", ResourceType = typeof(ViewResources))]
        public string Environment { get; set; }

        [Display(Name = "Status", ResourceType = typeof(ViewResources))]
        public bool IsActive { get; set; }

        [Display(Name = "Default", ResourceType = typeof(ViewResources))]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>The criteria.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Site_InsertSiteURL";
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