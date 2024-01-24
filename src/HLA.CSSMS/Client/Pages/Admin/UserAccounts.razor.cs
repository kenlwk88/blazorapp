using HLA.CSSMS.Shared.Dtos;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace HLA.CSSMS.Client.Pages.Admin
{
    public partial class UserAccounts
    {
        [Inject] IUserAccountManager UserAccountManager { get; set; }

        int mode = 0; //0=main, 1=add/edit
        int pageSize = 20;
        bool showError = false;
        bool showInfo = false;
        bool showLoading = false;
        string message = string.Empty;
        string selectedUserId = string.Empty;
        UserAccountDto user = new UserAccountDto();
        ChangePasswordDto changePasswordDto = new ChangePasswordDto();

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
            mode = 1;
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
                mode = 1;
            }
            else 
            {
                showError = true;
                message = "Please select a record";
                mode = 0;
            }
            selectedUserId = string.Empty;
        }


        private void CancelEdit()
        {
            Init();
            user = new UserAccountDto();
            mode = 0;
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
            mode = 0;
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
        private async Task ChangeUserPassword()
        {
            Init();
            if (!string.IsNullOrEmpty(selectedUserId))
            {
                changePasswordDto = new ChangePasswordDto();
                var result = await UserAccountManager.GetUserDetails(selectedUserId);
                changePasswordDto.UserId = selectedUserId;
                changePasswordDto.UserName = result.Data.UserName;
                changePasswordDto.Email = result.Data.Email;
                mode = 2;
            }
            else 
            {
                showError = true;
                message = "Please select a record";
            }
        }
        private void CancelChangeUserPassword()
        {
            Init();
            changePasswordDto = new ChangePasswordDto();
            selectedUserId = string.Empty;
            mode = 0;
        }
        private async Task UpdateUserPassword()
        {
            Init();
            if (!string.IsNullOrEmpty(changePasswordDto.UserId))
            {
                var result = await UserAccountManager.UpdateUserPassword(changePasswordDto);

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
                changePasswordDto = new ChangePasswordDto();
                selectedUserId = string.Empty;
                await UserAccountManager.GetUserAccounts();
                mode = 0;
            }
            else
            {
                showError = true;
                message = "Please select a record";
                mode = 0;
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
