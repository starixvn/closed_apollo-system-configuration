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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    using Fanex.NAP.Core.Criteria.Function;
    using Fanex.NAP.Core.Criteria.MailingList;

    public interface IMailingListRepository
    {
        ICommandResult Insert(ref InsertMailingListCriteria criteria, ICriteria logCriteria);
    }
}
