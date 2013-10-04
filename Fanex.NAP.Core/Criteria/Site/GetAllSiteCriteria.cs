// --------------------------------------------------------------------------
// <copyright file="GetAllSiteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.16">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Site
{
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all
    /// </summary>
    public class GetAllSiteCriteria : CriteriaBase
    {
        public GetAllSiteCriteria()
        {
            Environment = Constant.UAT;
        }

        public string Environment { get; set; }

        public bool? IsActive { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_Site_GetAll.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Site_GetAll";
        }

        /// <summary>
        /// Check data's constrains
        /// </summary>
        /// <returns>True: valid; False: invalid.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
