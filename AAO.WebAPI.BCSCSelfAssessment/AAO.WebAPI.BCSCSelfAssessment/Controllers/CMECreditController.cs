namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;

    public class CMECreditController : ApiController
    {
        [Route("api/CMECredit/GetCreditDetails")]
        [HttpPost]
        public List<CMECreditVM> GetCreditDetails(UserIdVM user)
        {
            return CMECreditBL.GetCreditDetails(user);
        }
    }
}
