namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    public static class SpecialityBL
    {
        public static List<SubSpecialityDetailVM> GetSpecialityList(ExamDTO exam)
        {
            return SpecialityDAL.GetSpecialityList(exam.UserId);
        }
    }
}
