using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play3_Client_Side.Adapter
{
    interface IUpdater
    {
        Task LoadData(string apiUrl, Action<string> onSuccess = null, Action<string> onFailure = null);
        void PostData(string apiUrl, Dictionary<string, string> data,
            Action<string> onSuccess = null, Action<string> onFailure = null);
        void DeleteData(string apiUrl, string uuid,
            Action<string> onSuccess = null, Action<string> onFailure = null);
    }
}
