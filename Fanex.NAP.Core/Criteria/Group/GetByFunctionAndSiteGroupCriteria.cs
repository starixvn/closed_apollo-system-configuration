// --------------------------------------------------------------------------
// <copyright file="GetByFunctionAndSiteGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.20">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Group
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get by function and site.
    /// </summary>
    public class GetByFunctionAndSiteGroupCriteria : CriteriaBase
    {
        public int FunctionId { get; set; }

        public int SiteId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Group_GetGroupByFunctionAndSite";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}