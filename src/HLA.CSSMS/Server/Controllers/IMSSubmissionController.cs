
using HLA.CSSMS.Server.Services.IMS;
using HLA.CSSMS.Shared.Dtos.IMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLA.CSSMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IMSSubmissionController : ControllerBase
    {
        private readonly IIMSSubmissionService _iIMSSubmissionService;

        public IMSSubmissionController(IIMSSubmissionService iMSSubmissionService)
        {
            _iIMSSubmissionService = iMSSubmissionService;
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<ServiceResponse<List<IMSSubmissionsDto>>>> GetList([FromBody] IMSSubmissionFilter filter)
        {
            var result = await _iIMSSubmissionService.GetSubmissionCases(filter);
            return result;
        }
    }
}
