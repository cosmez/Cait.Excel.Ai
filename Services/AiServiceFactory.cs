using System;
using System.ClientModel;
using Microsoft.Extensions.AI;
using System.Threading.Tasks;
using OpenAI;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace Cait.Excel.Ai.Services
{
    public class AiServiceFactory
    {
        private static string GetApiKey(AiServiceType aiServiceType)
        {

            switch (aiServiceType)
            {
                case AiServiceType.OpenAi:
                {
                    string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
                    return apiKey;
                }
                case AiServiceType.AzureOpenAi:
                {
                    string apiKey = Environment.GetEnvironmentVariable("AZURE_API_KEY");
                    return apiKey;
                }
                case AiServiceType.Google:
                {
                    string apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
                    return apiKey;
                }
                case AiServiceType.Together:
                {
                    string apiKey = Environment.GetEnvironmentVariable("TOGETHERAI_API_KEY");
                    return apiKey;
                }
                case AiServiceType.Local:
                {
                    return null;
                }
            }

            throw new Exception($"No configuration provided for Type: {aiServiceType}");
        }

        public static IChatClient GetChatClient(string model, AiServiceType aiServiceType, string endPointUrl = null)
        {
            var apiKey = GetApiKey(aiServiceType);
            var apiCredentials = new ApiKeyCredential(apiKey);
            switch (aiServiceType)
            {
                case AiServiceType.Google:
                {
                    string baseUrl = "https://generativelanguage.googleapis.com/v1beta/openai/";
                    var clientOptions = new OpenAIClientOptions()
                    {
                        Endpoint = new Uri(baseUrl)
                    };
                    var client = new ChatClient(model, apiCredentials, clientOptions);
                    return client.AsChatClient();
                }
                case AiServiceType.Together:
                {
                    string baseUrl = "https://api.together.xyz/v1";
                    var clientOptions = new OpenAIClientOptions()
                    {
                        Endpoint = new Uri(baseUrl)
                    };
                    var client = new ChatClient(model, apiCredentials, clientOptions);
                    return client.AsChatClient();
                }
                case AiServiceType.Local:
                {
                    if (endPointUrl is null) throw new ArgumentNullException(nameof(endPointUrl));
                    var clientOptions = new OpenAIClientOptions()
                    {
                        Endpoint = new Uri(endPointUrl)
                    };
                    var client = new ChatClient(model, apiCredentials, clientOptions);
                    return client.AsChatClient();
                }
                case AiServiceType.AzureOpenAi:
                {
                    //aiConfiguration.BaseUrl = configuration.BaseUrl;
                    //aiConfiguration.ApiVersion = "2024-07-01-preview";
                    //aiConfiguration.Model = aiConfiguration.Model.Replace("azure-", "");
                    //return new OpenAiService(_databaseService, _userService, aiConfiguration);
                    break;
                }
                case AiServiceType.OpenAi:
                default:
                {
                    var client = new ChatClient(model, apiCredentials);
                    return client.AsChatClient();
                }
            }

            throw new Exception($"No configuration provided for Type: {aiServiceType}");

        }
    }
}
