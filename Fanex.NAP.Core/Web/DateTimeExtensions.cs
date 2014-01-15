// --------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.10.11">Created</change>
// </history>

namespace Fanex.NAP.Core.Web
{
    using System;

    /// <summary>
    /// Contains date extension methods.
    /// </summary>
    public static class DateTimeExtensions
    {
        public const string ShortDateFormat = "MM/dd/yyyy";

        /// <summary>
        /// Gets first date of month.
        /// </summary>
        /// <param name="dateTime">DateTime type that the method needs to be attached to.</param>
        /// <returns>First date of month.</returns>
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Gets last date of month.
        /// </summary>
        /// <param name="dateTime">DateTime type that the method needs to be attached to.</param>
        /// <returns>Last date of month.</returns>
        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Format a DateTime value to short date format, separated by '-'.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as 2013-10-10.</returns>
        public static string ToShortFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortFormatedDate();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a DateTime value to short date format, separated by '-'.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as 2013-10-10.</returns>
        public static string ToShortFormatedDate(this DateTime date)
        {
            return date.ToString(ShortDateFormat);
        }

        /// <summary>
        /// Format a DateTime value to short date format, separated by '-'.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as 2013-10-10.</returns>
        public static string ToShortFormatedDateWithoutSeparator(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortFormatedDateWithoutSeparator();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a DateTime value to short date format, separated by '-'.
        /// </summary>
        /// <param name="date">DateTime type that the method needs to be attached to.</param>
        /// <returns>Formatted date such as 2013-10-10.</returns>
        public static string ToShortFormatedDateWithoutSeparator(this DateTime date)
        {
            return date.ToString(ShortDateFormat).Replace("/", string.Empty);
        }
    }
}