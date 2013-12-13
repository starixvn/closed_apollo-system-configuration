// --------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Business;

    /// <summary>
    /// HTML Helpers.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        #region Truncate
        /// <summary>
        /// Truncates a string if it is longer than a given length.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="value">Input string.</param>
        /// <param name="length">Maximum length.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString Truncate(this HtmlHelper htmlHelper, string value, int length)
        {
            var builder = new TagBuilder("a");

            builder.MergeAttribute("href", "javascript:alert(this.title)");
            builder.MergeAttribute("title", value);
            builder.InnerHtml = "[...]";
            value = value.Length <= length
                ? value
                : string.Format("{0} {1}", value.Substring(0, length), builder.ToString());

            return new HtmlString(value);
        }
        #endregion

        #region Radio Button List

        /// <summary>
        /// Radio button list helper.
        /// </summary>
        /// <param name="helper">HTML helper.</param>
        /// <param name="name">The element name.</param>
        /// <param name="items">List of select list item.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString RadioButtonList(
            this HtmlHelper helper,
            string name,
            IEnumerable<SelectListItem> items)
        {
            object htmlAttributes = null;
            return helper.RadioButtonList(name, items, htmlAttributes);
        }

        /// <summary>
        /// Radio button list helper.
        /// </summary>
        /// <param name="helper">HTML helper.</param>
        /// <param name="name">The element name.</param>
        /// <param name="items">List of select list item.</param>
        /// <param name="htmlAttributes">HTML attributes.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString RadioButtonList(
            this HtmlHelper helper,
            string name,
            IEnumerable<SelectListItem> items,
            object htmlAttributes)
        {
            return helper.RadioButtonList(name, items, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Radio button list helper.
        /// </summary>
        /// <param name="helper">HTML helper.</param>
        /// <param name="name">The element name.</param>
        /// <param name="items">List of select list item.</param>
        /// <param name="htmlAttributes">HTML attributes.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString RadioButtonList(
            this HtmlHelper helper,
            string name,
            IEnumerable<SelectListItem> items,
            IDictionary<string, object> htmlAttributes)
        {
            Check.Argument.IsNotEmpty(name, "name");

            var rdoList = new StringBuilder();
            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();

            rdoList.Append("<div class='rdolist'>");

            foreach (var item in items)
            {
                string itemId = string.Format("{0}-{1}", name, item.Value);

                htmlAttributes.Remove("id");
                htmlAttributes.Add("id", itemId);

                rdoList.Append(helper.RadioButton(name, item.Value, item.Selected, htmlAttributes));
                rdoList.Append(helper.Label(itemId, item.Text));
            }

            rdoList.Append("</div>");

            return new HtmlString(rdoList.ToString());
        }

        /// <summary>
        /// Radio button list helper.
        /// </summary>
        /// <typeparam name="TModel">Model object type.</typeparam>
        /// <typeparam name="TProperty">Property object type.</typeparam>
        /// <param name="helper">HTML helper.</param>
        /// <param name="expression">Property expression.</param>
        /// <param name="items">List of select list item.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items)
        {
            object htmlAttributes = null;
            return helper.RadioButtonListFor(expression, items, htmlAttributes);
        }

        /// <summary>
        /// Radio button list helper.
        /// </summary>
        /// <typeparam name="TModel">Model object type.</typeparam>
        /// <typeparam name="TProperty">Property object type.</typeparam>
        /// <param name="helper">HTML helper.</param>
        /// <param name="expression">Property expression.</param>
        /// <param name="items">List of select list item.</param>
        /// <param name="htmlAttributes">HTML attributes.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items,
            object htmlAttributes)
        {
            return helper.RadioButtonListFor(
                expression,
                items,
                (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Radio button list helper.
        /// </summary>
        /// <typeparam name="TModel">Model object type.</typeparam>
        /// <typeparam name="TProperty">Property object type.</typeparam>
        /// <param name="helper">HTML helper.</param>
        /// <param name="expression">Property expression.</param>
        /// <param name="items">List of select list item.</param>
        /// <param name="htmlAttributes">HTML attributes.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items,
            IDictionary<string, object> htmlAttributes)
        {
            Check.Argument.IsNotNull(expression, "expression");

            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            string name = ExpressionHelper.GetExpressionText(expression);
            var rdoList = new StringBuilder();

            rdoList.Append("<div class='rdolist'>");

            foreach (var item in items)
            {
                string itemId = string.Format("{0}-{1}", name, item.Value);

                htmlAttributes.Remove("id");
                htmlAttributes.Add("id", itemId);

                rdoList.Append(helper.RadioButtonFor(expression, item.Value, htmlAttributes));
                rdoList.Append(helper.Label(itemId, item.Text));
            }

            rdoList.Append("</div>");

            return new HtmlString(rdoList.ToString());
        }

        #endregion

        /// <summary>
        /// Converts the value of the current System.DateTime object to its equivalent string
        /// representation using the specified format.
        /// </summary>
        /// <param name="value">Date time value.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="timeFormat">Time format.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString ToFullDateString(
            this DateTime value, 
            string dateFormat = "MMM dd, yyyy",
            string timeFormat = "HH:mm tt")
        {
            return new HtmlString(
                string.Format(
                    "<span class='datetime'><span class='date'>{0}</span> <span class='time'>{1}</span></span>",
                    value.ToString(dateFormat),
                    value.ToString(timeFormat)));
        }

        /// <summary>
        /// Converts the value of the user level to its equivalent string representation.
        /// </summary>
        /// <param name="value">User level value.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString ToUserLevel(this int value)
        {
            string level;

            switch (value)
            {
                case (int)Constant.Level.PortalAdmin:
                    level = ViewResources.Portal_Admin;
                    break;

                case (int)Constant.Level.SiteAdmin:
                    level = ViewResources.Site_Admin;
                    break;

                case (int)Constant.Level.User:
                    level = ViewResources.User;
                    break;

                default:
                    level = string.Empty;
                    break;
            }

            return new HtmlString(level);
        }

        /// <summary>
        /// Convert the value of the policy to string.
        /// </summary>
        /// <param name="value">Policy value.</param>
        /// <returns>HTML string.</returns>
        public static IHtmlString ToPolicy(this int value)
        {
            string policy = string.Empty;

            switch (value)
            {
                case (int)Constant.Policy.Host:
                    policy = ViewResources.Host;
                    break;

                case (int)Constant.Policy.Read:
                    policy = ViewResources.Read;
                    break;

                default:
                    policy = ViewResources.None;
                    break;
            }

            return new HtmlString(policy);
        }

        /// <summary>
        /// Decode xml of [f_EncodeXMLValue].
        /// </summary>
        /// <param name="value">Encoded XML value.</param>
        /// <returns>XML value.</returns>
        public static string DecodeXMLValue(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("&amp;", "&");
                value = value.Replace("&lt;", "<");
                value = value.Replace("&gt;", ">");
                value = value.Replace("&quot;", "\"");
                value = value.Replace("&apos;", "''");
            }

            return value;
        }

        /// <summary>
        /// Get location name.
        /// </summary>
        /// <param name="geoId">Geographical ID.</param>
        /// <returns>Geographical name.</returns>
        public static string GetGeoName(this string geoId)
        {
            var listCulture = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in listCulture)
            {
                var region = new RegionInfo(culture.LCID);

                if (region.GeoId.ToString().Equals(geoId))
                {
                    return region.DisplayName;
                }
            }

            return geoId;
        }

        /// <summary>
        /// Get operating system form user agent string.
        /// </summary>
        /// <param name="platform">The platform.</param>
        /// <returns>Operating system.</returns>
        public static string GetOperatingSystem(this string platform)
        {
            string[] array = UserAgentTools.GetOS(platform);
            return array != null ? array[1] : platform;
        }
    }
}