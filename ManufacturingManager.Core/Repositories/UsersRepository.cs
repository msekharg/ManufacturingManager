﻿using System.Data;
using System.Data.Common;
using Dapper;
using ManufacturingManager.ADO;
using Microsoft.Data.SqlClient;

namespace ManufacturingManager.Core.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public async Task<IEnumerable<User>> GetUserList(bool onlyActive)
        {
            IEnumerable<User> list = new List<User>();

            if (AppCache.Users  is { Count: > 0 })
            {
                list = AppCache.Users;
            }
            else
            {
                var connString = DatabaseFactory.GetDbConnString("CMRS");
                try
                {
                    //string connString = Configuration.ChangeManagementConnectionString();
                    using SqlConnection conn = new SqlConnection(connString);

                    string strSelectCmd =
                        $"SELECT TOP 1000 UserId, FirstName, MiddleName, LastName, Email, UserRoleId, LastAccess, IsActive, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM [QualityAssuranceManager].[dbo].[User] WHERE IsActive = 1";

                    conn.Open();

                    list =  conn.QueryAsync<User>(strSelectCmd).Result;

                    AppCache.Users = list.ToList();
                }
                catch (SqlException exception)
                {
                    // Logging.WriteToLog($"Exception in UsersRepository.GetUserList() message: {exception.Message}",
                    //     LoggingCategoryEnum.Error, exception);
                }
            }
            if (onlyActive)
            {
                list = list.Where(x => x.IsActive).ToList();
            }
            return await Task.FromResult(list);
        }

        private void UpdateCache(User user, string operation)
        {
            if (AppCache.Users is List<User> userList)
            {
                var appCacheUserIndex = userList.FindIndex(u => u.UserId == user.UserId);

                if (operation.ToLower().Equals("update"))
                {
                    if(appCacheUserIndex > 0)
                        userList[appCacheUserIndex] = user;
                }
                else if (operation.ToLower().Equals("delete"))
                {
                    if(appCacheUserIndex > 0)
                        AppCache.Users.RemoveAt(appCacheUserIndex);
                }
            }
            else
            {
                // If not a List, you can add a different handling logic here
                Console.WriteLine("AppCache.Users is not a List.");
            }
        }
        
        // public async Task<string> GetEmailsForAdministratorsAndCoordinators()
        // {
        //     string mailList="";
        //
        //         try
        //         {
        //
        //             Database db = DatabaseFactory.CreateDatabase("PCSCase");
        //             await using DbCommand dbCommand = db.GetStoredProcCommand("UsersListAdminCoord");
        //             using DataSet ds = db.ExecuteDataSet(dbCommand);
        //             if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //             {
        //                 foreach (DataRow dr in ds.Tables[0].Rows)
        //                 {
        //                     Users record = new Users();
        //                     AssignValues(record, dr);
        //                     if (!string.IsNullOrEmpty(mailList))
        //                         mailList += ";";
        //                     mailList += record.Email;
        //                 }
        //                
        //             }
        //         }
        //         catch (SqlException exception)
        //         {
        //             Logging.WriteToLog($"Exception in UsersRepository.GetEmailsForAdministratorsAndCoordinators() message: {exception.Message}",
        //                 LoggingCategoryEnum.Error, exception);
        //         }
        //    
        //     return await Task.FromResult(mailList);
        // }


        //public Users Users(int userId)
        //{
        //    Users user = new();
        //    try
        //    {
        //        Database db = DatabaseFactory.CreateDatabase("PCScase");
        //        using (DbCommand dbCommand = db.GetStoredProcCommand("UsersGetById"))
        //        {
        //            db.AddInParameter(dbCommand, "@userId", DbType.Int32, userId);
        //            using (DataSet ds = db.ExecuteDataSet(dbCommand))
        //            {
        //                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //                {
        //                    var dr = ds.Tables[0].Rows[0];
        //                    AssignValues(user, dr);

        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException exception)
        //    {
        //        Logging.WriteToLog($"Exception in Users.Users() message: {exception.Message}",
        //             LoggingCategoryEnum.Error, exception);
        //    }

        //    return user;



        //}
        public User Users(int userId)
        {
            User user;
            if (AppCache.Users == null || AppCache.Users.Count == 0)
            {
                user = GetUserList(false).Result.FirstOrDefault(x => x.UserId  == userId );
            }
            else
            {
                //Get Values from Cache
                user = AppCache.Users.FirstOrDefault(x => x.UserId == userId );
            }

            return user;
        }

        public User Users(string email)
        {
            User user;
            if (AppCache.Users == null || AppCache.Users.Count == 0)
            {
                user = GetUserList(false).Result.FirstOrDefault(x => x.Email  == email );
            }
            else
            {
                //Get Values from Cache
                user = AppCache.Users.FirstOrDefault(x => x.Email == email );
            }

            return user;
        }
        public async Task<bool> Add(User user, int createdBy)
        {
            bool ret;
            int userId = 0;
            try
            {
                user.CreatedBy = "admin"; //dimension.InspectorName;
                 user.CreatedDate = DateTime.Now;
                 user.UpdatedBy = "admin"; //dimension.InspectorName;
                 user.UpdatedDate = DateTime.Now;
                var connString = DatabaseFactory.GetDbConnString("CMRS");
                var insertQuery =
                    "INSERT INTO dbo.[User](FirstName,MiddleName,LastName,Email,UserRoleId,LastAccess,IsActive,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) OUTPUT INSERTED.UserId VALUES (@FirstName,@MiddleName,@LastName,@Email,@UserRoleId,@LastAccess,@IsActive,@CreatedBy,@CreatedDate,@UpdatedBy,@UpdatedDate)";

                using var conn = new SqlConnection(connString);
                conn.Open();
                userId = conn.ExecuteScalar<int>(insertQuery, user);

                ret = userId > 0;
                if(ret)
                    AppCache.Users.Add(user);
            }
           
            catch (SqlException exception)
            {
                // Logging.WriteToLog($"Exception in Users.Add() message: {exception.Message}",
                //     LoggingCategoryEnum.Error, exception);
                ret = false;
            }
            //
            // if (AppCache.Users != null)
            //         AppCache.Users.Clear();
            
            return await Task.FromResult(ret);
        }
        public async Task<int> Update(User user, int updatedBy)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            // user.UpdatedBy = dimension.InspectorName;
            // dimension.UpdatedDate = DateTime.Now;
            string sql =
                "UPDATE [dbo].[User] SET " +
                "FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, UserRoleId = @UserRoleId" +
                " WHERE UserId = @UserId";

            await using SqlConnection conn = new(connString);
            conn.Open();
            int recordUpdated = await conn.ExecuteAsync(sql, user);
            
            if(recordUpdated > 0)
                UpdateCache(user, "UPDATE");
            
            return recordUpdated;
        }

        public async Task<int> Delete(User user)
        {
            // user.UpdatedBy = dimension.InspectorName;
            // dimension.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            const string query = @"Update dbo.[User] SET IsActive = 0 WHERE UserId = @userId";

            await using SqlConnection conn = new(connString);
            conn.Open();
            int recordDeleted = await conn.ExecuteAsync(query, new
            {
               user.UserId
            });

            if(recordDeleted > 0)
                UpdateCache(user, "DELETE");
            
            return recordDeleted; 
        }
        
        public async Task<bool> UpdateLoginDate(int userId)
        {
            bool ret;
            try
            {
                var db = DatabaseFactory.CreateDatabase("PCSCase");
                await using var dbCommand = db.GetStoredProcCommand("UsersUpdateLoginDate");
                db.AddInParameter(dbCommand, "@UserId", DbType.Int32, userId);
                // Execute the query
                db.ExecuteNonQuery(dbCommand);
                ret = true;
            }
            catch (SqlException exception)
            {
                // Logging.WriteToLog($"Exception in Users.UpdateLoginDate() message: {exception.Message}",
                //     LoggingCategoryEnum.Error, exception);
                ret = false;
            }
            return await Task.FromResult(ret);

        }
        public void AssignValues(User r, DataRow dr)
        {
            try
            {
                r.UserId = Convert.ToInt32(dr["UserId"]);
                r.FirstName = dr["FirstName"].ToString();
                r.MiddleName = dr["MInitial"].ToString();
                r.LastName = dr["LastName"].ToString();
                r.Email = dr["Email"].ToString();
                r.UserRoleId = Convert.ToInt32(dr["ProfileId"]);
                r.LastAccess = DataUtil.GetNullableDateTime(dr, "LastAccess");
                r.IsActive = Convert.ToBoolean(dr["Active"]);

            }
            catch (Exception ex)
            {
                // Logging.WriteToLog($"Exception in Users.AssignValues() message: {ex.Message}", LoggingCategoryEnum.Error, ex);
                throw;
            }
        }
    }
}
