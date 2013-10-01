// --------------------------------------------------------------------------
// <copyright file="IMailingListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.27">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Framework.Data;

    public interface IMailingListService : IBaseService
    {
        IMailingList GetById(int listId);

        IMailingList GetByKey(string listKey);

        IMailingList GetByName(string listName);

        IEnumerable<IMailingList> GetBySite(GetMailingListBySiteCriteria criteria);

        ICommandResult Insert(InsertMailingListCriteria criteria);

        ICommandResult Update(ICriteria criteria);
    }
}
