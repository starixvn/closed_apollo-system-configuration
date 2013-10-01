// --------------------------------------------------------------------------
// <copyright file="UpdateMailingListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.30">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MailingList
{
    using System;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Framework.Data;
    using System.ComponentModel.DataAnnotations;

    public class UpdateMailingListCriteria : CriteriaBase
    {
        public UpdateMailingListCriteria()
        {
            this.Environment = Constant.UAT;
        }

        public int ListId { get; set; }

        public string ListKey { get; set; }

        [Required]
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

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_MailingLists_Update";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
