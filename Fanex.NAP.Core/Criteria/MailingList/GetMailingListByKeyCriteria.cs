// --------------------------------------------------------------------------
// <copyright file="GetMailingListByKeyCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.10.1">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MailingList
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetMailingListByKey Criteria.
    /// </summary>
    public class GetMailingListByKeyCriteria : CriteriaBase
    {
        public string ListKey { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_MailingLists_GetByKey";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
