// --------------------------------------------------------------------------
// <copyright file="ICommand.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.6">Created</change>
// </history>

namespace Fanex.NAP.Core.Command
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ICommand
    {
        string SettingKey { get; set; }

        IDataParameterCollection GetParameter();
    }
}