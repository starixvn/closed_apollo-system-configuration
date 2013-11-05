// --------------------------------------------------------------------------
// <copyright file="GetMailingListByFunctionKeyAndSiteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.10.4">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MailingList
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetMailingListByFunctionKeyAndSite Criteria.
    /// </summary>
    public class GetMailingListByFunctionKeyAndSiteCriteria : CriteriaBase
    {
        public string FunctionKey { get; set; }

        public int SiteId { get; set; }

        public bool? IsInternal { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_MailingList_GetByFunctionKeyAndSite";
        }

        public override bool IsValid()
        {
            return this.SiteId > 0;
        }
    }
}
