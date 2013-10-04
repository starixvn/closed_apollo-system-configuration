// --------------------------------------------------------------------------
// <copyright file="SearchAllMenuItemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.29">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MenuItem
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Search all.
    /// </summary>
    public class SearchAllMenuItemCriteria : CriteriaBase
    {
        public string Environment { get; set; }

        public string MenuItemName { get; set; }

        public bool? IsActive { get; set; }

        public int SiteId { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_MenuItem_GetAll.</returns>
        public override string GetSettingKey()
        {
            return "NAP_MenuItem_Search";
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
