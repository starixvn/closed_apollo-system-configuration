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
    /// Custom configuration for NAP.
    /// </summary>    
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

        public bool EnableDirectLogin
        {
            get
            {
                var result = false;
                var check = bool.TryParse(_EnableDirectLogin, out result);

                result = check ? result : Equals(_EnableDirectLogin, "1");

                return result;
            }

            set
            {
                _EnableDirectLogin = value.ToString();
            }
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

        public bool EnableCheckUM
        {
            get
            {
                var result = false;
                var check = bool.TryParse(this._EnableCheckUM, out result);

                result = check ? result : Equals(this._EnableCheckUM, "1");

                return result;
            }

            set
            {
                this._EnableCheckUM = value.ToString();
            }
        }        

        public bool EnableReCheckIn
        {
            get
            {
                var result = false;
                var check = bool.TryParse(_EnableReCheckIn, out result);

                result = check ? result : Equals(_EnableReCheckIn, "1");

                return result;
            }

            set
            {
                _EnableReCheckIn = value.ToString();
            }
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

        [ConfigurationProperty(__ENABLE_DIRECT_LOGIN, IsRequired = true)]
        private string _EnableDirectLogin
        {
            get { return (string)this[__ENABLE_DIRECT_LOGIN]; }
            set { this[__ENABLE_DIRECT_LOGIN] = value; }
        }

        [ConfigurationProperty(__ENABLE_CHECK_UM, IsRequired = true)]
        private string _EnableCheckUM
        {
            get { return (string)this[__ENABLE_CHECK_UM]; }
            set { this[__ENABLE_CHECK_UM] = value; }
        }

        [ConfigurationProperty(__ENABLE_RECHECKIN, IsRequired = true)]
        private string _EnableReCheckIn
        {
            get { return (string)this[__ENABLE_RECHECKIN]; }
            set { this[__ENABLE_RECHECKIN] = value; }
        }
    }

    /// <summary>
    /// Server Collection, map to collection in configuration file.
    /// </summary>    
    public class DomainCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainCollection"/> class.
        /// </summary>
        public DomainCollection()
        {
        }

        /// <summary>
        /// Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection" />.
        /// </summary>
        /// <returns>The <see cref="T:System.Configuration.ConfigurationElementCollectionType" /> of this collection.</returns>
        /// <value>Configuration element collection.</value>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="DomainElement"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="DomainElement"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns>Return domain element object.</returns>
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

        /// <summary>
        /// Adds the specified mapping element.
        /// </summary>
        /// <param name="mappingElement">The mapping element.</param>
        public void Add(DomainElement mappingElement)
        {
            BaseAdd(mappingElement);
        }

        /// <summary>
        /// Adds a configuration element to the <see cref="T:System.Configuration.ConfigurationElementCollection" />.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to add.</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DomainElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement" /> to return the key for.</param>
        /// <returns>
        /// An <see cref="T:System.Object" /> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement" />.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DomainElement)element).Name + ((DomainElement)element).Path;
        }        
    }

    /// <summary>
    /// Domain Element.
    /// </summary>    
    public class DomainElement : ConfigurationElement
    {
        private const string __NAP_DOMAIN_NAME = "Name";

        private const string __NAP_DOMAIN_PATH = "Path";

        /// <summary>
        /// Gets or sets the domain name.
        /// </summary>
        /// <value>
        /// The domain name.
        /// </value>
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
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path name.
        /// </value>
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