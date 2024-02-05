namespace HLA.CSSMS.Client.Services.UserAccountManager
{
    public class UserAccountManager : IUserAccountManager
    {
        private readonly HttpClient _http;
        private readonly IAuthService _authService;
        public UserAccountManager(HttpClient http, IAuthService authService)
        {
            _http = http;
            _authService = authService;
        }

        public event System.Action OnChange;

        public List<UserAccountDto> UserList { get; set; } = new List<UserAccountDto>();

        public async Task GetUserAccounts()
        {
            var response = await _http.GetAsync("api/UserAccount/GetUserAccounts");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.Logout();
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<List<UserAccountDto>>>();
                if (result.Success)
                {
                    UserList = result.Data;
                }
            }
        }

        public async Task<ServiceResponse<UserAccountDto>> GetUserDetails(string userId)
        {
            var response = await _http.GetAsync($"api/UserAccount/GetUserDetails/{userId}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.Logout();
                return new();
            }
            else
            {
                return await response.Content.ReadFromJsonAsync<ServiceResponse<UserAccountDto>>();
            }
        }

        public async Task<ServiceResponse<int>> CreateNewUserAccount(UserAccountDto userAccount)
        {
            var response = await _http.PostAsJsonAsync("api/auth/CreateUserAccount", userAccount);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.Logout();
                return new();
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
                return result;
            }
        }

        public async Task<ServiceResponse<int>> UpdateUserAccount(UserAccountDto userAccount)
        {
            var response = await _http.PostAsJsonAsync("api/auth/UpdateUserAccount", userAccount);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.Logout();
                return new();
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
                return result;
            }
        }

        public async Task<ServiceResponse<int>> DeleteUserAccount(string userId)
        {
            var response = await _http.DeleteAsync($"api/auth/DeleteUserAccount/{userId}");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.Logout();
                return new();
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
                return result;
            }
        }
        public async Task<ServiceResponse<int>> UpdateUserPassword(ChangePasswordDto updateUserPassword)
        {
            var response = await _http.PostAsJsonAsync("api/auth/UpdateUserPassword", updateUserPassword);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authService.Logout();
                return new();
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ServiceResponse<int>>();
                return result;
            }
        }
    }
}
