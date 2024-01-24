using HLA.CSSMS.Shared.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;

namespace HLA.CSSMS.Client.Services.IMS.Submission.IMSSubmissionManager
{
    public class IMSSubmissionManager : IIMSSubmissionManager
    {
        private readonly HttpClient _http;
        public IMSSubmissionManager(HttpClient http)
        {
            _http = http;
        }

        public event System.Action OnChange;

        public  List<IMSSubmissionsDto> SubmissionCases { get; set; } = new List<IMSSubmissionsDto>();

        public async Task GetList(DateTime fromDate, DateTime toDate, string refNo, string status, string policyNo)
        {
            IMSSubmissionFilter filter = new()
            {
                FromDate = fromDate,
                ToDate = toDate,
                RefNo = refNo,
                Status = status,
                PolicyNo = policyNo
            };
            var response = await _http.PostAsJsonAsync("api/IMSSubmission/GetList", filter);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<IMSSubmissionsDto>>>();
            if (result.Success)
            {
                SubmissionCases = result.Data;
            }
        }
    }
}
