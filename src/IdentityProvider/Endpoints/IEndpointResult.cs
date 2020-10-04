using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IdentityProvider.Endpoints
{
    public interface IEndpointResult
    {
        public string Name { get; }
        public Task ExecuteAsync(HttpContext context, CancellationToken cancellationToken = default);
    }
}