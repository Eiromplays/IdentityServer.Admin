namespace Eiromplays.IdentityServer.Application.Identity.IdentityResources;

public class CreateIdentityResourceRequestValidator : CustomValidator<CreateIdentityResourceRequest>
{
    public CreateIdentityResourceRequestValidator(IStringLocalizer<CreateIdentityResourceRequestValidator> T)
    {
        RuleFor(p => p.Name)
            .NotEmpty();

        RuleFor(p => p.DisplayName)
            .NotEmpty()
            .MaximumLength(75);

        RuleFor(p => p.Description)
            .NotEmpty()
            .MaximumLength(75);
    }
}