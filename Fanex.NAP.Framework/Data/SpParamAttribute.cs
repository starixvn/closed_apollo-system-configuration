// --------------------------------------------------------------------------
// <copyright file="SpParamAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.6.25">Created</change>
//   <change who="Anders.Le" date="2012.6.25">Add comment</change>
// </history>
namespace Fanex.NAP.Framework.Data
{
    using System;

    /// <summary>
    /// Store procedure parameter attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SpParamAttribute : Attribute
    {
        public string SqlParamName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use this property to create a parameter.
        /// </summary>
        /// <value>True : ignored, false : not ignored.</value>
        public bool Ignored { get; set; }
    }
}
