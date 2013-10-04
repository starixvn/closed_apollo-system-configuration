// --------------------------------------------------------------------------
// <copyright file="GetByUserGroupCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get by user
    /// </summary>
    public class GetByUserGroupCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Group_GetByUser";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}