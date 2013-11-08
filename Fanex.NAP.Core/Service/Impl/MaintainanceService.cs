// --------------------------------------------------------------------------
// <copyright file="MaintainanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.29">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using System;
    using Apollo.UMService;
    using Fanex.Logging;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Interface;

    /// <summary>
    /// Maintenance service.
    /// </summary>
    public class MaintainanceService : BaseService, IMaintainanceService
    {
        /// <summary>
        /// Get down time information.
        /// </summary>
        /// <returns>Down time object.</returns>
        public Downtime GetDowntimeInfo()
        {
            bool isdowntime = false;
            var downTime = new Downtime
            {
                From = "0:00",
                To = "0:00",
                IsDowntime = false
            };

            if (!NapSettings.Settings.EnableCheckUM)
            {
                return downTime;
            }

            string timeFrom = string.Empty;
            string timeTo = string.Empty;
            var umctrl = new UMBusiness();

            // gets Agent Site UM Status.
            var maintenanceInfo = umctrl.GetMaintenanceInfo(Sites.CSD, NapSettings.Settings.UMAddress);

            if (!ReferenceEquals(maintenanceInfo, null))
            {
                isdowntime = maintenanceInfo.Status;
                timeFrom = maintenanceInfo.Start_time;
                timeTo = maintenanceInfo.End_time;
            }
            else
            {
                return downTime;
            }

            if (isdowntime)
            {
                downTime.From = timeFrom;
                downTime.To = timeTo;
                downTime.IsDowntime = true;
            }

            return downTime;
        }
    }
}