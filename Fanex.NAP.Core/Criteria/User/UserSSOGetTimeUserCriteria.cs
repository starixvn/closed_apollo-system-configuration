// --------------------------------------------------------------------------
// <copyright file="UserSSOGetTimeUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.27">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// SSO get time.
    /// </summary>
    public class UserSSOGetTimeUserCriteria : CriteriaBase
    {
        public DateTime Time { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_SSOGetTime";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}