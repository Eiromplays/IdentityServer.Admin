﻿using AutoMapper;
using Eiromplays.IdentityServer.Application.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

[AutoMap(typeof(UserDto), ReverseMap = true)]
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string? DisplayName { get; set; }

    [ProtectedPersonalData]
    public override string? Email { get; set; }

    [PersonalData]
    public string? ProfilePicture { get; set; }

    [PersonalData]
    public string? GravatarEmail { get; set; }

    [PersonalData]
    public double Credits { get; set; }

    [PersonalData]
    public string? DiscordId { get; set; }
    
    public DateTime Created { get; init; }
    public string? CreatedBy { get; init; }

    public DateTime? LastModified { get; init; }
    public string? LastModifiedBy { get; init; }
    
    public string? ObjectId { get; set; }
}