﻿using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenHandler _tokenHandler;

    public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
    {
        _userRepository = userRepository;
        _tokenHandler = tokenHandler;
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Index(Models.DTO.LoginRequest loginRequest)
    {
        // Check authentication
        var user = await _userRepository.AuthenticateAsync(
            loginRequest.Username, loginRequest.Password);
        // Generate JWT Token
        if (user is not null)
        {
            var token = await _tokenHandler.CreateTokenAsync(user);
            return Ok(token);
        }

        return BadRequest("Username or password is incorrect");
    }
}