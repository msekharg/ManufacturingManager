namespace ManufacturingManager.Web.Configuration
{
    public class AppSettings
    {
        private readonly IConfiguration _configuration;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public string TypeOfSite => _configuration.GetSection("CaseAppSettings").GetSection("TypeOfSite").Value;

        public Int16 SessionTimeoutInMinutes
        {
            get
            {
                var val = _configuration.GetSection("CaseAppSettings").GetSection("SessionTimeoutInMinutes").Value;
                Int16.TryParse(val, out var retvalue);
                if (retvalue == 0)
                {
                    retvalue = 15;
                }

                return retvalue;


            }
        }

        public Int16 SessionWarningMinutes
        {
            get
            {
                var val = _configuration.GetSection("CaseAppSettings").GetSection("SessionWarningMinutes").Value;
                Int16.TryParse(val, out var retvalue);
                if (retvalue == 0)
                {
                    retvalue = 2;
                }

                return retvalue;

            }
        }

        public string SecurityServiceUrl => _configuration.GetSection("CaseAppSettings").GetSection("SecurityServiceUrl").Value;
        public string UserNameForSecurityService => _configuration.GetSection("CaseAppSettings").GetSection("UserNameForSecurityService").Value;
        public string PasswordForSecurityService => _configuration.GetSection("CaseAppSettings").GetSection("PasswordForSecurityService").Value;
        public string ActivateConcurrentSessions => _configuration.GetSection("CaseAppSettings").GetSection("ActivateConcurrentSessions").Value;
        public string ApplicationId => _configuration.GetSection("CaseAppSettings").GetSection("ApplicationId").Value;
        public string ContactPhone => _configuration.GetSection("CaseAppSettings").GetSection("ContactPhone").Value;
        public string ContactEmail => _configuration.GetSection("CaseAppSettings").GetSection("ContactEmail").Value;
        public bool CanUseTestingUsers
        {
            get
            {
                var testusers = _configuration.GetSection("CaseAppSettings").GetSection("CanUseTestingUsers").Value;
                if (testusers != null && testusers.ToLower() == "true")
                    return true;
                return false;
            }

        }
        public string RepositoryId => _configuration.GetSection("FileNetSettings").GetSection("RepositoryId").Value;
        public string FolderId => _configuration.GetSection("FileNetSettings").GetSection("FolderId").Value;

        public string SOA_CMIS_ObjectServicePassword => _configuration.GetSection("FileNetSettings").GetSection("SOA_CMIS_ObjectServicePassword").Value;

        public string SOA_CMIS_ObjectServiceUsername => _configuration.GetSection("FileNetSettings").GetSection("SOA_CMIS_ObjectServiceUsername").Value;

        public string SOA_CMIS_ObjectServiceEndpointUrl => _configuration.GetSection("FileNetSettings").GetSection("SOA_CMIS_ObjectServiceEndpointUrl").Value;


        public String TempUploadFileFolderPath => _configuration.GetSection("FileNetSettings").GetSection("TempUploadFileFolderPath").Value;

        public String TempExportDocumentFolderPath => _configuration.GetSection("FileNetSettings").GetSection("TempExportDocumentFolderPath").Value;

        public String SOA_CMIS_DiscoveryServiceEndpointUrl => _configuration.GetSection("FileNetSettings").GetSection("SOA_CMIS_DiscoveryServiceEndpointUrl").Value;

        public String SOA_CMIS_DiscoveryServiceUsername => _configuration.GetSection("FileNetSettings").GetSection("SOA_CMIS_DiscoveryServiceUsername").Value;

        public String SOA_CMIS_DiscoveryServicePassword => _configuration.GetSection("FileNetSettings").GetSection("SOA_CMIS_DiscoveryServicePassword").Value;

        public string AntivirusLocation => _configuration.GetSection("CaseAppSettings").GetSection("VirusScanner.ExecutableFilePath").Value;
    }
}
