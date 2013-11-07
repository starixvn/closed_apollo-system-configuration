// --------------------------------------------------------------------------
// <copyright file="IMaintainanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.29">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Interface
{
    using Fanex.NAP.Core.Model.ValueObject;

    /// <summary>
    /// Maintenance repository interface.
    /// </summary>
    public interface IMaintainanceService : IBaseService
    {
        Downtime GetDowntimeInfo();
    }
}