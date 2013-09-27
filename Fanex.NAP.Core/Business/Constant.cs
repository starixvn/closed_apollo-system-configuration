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
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
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

        public const string IT = "IT";

        public enum Policy
        {
            None = 0,
            Read = 1,
            Host = 2
        }

        public enum Crud
        {
            Read = 0,
            Host = 1
        }

        public enum Level
        {
            User = 0,
            SiteAdmin = 50,
            PortalAdmin = 100
        }
    }
}