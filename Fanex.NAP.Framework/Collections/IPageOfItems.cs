// --------------------------------------------------------------------------
// <copyright file="IPageOfItems.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.11">Created</change>
// </history>

namespace Fanex.NAP.Framework.Collections
{
    using System.Collections.Generic;

    public interface IPageOfItems<out T> : IEnumerable<T>
    {
        int PageNumber { get; set; }

        int PageSize { get; set; }

        int TotalItemCount { get; set; }

        int TotalPageCount { get; }

        int StartPosition { get; }

        int EndPosition { get; }
    }
}
