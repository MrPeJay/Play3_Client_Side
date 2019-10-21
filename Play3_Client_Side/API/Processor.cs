using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Play3_Client_Side.API
{
    public class Processor
    {
        public static async Task LoadData (string apiUrl, Action<string> onSuccess, Action<string> onFailure)
        {
            using(var response = await ApiHelper.ApiClient.GetAsync(ApiHelper.ApiClient.BaseAddress + apiUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<string>();
                    onSuccess?.Invoke(data);
                }
                else
                {
                    onFailure?.Invoke(response.ReasonPhrase);
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async void PostData(string apiUrl, Dictionary<string, string> data, Action onSuccess = null, Action<string> onFailure = null)
        {
            var content = new FormUrlEncodedContent(data);

            using (var response = await ApiHelper.ApiClient.PostAsync(ApiHelper.ApiClient.BaseAddress + apiUrl, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    onSuccess?.Invoke();
                }
                else
                {
                    onFailure?.Invoke(response.ReasonPhrase);
                }
            }
        }
    }
}
