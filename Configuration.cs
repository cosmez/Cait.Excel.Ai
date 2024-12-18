using System;
using System.IO;
using System.Text.Json;

namespace Cait.Excel.Ai
{


    public class Configuration
    {
        public string Provider { get; set; } = "OpenAI";
        public string ApiKey { get; set; }
        public string Model { get; set; }
        public AiServiceType ServiceType { get; set; }
        public string Url { get; set; }

        public string System { get; set; } = @"Eres un bot de soporte para Excel, cuando se te pida algun calculo solo responde con la respuesta.
            Responde sin markdown, solo texto plano y sin emojis a menos que se te pida.";

        public float Temperature { get; set; } = 1.0f;


        // Add other settings as needed
    }

    public class ConfigurationManager
    {
        private static readonly string ConfigFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "cait.excel.conf");

        public static void SaveConfiguration(Configuration config)
        {
            try
            {
                string json = JsonSerializer.Serialize(config);
                File.WriteAllText(ConfigFilePath, json);
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }

        public static Configuration GetConfiguration()
        {
            try
            {
                if (File.Exists(ConfigFilePath))
                {
                    string json = File.ReadAllText(ConfigFilePath);
                    return JsonSerializer.Deserialize<Configuration>(json);
                }
                else
                {
                    // Return default configuration if file does not exist
                    return new Configuration();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error al cargar configuracion: {ex.Message}");
                return new Configuration();
            }
        }
    }
}
