// --------------------------------------------------------------------------
// <copyright file="AssignFunctionToUsersCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.11.1">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Function
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Assign function to users.
    /// </summary>
    public class AssignFunctionToUsersCriteria : CriteriaBase
    {
        protected readonly string AssignPermissionTpl = "<FunctionPolicy UserId='{0}' Policy='{1}'></FunctionPolicy>";

        public int FunctionId { get; set; }

        public int SiteId { get; set; }

        /// <summary>
        /// Gets permission string.
        /// </summary>
        /// <value>
        /// <ROOT><User UserId="1" Policy="2"></User></ROOT>.
        /// </value>
        public string Value
        {
            get
            {
                var value = new StringBuilder("<ROOT>");

                if (UserRead != null)
                {
                    foreach (var userId in UserRead)
                    {
                        value.AppendFormat(AssignPermissionTpl, userId, 1); // 1 -> read
                    }
                }

                if (UserHost != null)
                {
                    foreach (var userId in UserHost)
                    {
                        value.AppendFormat(AssignPermissionTpl, userId, 2); // 2 -> host
                    }
                }

                return value.Append("</ROOT>").ToString();
            }
        }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [SpParam(Ignored = true)]
        public List<string> UserRead { get; set; }

        [SpParam(Ignored = true)]
        public List<string> UserHost { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        /// <value>Setting key : NAP_Function_AssignFunctionToUsers </value>
        public override string GetSettingKey()
        {
            return "NAP_Function_AssignFunctionToUsers";
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}