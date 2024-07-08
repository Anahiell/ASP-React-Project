using ASP_SPU221_HMW.Models.Home.SignUp;
using ASP_SPU221_HMW.Services.Upload;
using AzureSpu221MyV.Models;
using AzureSpu221MyV.Models.Languages;
using AzureSpu221MyV.Models.Translator;
using AzureSpu221MyV.Services.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Diagnostics;
using System.Text;

namespace AzureSpu221MyV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IContainerProvider _containerProvider;
        private readonly IUploadService _uploadService;


        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IContainerProvider containerProvider, IUploadService uploadService)
        {
            _logger = logger;
            _configuration = configuration;
            _containerProvider = containerProvider;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public ViewResult UserPageAuth()
        {
            return View();
        }

        public async Task<IActionResult> CosmosAsync(SignUpFormModel? FormModel)
        {
            if (FormModel.AvatarFile != null)
            {
                try
                {
                    FormModel.SavedFileName =
                        _uploadService.SaveFormFile(
                            FormModel.AvatarFile,
                            "wwwroot/img/avatars");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            Container container = await _containerProvider.GetContainerAsync();
            var sqlQueryText = "SELECT * FROM c WHERE c.partitionKey = 'users'";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Data.User> queryResultSetIterator =
                container.GetItemQueryIterator<Data.User>(queryDefinition);

            List<Data.User> users = [];

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Data.User> currentResultSet =
                    await queryResultSetIterator.ReadNextAsync();
                foreach (Data.User user in currentResultSet)
                {
                    users.Add(user);

                }
            }
            ViewData["users"] = users;
            return View();
        }
        [HttpPost]
        public IActionResult SignInAsync(string email, string password)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUpAsync(SignUpFormModel signUpFormModel)
        {
           
            Container container = await _containerProvider.GetContainerAsync();
            String salt = Guid.NewGuid().ToString()[..3];
            Data.User user = new()
            {
                Id = Guid.NewGuid(),
                Name = signUpFormModel.UserName,
                Email = signUpFormModel.UserEmail,
                Salt = salt,
                DK = salt + signUpFormModel.UserPassword,
                Birthdate = DateOnly.FromDateTime(signUpFormModel.Birthdate!.Value),
                AvatarUrl =  signUpFormModel.SavedFileName?.ToString()

            };
            ItemResponse<Data.User> response =
                await container.CreateItemAsync<Data.User>(
                    user, new PartitionKey(user.PartitionKey));
            _logger.LogInformation("Created item in database with id: {0} Operation consumed {1} RUs.\n", response.Resource.Id, response.RequestCharge);

            return RedirectToAction("Cosmos");
        }
        public async Task<IActionResult> DeleteUser([FromRoute] String id)
        {
            Container container = await _containerProvider.GetContainerAsync();
            ItemResponse<Data.User> response =
                await container.DeleteItemAsync<Data.User>(id, new PartitionKey("users"));

            _logger.LogInformation("Delete item in database with id: {0} Operation consumed {1} RUs.\n", id, response.RequestCharge);

            return RedirectToAction("Cosmos");
        }
        public async Task<IActionResult> TranslatorAsync(TranslatorFormModel? formModel)
        {
            using HttpClient client = new();
            TranslatorViewModel model = new()
            {
                FormModel = formModel,
                LanguageResponse = LanguageResponse
                    .FromJson(await client
                    .GetStringAsync("https://api.cognitive.microsofttranslator.com/languages?api-version=3.0"))

            };
            if (formModel?.IsGoPressed ?? false)
            {
                model.TranslatorResponse = await GetTranslatorResponse(formModel);
            }
            return View(model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpPost]
        public async Task<JsonResult> TranslateApi(TranslatorFormModel formModel)
        {
            if (string.IsNullOrWhiteSpace(formModel.OriginalText))
            {
                return Json(new { error = "Original text is empty." });
            }
            var translatorResponse = await GetTranslatorResponse(formModel);
            return Json(translatorResponse.items.First().translations.First());
        }
        private async Task<TranslatorResponse> GetTranslatorResponse(TranslatorFormModel formModel)
        {
            var azureSection = _configuration.GetSection("azure");
            var translatorSection = azureSection.GetSection("translator");
            if (translatorSection == null)
            {
                throw new Exception("Configuration load error (missing azuresetting.json'?)");
            }
            String? endpoint = translatorSection.GetSection("endpoint").Value;
            String? region = translatorSection.GetSection("region").Value;
            String? key = translatorSection.GetSection("key").Value;
            if (endpoint == null || region == null || key == null)
            {
                throw new Exception("Configuration parse error(missing 'endpoint' or 'region' or 'key')");
            }
            string path = $"/translate?api-version=3.0&from={formModel.LanguageFrom}&to={formModel.LanguageTo}";
            object[] body = [new { Text = formModel.OriginalText }];
            var requestBody = System.Text.Json.JsonSerializer.Serialize(body);



            using HttpRequestMessage request = new();

            // Build the request.
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(endpoint + path);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            // location required if you're using a multi-service or regional (not global) resource.
            request.Headers.Add("Ocp-Apim-Subscription-Region", region);
            using HttpClient client = new();
            // Send the request and get response.
            HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
            // Read response as a string.
            string result = await response.Content.ReadAsStringAsync();
            return new()
            {
                items = System.Text.Json.JsonSerializer.Deserialize<List<ResponseItem>>(result)!
            };
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
/*
 [{"translations":[{"text":"Я б дуже ...!","to":"uk"}]}]
*/