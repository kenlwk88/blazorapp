namespace HLA.CSSMS.Client.Pages.IMS.Submission
{
    public partial class IMSSubmissionsList
    {
        [Inject] IIMSSubmissionManager SubmissionManager { get; set; }

        int mode = 0; //0=main, 1=view error
        int pageSize = 20;
        bool showError = false;
        bool showInfo = false;
        bool showLoading = false;
        string message = string.Empty;
        DateTime fromDate;
        DateTime toDate;
        string filterRefNo = string.Empty;
        string filterStatus = string.Empty; //0=All, 1=In Progress, 2=Completed
        IMSSubmissionsDto submission = new IMSSubmissionsDto();

        SfGrid<IMSSubmissionsDto> _grid;
        protected override async Task OnInitializedAsync()
        {
            fromDate = DateTime.Now.AddDays(-7);
            toDate = DateTime.Now;
            filterRefNo = string.Empty;
            filterStatus = "0";
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
                    await SubmissionManager.GetList(fromDate, toDate, filterRefNo, filterStatus);
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
        private async Task ViewError()
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
        private void CancelViewError()
        {
            Init();
            mode = 0;
            submission = new IMSSubmissionsDto();
        }
    }
}
