using POS.Data.DataAccess;
using POS.Data.Models;
using POS.Data.Repositories.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace POS.Data.Repositories.Implementation
{
    public class UserRepository: IUserRepository
    {
        private readonly ISQLDataAccess _db;

        public UserRepository(ISQLDataAccess db)
        {
            _db = db;
        }
        public async Task<bool> IsEmailExists(string Email)
        {
            string spName = "sp_EmailExists";
            int result =await _db.GetScalarValue(spName,new { Email });
            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UserModel> getUserByID(int? UserID)
        {
            try
            {
                string spName = "sp_getUserById";
                var user = await _db.GetSingleRow<UserModel, dynamic>(spName, new { UserID });
                if (user != null)
                {
                    if(user.RoleID == (int)roleId.Admin)
                    {
                        user.Role = roleId.Admin.ToString();
                    }
                    else if(user.RoleID == (int)roleId.Employee)
                    {
                        user.Role = roleId.Employee.ToString();
                    }
                    else if(user.RoleID == (int)roleId.Inspector)
                    {
                        user.Role = roleId.Inspector.ToString();
                    }

                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public async Task<int> AddNewUser(UserModel model)
        {
            try
            {
                //string spName = "sp_EmailExists";
                model.Password = HashPasword(model.Password, out var salt);
                model.Salt = salt;
                string spName = "sp_registerUser";
                if(model.Role == roleId.Admin.ToString())
                {
                    model.RoleID = (int)roleId.Admin;
                }
                else if(model.Role == roleId.Employee.ToString())
                {
                    model.RoleID = (int)roleId.Employee;
                }
                else if (model.Role == roleId.Inspector.ToString())
                {
                    model.RoleID = (int)roleId.Inspector;
                }
                DateTime createdAt = DateTime.Now;
                var totalRows = await _db.SaveData<dynamic>(spName, new
                {
                    model.UserName,
                    model.Email,
                    model.Phone,
                    model.RoleID,
                    model.Password,
                    model.State,
                    model.ImagePath,
                    model.Salt,
                    createdAt
                });
                if (totalRows > 0)
                {
                    return totalRows;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception ex)
            {
                return 0;
            }
            
        }
        public async Task<List<UserModel>> getUsers()
        {
            try
            {
                string spName = "sp_getUsers";
                var users = await _db.GetData<UserModel,dynamic>(spName, new { });
                if(users != null)
                {
                    return users.ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public async Task<int> editUser(UserModel user)
        //{
        //    try
        //    {
        //        var userModel = await getUserByID(user.UserID);
        //        var isMatch = isMatchHashPassword(userModel.Password, user.Password, userModel.Salt);
        //        if(isMatch)
        //        {

        //        }
        //        userModel.Password
        //    }
        //    catch(Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return 0;
        //    }
        //}

        #region Private Methods
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private string HashPasword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }

        private bool isMatchHashPassword(string storedPassword, string enteredPassword, byte[] salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(enteredPassword),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            if(storedPassword == Convert.ToHexString(hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
