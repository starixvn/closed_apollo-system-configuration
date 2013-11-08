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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
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

                return value;
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
            string name = geoId.ToString();
            var listCulture = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var culture in listCulture)
            {
                var region = new RegionInfo(culture.LCID);

                if (region.GeoId.ToString() == geoId)
                {
                    return region.DisplayName;
                }
            }

            return name;
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

    /// <summary>
    /// User agent tools.
    /// <see cref="http://nerds.palmdrive.net/useragent/index.html"/>
    /// </summary>
    public static class UserAgentTools
    {
        public static string[] GetArray(string a, string b)
        {
            string[] res = new string[2];

            res[0] = a;
            res[1] = b;

            return res;
        }

        /// <summary>
        /// Get bot name.
        /// </summary>
        /// <param name="userAgent">The user agent information.</param>
        /// <returns>Bot name string array.</returns>
        public static string[] GetBotName(string userAgent)
        {
            userAgent = userAgent.ToLower();
            int pos = 0;
            string res = string.Empty;

            if ((pos = userAgent.IndexOf("help.yahoo.com/")) > -1)
            {
                res = "Yahoo";
                pos += 7;
            }
            else if ((pos = userAgent.IndexOf("google/")) > -1)
            {
                res = "Google";
                pos += 7;
            }
            else if ((pos = userAgent.IndexOf("msnbot/")) > -1)
            {
                res = "MSNBot";
                pos += 7;
            }
            else if ((pos = userAgent.IndexOf("googlebot/")) > -1)
            {
                res = "Google";
                pos += 10;
            }
            else if ((pos = userAgent.IndexOf("webcrawler/")) > -1)
            {
                res = "WebCrawler";
                pos += 11;
            }
            else if ((pos = userAgent.IndexOf("inktomi")) > -1)
            {
                res = "Inktomi";
                pos = -1;
            }
            else if ((pos = userAgent.IndexOf("teoma")) > -1)
            {
                res = "Teoma";
                pos = -1;
            }

            if (string.IsNullOrEmpty(res))
            {
                return null;
            }

            return GetArray(res, res);
        }

        /// <summary>
        /// Get operation system name.
        /// </summary>
        /// <param name="userAgent">The user agent information.</param>
        /// <returns>Operation system array string.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public static string[] GetOS(string userAgent)
        {
            string[] arrayBog = GetBotName(userAgent);

            if (arrayBog != null)
            {
                return arrayBog;
            }

            int pos;

            if ((pos = userAgent.IndexOf("Windows-NT")) > -1)
            {
                return GetArray("Win", "Window NT");
            }

            if (userAgent.IndexOf("Windows NT") > -1)
            {
                // The different versions of Windows NT are decoded in the verbosity level 2
                // IE: Windows NT 5.1 = Windows XP
                if ((pos = userAgent.IndexOf("Windows NT 5.1")) > -1)
                {
                    return GetArray("Win", "Window XP");
                }

                if ((pos = userAgent.IndexOf("Windows NT 6.0")) > -1)
                {
                    return GetArray("Win", "Window Vista");
                }

                if ((pos = userAgent.IndexOf("Windows NT 6.1")) > -1)
                {
                    return GetArray("Win", "Window 7");
                }

                if ((pos = userAgent.IndexOf("Windows NT 5.0")) > -1)
                {
                    return GetArray("Win", "Window 2000");
                }

                if ((pos = userAgent.IndexOf("Windows NT 5.2")) > -1)
                {
                    return GetArray("Win", "Window 2003");
                }

                if ((pos = userAgent.IndexOf("Windows NT 4.0")) > -1)
                {
                    return GetArray("Win", "Windows NT4");
                }

                if ((pos = userAgent.IndexOf("Windows NT)")) > -1)
                {
                    return GetArray("Win", "Windows NT");
                }

                if ((pos = userAgent.IndexOf("Windows NT;")) > -1)
                {
                    return GetArray("Win", "Windows NT");
                }
            }

            if (userAgent.IndexOf("Win") > -1)
            {
                if (userAgent.IndexOf("Windows") > -1)
                {
                    if ((pos = userAgent.IndexOf("Windows 98")) > -1)
                    {
                        return GetArray("Win", "Windows 98");
                    }

                    if ((pos = userAgent.IndexOf("Windows_98")) > -1)
                    {
                        return GetArray("Win", "Windows 98");
                    }

                    if ((pos = userAgent.IndexOf("Windows 2000")) > -1)
                    {
                        return GetArray("Win", "Window 2000");
                    }

                    if ((pos = userAgent.IndexOf("Windows 95")) > -1)
                    {
                        return GetArray("Win", "Window 95");
                    }

                    if ((pos = userAgent.IndexOf("Windows 9x")) > -1)
                    {
                        return GetArray("Win", "Window 9x");
                    }

                    if ((pos = userAgent.IndexOf("Windows ME")) > -1)
                    {
                        return GetArray("Win", "Window ME");
                    }

                    if ((pos = userAgent.IndexOf("Windows CE;")) > -1)
                    {
                        return GetArray("Win", "Window CE");
                    }

                    if ((pos = userAgent.IndexOf("Windows 3.1")) > -1)
                    {
                        return GetArray("Win", "Window 3.1");
                    }

                    // If no version was found, rely on the following code to detect "WinXX"
                    // As some User-Agents include two references to Windows
                    // Ex: Mozilla/5.0 (Windows; U; Win98; en-US; rv:1.5)
                }
            }

            if ((pos = userAgent.IndexOf("Mac OS X")) > -1)
            {
                if (userAgent.IndexOf("iPhone") > -1)
                {
                    pos = userAgent.IndexOf("iPhone OS");

                    if (userAgent.IndexOf("iPod") > -1)
                    {
                        return GetArray("iOS", "iOS-iPod");
                    }
                    else
                    {
                        return GetArray("iOS", "iOS-iPhone");
                    }
                }

                if (userAgent.IndexOf("iPad") > -1)
                {
                    return GetArray("iOS", "iOS-iPad");
                }
                else
                {
                    return GetArray("Mac", "MacOSX");
                }
            }

            if ((pos = userAgent.IndexOf("Android")) > -1)
            {
                return GetArray("Linux", "Android");
            }

            if ((pos = userAgent.IndexOf("Mac_PowerPC")) > -1)
            {
                return GetArray("Mac", "MacPPC");
            }

            if ((pos = userAgent.IndexOf("Macintosh")) > -1)
            {
                if (userAgent.IndexOf("PPC") > -1)
                {
                    return GetArray("Mac", "MacPPC");
                }
                else
                {
                    return GetArray("Mac?", "Mac?");
                }
            }

            if ((pos = userAgent.IndexOf("FreeBSD")) > -1)
            {
                return GetArray("*BSD", "*BSD FreeBSD");
            }

            if ((pos = userAgent.IndexOf("OpenBSD")) > -1)
            {
                return GetArray("*BSD", "*BSD OpenBSD");
            }

            if ((pos = userAgent.IndexOf("Linux")) > -1)
            {
                if ((pos = userAgent.IndexOf("Ubuntu/")) > -1)
                {
                    return GetArray("Linux", "Ubuntu");
                }

                return GetArray("Linux", "Linux");
            }

            if ((pos = userAgent.IndexOf("CentOS")) > -1)
            {
                return GetArray("Linux", "Linux CentOS");
            }

            if ((pos = userAgent.IndexOf("NetBSD")) > -1)
            {
                return GetArray("*BSD", "*BSD NetBSD");
            }

            if ((pos = userAgent.IndexOf("Unix")) > -1)
            {
                return GetArray("Linux", "Linux");
            }

            if ((pos = userAgent.IndexOf("SunOS")) > -1)
            {
                return GetArray("Unix", "SunOS");
            }

            if ((pos = userAgent.IndexOf("IRIX")) > -1)
            {
                return GetArray("Unix", "IRIX");
            }

            if ((pos = userAgent.IndexOf("SonyEricsson")) > -1)
            {
                return GetArray("SonyEricsson", "SonyEricsson");
            }

            if ((pos = userAgent.IndexOf("Nokia")) > -1)
            {
                return GetArray("Nokia", "Nokia");
            }

            if ((pos = userAgent.IndexOf("BlackBerry")) > -1)
            {
                return GetArray("BlackBerry", "BlackBerry");
            }

            if ((pos = userAgent.IndexOf("SymbianOS")) > -1)
            {
                return GetArray("SymbianOS", "SymbianOS");
            }

            if ((pos = userAgent.IndexOf("BeOS")) > -1)
            {
                return GetArray("BeOS", "BeOS");
            }

            if ((pos = userAgent.IndexOf("Nintendo Wii")) > -1)
            {
                return GetArray("Nintendo Wii", "Nintendo Wii");
            }

            if ((pos = userAgent.IndexOf("J2ME/MIDP")) > -1)
            {
                return GetArray("Java", "J2ME");
            }

            return GetArray("<b>?</b>", "<b>?</b>");
        }
    }

    /// <summary>
    /// Bundle helper.
    /// </summary>
    public static class BundleHelper
    {
        public static IHtmlString RenderScript(string path)
        {
            string virtualPath = path.Substring(0, path.Length - 3);
            BundleTable.Bundles.Add(new ScriptBundle(virtualPath).Include(path));

            return Scripts.Render(virtualPath);
        }
    }
}