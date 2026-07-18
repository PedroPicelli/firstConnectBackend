using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Create;
using Models.Dtos.Response;
using Models.Dtos.Login;
using Services;
using Models.Dtos.Edit;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Models.Database;

namespace Controllers;

[ApiController]
[Route("user")]
public class UserController : ControllerBase {
    private readonly UserService _userService;
    private readonly TokenService _tokenService;


    public UserController(UserService userService, TokenService tokenService) {
        
        _userService = userService;
        _tokenService = tokenService;

    }


    [Authorize]
    [HttpGet("me")]
    async public Task<IActionResult> GetUser() {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null) {
            return Unauthorized();
        }
        
        try {
            var user = await _userService.GetFullUserAsync(int.Parse(userId));

            return Ok(_userService.getUserResponseDto(user));

        } catch(KeyNotFoundException) {
            return NotFound();
        }
    }



    [Authorize]
    [HttpPut("me")]
    async public Task<IActionResult> EditProfile([FromBody] EditProfileDto editDto) {
    
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(userId == null) {
            return Unauthorized();
        }

        try {
            var user = await _userService.EditProfileAsync(editDto, int.Parse(userId));


            var token = _tokenService.GenerateToken(user);

            return Ok(new {
                Token = token,
                User = _userService.getUserResponseDto(await _userService.GetFullUserAsync(int.Parse(userId)))
            });

        } catch(KeyNotFoundException) {
            
            return NotFound();

        }

    }


}

