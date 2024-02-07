namespace HLA.CSSMS.Client.Pages.UserPages
{
    public partial class Register
    {
        [Inject] IAuthService AuthService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        RegisterDto user = new RegisterDto();
        string message = string.Empty;
        string messageCssClass = string.Empty;
        bool showLoading = false;

        async Task HandleRegistration()
        {
            showLoading = true;
            var result = await AuthService.Register(user);
            showLoading = false;
            message = result.Message;
            if (result.Success)
                NavManager.NavigateTo("/login");
            else
                messageCssClass = "text-danger";
        }
    }
}
