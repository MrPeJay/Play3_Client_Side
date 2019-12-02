using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Play3_Client_Side.API
{
    public class Processor
    {
        /// <summary>
        /// Get data from server API.
        /// </summary>
        /// <param name="apiUrl">Sever API uri</param>
        /// <param name="onSuccess">Action triggered if data from server API is got successfully</param>
        /// <param name="onFailure">Action triggered if data from server API hasn't been received</param>
        /// <returns></returns>
        public async Task LoadData(string apiUrl, Action<string> onSuccess = null, Action<string> onFailure = null)
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

        /// <summary>
        /// Post data to server API.
        /// </summary>
        /// <param name="apiUrl">Sever API uri</param>
        /// <param name="type">Post data type</param>
        /// <param name="data">Contents to send.</param>
        /// <param name="onSuccess">Action triggered if post to server API is successful</param>
        /// <param name="onFailure">Action triggered if post to server API failed</param>
        /// <param name="uuid">Object to delete UUID</param>
        public async void PostData(string apiUrl, PostDataType type, Dictionary<string, string> data = null,
            Action<string> onSuccess = null, Action<string> onFailure = null, string uuid = null)
        {
            FormUrlEncodedContent content;

            switch (type)
            {
                case PostDataType.Post:
                    content = new FormUrlEncodedContent(data);

                    using (var response =
                        await ApiHelper.ApiClient.PostAsync(ApiHelper.ApiClient.BaseAddress + apiUrl, content))
                    {
                        Action(response);
                    }
                    break;
                case PostDataType.Put:
                    content = new FormUrlEncodedContent(data);

                    using (var response =
                        await ApiHelper.ApiClient.PutAsync(ApiHelper.ApiClient.BaseAddress + apiUrl, content))
                    {
                        Action(response);
                    }
                    break;
                case PostDataType.Delete:
                    using (var response =
                        await ApiHelper.ApiClient.DeleteAsync(
                            ApiHelper.ApiClient.BaseAddress + apiUrl + "?uuid=" + uuid))
                    {
                        Action(response);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            async void Action(HttpResponseMessage response)
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
    }
}
