using Core.Features.Usuario.command;
using Core.Features.Usuario.queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Catalogos;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public UsuarioController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Obtener datos del usuario
    /// </summary>
    [HttpGet()]
    public async Task<DataUserResponse> getData()
    {
        return await _mediator.Send(new DataUser(){ });
    }
    
    /// <summary>
    /// Validación de la clave del administrador
    /// </summary>
    [HttpGet("ClaveAdmin/{clave}")]
    public async Task<ClaveAdminResponse> getClaveAdmin(string clave)
    {
        return await _mediator.Send(new ClaveAdmin(){Clave = clave});
    }
    
    /// <summary>
    /// Foto de perfil del usuario
    /// </summary>
    [HttpGet("FotoPerfil")]
    public async Task<IActionResult> fotoPerfil()
    {
        var response = await _mediator.Send(new ViewPicture());
        return File(response.Foto, "image/jpeg", $"{response.Username}.jpg");
    }
    
    /// <summary>
    /// Crear usuario
    /// </summary>
    [AllowAnonymous]
    [HttpPost()]
    public async Task<IActionResult> PostUser([FromForm] CreateUser command)
    {
        await _mediator.Send(command);
        return Ok("Se registro el usuario correctamente");
    }
    
    /// <summary>
    /// Modificar usuario
    /// </summary>
    [HttpPut()]
    public async Task<IActionResult> PutUser([FromForm] ModificarUsuario command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico el usuario correctamente");
    }
    
    /// <summary>
    /// Modificar foto del usuario
    /// </summary>
    [HttpPut("Foto")]
    public async Task<IActionResult> PutFoto([FromForm] ModificarFoto command)
    {
        await _mediator.Send(command);
        return Ok("Se modifico la foto del usuario correctamente");
    }
}