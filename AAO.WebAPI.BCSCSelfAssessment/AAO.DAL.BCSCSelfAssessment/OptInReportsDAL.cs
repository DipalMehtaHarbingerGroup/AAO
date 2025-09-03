namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Common.BCSCSelfAssessment;
    using SQLHelper.BCSCSelfAssessment;
    using SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public class OptInReportsDAL
    {
        public static List<OptInReports> OptInReports(OptInReports optInReports)
        {
            string ordering = string.Empty;
            if (optInReports.OrderBy != null)
            {
                ordering = " order by " + optInReports.OrderBy + " " + optInReports.Seq;
            }

            List<OptInReports> reportList = new List<OptInReports>();
            SqlParameter[] objSqlParameter =
            {
                 new SqlParameter("@UserYear ", optInReports.Year),
                 new SqlParameter("@Order", ordering),
            };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetOptInReports", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    OptInReports reportListBO = new OptInReports();
                    reportListBO.UserName = Convert.ToString(objSqlDataReader["UserName"]);

                    object customerIdObj = objSqlDataReader["MasterCustomerID"];
                    reportListBO.MasterCustomerID = customerIdObj is DBNull ? string.Empty : Convert.ToString(objSqlDataReader["MasterCustomerID"]);
                    reportListBO.OptIn = Convert.ToString(objSqlDataReader["OptIn"]);
                    reportList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return reportList;
        }

        public static List<OptInReports> GetOptIn(OptInReports optInReports)
        {
            List<OptInReports> reportList = new List<OptInReports>();
            SqlParameter[] objSqlParameter =
            {
                  new SqlParameter("@UserYear ", optInReports.Year),
            };

            using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetOptIn", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    OptInReports reportListBO = new OptInReports();
                    reportListBO.OptIn = Convert.ToString(objSqlDataReader["OptIn"]);
                    reportList.Add(reportListBO);
                }

                objSqlDataReader.Close();
            }

            return reportList;
        }
    }
}
