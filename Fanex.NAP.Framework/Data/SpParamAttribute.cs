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
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SpParamAttribute : Attribute
    {
        /// <summary>
        /// Parameter name.
        /// </summary>
        public string SqlParamName { get; set; }

        /// <summary>
        /// Set true to indicate not use this property to create a parameter.
        /// </summary>
        public bool Ignored { get; set; }
    }
}
