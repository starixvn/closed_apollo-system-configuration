// --------------------------------------------------------------------------
// <copyright file="UpdatePasswordCustCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.21">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.User
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Update customer password.
    /// </summary>
    public class UpdatePasswordCustCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(Ignored = true)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Old_Password", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidFieldValidationOldPassword",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(Ignored = true)]
        public string OldUserPwd { get; set; }

        [Required]
        [Display(Name = "New_Password", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidFieldValidationNewPassword",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string UserPwd { get; set; }        

        [Required]
        [Display(Name = "Confirm_Password", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        [RegularExpression(@"^[\w\s]{1,}$",
            ErrorMessageResourceName = "InvalidFieldValidationConfirmPassword",
            ErrorMessageResourceType = typeof(ViewResources))]
        [SpParam(Ignored = true)]
        [Compare("UserPwd",
            ErrorMessageResourceName = "PropertiesMustMatchErrMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string ConfirmUserPwd { get; set; }
        
        public override string GetSettingKey()
        {
            return "NAP_Cust_UpdatePassword";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}