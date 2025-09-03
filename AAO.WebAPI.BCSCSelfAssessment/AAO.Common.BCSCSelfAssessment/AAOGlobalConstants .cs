namespace AAO.Common.BCSCSelfAssessment
{
    using System.Configuration;

    public static class AAOGlobalConstants
    {
        public static string SiteWebAPIUrl = ConfigurationManager.AppSettings["AAO_API_URL"];
    }
}
