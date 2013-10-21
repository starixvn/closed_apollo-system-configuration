// --------------------------------------------------------------------------
// <copyright file="GetMailingListByUserAndSiteIncludeGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.30">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MailingList
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetMailingListBySite Criteria.
    /// </summary>
    public class GetMailingListByUserAndSiteIncludeGroupCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SiteId { get; set; }

        public string Environment { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsInternal { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_MailingList_GetByUserAndSiteIncludeGroup";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
