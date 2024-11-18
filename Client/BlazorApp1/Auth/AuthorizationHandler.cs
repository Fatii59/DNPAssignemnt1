
namespace BlazorApp1.Auth;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

public class AuthorizationHandler : DelegatingHandler
{
    private readonly SimpleAuthProvider _authProvider;

    public AuthorizationHandler(SimpleAuthProvider authProvider)
    {
        _authProvider = authProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var token = authState.User.FindFirst("jwt")?.Value;

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
