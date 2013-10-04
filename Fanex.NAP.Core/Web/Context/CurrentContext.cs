// --------------------------------------------------------------------------
// <copyright file="CurrentContext.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Web.Context
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Web;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Configuration;

    /// <summary>
    /// Current context.
    /// </summary>
    public class CurrentContext : ICurrentContext
    {
        private const string _CONFIGNAPHUBSITEID = "SiteId";

        public string IP
        {
            get
            {
                string ip = string.Empty;

                foreach (IPAddress itemIPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
                {
                    if (itemIPA.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        ip = itemIPA.ToString();
                        break;
                    }
                }

                if (!ip.IsNullOrTrimEmpty())
                {
                    return ip;
                }

                foreach (IPAddress itemIPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (itemIPA.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        ip = itemIPA.ToString();
                        break;
                    }
                }

                return ip.Trim();
            }
        }

        public string DomainName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            }
        }

        public string BrowserName
        {
            get { return HttpContext.Current.Request.Browser.Browser + "/" + HttpContext.Current.Request.Browser.Version; }
        }

        public string Platform
        {
            get
            {
                var platform = HttpContext.Current.Request.UserAgent;

                return platform;
            }
        }

        public string CountryCode
        {
            get
            {
                RegionInfo regionInfo = this.ResolveCountry(HttpContext.Current.Request);

                return regionInfo == null ? null : regionInfo.GeoId.ToString(CultureInfo.CurrentCulture);
            }
        }

        private CultureInfo ResolveCulture(HttpRequest request)
        {
            string[] languages = request.UserLanguages;

            if (languages == null || languages.Length == 0)
            {
                return null;
            }

            try
            {
                string language = languages[0].ToLowerInvariant().Trim();

                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public DomainElement GetCurrentDomain()
        {
            var domainList = NapSettings.Settings.Domains;

            string currentDomain = HttpContext.Current.Request.Url.Authority;
            string currentPath = HttpContext.Current.Request.ApplicationPath;

            foreach (DomainElement domain in domainList)
            {
                if (string.Compare(domain.Name, currentDomain, true) == 0 && string.Compare(domain.Path, currentPath, true) == 0)
                {
                    return domain;
                }
            }

            return new DomainElement
            {
                Name = currentDomain,
                Path = currentPath
            };
        }

        private RegionInfo ResolveCountry(HttpRequest request)
        {
            CultureInfo culture = this.ResolveCulture(request);

            if (culture != null)
            {
                return new RegionInfo(culture.LCID);
            }

            return null;
        }
    }
}