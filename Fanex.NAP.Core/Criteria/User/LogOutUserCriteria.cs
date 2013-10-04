// --------------------------------------------------------------------------
// <copyright file="LogOutUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Logout user.
    /// </summary>
    public class LogOutUserCriteria : CriteriaBase
    {       
        public int OnUserId { get; set; }      

        public override string GetSettingKey()
        {
            return "NAP_User_Logout";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}