// --------------------------------------------------------------------------
// <copyright file="GetUserSiteFunctionKeyPolicyUserCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get user site function key policy.
    /// </summary>
    public class GetUserSiteFunctionKeyPolicyUserCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SiteId { get; set; }

        public string FunctionKey { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_GetUserSiteFunctionKeyPolicy";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}