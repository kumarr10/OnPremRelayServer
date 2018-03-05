using ConEdison.EHSApps.WebAPI.SPListDataService;
using ConEdison.EHSApps.WebAPI.SPSearchService;
using System;
using System.Configuration;
using System.Net;

namespace EHSApps.WebAPI.PolicyProcedure.Data
{
    public static class Global
    {
        public static string SvcConn = ConfigurationManager.AppSettings["SvcConn"];
     
        public static EHSPortalDataContext Context
        {
            get
            {
                EHSPortalDataContext dataContext = new EHSPortalDataContext(new Uri(Global.SvcConn));
                dataContext.Credentials = CredentialCache.DefaultNetworkCredentials;
                return dataContext;
            }
        }

        public static QueryService SearchContext
        {
            get
            {
                QueryService dataContext = new QueryService();              
                dataContext.Credentials = CredentialCache.DefaultCredentials;
                //dataContext.ClientCredentials.UserName.Password = CredentialCache.DefaultCredentials.Password;
                // dataContext.Credentials = CredentialCache.DefaultNetworkCredentials;
                return dataContext;
            }
        }
    }
}
