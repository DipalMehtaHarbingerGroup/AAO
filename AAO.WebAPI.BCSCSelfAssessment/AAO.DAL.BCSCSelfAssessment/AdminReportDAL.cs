namespace AAO.DAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public static class AdminReportDAL
    {
        public static List<AdminReportVM> AdminReportDetails(AdminReportVM adminReportDetails)
        {
            List<AdminReportVM> reportList = new List<AdminReportVM>();
            try
            {
                /*SqlParameter[] objSqlParameter =
                {
                                                     new SqlParameter("@StartDate", adminReportDetails.ExamStartDate),
                                                     new SqlParameter("@EndDate", adminReportDetails.ExamCompletedDate),
                                                     new SqlParameter("@UserYear ", adminReportDetails.Year),
                                                     new SqlParameter("@OrderColumn",  adminReportDetails.OrderBy),
                                                     new SqlParameter("@OrderSequence", adminReportDetails.Seq),
                                                 };*/

                SqlConnection con = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
                string procedure = "AAO_GetAvgAdminSectionReport";

                SqlCommand cmd = new SqlCommand(procedure, con);
                SqlDataReader r;

                // Configure command and add parameters.
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@StartDate", adminReportDetails.ExamStartDate));
                //cmd.Parameters.Add(new SqlParameter("@EndDate", adminReportDetails.ExamCompletedDate));

                //cmd.Parameters.Add(new SqlParameter("@StartDate", Convert.ToString(adminReportDetails.ExamStartDate)));
                //cmd.Parameters.Add(new SqlParameter("@EndDate", Convert.ToString(adminReportDetails.ExamCompletedDate)));

                cmd.Parameters.Add(new SqlParameter("@StartDate", Convert.ToDateTime(adminReportDetails.ExamStartDate).ToString("yyyy-MM-dd")));//Added for HG-362 issue
                cmd.Parameters.Add(new SqlParameter("@EndDate", Convert.ToDateTime(adminReportDetails.ExamCompletedDate).ToString("yyyy-MM-dd")));//Added for HG-362 issue
                cmd.Parameters.Add(new SqlParameter("@UserYear ", adminReportDetails.Year));
                cmd.Parameters.Add(new SqlParameter("@OrderColumn", adminReportDetails.OrderBy));
                cmd.Parameters.Add(new SqlParameter("@OrderSequence", adminReportDetails.Seq));
                cmd.CommandTimeout = 400;

                // Execute the command.
                con.Open();
                r = cmd.ExecuteReader();
                while (r.Read())
                {
                    AdminReportVM reportListBO = new AdminReportVM();
                    reportListBO.SubspecialtyId = Convert.ToInt32(r["SubspecialtyId"]);
                    reportListBO.SubspecialtyName = Convert.ToString(r["Subspecialty_Name"]);
                    reportListBO.Correct = Convert.ToInt32(r["correct"]);
                    reportListBO.InCorrect = Convert.ToInt32(r["InCorrect"]);
                    reportListBO.Score = Convert.ToInt32(r["Score"]);
                    reportListBO.UserId = Convert.ToInt32(r["UserId"]);
                    reportListBO.BCSCSectionNumber = Convert.ToInt32(r["RowNumber"]);
                    reportList.Add(reportListBO);
                }

                con.Close();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Write(ex.StackTrace);
                return null;
            }

            return reportList;

            /*using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAvgAdminSectionReport", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    AdminReportVM reportListBO = new AdminReportVM();
                    reportListBO.SubspecialtyId = Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                    reportListBO.SubspecialtyName = Convert.ToString(objSqlDataReader["Subspecialty_Name"]);
                    reportListBO.Correct = Convert.ToInt32(objSqlDataReader["correct"]);
                    reportListBO.InCorrect = Convert.ToInt32(objSqlDataReader["InCorrect"]);
                    reportListBO.Score = Convert.ToInt32(objSqlDataReader["Score"]);
                    reportListBO.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    reportListBO.BCSCSectionNumber = Convert.ToInt32(objSqlDataReader["RowNumber"]);
                    reportList.Add(reportListBO);
                }
                objSqlDataReader.Close();
            }*/
        }

        public static List<AdminReportVM> AdminReportDetailsExport(AdminReportVM adminReportDetails)
        {
            List<AdminReportVM> reportListExport = new List<AdminReportVM>();
            SqlParameter[] objSqlParameter =
            {
                                                 new SqlParameter("@StartDate", adminReportDetails.ExamStartDate),
                                                 new SqlParameter("@EndDate", adminReportDetails.ExamCompletedDate),
                                                 new SqlParameter("@UserYear ", adminReportDetails.Year),
                                             };

            /*using (SqlDataReader objSqlDataReader = SqlHelper.ExecuteReader(
                SqlConnectionProvider.GetConnectionString(DataAccessType.Read), CommandType.StoredProcedure, "AAO_GetAvgAdminSectionReportExport", objSqlParameter))
            {
                while (objSqlDataReader.Read())
                {
                    AdminReportVM reportListBOExport = new AdminReportVM();
                    reportListBOExport.SubspecialtyId = Convert.ToInt32(objSqlDataReader["SubspecialtyId"]);
                    reportListBOExport.SubspecialtyName = Convert.ToString(objSqlDataReader["Subspecialty_Name"]);
                    reportListBOExport.Correct = Convert.ToInt32(objSqlDataReader["correct"]);
                    reportListBOExport.InCorrect = Convert.ToInt32(objSqlDataReader["InCorrect"]);
                    reportListBOExport.Score = Convert.ToInt32(objSqlDataReader["Score"]);
                    reportListBOExport.UserId = Convert.ToInt32(objSqlDataReader["UserId"]);
                    reportListBOExport.BCSCSectionNumber = Convert.ToInt32(objSqlDataReader["RowNumber"]);
                    reportListExport.Add(reportListBOExport);
                }

                objSqlDataReader.Close();
            }
            */
            SqlConnection con = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string procedure = "AAO_GetAvgAdminSectionReportExport";

            SqlCommand cmd = new SqlCommand(procedure, con);
            SqlDataReader r;

            // Configure command and add parameters.
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StartDate", adminReportDetails.ExamStartDate));
            cmd.Parameters.Add(new SqlParameter("@EndDate", adminReportDetails.ExamCompletedDate));
            cmd.Parameters.Add(new SqlParameter("@UserYear ", adminReportDetails.Year));

            // cmd.Parameters.Add(new SqlParameter("@OrderColumn", adminReportDetails.OrderBy));
            // cmd.Parameters.Add(new SqlParameter("@OrderSequence", adminReportDetails.Seq));
            cmd.CommandTimeout = 400;

            // Execute the command.
            con.Open();
            r = cmd.ExecuteReader();
            while (r.Read())
            {
                AdminReportVM reportListBOExport = new AdminReportVM();
                reportListBOExport.SubspecialtyId = Convert.ToInt32(r["SubspecialtyId"]);
                reportListBOExport.SubspecialtyName = Convert.ToString(r["Subspecialty_Name"]);
                reportListBOExport.Correct = Convert.ToInt32(r["correct"]);
                reportListBOExport.InCorrect = Convert.ToInt32(r["InCorrect"]);
                reportListBOExport.Score = Convert.ToInt32(r["Score"]);
                reportListBOExport.UserId = Convert.ToInt32(r["UserId"]);
                reportListBOExport.BCSCSectionNumber = Convert.ToInt32(r["RowNumber"]);
                reportListExport.Add(reportListBOExport);
            }

            con.Close();
            return reportListExport;
        }

        public static List<AdminReportVM> GetPYGYear(AdminReportVM pgyDetails)
        {
            List<AdminReportVM> pgyList = new List<AdminReportVM>();
            SqlConnection connection = new SqlConnection(SqlConnectionProvider.GetConnectionString(DataAccessType.Read));
            string sqlQueryChoice = string.Empty;
            sqlQueryChoice = "Select distinct isnull(convert(varchar,year(ResidencyEnd)),'Non-Resident') as PGYear from aaouser where Role ='U'";
            connection.Open();
            SqlCommand cmd = new SqlCommand(sqlQueryChoice, connection);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    AdminReportVM pgyListBO = new AdminReportVM();
                    pgyListBO.PGYYear = Convert.ToString(reader["PGYear"]);
                    pgyList.Add(pgyListBO);
                }

                reader.Close();
            }

            return pgyList;
        }
    }
}
