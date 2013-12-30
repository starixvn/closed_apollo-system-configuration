// --------------------------------------------------------------------------
// <copyright file="IMailingListRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.27">Created</change>
// </history>

namespace Fanex.NAP.Core.Repository.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    public interface IMailingListRepository
    {
        IMailingList Get(ICriteria criteria);

        IEnumerable<IMailingList> Fetch(ICriteria criteria);

        ICommandResult Insert(ref InsertMailingListCriteria criteria, ICriteria logCriteria);

        ICommandResult Update(ICriteria criteria, ICriteria logCriteria);
    }
}
