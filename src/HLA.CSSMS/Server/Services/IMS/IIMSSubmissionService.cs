namespace HLA.CSSMS.Server.Services.IMS
{
    public interface IIMSSubmissionService
    {
        Task<ServiceResponse<List<IMSSubmissionsDto>>> GetSubmissionCases(IMSSubmissionFilter filter);
        Task<ServiceResponse<List<DocumentsDto>>> GetDocs(string refNo);
    }
}
