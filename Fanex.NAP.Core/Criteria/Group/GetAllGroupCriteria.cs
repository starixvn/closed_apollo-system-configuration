// --------------------------------------------------------------------------
// <copyright file="GetAllGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.20">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Group
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all group.
    /// </summary>
    public class GetAllGroupCriteria : CriteriaBase
    {
        public bool? IsActive { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Group_GetAll";
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