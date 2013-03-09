// --------------------------------------------------------------------------
// <copyright file="ISite.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.1">Created</change>
//   <change who="Lewis.Le" date="2012.8.29">Added NumOfUser</change>
// </history>

namespace Fanex.NAP.Core.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Domain;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ISite : IEntity<byte, string>
    {
        string Description { get; set; }

        string Url { get; set; }

        string TestUrl { get; set; }

        bool IsActive { get; set; }

        string Environment { get; set; }

        int NumOfUser { get; set; }
    }
}