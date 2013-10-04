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
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Repository.Impl;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// MailingList Service.
    /// </summary>
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

        /// <summary>
        /// Get MailingList by id.
        /// </summary>
        /// <param name="listId">Mailing list ID.</param>
        /// <returns>MailingList Object.</returns>
        public IMailingList GetById(int listId)
        {
            try
            {
                var criteria = new GetMailingListByIdCriteria { ListId = listId };
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

        /// <summary>
        /// Get MailingList by key.
        /// </summary>
        /// <param name="listKey">Mailing list key.</param>
        /// <returns>MailingList Object.</returns>
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

        /// <summary>
        /// Get MailingList by name.
        /// </summary>
        /// <param name="listName">Mailing list name.</param>
        /// <returns>MailingList Object.</returns>
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

        /// <summary>
        /// Get list Mailing List form site with grand.
        /// </summary>
        /// <param name="criteria">GetMailingListBySite Criteria to query database.</param>
        /// <returns>List MailingList Object.</returns>
        public IEnumerable<IMailingList> GetBySite(GetMailingListBySiteCriteria criteria)
        {
            Check.Argument.IsNotEmpty(criteria.Environment, "Environment");

            try
            {
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

        /// <summary>
        /// Get list Mailing List form site with grand.
        /// </summary>
        /// <param name="criteria">GetMailingListBySite Criteria to query database.</param>
        /// <returns>List MailingList Object.</returns>
        public IEnumerable<IMailingList> GetByFunctionKeyAndSite(GetMailingListByFunctionKeyAndSiteCriteria criteria)
        {
            Check.Argument.IsNotEmpty(criteria.FunctionKey, "FunctionKey");

            try
            {
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

        /// <summary>
        /// Insert a Mailing List to DB.
        /// </summary>
        /// <param name="criteria">InsertMailingList Criteria contains field to insert to DB.</param>
        /// <returns>ICommand Result.</returns>
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

        /// <summary>
        /// Update a Mailing List to DB.
        /// </summary>
        /// <param name="criteria">Criteria contains field to update to DB.</param>
        /// <returns>ICommand Result.</returns>
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
