// --------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.NAP.Framework.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Fanex.Common.Helper;

    /// <summary>
    /// HTML helper.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        #region Render Datetime Extension
        private static readonly DateTimeFormatInfo _DateTimeFormat = 
            DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture);

        /// <summary>
        /// Dates the picker.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The element name.</param>
        /// <param name="date">The selected date.</param>
        /// <param name="minDate">The min date.</param>
        /// <param name="maxDate">The max date, default is now.</param>
        /// <param name="formatDate">The format date.</param>
        /// <param name="clientFormateDate">Format date time of client side.</param>
        /// <param name="cssDatePicker">The CSS date picker.</param>
        /// <returns>Date picker MVC HTML string.</returns>
        public static MvcHtmlString DatePicker(
            this HtmlHelper htmlHelper, 
            string name, 
            DateTime? date,
            DateTime? minDate = null, 
            DateTime? maxDate = null, 
            string formatDate = "MMM dd, yyyy",
            string clientFormateDate = "yyyy/MM/dd", 
            string cssDatePicker = "datepicker")
        {
            Check.Argument.IsNotNull(formatDate, "formateDate");
            Check.Argument.IsNotNull(clientFormateDate, "clientFormateDate");

            if (maxDate == null || !maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }

            var textbox = htmlHelper.TextBox(
                name,
                date.HasValue ? date.Value.ToString(formatDate) : string.Empty,
                new
                {
                    @class = cssDatePicker,
                    mindate = minDate != null && minDate != DateTime.MinValue
                    ? ((DateTime)minDate).ToString(clientFormateDate)
                    : "null",
                    maxdate = ((DateTime)maxDate).ToString(clientFormateDate)
                });

            return MvcHtmlString.Create(textbox.ToString());
        }

        /// <summary>
        /// <see cref="DatePicker"/>
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="minDate">The min date.</param>
        /// <param name="maxDate">The max date.</param>
        /// <param name="formatDate">The format date.</param>
        /// <returns>Date picker MVC HTML string.</returns>
        public static MvcHtmlString DatePickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html, 
            Expression<Func<TModel, TValue>> expression, 
            DateTime? minDate = null, 
            DateTime? maxDate = null, 
            string formatDate = "MMM dd, yyyy")
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            DateTime? value = DateTime.Now;

            if (metaData.Model is DateTime?)
            {
                value = (DateTime?)metaData.Model;
            }
            else
            {
                value = null;
            }

            return html.DatePicker(metaData.PropertyName, value, minDate, maxDate, formatDate);
        }

        /// <summary>
        /// Years the picker.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The element name.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="currentYear">The current year.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Year picker MVC HTML string.</returns>
        public static MvcHtmlString YearPicker(
            this HtmlHelper htmlHelper, 
            string name, 
            int fromYear = 2006, 
            int currentYear = 2000, 
            object htmlAttributes = null)
        {
            var list = new List<SelectListItem>();
            var now = DateTime.Now.Year;

            for (var i = now; i >= fromYear; i--)
            {
                list.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = i == currentYear
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, htmlAttributes);

            return MvcHtmlString.Create(dropDownList.ToString());
        }

        /// <summary>
        /// Years the picker for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Year picker MVC HTML string.</returns>
        public static MvcHtmlString YearPickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html, 
            Expression<Func<TModel, TValue>> expression, 
            int fromYear = 2006, 
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var currentYear = (int)metaData.Model;

            return html.YearPicker(metaData.PropertyName, fromYear, currentYear, htmlAttributes);
        }
        
        /// <summary>
        /// Month picker.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="name">Month picker name.</param>
        /// <param name="currentMonth">Current month.</param>
        /// <param name="htmlAttributes">HTML attributes.</param>
        /// <returns>Month picker MVC HTML string.</returns>
        public static MvcHtmlString MonthPicker(
            this HtmlHelper htmlHelper, 
            string name, 
            int currentMonth, 
            object htmlAttributes = null)
        {
            // currentYear = currentYear ?? DateTime.Now.Year;
            var list = new List<SelectListItem>();
            var endMonth = 12; // (currentYear.Value < DateTime.Now.Year) ? 12 : DateTime.Now.Month;
            
            for (var i = endMonth; i >= 1; i--)
            {
                list.Add(new SelectListItem
                {
                    Text = _DateTimeFormat.GetMonthName(i),
                    Value = i.ToString(),
                    Selected = i == currentMonth
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, htmlAttributes);

            return MvcHtmlString.Create(dropDownList.ToString());
        }

        /// <summary>
        /// Month picker for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="currentMonth">The current month.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Month picker MVC HTML string.</returns>
        public static MvcHtmlString MonthPickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            int currentMonth, 
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            currentMonth = (int)metaData.Model;

            return html.MonthPicker(metaData.PropertyName, currentMonth, htmlAttributes);
        }
        #endregion
    }
}
