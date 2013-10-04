// --------------------------------------------------------------------------
// <copyright file="IServiceContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Interface
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Service context interface.
    /// </summary>
    public interface IServiceContext
    {
        /// <summary>
        /// User ID.
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// Site ID.
        /// </summary>
        int SiteId { get; set; }

        string Ip { get; set; }
    }
}
