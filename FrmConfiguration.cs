using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cait.Excel.Ai.Services;

namespace Cait.Excel.Ai
{
    public partial class FrmConfiguration : Form
    {
        public FrmConfiguration()
        {
            InitializeComponent();
        }

        private void cbProveedor_SelectedValueChanged(object sender, EventArgs e)
        {
            txtApiKey.Text = string.Empty;
            txtUrl.Text = string.Empty;

            txtModel.Text = AiServiceFactory.GetDefaultModel(cbProveedor.SelectedItem.ToString());

            if (cbProveedor.SelectedItem.ToString() == "OpenAI")
            {
                txtApiKey.Text = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
                txtUrl.Enabled = false;
            }
            else if (cbProveedor.SelectedItem.ToString() == "AzureOpenAI")
            {
                txtApiKey.Text = Environment.GetEnvironmentVariable("AZURE_API_KEY");
                txtUrl.Enabled = true;
            }
            else if (cbProveedor.SelectedItem.ToString() == "Gemini")
            {
                txtApiKey.Text = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
                txtUrl.Enabled = false;
            }
            else if (cbProveedor.SelectedItem.ToString() == "Together")
            {
                txtApiKey.Text = Environment.GetEnvironmentVariable("TOGETHERAI_API_KEY");
                txtUrl.Enabled = false;
            }
            else if (cbProveedor.SelectedItem.ToString() == "Local")
            {
                txtApiKey.Text = "";
                txtUrl.Enabled = true;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cbProveedor.SelectedItem.ToString() == "OpenAI")
            {
                Environment.SetEnvironmentVariable("OPENAI_API_KEY", txtApiKey.Text);
            }
            else if (cbProveedor.SelectedItem.ToString() == "AzureOpenAI")
            {
                Environment.SetEnvironmentVariable("AZURE_API_KEY", txtApiKey.Text);
            }
            else if (cbProveedor.SelectedItem.ToString() == "Gemini")
            {
                Environment.SetEnvironmentVariable("GEMINI_API_KEY", txtApiKey.Text);
            }
            else if (cbProveedor.SelectedItem.ToString() == "Together")
            {
                Environment.SetEnvironmentVariable("TOGETHERAI_API_KEY", txtApiKey.Text);
            }
            else if (cbProveedor.SelectedItem.ToString() == "Local")
            {
                
            }


            txtModel.Text = AiServiceFactory.GetDefaultModel(cbProveedor.SelectedItem.ToString());
            var configuration = new Configuration();
            configuration.Provider = cbProveedor.SelectedItem.ToString();
            configuration.ApiKey = txtApiKey.Text;
            configuration.Url = txtUrl.Text;
            configuration.Model = txtModel.Text;
            configuration.System = txtSistema.Text;
            configuration.Temperature = (float)txtTemperature.Value;
            ConfigurationManager.SaveConfiguration(configuration);

            Close();
        }

        private void FrmConfiguration_Load(object sender, EventArgs e)
        {
            var configuration = ConfigurationManager.GetConfiguration();
            cbProveedor.SelectedItem = configuration.Provider;
            txtApiKey.Text = configuration.ApiKey;
            txtModel.Text = configuration.Model;
            txtUrl.Text = configuration.Url;
            txtSistema.Text = configuration.System;
            txtTemperature.Value = (decimal)configuration.Temperature;
            cbProveedor_SelectedValueChanged(sender, EventArgs.Empty);
        }

        private void lnkObtener_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cbProveedor.SelectedItem.ToString() == "OpenAI")
            {
                System.Diagnostics.Process.Start("https://platform.openai.com/");
            }
            else if (cbProveedor.SelectedItem.ToString() == "AzureOpenAI")
            {
                System.Diagnostics.Process.Start("https://azure.microsoft.com/en");
            }
            else if (cbProveedor.SelectedItem.ToString() == "Gemini")
            {
                System.Diagnostics.Process.Start("https://ai.google.dev/gemini-api/docs/api-key?hl=es-419");
            }
            else if (cbProveedor.SelectedItem.ToString() == "Together")
            {
                System.Diagnostics.Process.Start("https://www.together.ai/");
            }
            else if (cbProveedor.SelectedItem.ToString() == "Local")
            {

            }
        }
    }
}
