﻿namespace Eiromplays.IdentityServer.Application.DTOs.Role;

public class RoleClaimDto
{
    public int ClaimId { get; set; }

    public string? RoleId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }
}