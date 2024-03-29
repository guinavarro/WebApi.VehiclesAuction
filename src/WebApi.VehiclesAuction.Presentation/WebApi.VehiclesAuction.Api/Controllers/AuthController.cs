﻿using Microsoft.AspNetCore.Authentication.BearerToken;
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

        ///<remarks>
        /// Cadastra o usuário com a role de Admin nas tabelas do Identity
        /// </remarks>
        /// <summary>
        /// Cadastro de usuário
        /// </summary>
        /// <param name="registerViewModel">Parâmetros para cadastro de usuário Admin.</param>
        /// <response code="200">Usuário cadastrado com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="500">Retorna erros internos caso ocorram</response>
        /// <returns></returns>
        [ProducesResponseType(typeof(JsonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost("register-admin-user")]
        public async Task<IActionResult> RegisterAdminUser([FromBody] RegisterAdminUserViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new JsonResponse(false, "Erro ao tentar realizar o cadastro. Por favor, verifique os campos e tente novamente."));

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
            if (user is not null)
                return BadRequest(new JsonResponse(false, $"Usuário já cadastrado para o email informado."));

            var newUser = new IdentityUser { UserName = registerViewModel.Name, Email = registerViewModel.Email };
            var createUser = await _userManager.CreateAsync(newUser!, registerViewModel.Password);

            if (!createUser.Succeeded)
                return BadRequest(new JsonResponse(false, $"Erro ao tentar realizar o cadastro. Por favor tente novamente..."));

            // Verifica se já existe a role criada na Base, se não existir, cria na hora
            var checkAdminRole = await _roleManager.FindByNameAsync("Admin");
            if (checkAdminRole is null)
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });

            await _userManager.AddToRoleAsync(newUser, "Admin");

            return Ok(new JsonResponse(true, "Cadastro realizado com sucesso!"));
        }


        ///<remarks>
        /// Cadastra o usuário com a role de Participant na tabela de Participant
        /// </remarks>
        /// <summary>
        /// Cadastro de usuário
        /// </summary>
        /// <param name="registerViewModel">Parâmetros para cadastro de usuário Participant.</param>
        /// <param name="cancellationToken">Cancellation Token para o cancelamento dos métodos assíncronos</param>
        /// <response code="200">Usuário cadastrado com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="500">Retorna erros internos caso ocorram</response>
        /// <returns></returns>
        [ProducesResponseType(typeof(JsonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost("register-participant-user")]
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

            var checkParticipantRole = await _roleManager.FindByNameAsync("Participant");
            if (checkParticipantRole is null)
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Participant" });

            await _userManager.AddToRoleAsync(newUser, "Participant");

            return Ok(new JsonResponse(true, "Cadastro realizado com sucesso!"));
        }


        ///<remarks>
        /// Realiza a autenticação do usuário na API, independente da role. Retornando um Bearer Token.
        /// </remarks>
        /// <summary>
        /// Autentização de Usuários
        /// </summary>
        /// <param name="loginViewModel">Parâmetros para autenticação dos usuários.</param>
        /// <response code="200">Usuário autenticado com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="500">Retorna erros internos caso ocorram</response>
        /// <returns></returns>
        [ProducesResponseType(typeof(JsonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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
            int? participantId = null;

            if (userRole.First() != "Admin")
            {
                var getParticipantId = await _participantServices.GetParticipantIdByEmail(loginViewModel.Email);

                if (getParticipantId.Success)
                    participantId = getParticipantId.Object;
            }

            var token = GenerateJwtToken(user.UserName, user.Email, userRole.First(), participantId);
            return Ok(new JsonResponse(true, $"Login realizado com sucesso. Token: {token}"));
        }


        private string GenerateJwtToken(string userName, string email, string role, int? participantId = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim("ParticipantId", participantId != null ? participantId.ToString() : string.Empty)
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
