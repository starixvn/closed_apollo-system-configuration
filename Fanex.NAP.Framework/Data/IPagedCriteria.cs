// --------------------------------------------------------------------------
// <copyright file="IPagedCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.19">Created</change>
// </history>

namespace Fanex.NAP.Framework.Data
{
    public interface IPagedCriteria : ICriteria
    {
        int PageIndex { get; set; }

        int PageSize { get; set; }

        int TotalRecords { get; set; }
    }
}
