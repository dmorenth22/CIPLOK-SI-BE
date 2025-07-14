using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CIPLOK_SI_BE.Middleware
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Skip login endpoint
            if (context.Request.Path.StartsWithSegments("/api/Authienticaton/login", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Authorization header missing or invalid.");
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrWhiteSpace(token) || !token.Contains("."))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid JWT format.");
                return;
            }

            try
            {
                var claims = DecodeJwtClaims(token);

                var identity = new ClaimsIdentity(claims, "jwt");
                context.User = new ClaimsPrincipal(identity);

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync($"Failed to process token: {ex.Message}");
            }
        }

        private List<Claim> DecodeJwtClaims(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Token must have header, payload, and signature");

            var payload = parts[1];
            // Pad if necessary
            payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');

            var jsonBytes = Convert.FromBase64String(payload);
            var json = Encoding.UTF8.GetString(jsonBytes);

            var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

            var claims = new List<Claim>();
            foreach (var item in data!)
            {
                if (item.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var sub in item.Value.EnumerateArray())
                    {
                        claims.Add(new Claim(item.Key, sub.GetString()!));
                    }
                }
                else
                {
                    claims.Add(new Claim(item.Key, item.Value.ToString() ?? ""));
                }
            }

            return claims;
        }
    }
}
