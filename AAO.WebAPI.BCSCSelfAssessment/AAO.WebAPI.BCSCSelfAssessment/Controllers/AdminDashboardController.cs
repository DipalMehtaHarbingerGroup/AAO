namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using AAO.BAL.BCSCSelfAssessment;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DTO.BCSCSelfAssessment;

    // [RoutePrefix("api/login")]
    public class AdminDashboardController : ApiController
    {
        [Route("api/admindashboard/GetUserListView")]
        [HttpPost]
        [HttpGet]
        public List<UserDTO> GetUserList()
        {
            return UserBL.GetUserList().ToList();
        }

        [Route("api/admindashboard/SubmitQASData")]
        [HttpPost]
        public async Task<ResponseStatusVM> SubmitQASData(ServiceCallVM serviceCall)
        {
             return await ImportBL.ImportQuestion(serviceCall);
        }

        [Route("api/admindashboard/UpdateQASData")]
        [HttpPost]
        public async Task<ResponseStatusVM> UpdateQASData(ServiceCallVM serviceCall)
        {
            //serviceCall.Status = 7;
            //serviceCall.Modifiedafter = '03-11-2024';
            return await ImportBL.UpdateQASData(serviceCall, false);
        }

        // This API will call only once when new book addition within the systems
        [Route("api/admindashboard/UpdateBCSCExcerptQASData")]
        [HttpPost]
        public async Task<ResponseStatusVM> UpdateBCSCExcerptQASData(ServiceCallVM serviceCall)
        {
            return await ImportBL.UpdateQASData(serviceCall, true);
        }

        [Route("api/admindashboard/RetiredQASData")]
        [HttpPost]
        public async Task<ResponseStatusVM> RetiredQASData(ServiceCallVM serviceCall)
        {
            return await ImportBL.RetiredQASData(serviceCall, false);
        }

        [Route("api/admindashboard/RetiredBCSCExcerptQASData")]
        [HttpPost]
        public async Task<ResponseStatusVM> RetiredBCSCExcerptQASData(ServiceCallVM serviceCall)
        {
            return await ImportBL.RetiredQASData(serviceCall, true);
        }

        [Route("api/admindashboard/GetQuestionCount")]
        [HttpPost]
        public int GetQuestionCount(ServiceCallVM serviceCall)
        {
             return ImportBL.GetQuestionCount(serviceCall);
        }

        [Route("api/admindashboard/UpdateBCSCMapperContent")]
        [HttpPost]
        [HttpGet]
        public int UpdateBCSCMapperContent()
        {
            return ImportBL.UpdateBCSCMapperContent();
        }

        [Route("api/admindashboard/UpdateBCSCMapperContentField")]
        [HttpPost]
        [HttpGet]
        public int UpdateBCSCMapperContentField()
        {
            return ImportBL.UpdateBCSCMapperContentField();
        }

        [Route("api/admindashboard/UpdateBCSCMapperContentOnBookChange")]
        [HttpPost]
        [HttpGet]
        public int UpdateBCSCMapperContentOnBookChange()
        {
            //Function for Book Upload
            return ImportBL.UpdateBCSCMapperContentOnBookChange();
        }
    }
}
