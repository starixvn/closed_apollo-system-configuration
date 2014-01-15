// --------------------------------------------------------------------------
// <copyright file="IBaseService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
//   <change who="Edward.Nguyen" date="2012.8.14">Add GetRepositoryExecLog</change>
// </history>

namespace Fanex.NAP.Core.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.Core.Criteria.Misc;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Base service interface.
    /// </summary>
    public interface IBaseService
    {
        IServiceContext ServiceContext { get; set; }

        IEnumerable<T> GetData<T>(ICriteria criteria);

        IEnumerable<T> GetData<T>(ICriteria criteria, out int totalItems);

        T GetItem<T>(ICriteria criteria) where T : new();

        void HandleException(Exception ex);

        LogExecSpCriteria GetLogExecSpCriteria();
    }
}
