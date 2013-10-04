// --------------------------------------------------------------------------
// <copyright file="UpdateStatusGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.20">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Group
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update group status.
    /// </summary>
    public class UpdateStatusGroupCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Group_UpdateStatus";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}