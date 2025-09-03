namespace AAO.SQLHelper.BCSCSelfAssessment
{
    using System.Configuration;

    public enum DataAccessType
    {
        Read, // Read indicates Read operations
        Write, // Write indicates Insert/Delete/Update operations
    }

    public class SqlConnectionProvider
    {
        public static string GetConnectionString(DataAccessType enumDataAccessType)
        {
            string connectionString = string.Empty;
            switch (enumDataAccessType)
            {
                case DataAccessType.Read:
                    connectionString = ConfigurationManager.ConnectionStrings["AAODBConnection"].ConnectionString;
                    break;
                case DataAccessType.Write:
                    connectionString = ConfigurationManager.ConnectionStrings["AAODBConnection"].ConnectionString;
                    break;
            }

            return connectionString;
        }
    }
}
