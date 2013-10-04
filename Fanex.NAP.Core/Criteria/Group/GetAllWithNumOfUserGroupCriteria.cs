// --------------------------------------------------------------------------
// <copyright file="GetAllWithNumOfUserGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.20">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Group
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all with number of user.
    /// </summary>
    public class GetAllWithNumOfUserGroupCriteria : CriteriaBase
    {
        public bool? IsActive { get; set; }
       
        [Display(Name = "Group_Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 0,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string GroupName { get; set; }

        public string Environment { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_Group_GetAllWithNumOfUser";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}