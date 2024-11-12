// /* summary
//  Usually we get the user information from HttpContext but I can't get  HttpContext from Blazor Server app in this library
//  The workaround is call instantiate the user after logging
//      Logging.SetUpUserForLogger(userLogged);   
// */
//
// using System.Data;
// using System.Globalization;
// using System.Text.RegularExpressions;
// using log4net;
//
// namespace ManufacturingManager.Core
// {
//    
//
//     public  class Logging 
//     {
//   
//         private static readonly ILog Log = LogManager.GetLogger("Logging");
//         private static readonly ILog MailLog = LogManager.GetLogger("EmailLogging");
//        
//         private static string _userLogged;
//
//        
//         public static void SetUpUserForLogger(string userLogged)
//         {
//             _userLogged = userLogged;
//         }
//         private static string UserName
//         {
//             get
//             {
//                 if (!string.IsNullOrEmpty(_userLogged))
//                 {
//                     return _userLogged;
//                 }
//
//                 return "** Unknown **";
//
//             }
//         }
//        
//         private static string UserNameMessage => $" For user {UserName}. ";
//
//         public static void WriteToLog(string message)
//         {
//
//             //Fortify reports the following error related to "message" argument passed in this function.
//             //The method LogErrorMessage() in Logging.cs writes unvalidated user input to the log on line 91. 
//             //An attacker could take advantage of this behavior to forge log entries or inject malicious content into the log.
//             //I reported this a not an issue because there is no Uer input. All messages are written in the application and passed as 
//             //a plain text in the function.
//             //I also sanitized the message using SanitizeText() method below but is is still reported as an issue.
//
//             message = SanitizeText(message);
//             LogErrorMessage(message, null);
//         }
//
//         public static void WriteToLog(string message, LoggingCategoryEnum category)
//         {
//             message = SanitizeText(message);
//             switch (category)
//             {
//                 case LoggingCategoryEnum.Error:
//                     LogErrorMessage(message, null);
//                     break;
//                 case LoggingCategoryEnum.Exceptions:
//                     LogFatalMessage(message, null);
//                     break;
//                 case LoggingCategoryEnum.General:
//                     LogInfoMessage(message, null);
//                     break;
//                 case LoggingCategoryEnum.Warning:
//                     LogWarningMessage(message, null);
//                     break;
//                 case LoggingCategoryEnum.Debug:
//                     LogDebugMessage(message, null);
//                     break;
//             }
//         }
//
//         public static void WriteToLog(string message, LoggingCategoryEnum category, Exception exception)
//         {
//             if (!string.IsNullOrEmpty(exception.StackTrace))
//             {
//                 message = $"{message} Stack: {exception.StackTrace}";
//             }
//
//             if (exception.InnerException != null && !string.IsNullOrEmpty(exception.InnerException.Message))
//             {
//                 message += exception.InnerException.Message;
//             }
//
//             message = SanitizeText(message);
//
//             switch (category)
//             {
//                 case LoggingCategoryEnum.Error:
//                     LogErrorMessage(message, exception);
//                     break;
//                 case LoggingCategoryEnum.Exceptions:
//                     LogFatalMessage(message, exception);
//                     break;
//
//                 case LoggingCategoryEnum.General:
//                     LogInfoMessage(message, exception);
//                     break;
//                 case LoggingCategoryEnum.Warning:
//                     LogWarningMessage(message, exception);
//                     break;
//                 case LoggingCategoryEnum.Debug:
//                     LogDebugMessage(message, exception);
//                     break;
//
//             }
//         }
//
//         private static void LogErrorMessage(string message, Exception exception)
//         {
//             var cleanMessage = SanitizeText(UserNameMessage + message);
//             int logId = LogMessageToDb(cleanMessage, LoggingCategoryEnum.Error, exception);
//             var msg = GetMessageFromDb(logId);
//             if (string.IsNullOrEmpty(msg))
//             {
//                 msg = $"User: {UserNameMessage} PCS Portal Error {exception.GetType()} Id= {logId}";
//
//             }
//
//             if (Log.IsErrorEnabled)
//             {
//                 Log.Error(msg);
//             }
//
//             if (MailLog.IsErrorEnabled)
//             {
//                 MailLog.Error(msg);
//             }
//         }
//
//         private static void LogFatalMessage(string message, Exception exception)
//         {
//             var cleanMessage = SanitizeText(UserNameMessage + message);
//             //To fix Low SWA issue "System Information Leak: Internal" more than 10K issues.,
//             //Previous implementation we saved error message in log text file and then in the DB.
//             //New implementation we save it in the DB, then get message from db and save it in log file.
//             int logId = LogMessageToDb(cleanMessage, LoggingCategoryEnum.Error, exception);
//             var msg = GetMessageFromDb(logId);
//             if (string.IsNullOrEmpty(msg))
//             {
//                 msg = $"User: {UserNameMessage} PCS Portal Error {exception.GetType()} Id= {logId}";
//
//             }
//             if (Log.IsFatalEnabled)
//             {
//                 Log.Fatal(msg);
//             }
//             if (MailLog.IsErrorEnabled)
//             {
//                 MailLog.Fatal(msg);
//             }
//         }
//
//         private static void LogInfoMessage(string message, Exception exception)
//         {
//             var cleanMessage = SanitizeText(UserNameMessage + message);
//             //To fix Low SWA issue "System Information Leak: Internal" more than 10K issues.,
//             //Previous implementation we saved error message in log text file and then in the DB.
//             //New implementation we save it in the DB, then get message from db and save it in log file.
//             int logId = LogMessageToDb(cleanMessage, LoggingCategoryEnum.General, exception);
//             var msg = GetMessageFromDb(logId);
//             if (string.IsNullOrEmpty(msg))
//             {
//                 msg = $"User: {UserNameMessage} PCS Portal Error {exception.GetType()} Id= {logId}";
//
//             }
//             if (Log.IsInfoEnabled)
//             {
//                 Log.Info(msg);
//
//             }
//
//         }
//
//         private static void LogWarningMessage(string message, Exception exception)
//
//         {
//             message = SanitizeText(UserNameMessage + message);
//             //To fix Low SWA issue "System Information Leak: Internal" more than 10K issues.,
//             //Previous implementation we saved error message in log text file and then in the DB.
//             //New implementation we save it in the DB, then get message from db and save it in log file.
//             int logId = LogMessageToDb(message, LoggingCategoryEnum.Warning, exception);
//             var msg = GetMessageFromDb(logId);
//
//             if (Log.IsWarnEnabled)
//             {
//                 Log.Warn(msg);
//             }
//         }
//
//         private static void LogDebugMessage(string message, Exception exception)
//         {
//             message = SanitizeText(UserNameMessage + message);
//             //To fix Low SWA issue "System Information Leak: Internal" more than 10K issues.,
//             //Previous implementation we saved error message in log text file and then in the DB.
//             //New implementation we save it in the DB, then get message from db and save it in log file.
//             int logId = LogMessageToDb(message, LoggingCategoryEnum.Debug , exception);
//             var msg = GetMessageFromDb(logId);
//             if (Log.IsDebugEnabled)
//             {
//                 Log.Debug(msg);
//             }
//         }
//
//         public static int LogMessageToDb(string message, LoggingCategoryEnum loglevel, Exception exception)
//         {
//             try
//             {
//                 message = SanitizeText(message);
//                 var database = DatabaseFactory.CreateDatabase("PCSCase");
//                 using var dbCommand = database.GetStoredProcCommand("ErrorLogMessage");
//
//                 // Add the parameters
//                 database.AddInParameter(dbCommand, "@Date", DbType.String, DateTime.Now.ToString(CultureInfo.InvariantCulture));
//                 database.AddInParameter(dbCommand, "@User", DbType.String, UserName);
//                 database.AddInParameter(dbCommand, "@Level", DbType.String, loglevel.ToString());
//                 database.AddInParameter(dbCommand, "@Message", DbType.String, message);
//                 database.AddInParameter(dbCommand, "@Exception", DbType.String, exception != null ? exception.ToString() : "");
//                 database.AddOutParameter(dbCommand, "@Id", DbType.Int32, 4);
//
//                 // Execute the query
//                 database.ExecuteNonQuery(dbCommand);
//                 var id = Convert.ToInt32(database.GetParameterValue(dbCommand, "@id"));
//                 return id;
//
//             }
//             catch (Exception)
//             {
//                 MailLog.Error(@"DataBase logging LogMessageToDb failed for add error Message");
//                 throw;
//             }
//
//         }
//
//         public static string GetMessageFromDb(int id)
//         {
//             string retVal = string.Empty;
//             try
//             {
//                 var database = DatabaseFactory.CreateDatabase("PCSCase");
//                 using var dbCommand = database.GetStoredProcCommand("ErrorMessageGetById");
//                 try
//                 {
//                     // Add the parameters
//                     database.AddInParameter(dbCommand, "@Id", DbType.Int32, id);
//                     using var ds = database.ExecuteDataSet(dbCommand);
//                     if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
//                     {
//                         var dr = ds.Tables[0].Rows[0];
//                         retVal = dr["Message"].ToString();
//                     }
//
//                     return retVal;
//                 }
//                 catch (Exception)
//                 {
//                     MailLog.Error(@"DataBase logging LogMessageToDb failed to read error Message for id= " + id.ToString());
//                     throw;
//
//                 }
//             }
//             catch (Exception)
//             {
//                 MailLog.Error(@"DataBase logging LogMessageToDb failed for add error Message");
//                 throw;
//             }
//
//         }
//         private static string SanitizeText(string text)
//         {
//             if (string.IsNullOrWhiteSpace(text))
//                 return null;
//             //text = text.Replace(Environment.NewLine, "_");
//             const string whitelist = @"[^a-zA-Z0-9\.\\:\[]\?=;,\#+@ ]";
//             return Regex.Replace(text, whitelist, " ");
//         }
//        
//     }
//
//     public enum LoggingCategoryEnum
//     {
//         General,
//
//         Warning,
//
//         Debug,
//
//         Error,
//
//         Exceptions
//     }
// }
