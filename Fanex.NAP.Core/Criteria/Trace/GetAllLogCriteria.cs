// --------------------------------------------------------------------------
// <copyright file="GetAllLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Trace
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    public class GetAllLogCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd,yyyy}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd,yyyy}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }

        [Display(Name = "Filter", ResourceType = typeof(ViewResources))]
        public int FilterType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LogExecSp_Search";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
