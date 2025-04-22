using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using POS.Data.Models;

namespace POS.Data.DataAccess
{
    public class SQLDataAccess : ISQLDataAccess
    {
        private readonly IConfiguration _config;

        public SQLDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<T>> GetData<T,P>(string spName,P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<List<T>> GetDataList<T, P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return (List<T>)await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.Text);
        }
        public async Task<dynamic> GetScalarValue<P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            var result = await connection.ExecuteScalarAsync(spName, parameters, commandType: CommandType.StoredProcedure);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public async Task<dynamic> GetScalarStringValue<P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            var result = await connection.ExecuteScalarAsync(spName, parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<T?> GetSingleRow<T, P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryFirstOrDefaultAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }


        public async Task<int> SaveData<T>(string spName,T Parameters,string connectionId ="DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.ExecuteAsync(spName,Parameters, commandType: CommandType.StoredProcedure);
        }


        public async Task<IEnumerable<T>> GetQueryData<T, P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.Text);
        }

        public async Task<IEnumerable<T>> GetQueryTableData<T, P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync<P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            var connection = new SqlConnection(_config.GetConnectionString(connectionId));
            await connection.OpenAsync(); // optional, Dapper does it
            return await connection.QueryMultipleAsync(spName, parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<string>> GetQueryAsync<T, P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return (await connection.QueryAsync<string>(spName, parameters, commandType: CommandType.Text)).ToList();
        }

        public async Task<List<T>> GetDataListFromSP<T, P>(string spName, P parameters, string connectionId = "DefaultConnection")
        {
            using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
            return (List<T>)await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        }
        //public async Task<T> GetObjData<T, P>(string spName, P parameters, string connectionId = "DefaultConnection")
        //{
        //    using IDbConnection connection = new SqlConnection(_config.GetConnectionString(connectionId));
        //    return await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.StoredProcedure);
        //}
    }
}
