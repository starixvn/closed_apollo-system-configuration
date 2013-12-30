// --------------------------------------------------------------------------
// <copyright file="MailingListRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.27">Created</change>
// </history>

namespace Fanex.NAP.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// MailingList Repository.
    /// </summary>
    public class MailingListRepository : RepositoryBase<IMailingList>, IMailingListRepository
    {
        /// <summary>
        /// Get Mailing List Object.
        /// </summary>
        /// <param name="criteria">Criteria to query DB.</param>
        /// <returns>Mailing List Object.</returns>
        public override IMailingList Get(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.QueryEntity<MailingList, Site, Function, Function, MailingList>(
                        (m, si, mf, f) =>
                        {
                            m.Site = si;
                            m.MailFunction = mf;
                            m.Function = f;
                            return m;
                        },
                    "SiteId,ListFunctionId,FunctionId",
                        criteria.GetSpParams());
            }
        }

        /// <summary>
        /// Get list Mailing List Object.
        /// </summary>
        /// <param name="criteria">Criteria to query DB.</param>
        /// <returns>List Mailing List Object.</returns>
        public override IEnumerable<IMailingList> Fetch(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<MailingList, Site, Function, Function, MailingList>(
                    (m, si, mf, f) =>
                    {
                        m.Site = si;
                        m.MailFunction = mf;
                        m.Function = f;
                        return m;
                    },
                    "SiteId,ListFunctionId,FunctionId",
                    criteria.GetSpParams());
            }
        }

        /// <summary>
        /// Insert Mailing List Object.
        /// </summary>
        /// <param name="criteria">Criteria contains value to insert DB.</param>
        /// <param name="logCriteria">Criteria contains value to write log DB.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult Insert(ref InsertMailingListCriteria criteria, ICriteria logCriteria)
        {
            Hashtable hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                criteria.ListId = Convert.ToInt32(db.GetParameterValue("ListId"));

                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Update Mailing List Object.
        /// </summary>
        /// <param name="criteria">Criteria contains value to update DB.</param>
        /// <param name="logCriteria">Criteria contains value to write log DB.</param>
        /// <returns>ICommand Result.</returns>
        public ICommandResult Update(ICriteria criteria, ICriteria logCriteria)
        {
            Hashtable hash = criteria.GetSpParams();

            hash = logCriteria.AddSpParams(hash);
            hash = (new ResultCriteria()).AddSpParams(hash);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);

                return GetCommandResult(hash);
            }
        }
    }
}
