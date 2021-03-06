using Eiromplays.IdentityServer.Application.Auditing;
using Eiromplays.IdentityServer.Application.Common.Models;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Logs.Search;

public class Endpoint : Endpoint<AuditLogListFilter, PaginationResponse<AuditDto>>
{
    private readonly IAuditService _auditService;

    public Endpoint(IAuditService auditService)
    {
        _auditService = auditService;
    }

    public override void Configure()
    {
        Post("/logs/search");
        Summary(s =>
        {
            s.Summary = "Search logs using available filters.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Search, EiaResource.AuditLog));
    }

    public override async Task HandleAsync(AuditLogListFilter request, CancellationToken ct)
    {
        Response = await _auditService.SearchAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}