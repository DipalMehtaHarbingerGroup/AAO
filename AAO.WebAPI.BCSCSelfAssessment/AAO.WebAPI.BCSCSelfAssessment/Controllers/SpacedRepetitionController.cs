namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.Common.BCSCSelfAssessment;
    using BAL.BCSCSelfAssessment;
    using DTO.BCSCSelfAssessment;

    public class SpacedRepetitionController : ApiController
    {
        // Get SpacedRepetition Exam Status Count
        [Route("api/spacedRepetition/GetExamStatusCountSR")]
        [HttpPost]
        public ExamStatusCountSRVM GetExamStatusCountSR(ServiceCallVM examService)
        {
            return SpacedRepetitionBL.GetExamStatusCountSR(examService);
        }

        // Get SpacedRepetition Performance Overview Data
        [Route("api/spacedRepetition/SRPerformanceOverview")]
        [HttpPost]
        public List<SRPerformanceOverviewVM> GetSectionPerformanceOverviewSR(ServiceCallVM examService)
        {
            return SpacedRepetitionBL.GetSectionPerformanceOverviewSR(examService);
        }

        // Get SpacedRepetition Performance Overview Chapter Data
        [Route("api/spacedRepetition/SRPerformanceOverviewChapter")]
        [HttpPost]
        public List<SRPerformanceOverviewChapterVM> GetChapterPerformanceOverviewSR(ServiceCallVM examService)
        {
            return SpacedRepetitionBL.GetChapterPerformanceOverviewSR(examService);
        }

        // Get SpacedRepetition Performance Overview Topics Data
        [Route("api/spacedRepetition/SRPerformanceOverviewTopics")]
        [HttpPost]
        public List<SRPerformanceOverviewTopicsVM> GetTopicsPerformanceOverviewSR(ServiceCallVM examService)
        {
            return SpacedRepetitionBL.GetTopicsPerformanceOverviewSR(examService);
        }

        // Get SpacedRepetition Recommended Topics Data
        [Route("api/spacedRepetition/SRRecommendedTopics")]
        [HttpPost]
        public List<SRRecommendedTopicsVM> GetRecommendedTopicsSR(ServiceCallVM examService)
        {
            return SpacedRepetitionBL.GetRecommendedTopicsSR(examService);
        }

        // Get SpacedRepetition Recommended Topics Data To Export
        [Route("api/spacedRepetition/SRRecommendedTopicsExport")]
        [HttpPost]
        public List<SRRecommendedTopicsExportVM> GetRecommendedTopicsExportSR(ServiceCallVM serviceCallVM, SRRecommendedTopicsExportVM sRRecommendedTopicsExportVM)
        {
            return SpacedRepetitionBL.GetRecommendedTopicsExportSR(serviceCallVM, sRRecommendedTopicsExportVM);
        }

        [Route("api/spacedRepetition/SRRecommendedDelete")]
        [HttpPost]
        public int DelRecommendedTopic(SRRecommendedTopicsDeletedVM delRecommendedTopic)
        {
            return SpacedRepetitionBL.DeleteRecommendedTopic(delRecommendedTopic);
        }
    }
}