using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FetchLibrary
{
    internal class JsonDataAccess : IDataAccess
    {
        private static string path = Path.Combine(Assembly.GetEntryAssembly().Location);
        public void LoadData()
        {
            throw new NotImplementedException();
        }

        public void SaveData()
        {
            var saveData = JsonConvert.SerializeObject(UserTransactions.Transactions, Formatting.Indented);

        }
    }
}
