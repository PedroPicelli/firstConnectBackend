using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Create;
using Models.Dtos.Response;
using Models.Dtos.Login;
using Services;

namespace Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase {
    private readonly AuthService _authService;
    private readonly UserService _userService;
    private readonly TokenService _tokenService;


    public AuthController(UserService userService, AuthService authService, TokenService tokenService) {
        _userService = userService;
        _authService = authService;
        _tokenService = tokenService;

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



    [HttpPost("login")]
    async public Task<IActionResult> LoginAccount([FromBody] LoginAccountDto loginDto) {
        
        var user = await _authService.LoginUserAsync(loginDto);

        if(user == null) {
            return Unauthorized();
        }
        

        // Creating Token

        var token = _tokenService.GenerateToken(user);


        return Ok(new {
            token,
            user = _userService.getUserResponseDto(await _userService.GetFullUserAsync(user.Id))
        });
    }

    [HttpPost("register")]
    async public Task<IActionResult> CreateAccount([FromBody] CreateAccountDto createDto) {
        
        var user = await _authService.CreateUserAsync(createDto);

        if(user == null) {
            return Conflict(new {
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

