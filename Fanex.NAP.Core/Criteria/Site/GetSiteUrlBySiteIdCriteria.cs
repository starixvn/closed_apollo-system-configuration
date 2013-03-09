// --------------------------------------------------------------------------
// <copyright file="GetSiteUrlBySiteIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.10">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Site
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetSiteUrlBySiteIdCriteria : CriteriaBase
    {
        public int SiteId { get; set; }
        
        public string Environment { get; set; }

        /// <summary>
        /// Get the reference SP's Name which is specified in Config file.
        /// </summary>
        /// <returns>NAP_Site_GetSiteURLByUser</returns>
        public override string GetSettingKey()
        {
            return "NAP_Site_GetSiteURLBySiteId";
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
