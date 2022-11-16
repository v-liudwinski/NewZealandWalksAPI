﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Repositories;

public class TokenHandler : ITokenHandler
{
    private readonly IConfiguration _configuration;

    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> CreateTokenAsync(User user)
    {
        // Create Claims
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.GivenName, user.FirstName));
        claims.Add(new Claim(ClaimTypes.Surname, user.LastName));
        claims.Add(new Claim(ClaimTypes.Email, user.EmailAddress));
        
        // Loop into roles of user
        user.Roles.ForEach(role =>
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        });
        
        // Token creation
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}