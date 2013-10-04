// --------------------------------------------------------------------------
// <copyright file="GetByIdUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get by ID.
    /// </summary>
    public class GetByIdUserCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_GetById";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}