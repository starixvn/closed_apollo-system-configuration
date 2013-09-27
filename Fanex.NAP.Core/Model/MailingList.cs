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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Core.Domain;

    public class MailingList : IMailingList
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }
    }
}
