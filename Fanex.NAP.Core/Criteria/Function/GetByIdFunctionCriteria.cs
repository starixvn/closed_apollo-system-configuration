// --------------------------------------------------------------------------
// <copyright file="GetByIdFunctionCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get by ID.
    /// </summary>
    public class GetByIdFunctionCriteria : CriteriaBase
    {
        public int FunctionId { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_Function_GetById.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Function_GetById";
        }

        /// <summary>
        /// Check data's constrains.
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
