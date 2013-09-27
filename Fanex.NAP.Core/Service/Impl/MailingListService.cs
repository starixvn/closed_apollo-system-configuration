// --------------------------------------------------------------------------
// <copyright file="MailingListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.27">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Repository.Impl;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    public class MailingListService : BaseService, IMailingListService
    {
        private readonly IMailingListRepository _mailingListRepository;

        #region Ctors

        public MailingListService() : this(null)
        {
        }

        public MailingListService(IMailingListRepository mailingListRepository = null)
        {
            _mailingListRepository = mailingListRepository ?? new MailingListRepository();
        }

        #endregion

        public ICommandResult Insert(InsertMailingListCriteria criteria)
        {
            // check parameters
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                    if (criteria.IsValid() && logCriteria.IsValid())
                    {
                        return _mailingListRepository.Insert(ref criteria, logCriteria);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}
