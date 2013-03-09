// --------------------------------------------------------------------------
// <copyright file="AuthInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Web.Auth
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Xml.Serialization;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class AuthInfo
    {
        private int _userId;

        public int UserId
        {
            get 
            { 
                return this._userId; 
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("userid");
                }

                this._userId = value;
            }
        }

        private int _onuserId;

        public int OnUserId
        {
            get 
            {
                return this._onuserId; 
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("onUserid");
                }

                this._onuserId = value;
            }
        }

        public string ToXml()
        {
            string xml = string.Empty;
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AuthInfo));
                serializer.Serialize(sw, this);
                sw.Close();
                xml = sw.ToString();
            }

            return xml;
        }

        public static AuthInfo CreateUserData(string authInfoXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AuthInfo));
            AuthInfo authInfo = (AuthInfo)serializer.Deserialize(new StringReader(authInfoXml));
            return authInfo;
        }
    }
}