﻿using Eiromplays.IdentityServer.Domain.Common;
using Eiromplays.IdentityServer.Identity.DTOs;

namespace Eiromplays.IdentityServer.Identity.Events.User
{
    public class UserCreatedEvent : DomainEvent
    {
        public UserCreatedEvent(UserDto userDto)
        {
            UserDto = userDto;
        }

        public UserDto UserDto { get; }
    }
}
