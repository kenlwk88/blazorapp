namespace HLA.CSSMS.Client.Pages.UserPages
{
    public partial class Login
    {
        [Inject] IAuthService AuthService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        private LoginDto loginDto = new LoginDto();
        private bool ShowErrors;
        private string Error = "";
        private bool ShowLoading = false;
        private string hideLogin = isShowLogin(true);
        bool isShow;

        protected override async Task OnInitializedAsync()
        {
            await AuthService.Logout();
        }

        private async Task HandleLogin()
        {
            ShowErrors = false;

            ShowLoading = true;
            hideLogin = isShowLogin(false);
            var result = await AuthService.Login(loginDto);

            if (result.Success)
            {
                var returnUrl = NavManager.QueryString("returnUrl") ?? "/";
                NavManager.NavigateTo(returnUrl);
            }
            else
            {
                ShowLoading = false;
                hideLogin = isShowLogin(true);
                ShowErrors = true;
                Error = result.Message;
            }
        }
        private static string isShowLogin(bool isShow)
        {
            if (isShow)
                return "visible";
            return "hidden";
        }
    }
}
