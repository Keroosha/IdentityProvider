using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IdentityProvider.Endpoints
{
    public interface IEndpoint
    {
        public string Name { get; }

        public PathString Path { get; }

        public Task<IEndpointResult> ProcessAsync(HttpContext context, CancellationToken cancellationToken = default);
    }
}