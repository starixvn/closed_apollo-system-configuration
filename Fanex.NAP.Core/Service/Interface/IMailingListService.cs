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
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Framework.Data;

    public interface IMailingListService : IBaseService
    {
        ICommandResult Insert(InsertMailingListCriteria criteria);
    }
}
