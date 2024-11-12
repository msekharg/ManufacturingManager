using System.Data;
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
                        $"SELECT TOP 1000 UserId, FirstName, MiddleName, LastName, LoginName, Email, UserRoleId, LastAccess, IsActive, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM [dbo].[User] WHERE IsActive = 1";

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

        public User Users(string loginName)
        {
            User user;
            if (AppCache.Users == null || AppCache.Users.Count == 0)
            {
                user = GetUserList(false).Result.FirstOrDefault(x => x.LoginName  == loginName );
            }
            else
            {
                //Get Values from Cache
                user = AppCache.Users.FirstOrDefault(x => x.LoginName == loginName );
            }

            return user;
        }
        public async Task<bool> Add(User user, int createdBy)
        {
            bool ret;
            try
            {
                var db = DatabaseFactory.CreateDatabase("PCSCase");
                await using (var dbCommand = db.GetStoredProcCommand("UsersAdd"))
                {
                    db.AddInParameter(dbCommand, "@FirstName", DbType.String, user.FirstName);
                    db.AddInParameter(dbCommand, "@MInitial", DbType.String, user.MiddleName);
                    db.AddInParameter(dbCommand, "@LastName", DbType.String, user.LastName);
                    db.AddInParameter(dbCommand, "@VaLogon", DbType.String, user.LoginName);
                    db.AddInParameter(dbCommand, "@Email", DbType.String, user.Email);
                    db.AddInParameter(dbCommand, "@ProfileId", DbType.Int32, user.UserRoleId);
                    db.AddInParameter(dbCommand, "@createdBy", DbType.Int32, createdBy);
                    db.AddOutParameter(dbCommand, "@UserId", DbType.Int32, 4);
                    // Execute the query
                    db.ExecuteNonQuery(dbCommand);
                     
                    var retValue = Convert.ToInt32(db.GetParameterValue(dbCommand, "@UserId"));

                    // Execute the query
                    ret = retValue > 0;
                }
                if (AppCache.Users != null)
                    AppCache.Users.Clear();
            }

            catch (SqlException exception)
            {
                // Logging.WriteToLog($"Exception in Users.Add() message: {exception.Message}",
                //     LoggingCategoryEnum.Error, exception);
                ret = false;
            }
            return await Task.FromResult(ret);

        }
        public async Task<bool> Update(User user, int updatedBy)
        {
            bool ret;
            try
            {
                var db = DatabaseFactory.CreateDatabase("PCSCase");
                await using var dbCommand = db.GetStoredProcCommand("UsersUpdate");
                db.AddInParameter(dbCommand, "@UserId", DbType.Int32, user.UserId);
                db.AddInParameter(dbCommand, "@FirstName", DbType.String, user.FirstName);
                db.AddInParameter(dbCommand, "@MInitial", DbType.String, user.MiddleName);
                db.AddInParameter(dbCommand, "@LastName", DbType.String, user.LastName);
                db.AddInParameter(dbCommand, "@VaLogon", DbType.String, user.LoginName);
                db.AddInParameter(dbCommand, "@Email", DbType.String, user.Email);
                db.AddInParameter(dbCommand, "@ProfileId", DbType.Int32, user.UserRoleId);
                db.AddInParameter(dbCommand, "@Active", DbType.Boolean, user.IsActive);
                db.AddInParameter(dbCommand, "@UpdatedBy", DbType.Int32, updatedBy);
                // Execute the query
                db.ExecuteNonQuery(dbCommand);
                ret = true;
                if (AppCache.Users != null)
                    AppCache.Users.Clear();
            }
            catch (SqlException exception)
            {
                // Logging.WriteToLog($"Exception in Users.Update() message: {exception.Message}",
                //     LoggingCategoryEnum.Error, exception);
                ret = false;
            }
            return await Task.FromResult(ret);

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
                r.LoginName = dr["VaLogon"].ToString();
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
