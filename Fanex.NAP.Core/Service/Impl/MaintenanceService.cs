// --------------------------------------------------------------------------
// <copyright file="MaintenanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.29">Created</change>
// </history>

namespace Fanex.NAP.Core.Service.Impl
{
    using Apollo.UMService;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Model.ValueObject;
    using Fanex.NAP.Core.Service.Interface;

    /// <summary>
    /// Maintenance service.
    /// </summary>
    public class MaintenanceService : BaseService, IMaintenanceService
    {
        /// <summary>
        /// Get down time information.
        /// </summary>
        /// <returns>Down time object.</returns>
        public Downtime GetDowntimeInfo()
        {
            var downTime = new Downtime
            {
                From = "0:00",
                To = "0:00",
                IsDowntime = false
            };

            if (NapSettings.Settings.EnableCheckUM)
            {
                var umctrl = new UMBusiness();

                // gets Agent Site UM Status.
                var maintenanceInfo = umctrl.GetMaintenanceInfo(Sites.CSD, NapSettings.Settings.UMAddress);

                if (!ReferenceEquals(maintenanceInfo, null))
                {
                    if (maintenanceInfo.Status)
                    {
                        downTime.From = maintenanceInfo.Start_time;
                        downTime.To = maintenanceInfo.End_time;
                        downTime.IsDowntime = true;
                    }
                }
            }

            return downTime;
        }
    }
}