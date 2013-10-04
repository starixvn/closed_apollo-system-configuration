// --------------------------------------------------------------------------
// <copyright file="ITraceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.11.14">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Interface
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ITraceService : IBaseService
    {
        IQueryable<LogExecSp> SearchExecSp(
           int pageIndex,
           int pageSize,
           out int totalRows,
           string name,
           string username,
           DateTime from,
           DateTime to,
           int filterBy = 0);

        IQueryable<LogUserLogin> SearchUserLogin(
           int pageIndex,
           int pageSize,
           out int totalRows,
           string name,
           DateTime from,
           DateTime to,
           int siteId = 0);

        IQueryable<LogFunction> SearcFunction(
           int pageIndex,
           int pageSize,
           out int totalRows,
           string name,
           DateTime from,
           DateTime to,
           int siteId = 0);

        LogExecSp GetById(int logId);

        ICommandResult InsertLogUserLogin(
            int userId,
            int siteId,
            string ip,
            DateTime date,
            string domain,
            string broswser,
            string platform,
            string countryCode,
            string doing,
            int duration);

        ICommandResult InsertLogFunction(
            int functionId,
            int userId,
            int siteId,
            string param,
            string ip,
            DateTime date,
            string broswser);
    }
}