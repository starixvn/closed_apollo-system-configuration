// --------------------------------------------------------------------------
// <copyright file="IGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.1">Created</change>
// </history>

namespace Fanex.NAP.Core.Domain
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Domain;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IGroup : IEntity<short, string>
    {
        string Description { get; set; }

        bool IsActive { get; set; }

        IUser Creator { get; set; }

        DateTime CreatedDate { get; set; }

        IUser Modifier { get; set; }

        DateTime ModifiedDate { get; set; }
    }
}