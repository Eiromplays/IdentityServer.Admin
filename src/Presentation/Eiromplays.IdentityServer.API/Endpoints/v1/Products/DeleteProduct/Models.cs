using Eiromplays.IdentityServer.Application.Catalog.Products;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Products.DeleteProduct;

public class Models
{
    public class Request
    {
        public Guid Id { get; set; }
    }

    public class Response
    {
        public Guid Id { get; set; }
    }
}