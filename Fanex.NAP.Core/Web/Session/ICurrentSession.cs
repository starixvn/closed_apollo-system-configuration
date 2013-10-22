// --------------------------------------------------------------------------
// <copyright file="ICurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Web.Session
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Model.ValueObject;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ICurrentSession
    {
        string Username { get; set; }

        int UserId { get; set; }

        int Level { get; set; }

        int OnUserId { get; set; }

        string SessionId { get; set; }

        int GroupId { get; set; }

        string GroupName { get; set; }

        IList<MenuItem> Menu { get; set; }

        //Dictionary<SiteUrl, List<IMenuItem>> MenuDic { get; set; }

        void ClearAll();
    }
}