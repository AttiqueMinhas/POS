using POS.Data.Models.ModelVM.Request;


namespace POS.Data.Repositories.Account
{
    public interface IAccountRepository
    {
        Task<LoginResponse> GetUserByEmailOrUserNameAsync(string loginRequest);
        Task<List<string>> GetUserRolesAsync(int userId);
    }
}
