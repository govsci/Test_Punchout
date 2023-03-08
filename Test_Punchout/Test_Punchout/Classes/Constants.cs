using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Test_Punchout.Classes
{
    public static class Constants
    {
        public static string PrdEcomDbConnection = WebConfigurationManager.ConnectionStrings["PrdEcomDb"].ConnectionString;
        public static string TstEcomDbConnection = WebConfigurationManager.ConnectionStrings["TstEcomDb"].ConnectionString;

        public static string ProductionPunchoutURL = System.Configuration.ConfigurationManager.AppSettings["prodPunchoutURL"].ToString();
        public static string TestPunchoutURL = System.Configuration.ConfigurationManager.AppSettings["testPunchoutURL"].ToString();
        public static string BrowserFormPost = System.Configuration.ConfigurationManager.AppSettings["browserFormPost"].ToString();
        public static string TestPoURL = System.Configuration.ConfigurationManager.AppSettings["testPoUrl"].ToString();
        public static string ProdPoURL = System.Configuration.ConfigurationManager.AppSettings["prodPoUrl"].ToString();
        public static string ProdAuthUsers = System.Configuration.ConfigurationManager.AppSettings["prodAuthUsers"].ToString();

        public static string TimeStamp(DateTime value) { return value.ToString("yyyyMMddHHmmssffff"); }
        public static string cXMLTimeStamp(DateTime value) { return value.ToString("yyyy-MM-ddTHH:mm:ss"); }

        public static string LDAPServer = WebConfigurationManager.AppSettings["LDAP"];
        public static int LDAPPort = int.Parse(WebConfigurationManager.AppSettings["LDAPPort"]);
        public static string Domain = WebConfigurationManager.AppSettings["Domain"];
        public static string DirectoryPath = WebConfigurationManager.AppSettings["DirectoryPath"];
        public static string DirectoryDomain = WebConfigurationManager.AppSettings["DirectoryDomain"];
        public static string allowedRoles = WebConfigurationManager.AppSettings["allowedRoles"];
    }
}