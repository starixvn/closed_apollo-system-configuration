// --------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.10.4">Created</change>
// </history>

namespace Fanex.NAP.Admin.Core.Extensions
{
    using System;
    using System.IO;
    using Fanex.Logging;

    /// <summary>
    /// Utilities class.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Function to Read a file.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Content of file.</returns>
        public static string ReadTextFile(string filePath)
        {
            try
            {
                using (var sr = new StreamReader(filePath))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Get Date Format (year-month-day).
        /// </summary>
        /// <param name="dt">Object date need format.</param>
        /// <returns>Return string date.</returns>
        public static string ShortDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:yyyy-MM-dd}", dt);
        }

        /// <summary>
        /// Function to check is number or no.
        /// </summary>
        /// <param name="s">String input.</param>
        /// <returns>True or false.</returns>
        public static bool IsNumber(string s)
        {
            double number;
            if (double.TryParse(s, out number))
            {
                return true;
            }

            return false;
        }
    }
}
