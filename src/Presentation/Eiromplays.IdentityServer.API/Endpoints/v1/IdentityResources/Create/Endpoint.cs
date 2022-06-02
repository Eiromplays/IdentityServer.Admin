using Eiromplays.IdentityServer.Application.Identity.IdentityResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityResources.Create;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityResourceService _identityResourceService;
    
    public Endpoint(IIdentityResourceService identityResource)
    {
        _identityResourceService = identityResource;
    }

    public override void Configure()
    {
        Post("/identity-resources");
        Summary(s =>
        {
            s.Summary = "Creates a new IdentityResource.";
        });
        Version(1);
        Policies(EIAPermission.NameFor(EIAAction.Create, EIAResource.IdentityResources));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response.Message = await _identityResourceService.CreateAsync(req.Data, ct);

        await SendOkAsync(Response, cancellation: ct);
    }
}