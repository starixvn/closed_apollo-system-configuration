// --------------------------------------------------------------------------
// <copyright file="SearchUserExcludeFunctionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.11.1">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    public class SearchUserExcludeFunctionCriteria : CriteriaBase
    {       
        public string Username { get; set; }
        
        public int GroupId { get; set; }
       
        public int SiteId { get; set; }

        public int FunctionId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_SearchExcludeFunction";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}