// --------------------------------------------------------------------------
// <copyright file="UpdateStatusFunctionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Function
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update status function.
    /// </summary>
    public class UpdateStatusFunctionCriteria : CriteriaBase
    {
        public int FunctionId { get; set; }

        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_Function_UpdateStatus.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Function_UpdateStatus";
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
