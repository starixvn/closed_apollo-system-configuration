// --------------------------------------------------------------------------
// <copyright file="DownloadResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Frank.Ngo" date="2012.07.10">Created</change>
// </history>

namespace Fanex.NAP.Framework.Mvc
{
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    /// <see cref="http://haacked.com/archive/2008/05/10/writing-a-custom-file-download-action-result-for-asp.net-mvc.aspx"/>
    /// </summary>
    public class DownloadResult : ActionResult
    {
        public DownloadResult()
        {
        }

        public DownloadResult(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }

        public string VirtualPath
        {
            get;
            set;
        }

        public string FileDownloadName
        {
            get;
            set;
        }

        public string ErrorUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Execute result.
        /// </summary>
        /// <param name="context">Controller context.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (!string.IsNullOrEmpty(this.FileDownloadName))
            {
                this.FileDownloadName = this.FileDownloadName.Replace(" ", string.Empty);
                context.HttpContext.Response.AddHeader(
                    "content-disposition",
                    "attachment; filename=" + this.FileDownloadName);
            }

            string filePath = context.HttpContext.Server.MapPath(this.VirtualPath);
            if (!File.Exists(filePath))
            {
                context.HttpContext.Response.Redirect(this.ErrorUrl);
            }
            else
            {
                context.HttpContext.Response.TransmitFile(filePath);
            }
        }
    }
}
