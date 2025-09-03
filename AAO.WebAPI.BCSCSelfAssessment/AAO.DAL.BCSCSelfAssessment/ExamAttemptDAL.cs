namespace AAO.DAL.BCSCSelfAssessment
{
    using System.Data;
    using System.Data.SqlClient;
    using AAO.DTO.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment;
    using AAO.SQLHelper.BCSCSelfAssessment.DataAccessProvider;

    public static class ExamAttemptDAL
    {
    // public static int AddExamAttempt(ExamAttemptDTO examAttempt)
    //    {
    //        SqlParameter[] arrSqlParameter =
    //        {
    //                new SqlParameter("@ExamCreatedDate ", examAttempt.ExamCreatedDate),
    //                new SqlParameter("@ExamLastAttemptDate", examAttempt.ExamCreatedDate),
    //                new SqlParameter("@ExamId", examAttempt.ExamId),
    //                new SqlParameter("@UserId", examAttempt.UserId),
    //                new SqlParameter("@ExamStatus", examAttempt.ExamStatus),
    //                new SqlParameter("@TotalAllocatedTime ", examAttempt.TotalAllocatedTime),
    //                new SqlParameter("@TotalConsumedTime ", examAttempt.TotalConsumedTime),
    //                new SqlParameter("@id", SqlDbType.Int),
    //               };
    //        arrSqlParameter[arrSqlParameter.Length - 1].Direction = ParameterDirection.Output;
    //        SqlHelper.ExecuteNonQuery(SqlConnectionProvider.GetConnectionString(DataAccessType.Write), CommandType.StoredProcedure, "AAO_AddExamAttempt", arrSqlParameter);
    //        return int.Parse(arrSqlParameter[arrSqlParameter.Length - 1].Value.ToString());
    //    }
    }
}
