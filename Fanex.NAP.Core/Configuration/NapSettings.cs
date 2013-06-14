// --------------------------------------------------------------------------
// <copyright file="NapSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.17">Created</change>
// </history>

namespace Fanex.NAP.Core.Configuration
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Custom config for NAP
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class NapSettings : ConfigurationSection
    {
        #region consts

        private const string __NAP_SETTINGS = "NapSettings";
        private const string __ENVIRONMENT = "Environment";
        private const string __ENABLE_DIRECT_LOGIN = "EnableDirectLogin";
        private const string __HUB_URL = "HubUrl";
        private const string __SITE_ID = "SiteId";
        private const string __UM_ADDRESS = "UMAddress";
        private const string __ENABLE_CHECK_UM = "EnableCheckUM";
        private const string __ENABLE_RECHECKIN = "EnableReCheckin";
        private const string __UNAUTHORIZE_URL = "UnauthorizeUrl";
        private const string __NAP_DOMAIN = "Domain";
        private const string __NAP_DOMAIN__ADD = "add";

        #endregion consts

        private static NapSettings _settings = ConfigurationManager.GetSection(__NAP_SETTINGS) as NapSettings;

        public static NapSettings Settings
        {
            get
            {
                return _settings;
            }
        }

        public bool IsNapHub
        {
            get
            {
                return SiteId == 0;
            }
        }

        [ConfigurationProperty(__SITE_ID, IsRequired = true)]
        public int SiteId
        {
            get { return (int)this[__SITE_ID]; }
            set { this[__SITE_ID] = value; }
        }

        [ConfigurationProperty(__ENABLE_DIRECT_LOGIN, IsRequired = true)]
        public bool EnableDirectLogin
        {
            get { return (bool)this[__ENABLE_DIRECT_LOGIN]; }
            set { this[__ENABLE_DIRECT_LOGIN] = value; }
        }

        [ConfigurationProperty(__HUB_URL, IsRequired = true)]
        public string HubUrl
        {
            get { return (string)this[__HUB_URL]; }
            set { this[__HUB_URL] = value; }
        }

        [ConfigurationProperty(__ENVIRONMENT, IsRequired = true)]
        public string Environment
        {
            get { return (string)this[__ENVIRONMENT]; }
            set { this[__ENVIRONMENT] = value; }
        }

        [ConfigurationProperty(__UM_ADDRESS, IsRequired = true)]
        public string UMAddress
        {
            get { return (string)this[__UM_ADDRESS]; }
            set { this[__UM_ADDRESS] = value; }
        }

        [ConfigurationProperty(__ENABLE_CHECK_UM, IsRequired = true)]
        public bool EnableCheckUM
        {
            get { return (bool)this[__ENABLE_CHECK_UM]; }
            set { this[__ENABLE_CHECK_UM] = value; }
        }

        [ConfigurationProperty(__ENABLE_RECHECKIN, IsRequired = true)]
        public bool EnableReCheckIn
        {
            get { return (bool)this[__ENABLE_RECHECKIN]; }
            set { this[__ENABLE_RECHECKIN] = value; }
        }

        [ConfigurationProperty(__UNAUTHORIZE_URL, IsRequired = false)]
        public string UnauthorizeUrl
        {
            get { return (string)this[__UNAUTHORIZE_URL]; }
            set { this[__UNAUTHORIZE_URL] = value; }
        }

        [ConfigurationProperty(__NAP_DOMAIN, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(DomainCollection),
            AddItemName = __NAP_DOMAIN__ADD)]
        public DomainCollection Domains
        {
            get
            {
                var domainCollection = (DomainCollection)base[__NAP_DOMAIN];

                return domainCollection;
            }
        }
    }

    /// <summary>
    /// Server Collection, map to collection in config file
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class DomainCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Controller
        /// </summary>
        public DomainCollection()
        {
        }

        /// <summary>
        /// Collection Type
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        /// <summary>
        /// Add Element
        /// </summary>
        /// <param name="mappingElement">Mapping element</param>
        public void Add(DomainElement mappingElement)
        {
            BaseAdd(mappingElement);
        }

        /// <summary>
        /// Base Add
        /// </summary>
        /// <param name="element">Element</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        /// <summary>
        /// Create New Element
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DomainElement();
        }

        /// <summary>
        /// Get Element Key
        /// </summary>
        /// <param name="element">Element</param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DomainElement)element).Name + ((DomainElement)element).Path;
        }

        /// <summary>
        /// Get Element
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns></returns>
        public DomainElement this[int index]
        {
            get
            {
                return (DomainElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }
    }

    /// <summary>
    /// Domain Element
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class DomainElement : ConfigurationElement
    {
        private const string __NAP_DOMAIN_NAME = "Name";

        private const string __NAP_DOMAIN_PATH = "Path";

        /// <summary>
        /// Name
        /// </summary>
        [ConfigurationProperty(__NAP_DOMAIN_NAME, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)this[__NAP_DOMAIN_NAME];
            }

            set
            {
                this[__NAP_DOMAIN_NAME] = value;
            }
        }

        /// <summary>
        /// Path
        /// </summary>
        [ConfigurationProperty(__NAP_DOMAIN_PATH, IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)this[__NAP_DOMAIN_PATH];
            }

            set
            {
                this[__NAP_DOMAIN_PATH] = value;
            }
        }
    }
}