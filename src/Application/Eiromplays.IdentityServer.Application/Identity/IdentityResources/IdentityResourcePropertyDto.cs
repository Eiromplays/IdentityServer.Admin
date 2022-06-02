﻿namespace Eiromplays.IdentityServer.Application.Identity.IdentityResources;

public class IdentityResourcePropertyDto
{
    public int Id { get; set; }
    
    public string Key { get; set; } = default!;

    public string Value { get; set; } = default!;
    
    public int IdentityResourceId { get; set; }
}