// --------------------------------------------------------------------------
// <copyright file="NapAuthAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.14">Created</change>
// </history>

namespace Fanex.NAP.Core.Web.Attribute
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Context;

    /// <summary>
    /// <see cref="http://www.hanselman.com/blog/SystemThreadingThreadCurrentPrincipalVsSystemWebHttpContextCurrentUserOrWhyFormsAuthenticationCanBeSubtle.aspx"/>
    /// </summary>
    public class NapAuthAttribute : AuthorizeAttribute
    {
        #region Member

        private static readonly int _SiteId = Configuration.NapSettings.Settings.SiteId;

        private static readonly IUserService _userService = new UserService();

        private string _functionKey;

        private Constant.Policy _policy;

        #endregion Member

        #region Properties

        public string FunctionKey
        {
            get
            {
                return _functionKey == null ? string.Empty : _functionKey;
            }

            set
            {
                _functionKey = value;
            }
        }

        public Constant.Policy Policy
        {
            get
            {
                return _policy == Constant.Policy.None ? Constant.Policy.Read : _policy;
            }

            set
            {
                _policy = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// Authorize action handler.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext == null)
            {
                CloseConnection(filterContext);
                return;
            }

            // why it is null?
            var principal = filterContext.HttpContext.User as NapPrincipal;

            if (principal == null)
            {
                principal = new NapPrincipal(filterContext.HttpContext);
            }

            if (ReferenceEquals(principal, null) || ReferenceEquals(principal.AuthInfo, null))
            {
                CloseConnection(filterContext);
                return;
            }

            if (CheckFunctionAuthorization(principal.AuthInfo.UserId))
            {
                // allow access
                // log access function here
                // write log function here (pending due to performance)
            }
            else
            {
                CloseConnection(filterContext);
            }
        }

        /// <summary>
        /// Check function authorization.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>True : has permission, false : not.</returns>
        public bool CheckFunctionAuthorization(int userId)
        {
            if (userId <= 0)
            {
                return false;
            }

            var policy = _userService.GetUserSiteFunctionKeyPolicy(userId, _SiteId, FunctionKey);

            return policy >= (int)Policy;
        }

        /// <summary>
        /// Close connection.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        private void CloseConnection(AuthorizationContext filterContext)
        {
            if (filterContext == null
                || filterContext.HttpContext == null
                || filterContext.HttpContext.Request == null)
            {
                SignOut();
                return;
            }

            HttpResponseBase response = filterContext.HttpContext.Response;

            if (response == null)
            {
                SignOut();
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpStatusCodeResult(403, "<center><b>" + ViewResources.UnauthorizeMsg + "</b></center>");

                response.Buffer = true;
                response.StatusCode = 403; // Unauthorized status code
                response.Write("<center><b>" + ViewResources.UnauthorizeMsg + "</b></center>");
                response.Flush();
                response.Close();
                response.End();
            }
            else if (!filterContext.HttpContext.Request.IsAuthenticated)
            {
                SignOut();
            }
            else
            {
                string settingUrl = NapSettings.Settings.UnauthorizeUrl;
                response.Redirect(string.IsNullOrWhiteSpace(settingUrl) ? "~/Admin/Common/Unauthorize" : settingUrl, false);
            }
        }

        private void SignOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}