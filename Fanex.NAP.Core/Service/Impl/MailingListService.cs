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
    using Fanex.NAP.Core.Domain;
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;

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

        public IMailingList GetById(int listId)
        {
            try
            {
                var criteria = new GetMailingListByIdCriteria
                {
                    ListId = listId
                };

                if (criteria.IsValid())
                {
                    return _mailingListRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IMailingList GetByKey(string listKey)
        {
            try
            {
                var criteria = new GetMailingListByKeyCriteria
                {
                    ListKey = listKey
                };

                if (criteria.IsValid())
                {
                    return _mailingListRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IMailingList GetByName(string listName)
        {
            try
            {
                var criteria = new GetMailingListByNameCriteria
                {
                    ListName = listName
                };

                if (criteria.IsValid())
                {
                    return _mailingListRepository.Get(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public IEnumerable<IMailingList> GetBySite(GetMailingListBySiteCriteria criteria)
        {
            Check.Argument.IsNotEmpty(criteria.Environment, "Environment");

            try
            {
                //var criteria = new GetMailingListBySiteCriteria
                //{
                //    SiteId = siteId,
                //    Environment = env,
                //    IsActive = isActive.HasValue ? isActive.Value : true,
                //    IsInternal = isInternal.HasValue ? isInternal.Value : false
                //};

                if (criteria.IsValid())
                {
                    return _mailingListRepository.Fetch(criteria);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public ICommandResult Insert(InsertMailingListCriteria criteria)
        {
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

        public ICommandResult Update(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    LogExecSpCriteria logCriteria = GetLogExecSpCriteria();

                    if (criteria.IsValid() && logCriteria.IsValid())
                    {
                        return _mailingListRepository.Update(criteria, logCriteria);
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
