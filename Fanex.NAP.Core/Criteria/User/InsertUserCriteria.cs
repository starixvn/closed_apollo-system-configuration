// --------------------------------------------------------------------------
// <copyright file="InsertUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Insert user.
    /// </summary>
    public class InsertUserCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_Insert";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}