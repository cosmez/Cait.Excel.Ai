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
        public static IChatClient GetChatClient(string model, string proveedor, string endPointUrl = null)
        {
            var configuration = ConfigurationManager.GetConfiguration();
            var apiKey = configuration.ApiKey;
            var apiCredentials = new ApiKeyCredential(apiKey);
            switch (proveedor)
            {
                case "Gemini":
                {
                    string baseUrl = "https://generativelanguage.googleapis.com/v1beta/openai/";
                    var clientOptions = new OpenAIClientOptions()
                    {
                        Endpoint = new Uri(baseUrl)
                    };
                    var client = new ChatClient(model, apiCredentials, clientOptions);
                    return client.AsChatClient();
                }
                case "TogetherAI":
                {
                    string baseUrl = "https://api.together.xyz/v1";
                    var clientOptions = new OpenAIClientOptions()
                    {
                        Endpoint = new Uri(baseUrl)
                    };
                    var client = new ChatClient(model, apiCredentials, clientOptions);
                    return client.AsChatClient();
                }
                case "Local":
                {
                    if (endPointUrl is null) throw new ArgumentNullException(nameof(endPointUrl));
                    var clientOptions = new OpenAIClientOptions()
                    {
                        Endpoint = new Uri(endPointUrl)
                    };
                    var client = new ChatClient(model, apiCredentials, clientOptions);
                    return client.AsChatClient();
                }
                case "AzureOpenAI":
                {
                    //aiConfiguration.BaseUrl = configuration.BaseUrl;
                    //aiConfiguration.ApiVersion = "2024-07-01-preview";
                    //aiConfiguration.Model = aiConfiguration.Model.Replace("azure-", "");
                    //return new OpenAiService(_databaseService, _userService, aiConfiguration);
                    break;
                }
                case "OpenAI":
                default:
                {
                    var client = new ChatClient(model, apiCredentials);
                    return client.AsChatClient();
                }
            }

            throw new Exception($"No hay configuracion para: {proveedor}");

        }

        public static string GetDefaultModel(string proveedor)
        {
            switch (proveedor)
            {
                case "OpenAI":
                    return "gpt-4o-mini";
                case "AzureOpenAI":
                    return "gpt-4o-mini";
                case "Gemini":
                    return "gemini-1.5-flash";
                case "TogetherAI":
                    return "meta-llama/Llama-3.2-11B-Vision-Instruct-Turbo";
                case "Local":
                    return "llama3.2";
                default:
                    throw new Exception($"No hay configuracion para:  {proveedor}");
            }
        }
    }
}
