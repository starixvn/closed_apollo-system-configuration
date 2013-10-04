// --------------------------------------------------------------------------
// <copyright file="ResultCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Misc
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Result
    /// </summary>
    public class ResultCriteria : CriteriaBase
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public ResultCriteria()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }
    }
}
