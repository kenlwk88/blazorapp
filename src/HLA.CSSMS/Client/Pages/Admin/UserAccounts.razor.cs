namespace HLA.CSSMS.Client.Pages.Admin
{
    public partial class UserAccounts
    {
        [Inject] IUserAccountManager UserAccountManager { get; set; }

        int pageSize = 20;
        bool showError = false;
        bool showInfo = false;
        bool showLoading = false;
        string message = string.Empty;
        bool isInAccountEditMode = false;
        string selectedUserId = string.Empty;
        UserAccountDto user = new UserAccountDto();

        SfGrid<UserAccountDto> _grid;

        protected override async Task OnInitializedAsync()
        {
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
            Init();
            await UserAccountManager.GetUserAccounts();
            showLoading = false;
        }
        private async Task AddUserAccount()
        {
            Init();
            user = new UserAccountDto();
            isInAccountEditMode = true;
            selectedUserId = string.Empty;
        }


        private async Task EditUser()
        {
            Init();
            if (!string.IsNullOrEmpty(selectedUserId))
            {
                var result = await UserAccountManager.GetUserDetails(selectedUserId);
                if (result.Success)
                    user = result.Data;
                isInAccountEditMode = true;
            }
            else 
            {
                showError = true;
                message = "Please select a user";
                isInAccountEditMode = false;
            }
            selectedUserId = string.Empty;
        }


        private void CancelEdit()
        {
            Init();
            user = new UserAccountDto();
            isInAccountEditMode = false;
            selectedUserId = string.Empty;
        }


        private async Task UpdateUserAccount()
        {
            Init();
            if (!string.IsNullOrEmpty(user.UserId))
            {
                var result = await UserAccountManager.UpdateUserAccount(user);

                if (result.Success)
                {
                    showInfo = true;
                    message = result.Message;
                }
                else
                {
                    showError = true;
                    message = result.Message;
                }
            }
            else
            {
                var result = await UserAccountManager.CreateNewUserAccount(user);
                if (result.Success)
                {
                    showInfo = true;
                    message = result.Message;
                }
                else
                {
                    showError = true;
                    message = result.Message;
                }
            }

            user = new UserAccountDto();
            await UserAccountManager.GetUserAccounts();
            isInAccountEditMode = false;
            selectedUserId = string.Empty;
        }


        private async Task DeleteUser()
        {
            Init();
            if (!string.IsNullOrEmpty(selectedUserId))
            {
                var result = await UserAccountManager.DeleteUserAccount(selectedUserId);
                if (result.Success)
                {
                    showInfo = true;
                    message = result.Message;
                    await UserAccountManager.GetUserAccounts();
                }
                else
                {
                    showError = true;
                    message = result.Message;
                }
            }
            else
            {
                showError = true;
                message = "Please select a record";
            }
            selectedUserId = string.Empty;
        }
        private async Task GetSelectedRecords(RowSelectEventArgs<UserAccountDto> args)
        {
            Init();
            var user = await _grid.GetSelectedRecordsAsync();
            selectedUserId = user.FirstOrDefault().UserId;
            StateHasChanged();
        }
    }
}
