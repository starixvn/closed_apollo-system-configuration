// --------------------------------------------------------------------------
// <copyright file="SearchUserCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Search user.
    /// </summary>
    public class SearchUserCriteria : CriteriaBase
    {       
        public string Username { get; set; }
        
        public int GroupId { get; set; }
       
        public int SiteId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_Search";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}