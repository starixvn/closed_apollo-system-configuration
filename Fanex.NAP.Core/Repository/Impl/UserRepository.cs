// --------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
// </history>

namespace Fanex.NAP.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// User repository.
    /// </summary>
    public class UserRepository : RepositoryBase<IUser>, IUserRepository
    {
        /// <summary>
        /// Gets user.
        /// </summary>
        /// <param name="criteria">The criteria get user.</param>
        /// <returns>The user object.</returns>
        public override IUser Get(ICriteria criteria)
        {
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var objUser = objectDb.QueryEntity<User, Group, User, User, User>(
                    (u, g, uc, um) => { u.Group = g; u.Creator = uc; u.Modifier = um; return u; },
                    "GroupId,CreatorId,ModifierId",
                    criteria.GetSpParams());
                return objUser;
            }
        }

        /// <summary>
        /// Fetch user.
        /// </summary>
        /// <param name="criteria">The criteria fetch user.</param>
        /// <returns>List of user.</returns>
        public override IEnumerable<IUser> Fetch(ICriteria criteria)
        {
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var objListUser = objectDb.Query<User, Group, User, User, User>(
                        (u, g, uc, um) => { u.Group = g; u.Creator = uc; u.Modifier = um; return u; },
                        "GroupId,CreatorId,ModifierId",
                        criteria.GetSpParams());
                    return objListUser;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Fetch user.
        /// </summary>
        /// <param name="criteria">The criteria fetch user.</param>
        /// <param name="totalRows">Total rows.</param>
        /// <returns>List of user.</returns>
        public IQueryable<IUser> Fetch(ICriteria criteria, out int totalRows)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    Hashtable hash = criteria.GetSpParams();

                    var userList = db.Query<User>(hash);

                    totalRows = Convert.ToInt32(hash["totalrows"]);

                    return userList.AsQueryable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert user.
        /// </summary>
        /// <param name="entity">The user entity.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Insert(IUser entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="entity">The user entity.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Update(IUser entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="entity">The user entity.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Delete(IUser entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="logCriteria">The log criteria.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Insert(InsertCustCriteria criteria, ICriteria logCriteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();

                hash = logCriteria.AddSpParams(hash);
                hash = (new ResultCriteria()).AddSpParams(hash);

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);
                    criteria.CustId = Convert.ToInt32(db.GetParameterValue("CustId"));

                    return GetCommandResult(hash);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <param name="criteria">The criteria update user.</param>
        /// <param name="logCriteria">The log criteria update user.</param>
        /// <returns>Command result.</returns>
        public ICommandResult Update(ICriteria criteria, ICriteria logCriteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();

                hash = logCriteria.AddSpParams(hash);
                hash = (new ResultCriteria()).AddSpParams(hash);

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
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

        /// <summary>
        /// User check in.
        /// </summary>
        /// <param name="criteria">The criteria user check in.</param>
        public void UserCheckIn(ref UserCheckInUserCriteria criteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.ExecuteNonQuery(hash);
                    criteria.OnUserId = Convert.ToInt32(hash["onuserid"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// User recheck in.
        /// </summary>
        /// <param name="criteria">The criteria user recheck in.</param>
        public void UserReCheckIn(ref UserReCheckInUserCriteria criteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.ExecuteNonQuery(hash);
                    criteria.IsKickedOut = Convert.ToBoolean(hash["iskickedout"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// User logout.
        /// </summary>
        /// <param name="criteria">The criteria user logout.</param>
        public void UserLogOut(LogOutUserCriteria criteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.ExecuteNonQuery(hash);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// User SSO check in.
        /// </summary>
        /// <param name="criteria">The criteria user SSO check in.</param>
        public void UserSSOCheckIn(ref UserSSOCheckInUserCriteria criteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();

                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.ExecuteNonQuery(hash);
                    criteria.OnUserId = Convert.ToInt32(hash["onuserid"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Search user level.
        /// </summary>
        /// <param name="criteria">The criteria search user level.</param>
        /// <returns>Command result.</returns>
        public IEnumerable<UserLevel> Search(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        var userLevelList = new List<UserLevel>();

                        while (reader.Read())
                        {
                            userLevelList.Add(new UserLevel
                            {
                                Level = reader.ToInt("Level"),
                                User = reader.IsDBNull("UserId") ? null : new User
                                {
                                    Id = reader.ToInt("UserId"),
                                    FirstName = reader.ToString("FirstName"),
                                    LastName = reader.ToString("LastName"),
                                    Username = reader.ToString("Username"),
                                    Email = reader.ToString("Email"),
                                    Password = reader.ToString("UserPwd"),
                                    LastLoginDate = reader.ToDateTime("LastLoginDate"),
                                    CreatedDate = reader.ToDateTime("CreatedDate"),
                                    ModifiedDate = reader.ToDateTime("ModifiedDate"),
                                    Group = reader.IsDBNull("GroupId") ? null : new Group
                                    {
                                        Id = reader.ToShort("GroupId"),
                                        Name = reader.ToString("GroupName")
                                    },
                                    Creator = reader.IsDBNull("CreatorId") ? null : new User
                                    {
                                        Id = reader.ToInt("CreatorId"),
                                        Username = reader.ToString("CreatorUsername")
                                    },
                                    Modifier = reader.IsDBNull("ModifierId") ? null : new User
                                    {
                                        Id = reader.ToInt("ModifierId"),
                                        Username = reader.ToString("ModifierUsername")
                                    },
                                },
                                CreatedDate = reader.ToDateTime("CreatedDateUserSite"),
                                ModifiedDate = reader.ToDateTime("ModifiedDateUserSite"),
                                Creator = reader.IsDBNull("CreatorIdUserSite") ? null : new User
                                {
                                    Id = reader.ToInt("CreatorIdUserSite"),
                                    Username = reader.ToString("CreatorUsernameUserSite")
                                },
                                Modifier = reader.IsDBNull("ModifierIdUserSite") ? null : new User
                                {
                                    Id = reader.ToInt("ModifierIdUserSite"),
                                    Username = reader.ToString("ModifierUsernameUserSite")
                                },
                                IsActive = reader.ToBool("IsActive"),
                                LastLoginDate = reader.ToDateTime("LastLoginDateUserSite")
                            });
                        }

                        return userLevelList.GroupBy(u => u.User.Id).Select(u => u.First());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Search user.
        /// </summary>
        /// <param name="criteria">The criteria search user.</param>
        /// <param name="totalRows">Total rows.</param>
        /// <returns>List of users.</returns>
        public IQueryable<UserLevel> Search(ICriteria criteria, out int totalRows)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        var userLevelList = new List<UserLevel>();

                        while (reader.Read())
                        {
                            userLevelList.Add(new UserLevel
                            {
                                Level = reader.ToInt("Level"),
                                User = reader.IsDBNull("UserId") ? null : new User
                                {
                                    Id = reader.ToInt("UserId"),
                                    FirstName = reader.ToString("FirstName"),
                                    LastName = reader.ToString("LastName"),
                                    Username = reader.ToString("Username"),
                                    Email = reader.ToString("Email"),
                                    Password = reader.ToString("UserPwd"),
                                    LastLoginDate = reader.ToDateTime("LastLoginDate"),
                                    CreatedDate = reader.ToDateTime("CreatedDate"),
                                    ModifiedDate = reader.ToDateTime("ModifiedDate"),
                                    Group = reader.IsDBNull("GroupId") ? null : new Group
                                    {
                                        Id = reader.ToShort("GroupId"),
                                        Name = reader.ToString("GroupName")
                                    },
                                    Creator = reader.IsDBNull("CreatorId") ? null : new User
                                    {
                                        Id = reader.ToInt("CreatorId"),
                                        Username = reader.ToString("CreatorUsername")
                                    },
                                    Modifier = reader.IsDBNull("ModifierId") ? null : new User
                                    {
                                        Id = reader.ToInt("ModifierId"),
                                        Username = reader.ToString("ModifierUsername")
                                    },
                                },
                                CreatedDate = reader.ToDateTime("CreatedDateUserSite"),
                                ModifiedDate = reader.ToDateTime("ModifiedDateUserSite"),
                                Creator = reader.IsDBNull("CreatorIdUserSite") ? null : new User
                                {
                                    Id = reader.ToInt("CreatorIdUserSite"),
                                    Username = reader.ToString("CreatorUsernameUserSite")
                                },
                                Modifier = reader.IsDBNull("ModifierIdUserSite") ? null : new User
                                {
                                    Id = reader.ToInt("ModifierIdUserSite"),
                                    Username = reader.ToString("ModifierUsernameUserSite")
                                },
                                IsActive = reader.ToBool("IsActive"),
                                LastLoginDate = reader.ToDateTime("LastLoginDateUserSite"),
                                Site = reader.IsDBNull("SiteId") ? null : new Site
                                {
                                    Id = reader.ToByte("SiteId"),
                                    Name = reader.ToString("SiteName")
                                }
                            });
                        }

                        reader.Close();

                        totalRows = (int)db.GetParameterValue("totalrows");

                        return userLevelList.AsQueryable();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check if username existed.
        /// </summary>
        /// <param name="criteria">The criteria check username.</param>
        /// <returns>Check result.</returns>
        public bool CheckUsernameExist(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get user policy.
        /// </summary>
        /// <param name="criteria">The criteria get user policy.</param>
        /// <returns>List of user policy.</returns>
        public IEnumerable<UserPolicy> GetUserPolicy(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var userLevelList = new List<UserPolicy>();

                    while (reader.Read())
                    {
                        userLevelList.Add(new UserPolicy
                        {
                            Policy = reader.ToInt("Policy"),
                            User = reader.IsDBNull("UserId") ? null : new User
                            {
                                Id = reader.ToInt("UserId"),
                                FirstName = reader.ToString("FirstName"),
                                LastName = reader.ToString("LastName"),
                                Username = reader.ToString("Username"),
                                Email = reader.ToString("Email"),
                                Password = reader.ToString("UserPwd"),
                                LastLoginDate = reader.ToDateTime("LastLoginDate"),
                                CreatedDate = reader.ToDateTime("CreatedDate"),
                                ModifiedDate = reader.ToDateTime("ModifiedDate"),
                                Group = reader.IsDBNull("GroupId") ? null : new Group
                                {
                                    Id = reader.ToShort("GroupId"),
                                    Name = reader.ToString("GroupName")
                                },
                                Creator = reader.IsDBNull("CreatorId") ? null : new User
                                {
                                    Id = reader.ToInt("CreatorId"),
                                    Username = reader.ToString("CreatorUsername")
                                },
                                Modifier = reader.IsDBNull("ModifierId") ? null : new User
                                {
                                    Id = reader.ToInt("ModifierId"),
                                    Username = reader.ToString("ModifierUsername")
                                },
                            },
                            CreatedDate = reader.ToDateTime("CreatedDateUserFunction"),
                            ModifiedDate = reader.ToDateTime("ModifiedDateUserFunction"),
                            Creator = reader.IsDBNull("CreatorIdUserFunction") ? null : new User
                            {
                                Id = reader.ToInt("CreatorIdUserFunction"),
                                Username = reader.ToString("CreatorUsernameUserFunction")
                            },
                            Modifier = reader.IsDBNull("ModifierIdUserFunction") ? null : new User
                            {
                                Id = reader.ToInt("ModifierIdUserFunction"),
                                Username = reader.ToString("ModifierUsernameUserFunction")
                            }
                        });
                    }

                    return userLevelList;
                }
            }
        }

        /// <summary>
        /// Get user list site function key policy.
        /// </summary>
        /// <param name="criteria">The criteria get user list site function key policy.</param>
        /// <returns>User list site function key policy.</returns>
        public Hashtable GetUserListSiteFunctionKeyPolicy(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var hashTable = new Hashtable();

                    while (reader.Read())
                    {
                        var siteId = reader.ToByte("SiteId");
                        if (!hashTable.ContainsKey(siteId))
                        {
                            hashTable.Add(siteId, reader.ToInt("Policy"));
                        }
                    }

                    return hashTable;
                }
            }
        }

        /// <summary>
        /// User SSO get time.
        /// </summary>
        /// <param name="criteria">The criteria user SSO get time.</param>
        /// <returns>The SSO time.</returns>
        public string UserSSOGetTime(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var hashTable = criteria.GetSpParams();

                db.ExecuteNonQuery(hashTable);

                return hashTable["time"].ToString();
            }
        }
    }
}