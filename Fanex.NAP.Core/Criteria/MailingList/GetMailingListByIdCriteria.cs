// --------------------------------------------------------------------------
// <copyright file="GetMailingListByIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.10.1">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.MailingList
{
    using Fanex.NAP.Framework.Data;

    public class GetMailingListByIdCriteria : CriteriaBase
    {
        public int ListId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_MailingLists_GetById";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
