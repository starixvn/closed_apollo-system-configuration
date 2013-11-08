// --------------------------------------------------------------------------
// <copyright file="PagerList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.24">Created</change>
// </history>

namespace Fanex.NAP.Core.Helpers
{
    using System.Collections.Generic;
    using Webdiyer.WebControls.Mvc;

    /// <summary>
    /// Page list.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public class PagerList<T> : List<T>, IPagedList
    {
        public PagerList(int pageIndex, int pageSize, int totalItemCount, IEnumerable<T> items)
        {
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
            TotalItemCount = totalItemCount;
            AddRange(items);
        }

        public int CurrentPageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }
    }
}