using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FetchAPI.Models
{
    public interface IDataAccess
    {
        public void LoadData();
        public void SaveData();
    }
}
