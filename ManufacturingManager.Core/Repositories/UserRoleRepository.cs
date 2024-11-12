using System.Data;
using System.Data.Common;
using Dapper;
using ManufacturingManager.ADO;
using ManufacturingManager.Core.Models;
using Microsoft.Data.SqlClient;

namespace ManufacturingManager.Core.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        public UserRole UserRole(int userRoleId)
        {
            UserRole profile;
            if (AppCache.UserRoles == null || AppCache.UserRoles.Count == 0)
            {
                profile = GetUserRoleList().Result.FirstOrDefault(x => x.UserRoleId == userRoleId);
            }
            else
            {
                profile = AppCache.UserRoles.FirstOrDefault(x => x.UserRoleId == userRoleId);
            }
            return profile;
        }

        public async Task<IEnumerable<UserRole>> GetUserRoleList()
        {
            IEnumerable<UserRole> list = new List<UserRole>();

            if (AppCache.UserRoles is { Count: > 0 })
            {
                list = AppCache.UserRoles;
            }
            else
            {
                var connString = DatabaseFactory.GetDbConnString("CMRS");
                try
                {
                    //string connString = Configuration.ChangeManagementConnectionString();
                    using SqlConnection conn = new SqlConnection(connString);

                    string strSelectCmd =
                        $"SELECT TOP 1000 UserRoleId, UserRoleName, IsActive FROM [dbo].UserRole";

                    conn.Open();

                    list = await conn.QueryAsync<UserRole>(strSelectCmd);

                    AppCache.UserRoles = list.ToList();
                }
                catch (Exception ex)
                {
                }
            }

            return await Task.FromResult(list);
        }
    }
}
