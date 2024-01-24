namespace HLA.CSSMS.Client.Services.UserAccountManager
{
    public class UserAccountManager : IUserAccountManager
    {
        private readonly HttpClient _http;
        public UserAccountManager(HttpClient http)
        {
            _http = http;
        }

        public event System.Action OnChange;

        public List<UserAccountDto> UserList { get; set; } = new List<UserAccountDto>();

        public async Task GetUserAccounts()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<UserAccountDto>>>("api/UserAccount/GetUserAccounts");

            if (response != null && response.Data != null)
                UserList = response.Data;
        }

        public async Task<ServiceResponse<UserAccountDto>> GetUserDetails(string userId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<UserAccountDto>>($"api/UserAccount/GetUserDetails/{userId}");
            return response;
        }

        public async Task<ServiceResponse<int>> CreateNewUserAccount(UserAccountDto userAccount)
        {
            var response = await _http.PostAsJsonAsync("api/auth/CreateUserAccount", userAccount);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
            return result;
        }

        public async Task<ServiceResponse<int>> UpdateUserAccount(UserAccountDto userAccount)
        {
            var response = await _http.PostAsJsonAsync("api/auth/UpdateUserAccount", userAccount);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
            return result;
        }

        public async Task<ServiceResponse<int>> DeleteUserAccount(string userId)
        {
            var response = await _http.DeleteAsync($"api/auth/DeleteUserAccount/{userId}");
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
            return result;
        }
        public async Task<ServiceResponse<int>> UpdateUserPassword(ChangePasswordDto updateUserPassword)
        {
            var response = await _http.PostAsJsonAsync("api/auth/UpdateUserPassword", updateUserPassword);
            var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
            return result;
        }
    }
}
