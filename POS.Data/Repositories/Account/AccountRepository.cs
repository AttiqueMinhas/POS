using Dapper;
using Microsoft.Data.SqlClient;
using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Models.ModelVM.Request;
using System.Xml.Linq;

namespace POS.Data.Repositories.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ISQLDataAccess _dataRepository;
        public AccountRepository(ISQLDataAccess dataRepository)
        {
            _dataRepository = dataRepository;
        }
        public async Task<LoginResponse?> GetUserByEmailOrUserNameAsync(string emailorUserName)
        {
            string spName = "sp_GetUserByEmailorUserName";
            try
            {
                // Attempt to retrieve data from the database using stored procedure
                LoginResponse? response = await _dataRepository.GetSingleRow<LoginResponse, dynamic>(spName, new { EmailorUserName = emailorUserName });
                return response;
            }
            catch (SqlException ex)
            {
                var sp = "_addErrorLog";
                ErrorLogModel error = new ErrorLogModel();
                error.errorMsg = ex.Message;
                error.errorST = ex.StackTrace;
                var totalRowsCount = await _dataRepository.SaveData<dynamic>(sp, new
                {
                    error.errorMsg,
                    error.errorST
                });
                Console.WriteLine(totalRowsCount);
                return null;
            }
        }

        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            const string sql = @"
                SELECT r.RoleName 
                FROM Users u
                JOIN Roles r ON u.RoleId = r.RoleId
                WHERE u.UserId = @UserId AND r.IsActive = 1";

            return await _dataRepository.GetQueryAsync<List<string>,dynamic>(sql, new { UserId = userId });
        }
    }
}
