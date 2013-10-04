// --------------------------------------------------------------------------
// <copyright file="SettingKeyAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.11">Created</change>
// </history>

namespace Fanex.NAP.Framework.Commands
{
    using System;

    /// <summary>
    /// Setting key.
    /// </summary>
    public class SettingKeyAttribute : Attribute
    {
        public SettingKeyAttribute(string settingKey)
        {
        }

        public SettingKeyAttribute(string connectionStringName, string storeProcedureName)
        {
        }
    }
}
