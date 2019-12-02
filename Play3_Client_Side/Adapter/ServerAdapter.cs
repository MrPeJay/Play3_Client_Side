using Play3_Client_Side.API;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Play3_Client_Side.Adapter
{
    class ServerAdapter : IUpdater
    {
        Processor proc = new Processor();

        public Task LoadData(string apiUrl, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            return proc.LoadData(apiUrl, onSuccess, onFailure);
        }

        public void PostData(string apiUrl, Processor.PostDataType type, Dictionary<string, string> data = null,
            Action<string> onSuccess = null, Action<string> onFailure = null, string uuid = null)
        {
            proc.PostData(apiUrl, type, data, onSuccess, onFailure, uuid);
        }
    }
}
