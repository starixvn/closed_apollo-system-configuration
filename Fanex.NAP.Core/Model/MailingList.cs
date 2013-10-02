// --------------------------------------------------------------------------
// <copyright file="MailingList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.9.27">Created</change>
// </history>

namespace Fanex.NAP.Core.Model
{
    using Fanex.NAP.Core.Domain;

    /// <summary>
    /// MailingList Class.
    /// </summary>
    public class MailingList : IMailingList
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        public ISite Site { get; set; }

        public string Environment { get; set; }

        public IFunction MailFunction { get; set; }

        public IFunction Function { get; set; }

        public string ToList { get; set; }

        public string CCList { get; set; }

        public string BCCList { get; set; }

        public bool IsInternal { get; set; }

        public bool IsActive { get; set; }

        public byte Policy { get; set; }
    }
}
