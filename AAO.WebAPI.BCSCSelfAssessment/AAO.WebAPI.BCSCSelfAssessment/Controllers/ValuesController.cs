namespace AAO.WebAPI.BCSCSelfAssessment.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using AAO.WebAPI.BCSCSelfAssessment.ExceptionFilter;

    [RoutePrefix("api/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [CustomExceptionFilter]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
