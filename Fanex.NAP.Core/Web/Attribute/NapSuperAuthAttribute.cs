// --------------------------------------------------------------------------
// <copyright file="NapSuperAuthAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Core.Web.Attribute
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// NAP super authorize attribute.
    /// </summary>
    public class NapSuperAuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            try
            {
                if (filterContext.HttpContext == null)
                {
                    return;
                }

                if (!CheckSessionAuthorization())
                {
                    var response = filterContext.HttpContext.Response;

                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        response.Buffer = true;
                        response.StatusCode = 403; // Unauthorized status code
                        response.Write("<center><b>" + ViewResources.UnauthorizeMsg + "</b></center>");
                        response.Flush();
                        response.Close();
                        response.End();
                    }
                    else
                    {
                        response.Redirect("~/UserLogin/Login");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private bool CheckSessionAuthorization()
        {
            ICurrentSession session = new CurrentSession();

            if (session == null || string.IsNullOrEmpty(session.Username))
            {
                return false;
            }

            return true;
        }
    }
}
