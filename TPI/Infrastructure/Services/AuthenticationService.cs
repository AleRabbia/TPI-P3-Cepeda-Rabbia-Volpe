using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Models.Requests;
using Domain.Interfaces;
using Domain.Exceptions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class AuthenticationService : ICustomAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationServiceOptions _options;

        public AuthenticationService(IUserRepository userRepository, IOptions<AuthenticationServiceOptions> options)
        {
            _userRepository = userRepository;
            _options = options.Value;
        }
        private User? ValidateUser(AuthenticationRequest authenticationRequest)
        {
            if (string.IsNullOrEmpty(authenticationRequest.UserName) || string.IsNullOrEmpty(authenticationRequest.Password))
                return null;

            var user = _userRepository.GetByNameAsync(authenticationRequest.UserName);

            if (user != null && user.Password == authenticationRequest.Password)
            {
                return user;
            }

            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        public string Autenticar(AuthenticationRequest authenticationRequest)
        {
            //Paso 1: Validamos las credenciales
            var user = ValidateUser(authenticationRequest);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            //Paso 2: Crear el token
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("thisisthesecretforgeneratingakey(mustbeatleast32bitlong)")); //Traemos la SecretKey del Json. agregar antes: using Microsoft.IdentityModel.Tokens;
            var credentials = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>
        {
            new Claim("sub", user.Id.ToString()),
                new Claim("given_name", user.Name),
                new Claim("family_name", user.LastName),
                new Claim("role", user.Role.ToString())
        };

            var jwtSecurityToken = new JwtSecurityToken(
                _options.Issuer 
                , _options.Audience
                , claimsForToken 
                , DateTime.UtcNow 
                , DateTime.UtcNow.AddHours(1) 
                , credentials
                );

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return tokenToReturn.ToString();


        }

        public class AuthenticationServiceOptions
        {
            public const string AuthenticationService = "AuthenticationService";

            public string? Issuer { get; set; }
            public string? Audience { get; set; }
            public string? SecretForKey { get; set; }
        }

    }

}
