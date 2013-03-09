// --------------------------------------------------------------------------
// <copyright file="DTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.11.5">Created</change>
// </history>

namespace Fanex.NAP.Sample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.NAP.Framework.Domain;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EntityBase<TId, TName> : IEntity<TId, TName>
    {
        public TId Id
        {
            get;
            set;
        }

        public TName Name
        {
            get;
            set;
        }
    }

    /// TODO: Update summary.
    /// </summary>
    public class Reseller : EntityBase<int, string>
    {
        public Reseller()
        {
            this.Licensees = new List<Licensee>();
        }

        public List<Licensee> Licensees { get; set; }

        public override bool Equals(object obj)
        {
            return this.Id == (obj as Reseller).Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode() + Name.GetHashCode();
        }
    }

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Licensee : EntityBase<int, string>
    {
        public Reseller Reseller { get; set; }
    }
}