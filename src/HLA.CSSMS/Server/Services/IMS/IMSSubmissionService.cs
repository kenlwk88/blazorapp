﻿namespace HLA.CSSMS.Server.Services.IMS
{
    public class IMSSubmissionService : IIMSSubmissionService
    {
        private readonly IConfiguration _configuration;
        private readonly SubmissionRepo _submissionRepo;
        public IMSSubmissionService(IConfiguration configuration, SubmissionRepo submissionRepo)
        {
            _configuration = configuration;
            _submissionRepo = submissionRepo;
        }
        public async Task<ServiceResponse<List<IMSSubmissionsDto>>> GetSubmissionCases(IMSSubmissionFilter filter)
        {
            var response = new ServiceResponse<List<IMSSubmissionsDto>>();
            var data = await _submissionRepo.GetSubmissionCases(filter);
            List<IMSSubmissionsDto> result = new List<IMSSubmissionsDto>();
            result = data.GroupBy(x => x.RefNo).Select(y => y.First()).ToList();
            foreach (var item in result)
            {
                if (item.CopyStatus != "Completed")
                {
                    var errors = data.Where(x => x.RefNo == item.RefNo).Select(y => y.CopyErrorMsg).ToList();
                    var checkerror = errors.FirstOrDefault();
                    if (!string.IsNullOrEmpty(checkerror))
                        item.Errors.Messages = errors;
                }
                else if (item.DecyptStatus != "Completed")
                {
                    var errors = data.Where(x => x.RefNo == item.RefNo).Select(y => y.DecyptErrorMsg).ToList();
                    var checkerror = errors.FirstOrDefault();
                    if (!string.IsNullOrEmpty(checkerror))
                        item.Errors.Messages = errors;
                }
                else if (item.EAIStatus != "Completed")
                {
                    var errors = data.Where(x => x.RefNo == item.RefNo).Select(y => y.EAIErrorMsg).ToList();
                    var checkerror = errors.FirstOrDefault();
                    if (!string.IsNullOrEmpty(checkerror))
                        item.Errors.Messages = errors;
                }
                else if (item.ConvertStatus != "Completed")
                {
                    var errors = data.Where(x => x.RefNo == item.RefNo).Select(y => y.ConvertErrorMsg).ToList();
                    var checkerror = errors.FirstOrDefault();
                    if (!string.IsNullOrEmpty(checkerror))
                        item.Errors.Messages = errors;
                }
                else if (item.ExportStatus != "Completed")
                {
                    var errors = data.Where(x => x.RefNo == item.RefNo).Select(y => y.ExportErrorMsg).ToList();
                    var checkerror = errors.FirstOrDefault();
                    if (!string.IsNullOrEmpty(checkerror))
                        item.Errors.Messages = errors;
                }
            }
            if (data != null)
            {
                response.Success = true;
                response.Data = result;
            }
            else
            {
                response.Success = false;
                response.Message = "Failed to retrieve data";
            }
            return response;
        }
        public async Task<ServiceResponse<List<DocumentsDto>>> GetDocs(string refNo)
        {
            var response = new ServiceResponse<List<DocumentsDto>>();
            var data = await _submissionRepo.GetDocs(refNo);
            foreach (var item in data)
            {
                string[] arr = item.SourcePath.Split('\\');
                var filename = arr[arr.Length - 1];
                var url = _configuration["IMS_Submission_Pdf_Url"] + $"/{item.RefNo}/{filename}";
                item.Url = url;
            }
            if (data != null)
            {
                response.Success = true;
                response.Data = data.ToList();
            }
            else
            {
                response.Success = false;
                response.Message = "Failed to retrieve data";
            }
            return response;
        }
    }
}
