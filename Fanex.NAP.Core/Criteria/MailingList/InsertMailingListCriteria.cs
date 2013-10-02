// --------------------------------------------------------------------------
// <copyright file="InsertMailingListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.26">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MailingList
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// InsertMailingList Criteria.
    /// </summary>
    public class InsertMailingListCriteria : CriteriaBase
    {
        public InsertMailingListCriteria()
        {
            this.Environment = Constant.UAT;
        }

        public int ListId { get; set; }

        public string ListKey { get; set; }

        [Required]
        [Display(Name = "MailingList_Name", ResourceType = typeof(ViewResources))]
        public string ListName { get; set; }

        public string Description { get; set; }

        public int SiteId { get; set; }

        public string Environment { get; set; }

        [Required]
        public int ListFunctionId { get; set; }

        [Required]
        public int FunctionId { get; set; }

        public string ToList { get; set; }

        public string CCList { get; set; }

        public string BCCList { get; set; }

        public bool IsInternal { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_MailingLists_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
