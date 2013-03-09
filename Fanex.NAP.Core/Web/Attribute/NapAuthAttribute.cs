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
    using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class NapAuthAttribute : AuthorizeAttribute
    {
        #region Member

        private static readonly int _SiteId = Configuration.NapSettings.Settings.SiteId;

        private static readonly IUserService _UserService = new UserService();

        private static readonly ITraceService _TraceService = new TraceService();

        private static readonly IFunctionService _FunctionService = new FunctionService();

        private static readonly CurrentContext _CurrentContext = new CurrentContext();

        #endregion Member

        #region Properties

        private string _functionKey;

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

        private Constant.Policy _policy;

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

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            try
            {
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
                    /*
                    #region write log function

                    var function = _FunctionService.GetByKeyAndSite(FunctionKey.ToString(), _SiteId);

                    string controller = filterContext.RouteData.Values["controller"].ToString();

                    string action = filterContext.RouteData.Values["action"].ToString();

                    string id = filterContext.RouteData.Values["id"] != null ? filterContext.RouteData.Values["id"].ToString() : string.Empty;

                    string queryString = filterContext.HttpContext.Request.QueryString.ToString();

                    string param = string.Format("Controller : {0}; Action : {1}; Params : {2}{3}",
                                        controller,
                                        action, id != string.Empty ? "id=" + id : id,
                                        id != string.Empty && queryString != string.Empty ? "&" + queryString : queryString);

                    _TraceService.InsertLogFunction(function.Id,
                                    principal.AuthInfo.UserId,
                                    _SiteId,
                                    param,
                                    _CurrentContext.IP,
                                    DateTime.Now,
                                    _CurrentContext.BrowserName);

                    #endregion
                    */
                }
                else
                {
                    CloseConnection(filterContext);
                }
            }
            catch
            {
                throw;
            }
        }

        public bool CheckFunctionAuthorization(int userId)
        {
            if (userId <= 0)
            {
                return false;
            }

            var policy = _UserService.GetUserSiteFunctionKeyPolicy(userId, _SiteId, FunctionKey);

            return policy >= (int)Policy;
        }

        private void CloseConnection(AuthorizationContext filterContext)
        {
            try
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
                    response.Redirect(string.IsNullOrWhiteSpace(settingUrl) ? "~/Admin/Common/Unauthorize" : settingUrl);
                }
            }
            catch
            {
                throw;
            }
        }

        private void SignOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}