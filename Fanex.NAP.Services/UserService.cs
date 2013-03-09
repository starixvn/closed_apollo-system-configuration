// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.NAP.Services
{
    using System;
    using Fanex.NAP.Core.Services;
    using Fanex.NAP.Core;
    using Fanex.Common.Helper;
    using Fanex.NAP.Repository;
    using Fanex.NAP.Core.Repository;


    public class UserService //: ServiceBase, IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository repository = null)
        {
            _userRepository = repository ?? new UserRepository();
        }

        public UserService() : this(null) { }

        public IUser GetByUserId(int userId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            IUser user = null;
            try
            {                
                user = _userRepository.Get(userId);
                return user;
            }
            catch (Exception ex)
            { 
            
            }

            return user;
        }
    }

}
