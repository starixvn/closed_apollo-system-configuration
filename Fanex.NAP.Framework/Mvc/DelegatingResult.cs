// --------------------------------------------------------------------------
// <copyright file="DelegatingResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.7.18">Created</change>
// </history>

namespace Fanex.NAP.Framework.Mvc
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// <see cref="http://haacked.com/archive/2008/05/11/delegating-action-result.aspx"/>
    /// <example>
    /// public ActionResult Hello() {
    ///  return new DelegatingResult(context => {
    ///  context.HttpContext.Response.AddHeader("something", "something");
    ///  context.HttpContext.Response.Write("Hello World!");});
    /// }
    /// </example>
    /// </summary>
    public class DelegatingResult : ActionResult
    {
        public DelegatingResult(Action<ControllerContext> command)
        {
            this.Command = command;
        }

        public Action<ControllerContext> Command
        {
            get;
            private set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.Command(context);
        }
    }
}
