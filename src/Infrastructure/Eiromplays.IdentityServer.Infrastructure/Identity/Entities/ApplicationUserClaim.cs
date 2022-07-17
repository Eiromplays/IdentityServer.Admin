﻿using Eiromplays.IdentityServer.Domain.Common.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

public class ApplicationUserClaim : IdentityUserClaim<string>, IAuditableEntity
{
    public string? CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }

    public ApplicationUserClaim()
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }
}