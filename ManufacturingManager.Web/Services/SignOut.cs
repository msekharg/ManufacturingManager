//using Fsc.Services.Security.Client.NET6;

namespace ManufacturingManager.Web.Services
{
    public class SignOut 
    {
        // Sample SignOutProvider implementation using the provided FscSessionSignOut class.
        //Implementations inheriting from this class should not clear Session  

        public SignOut()
        {
            
        }
        protected  void LogException(Exception ex)
        {
                 // Logging.WriteToLog("Exceptions occurred on SignOut LogException", LoggingCategoryEnum.Exceptions, ex);
        }

        protected  void SiteLogout()
        {
      
            //Debug.WriteLine("Additional site logout actions performed here");

        }
    }
}
 