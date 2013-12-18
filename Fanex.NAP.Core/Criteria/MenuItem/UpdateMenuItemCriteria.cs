// --------------------------------------------------------------------------
// <copyright file="UpdateMenuItemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
//   <change who="Edward.Nguyen" date="2012.8.28">Change ModifiedBy To ModifierId</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MenuItem
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update menu item.
    /// </summary>
    public class UpdateMenuItemCriteria : CriteriaBase
    {
        public int MenuItemId { get; set; }

        [Required]
        [Display(Name = "Function", ResourceType = typeof(ViewResources))]
        public int FunctionId { get; set; }

        [Required]
        [Display(Name = "Site", ResourceType = typeof(ViewResources))]
        public int SiteId { get; set; }

        [SpParam(Ignored = true)]
        public string SiteName { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg", ErrorMessageResourceType = typeof(ViewResources))]
        public string MenuItemName { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewResources))]
        [StringLength(200, MinimumLength = 0,
            ErrorMessageResourceName = "InputLengthValidationMsg", ErrorMessageResourceType = typeof(ViewResources))]
        public string Description { get; set; }

        [Display(Name = "Parent", ResourceType = typeof(ViewResources))]
        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        [Required]
        [Display(Name = "Policy", ResourceType = typeof(ViewResources))]
        public int Policy { get; set; }

        [Display(Name = "URL", ResourceType = typeof(ViewResources))]
        [StringLength(200, MinimumLength = 0,
            ErrorMessageResourceName = "InputLengthValidationMsg", ErrorMessageResourceType = typeof(ViewResources))]
        public string Url { get; set; }

        [Display(Name = "Display_Order", ResourceType = typeof(ViewResources))]
        public int DisplayOrder { get; set; }

        [Display(Name = "Status", ResourceType = typeof(ViewResources))]
        public bool IsActive { get; set; }

        public int ModifierId { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        [Display(Name = "Environment", ResourceType = typeof(ViewResources))]
        public string Environment { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_MenuItem_Update";
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
