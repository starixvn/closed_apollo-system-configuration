// --------------------------------------------------------------------------
// <copyright file="SearchGroupExcludeFunctionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.11.1">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Group
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Search group.
    /// </summary>
    public class SearchGroupExcludeFunctionCriteria : CriteriaBase
    {       
        public string GroupName { get; set; }
      
        public int SiteId { get; set; }

        public int FunctionId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Group_SearchExcludeFunction";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}