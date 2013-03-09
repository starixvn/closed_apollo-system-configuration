// --------------------------------------------------------------------------
// <copyright file="ServiceContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.6">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Core.Service.Interface;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ServiceContext : IServiceContext
    {
        /// <summary>
        /// who perform a business action
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// which site user be in
        /// </summary>
        public int SiteId { get; set; }

        public string Ip { get; set; }
    }
}