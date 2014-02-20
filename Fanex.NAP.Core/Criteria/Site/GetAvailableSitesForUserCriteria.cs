// -----------------------------------------------------------------------
// <copyright file="GetAvailableSitesForUserCriteria.cs" company="Nexcel Solutions Viet Nam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Core.Criteria.Site
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get available sites for user.
    /// </summary>
    public class GetAvailableSitesForUserCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public bool LiveSitesOnly { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_User_GetAvailableSitesForUser";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
