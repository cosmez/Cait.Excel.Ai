using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Cait.Excel.Ai.Services;
using ExcelDna.Integration;
using Microsoft.Extensions.AI;

namespace Cait.Excel.Ai
{
    public class Prompts
    {
        private static readonly object PromptCallSentinel = new object();
        private static readonly string System = @"
            Eres un bot de soporte para Excel, cuando se te pida algun calculo solo responde con la respuesta.
            Responde sin markdown, solo texto plano y sin emojis a menos que se te pida.
        ";

        [ExcelDna.Integration.ExcelFunction(description: "Prompt a OpenAI")]
        public static object OpenAi(
            [ExcelArgument(Name = "Input", Description = "Para para enviar a OpenAI")]
            string input)
        {
            string system = System;
            double temperature = 1.0;

            var result = ExcelAsyncUtil.Run(nameof(OpenAi), new object[] { input, system, temperature }, delegate
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                
                var task = Task.Run(() => PromptAsync(input, system, temperature, "gpt-4o-mini", AiServiceType.OpenAi));
                return task.Result;
            });

            if (result.Equals(ExcelError.ExcelErrorNA))
            {
                return "Pensando..";
            }

            return result;
        }

        [ExcelDna.Integration.ExcelFunction(description: "Prompt a Gemini")]
        public static object Gemini(
            [ExcelArgument(Name = "Input", Description = "Para para enviar a Gemini")]
            string input)
        {
            string system = System;
            double temperature = 1.0;

            var result = ExcelAsyncUtil.Run(nameof(Gemini), new object[] { input, system, temperature }, delegate
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                var task = Task.Run(() => PromptAsync(input, system, temperature, "gemini-1.5-flash", AiServiceType.Google));
                return task.Result;
            });

            if (result.Equals(ExcelError.ExcelErrorNA))
            {
                return "Pensando..";
            }

            return result;
        }



        public static async Task<string> PromptAsync(string input,string system, double temperature, 
            string model, AiServiceType aiServiceType)
        {
            
            var chatClient = AiServiceFactory.GetChatClient(model, aiServiceType);
            var chatMessages = new List<ChatMessage>();
            if (!string.IsNullOrEmpty(system))
            {
                chatMessages.Add(new ChatMessage(ChatRole.System, system));
            }
            chatMessages.Add(new ChatMessage(ChatRole.User, input));
            var chatCompletionOptions = new ChatOptions()
            {
                Temperature = (float)temperature
            };
            var complete = await chatClient.CompleteAsync(chatMessages, chatCompletionOptions);

            return complete.Choices[0].Text ?? string.Empty;
        }
         
    }
}
