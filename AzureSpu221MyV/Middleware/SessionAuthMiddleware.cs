using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AzureSpu221MyV.Data;
using Microsoft.Azure.Cosmos;
using AzureSpu221MyV.Services.Data;

namespace AzureSpu221MyV.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SessionAuthMiddleware
    {
        private readonly RequestDelegate _next;
        
        public SessionAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, IContainerProvider containerProvider)
        {
            var userId = httpContext.Session.GetString("auth-user-id");
            if (userId != null)
            {
                var user = GetUserFromDatabase(userId, containerProvider);

                if (user != null)
                {
                    var claims = new Claim[]
                   {
                        new Claim(ClaimTypes.Name, user.Result.Name),
                        new Claim(ClaimTypes.Email, user.Result.Email),
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.UserData,user.Result.AvatarUrl ?? ""),
                        new Claim(ClaimTypes.DateOfBirth, user.Result.Birthdate.ToString())
                   };

                    httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, nameof(SessionAuthMiddleware)));


                }
            }
            return _next(httpContext);
        }
        private async Task<Data.User> GetUserFromDatabase(string userId, IContainerProvider containerProvider)
        {
            Container container = await containerProvider.GetContainerAsync();
            var query = new QueryDefinition($"SELECT * FROM c WHERE c.id = @userId")
                .WithParameter("@userId", userId);

            var iterator = container.GetItemQueryIterator<Data.User>(query);
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                foreach (var user in response)
                {
                    return user;
                }
            }

            return null;
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SessionAuthMiddlewareExtensions
    {
        public static IApplicationBuilder UseSessionAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SessionAuthMiddleware>();
        }
    }
}
