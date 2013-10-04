// --------------------------------------------------------------------------
// <copyright file="GetByUserAndFunctionKeyIncludeGroupFunctionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Function
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get by user and key
    /// </summary>
    public class GetByUserAndFunctionKeyIncludeGroupFunctionCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string FunctionKey { get; set; }
        
        public string Environment { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_Function_GetByUserAndFunctionKeyIncludeGroup</returns>
        public override string GetSettingKey()
        {
            return "NAP_Function_GetByUserAndFunctionKeyIncludeGroup";
        }

        /// <summary>
        /// Check data's constrains
        /// </summary>
        /// <returns>True: valid; False: invalid</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
