using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace AngularBFF.Net8.Api.Http
{
    public class BearerTokenHandler(ITokenService tokenService) : DelegatingHandler
    {
        private readonly ITokenService _tokenService = tokenService;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenService.GetAccessTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }
    }

    public interface ITokenService
    {
        Task<string> GetAccessTokenAsync();
    }

    public class TokenService(IHttpContextAccessor httpContextAccessor) : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<string> GetAccessTokenAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is not null)
            {
                var token = await httpContext.GetTokenAsync("access_token");
                if (string.IsNullOrEmpty(token))
                {
                    return string.Empty;
                }
                return token;
            }

            return string.Empty;
        }
    }



}
