using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayRaceFantasy
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Application.Run(new Banner());
            sendWakeUpRequest();
            sendWakeUpRequest();
            sendWakeUpRequest();
            sendWakeUpRequest();
            MakeHttpRequestAsync().Wait();
        }

        static async Task MakeHttpRequestAsync()
        {
            string endpoint = "https://racefantasy-api.onrender.com/stats/login/launcher";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Request successful: " + responseBody);
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }
        }

        static void sendWakeUpRequest()
        {
            string endpoint = "https://racefantasy-api.onrender.com/stats/login/server_wakeup";

            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    // Send a GET request to the server's endpoint to wake it up, and don't wait for the response
                    httpClient.GetAsync(endpoint);
                    Console.WriteLine("Server wake-up request sent.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while sending the wake-up request: " + ex.Message);
                }
            }
        }

    }
}
