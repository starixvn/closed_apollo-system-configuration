// --------------------------------------------------------------------------
// <copyright file="GetByUserIncludeGroupMenuItemCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Nguyen" date="2013.03.05">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MenuItem
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get by user include group
    /// </summary>
    public class GetByUserIncludeGroupMenuItemCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string Environment { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_MenuItem_GetByUserAndSiteIncludeGroup</returns>
        public override string GetSettingKey()
        {
            return "NAP_MenuItem_GetByUserIncludeGroup";
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