// --------------------------------------------------------------------------
// <copyright file="GetUserFunctionPolicyUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.31">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get user function policy.
    /// </summary>
    public class GetUserFunctionPolicyUserCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int FunctionId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_GetUserFunctionPolicy";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}