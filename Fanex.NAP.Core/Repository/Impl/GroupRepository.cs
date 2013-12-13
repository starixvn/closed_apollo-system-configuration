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

    /// <summary>
    /// Group repository.
    /// </summary>
    public class GroupRepository : RepositoryBase<IGroup>, IGroupRepository
    {
        /// <summary>
        /// Gets group.
        /// </summary>
        /// <param name="criteria">The criteria get group.</param>
        /// <returns>The group.</returns>
        public override IGroup Get(ICriteria criteria)
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

        /// <summary>
        /// Fetch group.
        /// </summary>
        /// <param name="criteria">The criteria fetch group.</param>
        /// <returns>List of group.</returns>
        public override IEnumerable<IGroup> Fetch(ICriteria criteria)
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

        /// <summary>
        /// Insert group.
        /// </summary>
        /// <param name="criteria">The criteria insert group.</param>
        /// <param name="logCriteria">The log criteria insert group.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Insert(ref InsertGroupCriteria criteria, ICriteria logCriteria)
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

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="criteria">The criteria update group.</param>
        /// <param name="logCriteria">The log criteria update group.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Update(ICriteria criteria, ICriteria logCriteria)
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

        /// <summary>
        /// Delete group.
        /// </summary>
        /// <param name="criteria">The criteria delete group.</param>
        /// <param name="logCriteria">The log criteria delete group.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Delete(ICriteria criteria, ICriteria logCriteria)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert group.
        /// </summary>
        /// <param name="entity">The group entity.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Insert(IGroup entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="entity">The group entity.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Update(IGroup entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete group.
        /// </summary>
        /// <param name="entity">The group entity.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Delete(IGroup entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets group policy.
        /// </summary>
        /// <param name="criteria">The criteria get group policy.</param>
        /// <returns>The group.</returns>
        public IEnumerable<GroupPolicy> GetGroupPolicy(ICriteria criteria)
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
    }
}