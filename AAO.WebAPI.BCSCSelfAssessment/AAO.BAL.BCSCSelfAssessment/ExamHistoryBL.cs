namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public static class ExamHistoryBL
    {
        public static List<ExamHistoryDTO> ExamHistoryDetails(ExamHistoryDTO examhistory)
        {
            return ExamHistoryDAL.ExamHistoryDetails(examhistory);
        }

        public static void DeleteExamHistoryDetails(ExamHistoryDTO examhistory)
        {
            ExamHistoryDAL.DeleteExamHistoryDetails(examhistory.ExamId, examhistory.UserId);
        }

        public static PdfDetailsDataVM GetPdfDetails(ExamHistoryDTO examhistory)
        {
           return ExamHistoryDAL.GetPdfDetails(examhistory);
        }

        // Reset Exam
        public static int ResetExam(int userId)
        {
            return ExamHistoryDAL.ResetExam(userId);
        }

        public static List<ExamHistoryDTO> InsertorAddEmail_GetOptOutDetails(ExamHistoryDTO values)
        {
            List<ExamHistoryDTO> email_list = new List<ExamHistoryDTO>();
            string[] emailArray = values.Emailids.Split(',');
            foreach (string email in emailArray)
            {
                ExamHistoryDTO emailids = ExamHistoryDAL.InsertorAddEmail_GetOptOutDetails(email, values);
                email_list.Add(emailids);
            }

            return email_list;
        }

        public static string OptOut(int id)
        {
            return ExamHistoryDAL.OptOut(id);
        }
    }
}
