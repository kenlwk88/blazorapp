namespace HLA.CSSMS.Client.Services.IMS.Submission.IMSSubmissionManager
{
    public interface IIMSSubmissionManager
    {
        event System.Action OnChange;
        List<IMSSubmissionsDto> SubmissionCases { get; set; }
        List<DocumentsDto> Documents { get; set; }
        Task GetList(DateTime fromDate, DateTime toDate, string refNo, string status, string policyNo);
        Task GetDocs(string refNo);
    }
}
