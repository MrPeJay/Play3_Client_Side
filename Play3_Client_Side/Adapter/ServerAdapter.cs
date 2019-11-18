using Play3_Client_Side.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play3_Client_Side.Adapter
{
    class ServerAdapter : IUpdater
    {
        Processor proc = new Processor();
        public void DeleteData(string apiUrl, string uuid, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            proc.DeleteData(apiUrl, uuid, onSuccess, onFailure);
        }

        public Task LoadData(string apiUrl, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            return proc.LoadData(apiUrl, onSuccess, onFailure);
        }

        public void PostData(string apiUrl, Dictionary<string, string> data, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            proc.PostData(apiUrl, data, onSuccess, onFailure);
        }
    }
}
