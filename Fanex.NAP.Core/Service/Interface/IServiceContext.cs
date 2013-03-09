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

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IServiceContext
    {
        /// <summary>
        /// who perform a business action
        /// </summary>
        int UserId { get; set; }

        /// <summary>
        /// which site user be in
        /// </summary>
        int SiteId { get; set; }

        string Ip { get; set; }
    }
}
