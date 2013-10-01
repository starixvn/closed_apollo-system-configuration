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
    using Fanex.Data;
    using Fanex.NAP.Core.Criteria.MailingList;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Framework.Data;
    using Fanex.NAP.Core.Model;
    using System.Collections.Generic;

    public class MailingListRepository : RepositoryBase<IMailingList>, IMailingListRepository
    {
        public override IMailingList Get(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<MailingList, Function, Function, MailingList>(
                         (m, mf, f) =>
                         {
                             m.MailFunction = mf;
                             m.Function = f;
                             return m;
                         },
                        "ListFunctionId,FunctionId",
                         criteria.GetSpParams());
                }
            }
            catch
            {
                throw;
            }
        }

        public override IEnumerable<IMailingList> Fetch(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<MailingList, Function, Function, MailingList>(
                        (m, mf, f) =>
                        {
                            m.MailFunction = mf;
                            m.Function = f;
                            return m;
                        },
                        "ListFunctionId,FunctionId",
                        criteria.GetSpParams());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICommandResult Insert(ref InsertMailingListCriteria criteria, ICriteria logCriteria)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICommandResult Update(ICriteria criteria, ICriteria logCriteria)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
