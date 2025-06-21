using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using WebBff.Handlers.Options;

namespace WebBff.Handlers
{
    public class BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
    {
        private BasicAuthenticationOptions _basicAuthenticationOptions;

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Scheme.Name == "VindiBasicAuthentication")
            {
                var username = Request.Query["username"].FirstOrDefault();
                var password = Request.Query["password"].FirstOrDefault();

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    return ValidateCredentials(username, password);
                else
                    return Task.FromResult(AuthenticateResult.Fail("Missing Username or Password"));
            }

            if (!Request.Headers.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues value))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(value);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                var username = credentials[0];
                var password = credentials[1];
                return ValidateCredentials(username, password);
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }

        private Task<AuthenticateResult> ValidateCredentials(string username, string password)
        {
            if (username == _basicAuthenticationOptions.Username && password == _basicAuthenticationOptions.Password)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
                return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
        }
    }
}