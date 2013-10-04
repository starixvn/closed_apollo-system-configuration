// --------------------------------------------------------------------------
// <copyright file="GetByIdLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Trace
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get by ID.
    /// </summary>
    public class GetByIdLogCriteria : CriteriaBase
    {       
        public int LogId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_LogExecSp_GetById";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
