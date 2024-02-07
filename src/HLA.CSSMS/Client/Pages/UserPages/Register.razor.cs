namespace HLA.CSSMS.Client.Pages.UserPages
{
    public partial class Register
    {
        [Inject] IAuthService AuthService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        RegisterDto user = new RegisterDto();
        string message = string.Empty;
        string messageCssClass = string.Empty;

        async Task HandleRegistration()
        {
            var result = await AuthService.Register(user);
            message = result.Message;
            if (result.Success)
                NavManager.NavigateTo("/login");
            else
                messageCssClass = "text-danger";
        }
    }
}
