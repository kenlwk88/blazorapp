namespace HLA.CSSMS.Server.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> UserLogin(LoginDto user);
        Task<ServiceResponse<int>> UserRegister(RegisterDto user);
        Task<ServiceResponse<int>> CreateUserAccount(UserAccountDto user);
        Task<ServiceResponse<int>> UpdateUserAccount(UserAccountDto user);
        Task<ServiceResponse<int>> DeleteUserAccount(string userId);
        Task<ServiceResponse<int>> UpdateUserPassword(ChangePasswordDto user);
        string GetUserId();
    }
}
