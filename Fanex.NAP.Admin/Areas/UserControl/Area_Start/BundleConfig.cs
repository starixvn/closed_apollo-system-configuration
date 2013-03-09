// --------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.10.29">Style Cop</change>
// </history>

namespace Fanex.NAP.Admin.Areas.UserControl.Area_Start
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Areas/UserControl/themes/styles").Include(
                "~/Areas/UserControl/themes/reset.css",
                "~/Areas/UserControl/themes/jquery-ui-1.8.23.smoothness.css",
                "~/Areas/UserControl/scripts/lib/tipsy/tipsy-1.0.0.css",
                "~/Areas/UserControl/scripts/lib/msgbox/msgbox-0.0.1.css"));

            bundles.Add(new ScriptBundle("~/Areas/UserControl/jquery").Include(
                "~/Areas/UserControl/scripts/lib/jquery-1.8.2.js"));

            bundles.Add(new ScriptBundle("~/Areas/UserControl/js").Include(
                "~/Areas/UserControl/scripts/lib/jquery.easing.1.3.js",
                "~/Areas/UserControl/scripts/lib/jquery-ui-1.8.24.js",
                "~/Areas/UserControl/scripts/lib/jquery.unobtrusive-ajax.js",
                "~/Areas/UserControl/scripts/lib/jquery.validate*",
                "~/Areas/UserControl/scripts/lib/tipsy/tipsy-1.0.0.js",
                "~/Areas/UserControl/scripts/lib/msgbox/msgbox-0.0.1.js",
                "~/Areas/UserControl/scripts/lib/jquery.cookie.js",
                "~/Areas/UserControl/scripts/NAP.js",
                "~/Areas/UserControl/scripts/_Layout.js",
                 "~/Areas/UserLogin/Script/ReCheckIn.js"));
        }
    }
}