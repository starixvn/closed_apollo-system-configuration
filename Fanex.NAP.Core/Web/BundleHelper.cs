// --------------------------------------------------------------------------
// <copyright file="BundleHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Web
{
    using System.Web;
    using System.Web.Optimization;

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