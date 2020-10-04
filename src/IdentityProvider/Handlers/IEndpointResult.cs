using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IdentityProvider.Handlers
{
    public interface IEndpointResult
    {
        public string Name { get; }
        public Task ApplyAsync(HttpContext context, CancellationToken cancellationToken = default);
    }
}