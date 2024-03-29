﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HLA.CSSMS.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserDbContext context,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
        }



        public string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);


        public async Task<ServiceResponse<string>> UserLogin(LoginDto user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
            if (!result.Succeeded)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Username and password are invalid."
                };
            }

            List<Claim> claims = await CreateClaims(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );
            var response = new ServiceResponse<string>
            {
                Success = true,
                Data = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "Registration successful!"
            };

            return response;
        }


        public async Task<ServiceResponse<int>> UserRegister(RegisterDto user)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User already exists."
                };
            }

            var displayName = user.FirstName;
            if (!string.IsNullOrEmpty(user.DisplayName))
                displayName = user.DisplayName;

            var newUser = new ApplicationUser { 
                UserName = user.Email, 
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = displayName
            };
            var result = await _userManager.CreateAsync(newUser, user.Password);
            await _userManager.AddToRoleAsync(newUser, "User");

            if (!result.Succeeded)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Registration failed!"
                };
            }

            return new ServiceResponse<int> {
                Success = true,
                Message = "Registration successful!" 
            };
        }


        public async Task<ServiceResponse<int>> CreateUserAccount(UserAccountDto user)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User already exists."
                };
            }

            var displayName = user.FirstName;
            if (!string.IsNullOrEmpty(user.DisplayName))
                displayName = user.DisplayName;

            var newCreateUser = new ApplicationUser {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = displayName,
                Notes = user.Notes,
            };

            var result = await _userManager.CreateAsync(newCreateUser, user.Password);
            await _userManager.AddToRoleAsync(newCreateUser, user.UserRole);

            if (!result.Succeeded)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "New account creation failed!"
                };
            }
            
            return new ServiceResponse<int> { 
                Success = true,
                Message = "New account creation successful!" 
            };
        }


        public async Task<ServiceResponse<int>> UpdateUserAccount(UserAccountDto user)
        {
            var currentUser = await _userManager.FindByIdAsync(user.UserId);
            currentUser.UserName = user.UserName;
            currentUser.NormalizedUserName = user.UserName.ToUpper();
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.DisplayName = user.DisplayName;
            currentUser.Notes = user.Notes;

            var result = await _userManager.UpdateAsync(currentUser);

            foreach (var role in Enum.GetValues(typeof(Roles)))
                await _userManager.RemoveFromRoleAsync(currentUser, role.ToString());
               
            await _userManager.AddToRoleAsync(currentUser, user.UserRole);

            if (!result.Succeeded)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Account updated failed!"
                };
            }

            return new ServiceResponse<int>
            {
                Success = true,
                Message = "Account updated successful!"
            };
        }



        public async Task<ServiceResponse<int>> DeleteUserAccount(string userId)
        {
            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser == null)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User account not found"
                };
            }
            var result = await _userManager.DeleteAsync(currentUser);

            if (!result.Succeeded)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User account deleted failed!"
                };
            }
            
            return new ServiceResponse<int>
            {
                Success = true,
                Message = "User account deleted successfully!"
            };
        }






        private async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower()
                .Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }


        private async Task<List<Claim>> CreateClaims(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email) ?? await _userManager.FindByNameAsync(login.Email);
            var roles = await _userManager.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, login.Email)
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }

        public async Task<ServiceResponse<int>> UpdateUserPassword(ChangePasswordDto user)
        {
            var currentUser = await _userManager.FindByIdAsync(user.UserId);
            var generateToken = await _userManager.GeneratePasswordResetTokenAsync(currentUser);
            if (!string.IsNullOrEmpty(generateToken))
            {
                var result = await _userManager.ResetPasswordAsync(currentUser, generateToken, user.Password);
                if (!result.Succeeded)
                {
                    return new ServiceResponse<int>
                    {
                        Success = false,
                        Message = "Password updated failed!"
                    };
                }
            }
            else 
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "Password updated failed!"
                };
            }
            return new ServiceResponse<int>
            {
                Success = true,
                Message = "Password updated successful!"
            };
        }
    }
}
