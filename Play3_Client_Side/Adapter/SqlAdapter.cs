using Play3_Client_Side.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play3_Client_Side.Adapter
{
    class SqlAdapter : IUpdater
    {
        SqlProvider sqlProvider = new SqlProvider();

        public void DeleteData(string sql, string uuid = null, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            sqlProvider.Delete(sql);
        }

        public Task LoadData(string sql, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            Action action = () =>
            {
                Console.WriteLine("Not implemented yet.");
            };

            return new Task(action);
        }

        public void PostData(string sql, Dictionary<string, string> data = null, Action<string> onSuccess = null, Action<string> onFailure = null)
        {
            sqlProvider.Insert(sql);
        }
    }
}
