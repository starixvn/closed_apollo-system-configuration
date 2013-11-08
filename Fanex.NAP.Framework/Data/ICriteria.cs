// --------------------------------------------------------------------------
// <copyright file="ICriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.11">Created</change>
//   <change who="Anders.Le" date="2012.6.25">Change property to method</change>
// </history>

namespace Fanex.NAP.Framework.Data
{
    using System.Collections;
    
    public interface ICriteria
    {
        /// <summary>
        /// Gets store procedure parameters.
        /// </summary>
        /// <returns>Hash table SP parameters.</returns>
        Hashtable GetSpParams();

        /// <summary>
        /// Add store procedure parameters.
        /// </summary>
        /// <param name="storeProcParamName">Store procedure parameter name.</param>
        /// <returns>SP parameters.</returns>
        Hashtable AddSpParams(Hashtable storeProcParamName);

        /// <summary>
        /// Gets setting key in order to get store procedure name and connection info.
        /// </summary>
        /// <returns>Setting key.</returns>
        string GetSettingKey();

        /// <summary>
        ///  Gets result from checking the criteria rule.
        /// </summary>
        /// <returns>Check result.</returns>
        bool IsValid();
    }
}
