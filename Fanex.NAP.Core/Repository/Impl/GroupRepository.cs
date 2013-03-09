// --------------------------------------------------------------------------
// <copyright file="GroupRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.14">Created</change>
// </history>

namespace Fanex.NAP.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Fanex.Common.Extensions;
    using Fanex.Data;

    using Fanex.NAP.Core.Criteria.Group;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GroupRepository : RepositoryBase<IGroup>, IGroupRepository
    {
        public override IGroup Get(ICriteria criteria)
        {
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var objGroup = objectDb.QueryEntity<Group, User, User, Group>(
                        (g, cu, mu) => { g.Creator = cu; g.Modifier = mu; return g; },
                        "CreatorId,ModifierId",
                        criteria.GetSpParams());
                    return objGroup;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override IEnumerable<IGroup> Fetch(ICriteria criteria)
        {
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var listGroup = objectDb.Query<Group, User, User, Group>(
                        (g, cu, mu) => { g.Creator = cu; g.Modifier = mu; return g; },
                        "CreatorId,ModifierId",
                        criteria.GetSpParams());

                    return listGroup.OrderBy(g => g.Name).ToList<Group>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICommandResult Insert(ref InsertGroupCriteria criteria, ICriteria logCriteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();

                hash = logCriteria.AddSpParams(hash);
                hash = (new ResultCriteria()).AddSpParams(hash);

                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.ExecuteNonQuery(hash);
                    criteria.GroupId = Convert.ToInt32(objectDb.GetParameterValue("GroupId"));
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

                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.ExecuteNonQuery(hash);
                    return GetCommandResult(hash);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICommandResult Delete(ICriteria criteria, ICriteria logCriteria)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(IGroup entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(IGroup entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Delete(IGroup entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupPolicy> GetGroupPolicy(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        var userLevelList = new List<GroupPolicy>();

                        while (reader.Read())
                        {
                            userLevelList.Add(new GroupPolicy
                            {
                                Policy = reader.ToInt("Policy"),
                                Group = reader.IsDBNull("GroupId") ? null : new Group
                                {
                                    Id = reader.ToShort("GroupId"),
                                    Name = reader.ToString("GroupName"),
                                    CreatedDate = reader.ToDateTime("CreatedDate"),
                                    ModifiedDate = reader.ToDateTime("ModifiedDate"),
                                    Creator = reader.IsDBNull("CreatorId") ? null : new User
                                    {
                                        Id = reader.ToInt("CreatorId"),
                                        Username = reader.ToString("CreatorUsername")
                                    },
                                    Modifier = reader.IsDBNull("ModifierId") ? null : new User
                                    {
                                        Id = reader.ToInt("ModifierId"),
                                        Username = reader.ToString("ModifierUsername")
                                    }
                                },
                                CreatedDate = reader.ToDateTime("CreatedDateGroupFunction"),
                                ModifiedDate = reader.ToDateTime("ModifiedDateGroupFunction"),
                                Creator = reader.IsDBNull("CreatorIdGroupFunction") ? null : new User
                                {
                                    Id = reader.ToInt("CreatorIdGroupFunction"),
                                    Username = reader.ToString("CreatorUsernameGroupFunction")
                                },
                                Modifier = reader.IsDBNull("ModifierIdGroupFunction") ? null : new User
                                {
                                    Id = reader.ToInt("ModifierIdGroupFunction"),
                                    Username = reader.ToString("ModifierUsernameGroupFunction")
                                }
                            });
                        }

                        return userLevelList.OrderBy(g => g.Group.Name).ToList<GroupPolicy>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}