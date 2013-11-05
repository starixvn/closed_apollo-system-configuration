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
    /// Maintainance service.
    /// </summary>
    public class MaintainanceService : BaseService, IMaintainanceService
    {
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

            try
            {
                // gets Agent Site UM Status.
                var umInfo = umctrl.GetMaintenanceInfo(Sites.CSD, NapSettings.Settings.UMAddress);

                if (!ReferenceEquals(umInfo, null))
                {
                    isdowntime = umInfo.Status;
                    timeFrom = umInfo.Start_time;
                    timeTo = umInfo.End_time;
                }
                else
                {
                    return downTime;
                }

                isdowntime = umInfo.Status;
                timeFrom = umInfo.Start_time;
                timeTo = umInfo.End_time;
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
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