using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Create;
using Models.Dtos.Response;
using Services;

namespace Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase {
    private readonly AuthService _authService;


    public AuthController(AuthService authService) {
        
        _authService = authService;

    }







    [HttpGet("availability/username")]
    async public Task<IActionResult> isUsernameAvailable([FromQuery] string username) {
        
        var isUsed = await _authService.IsUsernameAvailable(username);

        if(!isUsed) {
            return Conflict();
        }


        return Ok();
    }


    [HttpGet("availability/email")]
    async public Task<IActionResult> isEmailAvailable([FromQuery] string email) {
        
        var isUsed = await _authService.isEmailAvailable(email);

        if(!isUsed) {
            return Conflict();
        }


        return Ok();
    }



    [HttpPost("register")]
    async public Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createDto) {
        
        var user = await _authService.CreateUserAsync(createDto);

        if(user == null) {
            return BadRequest(new {
                message = "Email or username already in use."
            });
        }


        var userResponse = new AuthResponseDto {

            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            DisplayName = user.DisplayName

        };


        return Ok(userResponse);

    }
}

