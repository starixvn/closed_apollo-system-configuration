// --------------------------------------------------------------------------
// <copyright file="IMailingList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.27">Created</change>
// </history>

namespace Fanex.NAP.Core.Domain
{
    using Fanex.NAP.Framework.Domain;

    public interface IMailingList : IEntity<short, string>
    {
        string Key { get; set; }

        string Description { get; set; }

        ISite Site { get; set; }

        string Environment { get; set; }

        IFunction MailFunction { get; set; }

        IFunction Function { get; set; }

        string ToList { get; set; }

        string CCList { get; set; }

        string BCCList { get; set; }

        bool IsInternal { get; set; }

        bool IsActive { get; set; }

        byte Policy { get; set; }
    }
}
