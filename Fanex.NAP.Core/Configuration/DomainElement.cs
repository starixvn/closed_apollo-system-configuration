// --------------------------------------------------------------------------
// <copyright file="DomainElement.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.17">Created</change>
// </history>

namespace Fanex.NAP.Core.Configuration
{
    using System.Configuration;

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