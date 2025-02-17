using Core.Features.Cuenta.command;
using Core.Features.Cuenta.queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Catalogos;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class CuentaController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public CuentaController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Inicio de Sesión
    /// </summary>
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<LoginResponse> Login([FromBody] Login command)
    {
        return await _mediator.Send(command);
    }
    
    /// <summary>
    /// Validación de Token
    /// </summary>
    [AllowAnonymous]
    [HttpGet("VerifyUser/{token}")]
    public async Task<VerifyUserResponse> getVeryfyUser(string token)
    {
        return await _mediator.Send(new VerifyUser(){token = token});
    }
    
    /// <summary>
    /// Refrescar Token
    /// </summary>
    [AllowAnonymous]
    [HttpGet("RefreshToken")]
    public async Task<TokenRefreshResponse> Refresh([FromQuery] string refresh)
    {
        return await _mediator.Send(new TokenRefresh(){ Refresh = refresh });
    }
    
    /// <summary>
    /// Modificacion de contraseña
    /// </summary>
    [HttpPut("Contraseña")]
    public async Task<IActionResult> ModificacioContraseña([FromBody] ModificarContraseñas command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico la contraseña correctamente");
    }
    
    /// <summary>
    /// Modificacion de clave
    /// </summary>
    [HttpPut("Clave")]
    public async Task<IActionResult> ModificacioClave([FromBody] ModificarClave command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico la clave correctamente");
    }
    
    /// <summary>
    /// Cerrar Sesión
    /// </summary>
    [HttpDelete("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new Logout());
        return Ok("Se ha cerrado la sesión");
    }
}