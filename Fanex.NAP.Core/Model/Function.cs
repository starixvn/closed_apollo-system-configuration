// --------------------------------------------------------------------------
// <copyright file="Function.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.8">Created</change>
// </history>

namespace Fanex.NAP.Core.Model
{
    using System;
    using Fanex.NAP.Core.Domain;

    /// <summary>
    /// Function model.
    /// </summary>
    [Serializable]
    public class Function : IFunction
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        public IFunction Parent { get; set; }

        public short DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public byte Crud { get; set; }

        public string Environment { get; set; }        

        public ISite Site { get; set; }
        
        public IUser Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public IUser Modifier { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}