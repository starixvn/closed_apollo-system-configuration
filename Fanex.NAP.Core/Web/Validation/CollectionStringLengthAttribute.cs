// --------------------------------------------------------------------------
// <copyright file="CollectionStringLengthAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2012.10.9">Created</change>
// </history>

namespace Fanex.NAP.Core.Web.Validation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
        
    /// <summary>
    /// Validates StringLength of a list of strings.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CollectionStringLengthAttribute : ValidationAttribute
    {
        private StringLengthAttribute _stringLengthValidator;

        public CollectionStringLengthAttribute(int maximumLength)
            : base((Func<string>)(() => "StringLengthAttribute_ValidationError"))
        {
            this._stringLengthValidator = new StringLengthAttribute(this.MaximumLength = maximumLength)
            {
                MinimumLength = this.MinimumLength
            };
        }

        public int MaximumLength { get; private set; }

        public int MinimumLength { get; set; }

        public override string FormatErrorMessage(string name)
        {
            // bind error messages on runtime
            this._stringLengthValidator.ErrorMessage = this.ErrorMessage;
            this._stringLengthValidator.ErrorMessageResourceName = this.ErrorMessageResourceName;
            this._stringLengthValidator.ErrorMessageResourceType = this.ErrorMessageResourceType;

            return _stringLengthValidator.FormatErrorMessage(name);
        }

        public override bool IsValid(object values)
        {
            if (values is IEnumerable<string>)
            {
                foreach (string value in values as IEnumerable<string>)
                {
                    if (!_stringLengthValidator.IsValid(value))
                    {
                        return false; // exist one value is invalid
                    }
                }

                return true; // all inputs is valid
            }

            throw new InvalidCastException("Invalid input! CollectionStringLengthAttribute requires a IEnumerable<string>.");
        }
    }
}