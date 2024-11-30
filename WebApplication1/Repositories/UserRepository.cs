using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using WebApplication1.Data;
using WebApplication1.IRepositories;
using WebApplication1.Views.Account;

namespace WebApplication1.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IProcessAccess _processAccess;
        public UserRepository(IProcessAccess processAccess)
        {
            _processAccess = processAccess;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            string procedureName = "ACCOUNT_MGT";
            string Calltype = "DeleteUser";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@CallType", Calltype),
                    new SqlParameter("@Desc1", userId)
            };
            var result=await _processAccess.ExecuteTransactionalOperationAsync(procedureName, parameters);
            return result;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            string procedureName = "ACCOUNT_MGT";
            string Calltype = "GetUserByEmail";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@CallType", Calltype),
                    new SqlParameter("@Desc1", email)
            };
            var usr=await _processAccess.GetAllAsync<User>(procedureName, parameters);
            return usr[0];
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            string procedureName = "ACCOUNT_MGT";
            string Calltype = "UpdateUser";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@CallType", Calltype),
                    new SqlParameter("@Desc1", user.UserId),
                    new SqlParameter("@Desc2", user.Username),
                    new SqlParameter("@Desc3", user.Email),
                    new SqlParameter("@Desc4", user.Password),
                    new SqlParameter("@Desc5", user.PhoneNumber),
                    new SqlParameter("@Desc6", user.Address)
            };
            var result = await _processAccess.ExecuteTransactionalOperationAsync(procedureName, parameters);
            return result;
        }
        public async Task<bool> IsEmailRegistered(string email)
        {
            string procedureName = "ACCOUNT_MGT";
            string Calltype = "IsEmailRegistered";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@CallType", Calltype),
                    new SqlParameter("@Desc1", email)
            };
            var productDt = await _processAccess.GetDataSets(procedureName, parameters);
            return Convert.ToInt32(productDt.Tables[0].Rows[0]["UserCount"]) > 0;
        }

        public async Task<bool> RegisterUser(string username, string email, string password)
        {
            string procedureName = "ACCOUNT_MGT";
            string Calltype = "RegisterUser";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@CallType", Calltype),
                    new SqlParameter("@Desc1", username),
                    new SqlParameter("@Desc2", email),
                    new SqlParameter("@Desc3", password)
            };
            var result = await _processAccess.ExecuteTransactionalOperationAsync(procedureName, parameters);
            return result;
        }

        public async Task<bool> ValidateUser(string email, string password)
        {
            string procedureName = "ACCOUNT_MGT";
            string Calltype = "ValidateUser";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@CallType", Calltype),
                    new SqlParameter("@Desc1", email),
                    new SqlParameter("@Desc2", password)
            };
            var productDt = await _processAccess.GetDataSets(procedureName, parameters);
            return Convert.ToInt32(productDt.Tables[0].Rows[0]["UserCount"]) > 0;
        }
    }
}