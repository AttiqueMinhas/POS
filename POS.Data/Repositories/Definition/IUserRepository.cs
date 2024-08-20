using POS.Data.Models;
using POS.Data.Models.ModelVM.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repositories.Definition
{
    public interface IUserRepository
    {
        Task<bool> IsEmailExists(string Email);
        Task<int> AddNewUser(UserModel model);
        Task<List<UserModel>> getUsers();
        Task<UserModel> getUserByID(int? UserID);
        Task<int> editUser(EditRequestVM user);
        Task<int> deleteUser(int? UserID);
    }
}
