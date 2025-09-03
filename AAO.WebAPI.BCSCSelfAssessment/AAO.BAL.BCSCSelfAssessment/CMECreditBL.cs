namespace AAO.BAL.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using Common.BCSCSelfAssessment;
    using DAL.BCSCSelfAssessment;

    public static class CMECreditBL
    {
        public static List<CMECreditVM> GetCreditDetails(UserIdVM user)
        {
            return CMECreditDAL.GetCreditDetails(user);
        }
    }
}
