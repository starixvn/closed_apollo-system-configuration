// --------------------------------------------------------------------------
// <copyright file="AssignSiteUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class AssignSiteUserCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        /// <summary>
        /// <ROOT><SiteLevel SiteId="4" Level="3" ></SiteLevel></ROOT>
        /// </summary>
        public string Value { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_AssignSite";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}