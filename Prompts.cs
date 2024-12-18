using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cait.Excel.Ai.Services;
using ExcelDna.Integration;
using Microsoft.Extensions.AI;

namespace Cait.Excel.Ai
{
    public class Prompts
    {
        private static readonly object PromptCallSentinel = new object();

        [ExcelDna.Integration.ExcelFunction(description: "Prompt a AI por defecto")]
        public static object Prompt(
            [ExcelArgument(Name = "Input", Description = "Para a enviar")]
            string input,
            [ExcelArgument(Description = "Parametro adicional")] object[,] range = null)
        {
            var result = ExcelAsyncUtil.Run(nameof(Prompt), new object[] { input, range }, delegate
            {
                var config = ConfigurationManager.GetConfiguration();
                string system = config.System;
                double temperature = config.Temperature;
                string model = config.Model;
                string proveedor = config.Provider;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (string.IsNullOrEmpty(model))
                {
                    MessageBox.Show("No se ha configurado el proveedor IA", "Cosme AI Tools -> Configuracion");
                    return ExcelError.ExcelErrorGettingData;
                }

                var sb = new System.Text.StringBuilder();
                sb.Append(input);

                if (range != null)
                {
                    foreach (var cell in range)
                    {
                        if (cell != null && !(cell is ExcelEmpty) && !(cell is ExcelMissing))
                        {
                            sb.Append(' ');
                            sb.Append(cell.ToString());
                        }
                    }
                }

                var task = Task.Run(() => PromptAsync(sb.ToString(), system, temperature, model, proveedor));
                return task.Result;
            });

            if (result.Equals(ExcelError.ExcelErrorNA))
            {
                return "Pensando..";
            }

            return result;
        }

        [ExcelDna.Integration.ExcelFunction(description: "Prompt a AI con multiple salidas de manera vertical")]
        public static object PromptVertical(
            [ExcelArgument(Name = "Input", Description = "Para a enviar")]
            string input,
            [ExcelArgument(Description = "Parametros adicionales")] object[,] range = null)
        {
            var result = ExcelAsyncUtil.Run(nameof(PromptVertical), new object[] { input, range }, delegate
            {
                var config = ConfigurationManager.GetConfiguration();
                string system = config.System;
                double temperature = config.Temperature;
                string model = config.Model;
                string proveedor = config.Provider;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (string.IsNullOrEmpty(model))
                {
                    MessageBox.Show("No se ha configurado el proveedor IA", "Cosme AI Tools -> Configuracion");
                    return ExcelError.ExcelErrorGettingData;
                }

                var sb = new System.Text.StringBuilder();
                sb.Append(input);

                if (range != null)
                {
                    foreach (var cell in range)
                    {
                        if (cell != null && !(cell is ExcelEmpty))
                        {
                            sb.Append(' ');
                            sb.Append(cell.ToString());
                        }
                    }
                }

                var task = Task.Run(() => PromptMultipleAsync(sb.ToString(), system, temperature, model, proveedor));
                var output =  task.Result;
                var result2d = new object[output.Count, 1];  

                for (int i = 0; i < output.Count; i++)
                {
                    result2d[i, 0] = output[i];  
                }

                return result2d;
            });

            if (result.Equals(ExcelError.ExcelErrorNA))
            {
                return "Pensando..";
            }

            return result;
        }


        [ExcelDna.Integration.ExcelFunction(description: "Prompt a AI con multiple salidas de manera horizontal")]
        public static object PromptHorizontal(
            [ExcelArgument(Name = "Input", Description = "Para a enviar")]
            string input,
            [ExcelArgument(Description = "Parametros adicionales")] object[,] range = null)
        {
            var result = ExcelAsyncUtil.Run(nameof(PromptHorizontal), new object[] { input, range }, delegate
            {
                var config = ConfigurationManager.GetConfiguration();
                string system = config.System;
                double temperature = config.Temperature;
                string model = config.Model;
                string proveedor = config.Provider;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (string.IsNullOrEmpty(model))
                {
                    MessageBox.Show("No se ha configurado el proveedor IA", "Cosme AI Tools -> Configuracion");
                    return ExcelError.ExcelErrorGettingData;
                }

                var sb = new System.Text.StringBuilder();
                sb.Append(input);

                if (range != null)
                {
                    foreach (var cell in range)
                    {
                        if (cell != null && !(cell is ExcelEmpty))
                        {
                            sb.Append(' ');
                            sb.Append(cell.ToString());
                        }
                    }
                }

                var task = Task.Run(() => PromptMultipleAsync(sb.ToString(), system, temperature, model, proveedor));
                var output = task.Result;
                var result2d = new object[1, output.Count];

                for (int i = 0; i < output.Count; i++)
                {
                    result2d[0, i] = output[i];
                }

                return result2d;
            });

            if (result.Equals(ExcelError.ExcelErrorNA))
            {
                return "Pensando..";
            }

            return result;
        }


