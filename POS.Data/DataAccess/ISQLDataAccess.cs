using Dapper;
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
        Task<List<T>> GetDataList<T, P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<T?> GetSingleRow<T, P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<dynamic> GetScalarValue<P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<dynamic> GetScalarStringValue<P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<int> SaveData<T>(string spName, T Parameters, string connectionId = "DefaultConnection");
        Task<IEnumerable<T>> GetQueryData<T, P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<IEnumerable<T>> GetQueryTableData<T, P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<SqlMapper.GridReader> QueryMultipleAsync<P>(string spName, P parameters, string connectionId = "DefaultConnection");
        Task<List<string>> GetQueryAsync<T, P>(string spName, P parameters, string connectionId = "DefaultConnection");
    }
}
