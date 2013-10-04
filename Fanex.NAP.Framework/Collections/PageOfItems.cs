// --------------------------------------------------------------------------
// <copyright file="PageOfItems.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.11">Created</change>
// </history>

namespace Fanex.NAP.Framework.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Page of items.
    /// </summary>
    public class PageOfItems<T> : List<T>, IPageOfItems<T>
    {
        public PageOfItems(IEnumerable<T> items)
        {
            this.AddRange(items);
        }

        #region IPageOfItems<T> Members

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

        public int TotalPageCount
        {
            get
            {
                return (int)Math.Ceiling((double)this.TotalItemCount / this.PageSize);
            }
        }

        public int StartPosition
        {
            get
            {
                return (this.PageNumber - 1) * (this.PageSize + 1);
            }
        }

        public int EndPosition
        {
            get
            {
                return this.PageNumber * this.PageSize > this.TotalItemCount
                ? this.TotalItemCount
                : this.PageNumber * this.PageSize;
            }
        }

        #endregion
    }
}