        [ExcelDna.Integration.ExcelFunction(description: "Genera un Resumen")]
        public static object PromptResumen(
            [ExcelArgument(Description = "Contenido a resumir")] object[,] range,
            [ExcelArgument(Name = "Instrucciones", Description = "Instrucciones Adicionales")] string instrucciones = "")
        {
            var result = ExcelAsyncUtil.Run(nameof(PromptResumen), new object[] { range, instrucciones }, delegate
            {
                var config = ConfigurationManager.GetConfiguration();
                string system = config.System;
                double temperature = config.Temperature;
                string model = config.Model;
                string proveedor = config.Provider;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (string.IsNullOrEmpty(model))
                {
                    MessageBox.Show("No se ha configurado el proveedor IA", "Cosme AI Tools -> Configuracion");
                    return ExcelError.ExcelErrorGettingData;
                }

                var sb = new System.Text.StringBuilder();
                sb.AppendLine("Genera un resumen con el siguiente contenido de las celdas seleccionadas en un archivo de Excel: ");

                foreach (var cell in range)
                {
                    if (cell != null && !(cell is ExcelEmpty))
                    {
                        sb.Append(' ');
                        sb.Append(cell.ToString());
                    }
                }

                if (!string.IsNullOrEmpty(instrucciones))
                {
                    sb.AppendLine($"Con las siguientes instrucciones especiales: {instrucciones}");
                }

                var task = Task.Run(() => PromptAsync(sb.ToString(), system, temperature, model, proveedor));
                return task.Result;
            });

            if (result.Equals(ExcelError.ExcelErrorNA))
            {
                return "Pensando..";
            }

            return result;
        }

        [ExcelDna.Integration.ExcelFunction(description: "Traduce el Contenido")]
        public static object PromptTraduce(
            [ExcelArgument(Description = "Contenido a traducir")] object[,] range,
            [ExcelArgument(Name = "Idioma", Description = "Idioma a traducir")] string idioma = "english")
        {
            var result = ExcelAsyncUtil.Run(nameof(PromptResumen), new object[] { range, idioma }, delegate
            {
                var config = ConfigurationManager.GetConfiguration();
                string system = config.System;
                double temperature = config.Temperature;
                string model = config.Model;
                string proveedor = config.Provider;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (string.IsNullOrEmpty(model))
                {
                    MessageBox.Show("No se ha configurado el proveedor IA", "Cosme AI Tools -> Configuracion");
                    return ExcelError.ExcelErrorGettingData;
                }

                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"Genera un traduccion con el siguiente contenido de las celdas seleccionadas a Idioma {idioma}: ");
                sb.AppendLine("```");
                foreach (var cell in range)
                {
                    if (cell != null && !(cell is ExcelEmpty))
                    {
                        sb.Append(' ');
                        sb.Append(cell.ToString());
                    }
                }
                sb.AppendLine("```");


                var task = Task.Run(() => PromptAsync(sb.ToString(), system, temperature, model, proveedor));
                return task.Result;
            });

            if (result.Equals(ExcelError.ExcelErrorNA))
            {
                return "Pensando..";
            }

            return result;
        }


       

        /// <summary>
        /// Simplest prompt method, single input, single response
        /// </summary>
        /// <param name="input"></param>
        /// <param name="system"></param>
        /// <param name="temperature"></param>
        /// <param name="model"></param>
        /// <param name="proveedor"></param>
        /// <returns></returns>
        public static async Task<string> PromptAsync(string input,string system, double temperature, 
            string model, string proveedor)
        {
            
            var chatClient = AiServiceFactory.GetChatClient(model, proveedor);
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


        /// <summary>
        /// Single input, multiple outputs
        /// </summary>
        /// <param name="input"></param>
        /// <param name="system"></param>
        /// <param name="temperature"></param>
        /// <param name="model"></param>
        /// <param name="proveedor"></param>
        /// <returns></returns>
        public static async Task<List<string>> PromptMultipleAsync(string input, string system, double temperature,
            string model, string proveedor)
        {

            var chatClient = AiServiceFactory.GetChatClient(model, proveedor);
            var chatMessages = new List<ChatMessage>();

            var sb = new StringBuilder();
            sb.AppendLine("Response con un arreglo en json con el siguiente schema: ");
            sb.AppendLine("```");
            sb.AppendLine(@"
                {
                  ""type"": ""object"",
                  ""properties"": {
                    ""response"": {
                      ""type"": ""array"",
                      ""items"": {
                        ""type"": ""string""
                      }
                    }
                  }
                }
            ");
            sb.AppendLine("```");
            sb.AppendLine("el siguiente prompt:");
            sb.AppendLine(input);

            if (!string.IsNullOrEmpty(system))
            {
                chatMessages.Add(new ChatMessage(ChatRole.System, system));
            }
            chatMessages.Add(new ChatMessage(ChatRole.User, sb.ToString()));

            

            var chatCompletionOptions = new ChatOptions()
            {
                Temperature = (float)temperature,
                ResponseFormat = ChatResponseFormat.Json
            };
            var complete = await chatClient.CompleteAsync(chatMessages, chatCompletionOptions);
            List<string> values = new List<string>();
            using (JsonDocument doc = JsonDocument.Parse(complete.Choices[0].Text ?? string.Empty))
            {
                if (doc.RootElement.TryGetProperty("response", out JsonElement responseElement) && responseElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement item in responseElement.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            values.Add(item.GetString());
                        }
                    }
                }
            }

            return values;
        }

    }
}
