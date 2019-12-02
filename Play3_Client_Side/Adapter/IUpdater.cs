using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play3_Client_Side.API;

namespace Play3_Client_Side.Adapter
{
    interface IUpdater
    {
        Task LoadData(string apiUrl, Action<string> onSuccess = null, Action<string> onFailure = null);
        void PostData(string apiUrl, Processor.PostDataType type, Dictionary<string, string> data = null,
            Action<string> onSuccess = null, Action<string> onFailure = null, string uuid = null);
    }
}
