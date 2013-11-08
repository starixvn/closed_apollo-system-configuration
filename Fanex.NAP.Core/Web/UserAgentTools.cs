// --------------------------------------------------------------------------
// <copyright file="UserAgentTools.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Web
{
    using System.Diagnostics.CodeAnalysis;

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
}