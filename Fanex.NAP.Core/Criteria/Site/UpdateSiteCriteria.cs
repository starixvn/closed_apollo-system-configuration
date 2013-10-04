// --------------------------------------------------------------------------
// <copyright file="UpdateSiteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
//   <change who="Lewis.Le" date="2012.9.4">Added data annotation</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Site
{
    using System.ComponentModel.DataAnnotations;

    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update site.
    /// </summary>
    public class UpdateSiteCriteria : CriteriaBase
    {
        public UpdateSiteCriteria()
        {
            Environment = Constant.UAT;
        }

        public int SiteId { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidNameValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string SiteName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewResources))]
        [StringLength(200, MinimumLength = 0,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string Description { get; set; }

        [Display(Name = "Status", ResourceType = typeof(ViewResources))]
        public bool IsActive { get; set; }

        [Required]
        [Display(Name = "Environment", ResourceType = typeof(ViewResources))]
        public string Environment { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_Site_Update.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Site_Update";
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