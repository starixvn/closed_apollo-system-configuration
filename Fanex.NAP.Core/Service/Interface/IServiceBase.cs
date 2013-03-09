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
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Core.Criteria.Misc;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IBaseService
    {
        IServiceContext ServiceContext { get; set; }

        void HandleException(Exception ex);

        LogExecSpCriteria GetLogExecSpCriteria();
    }
}
