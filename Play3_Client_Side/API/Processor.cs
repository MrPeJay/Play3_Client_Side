using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Play3_Client_Side.API
{
    public class Processor
    {
        public static async Task LoadData(string apiUrl, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            using (var response = await ApiHelper.ApiClient.GetAsync(ApiHelper.ApiClient.BaseAddress + apiUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    onSuccess?.Invoke(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync())
                        .ToString());
                }
                else
                {
                    onFailure?.Invoke(response.ReasonPhrase);
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public static async void PostData(string apiUrl, Dictionary<string, string> data,
            Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            var content = new FormUrlEncodedContent(data);

            using (var response =
                await ApiHelper.ApiClient.PostAsync(ApiHelper.ApiClient.BaseAddress + apiUrl, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    onSuccess?.Invoke(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync())
                        .ToString());
                }
                else
                {
                    onFailure?.Invoke(response.ReasonPhrase);
                }
            }
        }

        public enum PostDataType
        {
            Post,
            Put, 
            Delete
        }

        public static async void DeleteData(string apiUrl, string uuid,
            Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            using (var response =
                await ApiHelper.ApiClient.DeleteAsync(ApiHelper.ApiClient.BaseAddress + apiUrl + "?uuid=" + uuid))
            {
                if (response.IsSuccessStatusCode)
                {
                    onSuccess?.Invoke(JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync())
                        .ToString());
                }
                else
                {
                    onFailure?.Invoke(response.ReasonPhrase);
                }
            }
        }
    }
}
