namespace AAO.BAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;
    using System.Configuration;
    using Serilog;

    public static class SpacedRepetitionBL
    {
        private static string _sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePath");
        private static Serilog.Core.Logger log = new LoggerConfiguration().WriteTo.RollingFile(_sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

        // Get SpacedRepetition Exam
        public static ExamDTO GetSpacedRepitionExam(int userId, string typeofCategory) // 'typeofCategory' added for HG-459 but not set in DB for report harm
        {
            return SpacedRepetitionDAL.GetSpacedRepetionExam(userId, typeofCategory);
        }

        // Get SpacedRepetition Exam Status Count
        public static ExamStatusCountSRVM GetExamStatusCountSR(ServiceCallVM examService)
        {
            log.Information("start method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            int examId = SpacedRepetitionDAL.GetExamIdBYUserIdSR(examService.userId);
            log.Information("end method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            return SpacedRepetitionDAL.GetExamStatusCountSR(examId);
        }

        // Get Section On SR Landing Performance Overview
        public static List<SRPerformanceOverviewVM> GetSectionPerformanceOverviewSR(ServiceCallVM examService)
        {
            log.Information("start method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            int examId = SpacedRepetitionDAL.GetExamIdBYUserIdSR(examService.userId);
            log.Information("end method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            return SpacedRepetitionDAL.GetSectionPerformanceOverviewSR(examId);

        }

        // Get Chapter in SR Performance Overview
        public static List<SRPerformanceOverviewChapterVM> GetChapterPerformanceOverviewSR(ServiceCallVM examService)
        {
            log.Information("start method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            int examId = SpacedRepetitionDAL.GetExamIdBYUserIdSR(examService.userId);
            log.Information("end method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            return SpacedRepetitionDAL.GetChapterPerformanceOverviewSR(examId, examService.SubpecialityId);

        }

        // Get Topics in SR Performance Overview
        public static List<SRPerformanceOverviewTopicsVM> GetTopicsPerformanceOverviewSR(ServiceCallVM examService)
        {
            log.Information("start method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            int examId = SpacedRepetitionDAL.GetExamIdBYUserIdSR(examService.userId);
            log.Information("end method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            return SpacedRepetitionDAL.GetTopicsPerformanceOverviewSR(examId, examService.ChapterNumber, examService.SubpecialityId, examService.ChapterName);
        }

        // Get Recommended Topics On SR Landing Page
        public static List<SRRecommendedTopicsVM> GetRecommendedTopicsSR(ServiceCallVM examService)
        {
            log.Information("start method :{0} examId:{1}  userId:{3} examType:{4}", "GetExamStatusCountSR", examService.examId, examService.userId, examService.examType);
            int examId = SpacedRepetitionDAL.GetExamIdBYUserIdSR(examService.userId);
            log.Information("end method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            return SpacedRepetitionDAL.GetRecommendedTopicsSR(examId, examService.userId);
        }

        // Get Recommended Topics Export On SR Landing Page
        public static List<SRRecommendedTopicsExportVM> GetRecommendedTopicsExportSR(ServiceCallVM examService, SRRecommendedTopicsExportVM sRRecommendedTopicsExportVM)
        {
            log.Information("start method :{0} examId:{1}  userId:{3} examType:{4} ", "GetExamStatusCountSR", examService.examId, examService.userId, examService.examType);
            int examId = SpacedRepetitionDAL.GetExamIdBYUserIdSR(examService.userId);
            log.Information("end method :{0} examId:{1}  userId:{3}", "GetExamStatusCountSR", examService.examId, examService.userId);
            return SpacedRepetitionDAL.GetRecommendedTopicsExportSR(examId);
        }

        public static int  DeleteRecommendedTopic(SRRecommendedTopicsDeletedVM delRecommendedTopic)
        {
            log.Information("start method :{0}   userId:{1} ", "DeleteRecommendedTopic", delRecommendedTopic.UserId);
            int record = SpacedRepetitionDAL.DeleteRecommendedTopic(delRecommendedTopic);
            log.Information("end method :{0}   userId:{1} ", "DeleteRecommendedTopic", delRecommendedTopic.UserId);
            return record;
        }
    }
}
