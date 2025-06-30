using Prova.Data;
using Prova.Services;
using Prova.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
namespace Prova.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{

    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    
    public IActionResult Login([FromBody] Usuario model, [FromServices] AppDbContext context)
        {
            var usuario = context.Usuarios.FirstOrDefault(x => x.Email == model.Email && x.Senha == model.Senha);
            if (usuario == null) return Unauthorized();

            var token = _jwtService.GenerateToken(usuario);
            return Ok(new { token });
        }
     [HttpPost("register")]
        public IActionResult Register([FromBody] Usuario model, [FromServices] AppDbContext context)
        {
            context.Usuarios.Add(model);
            context.SaveChanges();
            return Created("", model);
        }
}




