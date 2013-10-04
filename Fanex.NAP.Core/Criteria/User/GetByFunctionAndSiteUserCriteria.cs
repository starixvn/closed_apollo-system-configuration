// --------------------------------------------------------------------------
// <copyright file="GetByFunctionAndSiteUserCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get by function and site.
    /// </summary>
    public class GetByFunctionAndSiteUserCriteria : CriteriaBase
    {
        public int FunctionId { get; set; }

        public int SiteId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_GetUserByFunctionAndSite";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}