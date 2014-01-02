// --------------------------------------------------------------------------
// <copyright file="ObjectHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.NAP.Framework.Helpers
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Object helper.
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// Get property information.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Property information.</returns>
        public static PropertyInfo GetPropertyInfo(object obj, string propertyName)
        {
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return propertyInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Property value.</returns>
        public static object GetValue(object obj, string propertyName)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(obj, propertyName);

            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj, null);
            }

            return null;
        }

        /// <summary>
        /// Check if has property information.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>True : has, false : not have.</returns>
        public static bool HasPropertyInfo(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);

            return propertyInfo != null;
        }
    }
}
