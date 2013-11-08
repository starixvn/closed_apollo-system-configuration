// --------------------------------------------------------------------------
// <copyright file="AssignFunctionUserCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Assign user permission.
    /// </summary>
    public class AssignFunctionUserCriteria : CriteriaBase
    {
        protected readonly string AssignPermissionTpl = "<FunctionPolicy SiteId='{0}' FunctionId='{1}' Policy='{2}'></FunctionPolicy>";

        public int UserId { get; set; }

        /// <summary>
        /// Gets permission string.
        /// </summary>
        /// <value>
        /// <ROOT><FunctionPolicy SiteId="4" FunctionId="4" Policy="2"></FunctionPolicy></ROOT>.
        /// </value>
        public string Value
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (FuncRead != null)
                {
                    foreach (var funcId in FuncRead)
                    {
                        value.AppendFormat(AssignPermissionTpl, SiteId, funcId, 1); // 1 -> read
                    }
                }

                if (FuncHost != null)
                {
                    foreach (var funcId in FuncHost)
                    {
                        value.AppendFormat(AssignPermissionTpl, SiteId, funcId, 2); // 2 -> host
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        [Display(Name = "Site", ResourceType = typeof(ViewResources))]
        public int SiteId { get; set; }

        [SpParam(Ignored = true)]
        public int[] FuncRead { get; set; }

        [SpParam(Ignored = true)]
        public int[] FuncHost { get; set; }

        [SpParam(Ignored = true)]
        public int ReturnSiteId { get; set; }

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int GroupId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_AssignFunction";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}