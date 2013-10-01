// --------------------------------------------------------------------------
// <copyright file="GetMailingListBySiteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.30">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MailingList
{
    using Fanex.NAP.Framework.Data;

    public class GetMailingListBySiteCriteria : CriteriaBase
    {
        public int SiteId { get; set; }

        public string Environment { get; set; }

        public bool? IsActive { get; set; }

        public bool IsInternal { get; set; }

        public int SiteMailingList { get; set; }

        public string ParentFunctionKey { get; set; }

        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_MailingLists_GetBySite";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
