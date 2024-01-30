using Microsoft.JSInterop;
using Microsoft.JSInterop.WebAssembly;
using System;
using System.Security.Principal;

namespace HLA.CSSMS.Client.Pages.IMS.Submission
{
    public partial class IMSSubmissionsList
    {
        [Inject] IIMSSubmissionManager SubmissionManager { get; set; }
        [Inject] IJSRuntime jSRuntime { get; set; }

        int mode = 0; //0=main, 1=view error
        int pageSize = 20;
        bool showError = false;
        bool showInfo = false;
        bool showLoading = false;
        string message = string.Empty;
        DateTime fromDate;
        DateTime toDate;
        string filterRefNo = string.Empty;
        string filterPolicyNo = string.Empty;
        string filterStatus = string.Empty; //0=All, 1=In Progress, 2=Completed
        IMSSubmissionsDto submission = new IMSSubmissionsDto();
        DocumentsDto doc = new DocumentsDto();

        SfGrid<IMSSubmissionsDto> _grid;
        SfGrid<DocumentsDto> _gridDoc;
        protected override async Task OnInitializedAsync()
        {
            fromDate = DateTime.Now.AddDays(-5);
            toDate = DateTime.Now;
            filterRefNo = string.Empty;
            filterStatus = "0";
            await ReLoad();
        }

        private void Init()
        {
            showError = false;
            showInfo = false;
            message = string.Empty;
        }
        private async Task ReLoad()
        {
            showLoading = true;
            var totalDays = (toDate - fromDate).TotalDays;
            if (toDate >= fromDate)
            {
                if (totalDays > -1 && totalDays < 31)
                {
                    Init();
                    submission = new IMSSubmissionsDto();
                    await SubmissionManager.GetList(new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0), new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59), filterRefNo, filterStatus, filterPolicyNo);
                }
                else 
                {
                    showError = true;
                    message = "Only allow select last 30 days records";
                }
            }
            else 
            {
                showError = true;
                message = "Invalid date range";
            }
            showLoading = false;
        }
        private async Task GetSelectedRecords(RowSelectEventArgs<IMSSubmissionsDto> args)
        {
            Init();
            submission = (await _grid.GetSelectedRecordsAsync()).FirstOrDefault();
            StateHasChanged();
        }
        private async Task ViewSubmission()
        {
            Init();
            if (submission != null && !string.IsNullOrEmpty(submission.RefNo))
            {
                mode = 1;
            }
            else
            {
                showError = true;
                message = "Please select a record";
            }
        }
        private void CancelView()
        {
            Init();
            mode = 0;
            submission = new IMSSubmissionsDto();
        }
        private async Task ViewDocuments()
        {
            Init();
            if (submission != null && !string.IsNullOrEmpty(submission.RefNo))
            {
                await SubmissionManager.GetDocs(submission.RefNo);
                mode = 2;
            }
            else
            {
                showError = true;
                message = "Please select a record";
            }
        }
        private async Task GetSelectedDoc(RowSelectEventArgs<DocumentsDto> args)
        {
            Init();
            var doc = (await _gridDoc.GetSelectedRecordsAsync()).FirstOrDefault();
            if (doc != null) 
            {
                await jSRuntime.InvokeAsync<object>("open", doc.Url, "_blank");
            }
        }
    }
}
