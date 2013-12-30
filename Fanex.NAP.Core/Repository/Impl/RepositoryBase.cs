// --------------------------------------------------------------------------
// <copyright file="RepositoryBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kos.Nguyen" date="2012.8.15">Created</change>
// </history>

namespace Fanex.NAP.Core.Repository.Impl
{
    using System;
    using System.Collections;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Repository base.
    /// </summary>
    /// <typeparam name="T">Object type.</typeparam>
    public class RepositoryBase<T> : ReadonlyRepository<T>
    {
        /// <summary>
        /// Get command result.
        /// </summary>
        /// <param name="hash">Result data.</param>
        /// <returns>Command result.</returns>
        public CommandResult GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errcode"];
            var errMsg = hash["errmsg"];

            return new CommandResult
            {
                Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }       
    }
}