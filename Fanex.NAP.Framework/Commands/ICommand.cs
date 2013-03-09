// --------------------------------------------------------------------------
// <copyright file="ICommand.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.6.11">Created</change>
// </history>

namespace Fanex.NAP.Framework.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;    
    
    public interface ICommand
    {
        dynamic GetParams();
    }
}
