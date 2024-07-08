using AzureSpu221MyV.Services.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using AzureSpu221MyV.Data;
using Microsoft.AspNetCore.Session;
using Microsoft.Azure.Cosmos;

namespace AzureSpu221MyV.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IContainerProvider _containerProvider;

        public AuthController(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
        }
        [HttpGet]
        public async Task<object> Get(string email, string password)
        {
           
            var container = await _containerProvider.GetContainerAsync();

            var sqlQueryText = "SELECT * FROM c WHERE c.partitionKey = 'users' AND c.email = @Email";
            var queryDefinition = new QueryDefinition(sqlQueryText)
                .WithParameter("@Email", email);

            var queryResultSetIterator = container.GetItemQueryIterator<Data.User>(queryDefinition);
            var response = await queryResultSetIterator.ReadNextAsync();

            var user = response.FirstOrDefault();

            string status;
            if (user == null)
            {
                // Если пользователь не найден, возвращаем ошибку
                status = "error";
            }
            else
            {
                // Если пользователь найден, устанавливаем данные аутентификации в сеанс
                status = "success";
                HttpContext.Session.SetString("auth-user-id", user.Id.ToString());
            }

            return new { status };
        }
    }
}
