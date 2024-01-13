using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.VehiclesAuction.Api.Models;
using WebApi.VehiclesAuction.Domain.Interfaces.Services;
using WebApi.VehiclesAuction.Domain.Services;

namespace WebApi.VehiclesAuction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IParticipantServices _participantServices;

        public AuthController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration,
        RoleManager<IdentityRole> roleManager,
        IParticipantServices participantServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _participantServices = participantServices;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserViewModel registerViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new JsonResponse(false, "Erro ao tentar realizar o cadastro. Por favor, verifique os campos e tente novamente."));

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user is not null)
                return BadRequest(new JsonResponse(false, $"Usuário já cadastrado para o email informado."));

            var createParticipant = await _participantServices.AddParticipant(registerViewModel.Name, registerViewModel.Email, registerViewModel.Cep, cancellationToken);

            if (!createParticipant.Success)
                return BadRequest(new JsonResponse(false, createParticipant.GetErrorMessage()));

            var newUser = new IdentityUser { UserName = registerViewModel.Name, Email = registerViewModel.Email };
            var createUser = await _userManager.CreateAsync(newUser!, registerViewModel.Password);

            if (!createUser.Succeeded)
                return BadRequest(new JsonResponse(false, $"Erro ao tentar realizar o cadastro. Por favor tente novamente..."));

            if (registerViewModel.UserType is 1)
            {
                // Verifica se já existe a role criada na Base, se não existir, cria na hora
                var checkAdminRole = await _roleManager.FindByNameAsync("Admin");
                if (checkAdminRole is null)                
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });                

                await _userManager.AddToRoleAsync(newUser, "Admin");
            }
            else
            {
                var checkParticipantRole = await _roleManager.FindByNameAsync("Participant");
                if (checkParticipantRole is null)
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "Participant" });

                await _userManager.AddToRoleAsync(newUser, "Participant");
            }

            return BadRequest(new JsonResponse(true, "Cadastro realizado com sucesso!"));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new JsonResponse(false, "Erro ao tentar realizar o login. Por favor, verifique os campos e tente novamente."));

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user is null)
            {
                return Unauthorized(new JsonResponse(false, "Falha ao realizar o login. Usuário não encontrado."));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginViewModel.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new JsonResponse(false, "Falha ao realizar o login. Verifique suas credenciais e tente novamente."));
            }

            var userRole = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user.UserName, user.Email, userRole.First());
            return Ok(new JsonResponse(true, $"Login realizado com sucesso. Token: {token}"));
        }


        private string GenerateJwtToken(string userName, string email, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
                // TODO: SALVAR O ID
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
