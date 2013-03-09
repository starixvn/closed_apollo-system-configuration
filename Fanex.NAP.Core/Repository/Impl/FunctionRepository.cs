// --------------------------------------------------------------------------
// <copyright file="FunctionRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.13">Created</change>
//   <change who="Lewis.Le" date="2012.10.26">Added ICommandResult Duplicate(ref DuplicateFunctionCriteria criteria, ICriteria logCriteria)</change>
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
    using Fanex.NAP.Core.Criteria.Function;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Repository.Interface;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class FunctionRepository : RepositoryBase<IFunction>, IFunctionRepository
    {
        public override IFunction Get(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<Function, Function, Site, User, User, Function>(
                         (f, pf, s, cu, mu) =>
                         {
                             f.Parent = pf;
                             f.Site = s;
                             f.Creator = cu;
                             f.Modifier = mu;
                             return f;
                         },
                         "ParentId,SiteId,CreatorId,ModifierId",
                         criteria.GetSpParams());
                }
            }
            catch
            {
                throw;
            }
        }

        public override int Count(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return (int)db.ExecuteScalar(criteria.GetSpParams());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override IEnumerable<IFunction> Fetch(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<Function, Function, Site, User, User, Function>(
                        (f, pf, s, cu, mu) =>
                        {
                            f.Parent = pf;
                            f.Site = s;
                            f.Creator = cu;
                            f.Modifier = mu;
                            return f;
                        },
                        "ParentId,SiteId,CreatorId,ModifierId",
                        criteria.GetSpParams()).OrderBy(f => f.Name).ToList<Function>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<FunctionPolicy> FetchFunctionPolicy(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        var functionPolicyList = new List<FunctionPolicy>();

                        while (reader.Read())
                        {
                            functionPolicyList.Add(new FunctionPolicy
                            {
                                Function = new Function
                                {
                                    Id = reader.ToShort("FunctionId"),
                                    Name = reader.ToString("FunctionName"),
                                    Key = reader.ToString("FunctionKey"),
                                    Description = reader.ToString("Description"),
                                    Crud = reader.ToByte("CRUD"),
                                    DisplayOrder = reader.ToShort("DisplayOrder"),
                                    IsActive = reader.ToBool("IsActive"),
                                    Site = new Site
                                    {
                                        Id = reader.ToByte("SiteId"),
                                        Name = reader.ToString("SiteName")
                                    },
                                    Parent = reader.IsDBNull("ParentId") ? null : new Function
                                    {
                                        Id = reader.ToShort("ParentId"),
                                        Name = reader.ToString("ParentName")
                                    }
                                },
                                Site = new Site
                                {
                                    Id = reader.ToByte("SiteId"),
                                    Name = reader.ToString("SiteName")
                                },
                                Policy = reader.ToInt("Policy")
                            });
                        }

                        return functionPolicyList.OrderBy(f => f.Function.Name).ToList<FunctionPolicy>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<FunctionPolicy> FetchFullFunctionPolicy(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        var functionPolicyList = new List<FunctionPolicy>();

                        while (reader.Read())
                        {
                            functionPolicyList.Add(new FunctionPolicy
                            {
                                Function = new Function
                                {
                                    Id = reader.ToShort("FunctionId"),
                                    Name = reader.ToString("FunctionName"),
                                    Key = reader.ToString("FunctionKey"),
                                    Description = reader.ToString("Description"),
                                    Crud = reader.ToByte("CRUD"),
                                    DisplayOrder = reader.ToShort("DisplayOrder"),
                                    IsActive = reader.ToBool("IsActive"),
                                    Site = new Site
                                    {
                                        Id = reader.ToByte("SiteId"),
                                        Name = reader.ToString("SiteName")
                                    },
                                    Parent = reader.IsDBNull("ParentId") ? null : new Function
                                    {
                                        Id = reader.ToShort("ParentId"),
                                        Name = reader.ToString("ParentName")
                                    }
                                },
                                Site = new Site
                                {
                                    Id = reader.ToByte("SiteId"),
                                    Name = reader.ToString("SiteName")
                                },
                                Policy = reader.ToInt("Policy"),
                                Creator = reader.IsDBNull("CreatorId") ? null : new User
                                {
                                    Id = reader.ToInt("CreatorId"),
                                    Name = reader.ToString("CreatorUsername")
                                },
                                CreatedDate = reader.ToDateTime("CreatedDate"),
                                Modifier = reader.IsDBNull("ModifierId") ? null : new User
                                {
                                    Id = reader.ToInt("ModifierId"),
                                    Name = reader.ToString("ModifierUsername")
                                },
                                ModifiedDate = reader.ToDateTime("ModifiedDate")
                            });
                        }

                        return functionPolicyList.OrderBy(f => f.Function.Name).ToList<FunctionPolicy>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICommandResult Duplicate(ref DuplicateFunctionCriteria criteria, ICriteria logCriteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();
                hash = logCriteria.AddSpParams(hash);
                hash = (new ResultCriteria()).AddSpParams(hash);

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);

                    criteria.FunctionId = Convert.ToInt32(db.GetParameterValue("FunctionId"));

                    return GetCommandResult(hash);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICommandResult Insert(ref InsertFunctionCriteria criteria, ICriteria logCriteria)
        {
            try
            {
                Hashtable hash = criteria.GetSpParams();
                hash = logCriteria.AddSpParams(hash);
                hash = (new ResultCriteria()).AddSpParams(hash);

                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(hash);

                    criteria.FunctionId = Convert.ToInt32(db.GetParameterValue("FunctionId"));

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

        public ICommandResult Delete(ICriteria criteria, ICriteria logCriteria)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(IFunction entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(IFunction entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Delete(IFunction entity)
        {
            throw new NotImplementedException();
        }
    }
}