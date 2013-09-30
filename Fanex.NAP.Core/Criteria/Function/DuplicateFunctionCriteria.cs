// --------------------------------------------------------------------------
// <copyright file="DuplicateFunctionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.26">Created</change>
// </history>

namespace Fanex.NAP.Core.Criteria.Function
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Framework.Data;
    
    public class DuplicateFunctionCriteria : CriteriaBase
    {
        public int DuplicateFunctionId { get; set; }

        public int FunctionId { get; set; }

        public int SiteId { get; set; }

        [SpParam(Ignored = true)]
        public string SiteName { get; set; }

        [Required]
        [Display(Name = "Function_Name", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string FunctionName { get; set; }

        [Display(Name = "Function_Key", ResourceType = typeof(ViewResources))]
        [StringLength(50, MinimumLength = 2,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string FunctionKey { get; set; }

        [Display(Name = "Description", ResourceType = typeof(ViewResources))]
        [StringLength(200, MinimumLength = 0,
            ErrorMessageResourceName = "InputLengthValidationMsg",
            ErrorMessageResourceType = typeof(ViewResources))]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        [Display(Name = "CRUD", ResourceType = typeof(ViewResources))]
        public byte Crud
        {
            get
            {
                return Readonly ? (byte)Constant.Crud.Read : (byte)Constant.Crud.Host;
            }
        }

        [Display(Name = "Read_only", ResourceType = typeof(ViewResources))]
        public bool Readonly { get; set; }

        [Required]
        [Display(Name = "Display_Order", ResourceType = typeof(ViewResources))]
        public int DisplayOrder { get; set; }

        [Required]
        [Display(Name = "Status", ResourceType = typeof(ViewResources))]
        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Environment", ResourceType = typeof(ViewResources))]
        public string Environment { get; set; }

        [SpParam(Ignored = true)]
        public int ReturnSiteId { get; set; }

        public DuplicateFunctionCriteria()
        {
            this.Environment = Constant.UAT;
        }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        /// <value>Setting key : NAP_Function_Duplicate</value>
        public override string GetSettingKey()
        {
            return "NAP_Function_Duplicate";
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
