using HLA.CSSMS.Shared.Dtos;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http;

namespace HLA.CSSMS.Client.Services.IMS.Submission.IMSSubmissionManager
{
    public class IMSSubmissionManager : IIMSSubmissionManager
    {
        private readonly HttpClient _http;
        private readonly IAuthService _authService;
        public IMSSubmissionManager(HttpClient http, IAuthService authService)
        {
            _http = http;
            _authService = authService;
        }

        public event System.Action OnChange;

        public List<IMSSubmissionsDto> SubmissionCases { get; set; } = new List<IMSSubmissionsDto>();
        public List<DocumentsDto> Documents { get; set; } = new List<DocumentsDto>();

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
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.Logout();
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<IMSSubmissionsDto>>>();
                if (result.Success)
                {
                    SubmissionCases = result.Data;
                }
            }
        }
        public async Task GetDocs(string refNo)
        {
            var response = await _http.GetAsync($"api/IMSSubmission/GetDocs/{refNo}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.Logout();
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<DocumentsDto>>>();
                if (result.Success)
                {
                    Documents = result.Data;
                }
            }
        }
    }
}
