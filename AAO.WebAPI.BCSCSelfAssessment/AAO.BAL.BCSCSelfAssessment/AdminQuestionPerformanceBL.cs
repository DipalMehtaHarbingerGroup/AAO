namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;

    public class AdminQuestionPerformanceBL
    {
        public static List<AdminQuestionPerformanceVM> AdminQuestionPerformanceDetails(AdminQuestionPerformanceVM queDetails)
        {
            return AdminQuestionPerformanceDAL.AdminQuestionPerformanceDetails(queDetails);
        }

        public static int GetAllQuestionCount(AdminQuestionPerformanceVM queDetails)
        {
            return AdminQuestionPerformanceDAL.GetAllQuestionCount(queDetails);
        }

        public static List<AdminQuestionPerformanceVM> GetAllQuestionExport(AdminQuestionPerformanceVM queDetails)
        {
            return AdminQuestionPerformanceDAL.GetAllQuestionExport(queDetails);
        }

        public static async Task<List<AdminExamQuestionReport>> GetAllAnsweredQuestionExport(AdminQuestionPerformanceVM queDetails)
        {
            return await AdminQuestionPerformanceDAL.GetAllAnsweredQuestionExport(queDetails);
        }

        public static List<AdminExamQuestionReport> GetAllAnsweredQuestionExport1(AdminQuestionPerformanceVM queDetails)
        {
            return AdminQuestionPerformanceDAL.GetAllAnsweredQuestionExport1(queDetails);
        }
        public static int GetTableCountExport(AdminQuestionPerformanceVM queDetails)
        {
            return AdminQuestionPerformanceDAL.GetTableCountExport(queDetails);
        }

        
    }
}
