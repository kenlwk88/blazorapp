namespace HLA.CSSMS.Client.Pages.UserPages
{
    public partial class Logout
    {
        [Inject] IAuthService AuthService { get; set; }
        [Inject] NavigationManager NavManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await AuthService.Logout();
            NavManager.NavigateTo("/login", true);
        }
    }
}
