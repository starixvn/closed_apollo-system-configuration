// --------------------------------------------------------------------------
// <copyright file="ITraceRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.1">Created</change>
//   <change who="Lewis.Le" date="2012.10.5">Added FetchSiteUrl</change>
// </history>

namespace Fanex.NAP.Core.Repository.Interface
{
    using System.Linq;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Trace repository interface.
    /// </summary>
    public interface ITraceRepository : IDynamicRepository
    {
        IQueryable<LogExecSp> SearchLogExecSp(ICriteria criteria, out int totalRows);

        IQueryable<LogUserLogin> SearchLogUserLogin(ICriteria criteria, out int totalRows);

        IQueryable<LogFunction> SearchLogFunction(ICriteria criteria, out int totalRows);

        LogExecSp GetById(ICriteria criteria);

        ICommandResult InsertLog(ICriteria criteria);
    }
}