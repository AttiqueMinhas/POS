using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.DataAccess
{
    public interface ISQLDataAccess
    {
        Task<IEnumerable<T>> GetData<T, P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<T?> GetSingleRow<T, P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<int> GetScalarValue<P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<int> SaveData<T>(string spName, T Parameters, string connectionId = "DefaultConnection");
    }
}
