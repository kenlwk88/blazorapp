using Microsoft.AspNetCore.Mvc;

namespace HLA.CSSMS.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<ServiceResponse<string>> Login([FromBody] LoginDto request)
        {
            var result = await _authService.UserLogin(request);
            return result;
        }



        [HttpPost("Register")]
        public async Task<ServiceResponse<int>> Post([FromBody] RegisterDto request)
        {
            var result = await _authService.UserRegister(request);
            return result;
        }




        [HttpPost("CreateUserAccount")]
        [Authorize(Policy = "IsSuperAdmin")]
        public async Task<ServiceResponse<int>> CreateUserAccount(UserAccountDto request)
        {
            var result = await _authService.CreateUserAccount(request);
            return result;
        }


        [HttpPost("UpdateUserAccount")]
        [Authorize(Policy = "IsSuperAdmin")]
        public async Task<ServiceResponse<int>> UpdateUserAccount(UserAccountDto request)
        {
            var result = await _authService.UpdateUserAccount(request);
            return result;
        }



        [HttpDelete("DeleteUserAccount/{userId}")]
        [Authorize(Policy = "IsSuperAdmin")]
        public async Task<ServiceResponse<int>> DeleteUserAccount(string userId)
        {
            var result = await _authService.DeleteUserAccount(userId);
            return result;
        }





    }
}
