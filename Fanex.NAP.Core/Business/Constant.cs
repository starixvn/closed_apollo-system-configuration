// --------------------------------------------------------------------------
// <copyright file="Constant.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.8">Created</change>
// </history>

namespace Fanex.NAP.Core.Business
{
    /// <summary>
    /// The constant class.
    /// </summary>
    public static class Constant
    {
        public const string SPACE = " ";

        public const string OK = "OK";

        public const string ERROR = "ERROR";

        public const string UAT = "UAT";

        public const string LIVE = "LIVE";

        public const string SiteId = "SiteId";

        public const string Environment = "Environment";

        public const int SearchAllSite = -9999;

        /// <summary>
        /// The user policy.
        /// </summary>
        public const string IT = "IT";

        public enum Policy
        {
            /// <summary>
            /// The none permission.
            /// </summary>
            None = 0,

            /// <summary>
            /// The read permission.
            /// </summary>
            Read = 1,

            /// <summary>
            /// The host permission.
            /// </summary>
            Host = 2
        }

        /// <summary>
        /// Crud permission.
        /// </summary>
        public enum Crud
        {
            /// <summary>
            /// The read permission.
            /// </summary>
            Read = 0,

            /// <summary>
            /// The host permission.
            /// </summary>
            Host = 1
        }

        /// <summary>
        /// User level.
        /// </summary>
        public enum Level
        {
            /// <summary>
            /// The user level.
            /// </summary>
            User = 0,

            /// <summary>
            /// The site admin.
            /// </summary>
            SiteAdmin = 50,

            /// <summary>
            /// The portal admin.
            /// </summary>
            PortalAdmin = 100
        }
    }
}