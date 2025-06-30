using Prova.Data;
using Prova.Models;
using Prova.Services;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Prova.Controllers;
[ApiController]
[Route("api/lancamentos")]
[Authorize]
public class LancamentosController : ControllerBase
{
    private readonly AppDbContext _context;
    
    private int ObterUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null ? int.Parse(claim.Value) : throw new UnauthorizedAccessException("Usuário não autenticado.");
    }
    public LancamentosController(AppDbContext context)
    {
        _context = context;
        
    }
    [HttpPost("/categoria")]
    public async Task<IActionResult> CreateCategoria([FromBody] Categoria categoria)
    {
        var usuarioId = ObterUsuarioId();
        

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return CreatedAtAction("Get", new { id = categoria.Id }, categoria);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Lancamento lancamento)
    {
        var usuarioId = ObterUsuarioId();
        if (usuarioId != lancamento.UsuarioId)
            return Unauthorized();

        _context.Lancamentos.Add(lancamento);
        await _context.SaveChangesAsync();
        return CreatedAtAction("Get", new { id = lancamento.Id }, lancamento);
    }

    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lancamento>>> Get()
    {
        var usuarioId = ObterUsuarioId();
        return await _context.Lancamentos.Where(l => l.UsuarioId == usuarioId).ToListAsync();
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Lancamento lancamento)
    {
        var usuarioId = ObterUsuarioId();
        var existingLancamento = await _context.Lancamentos.FindAsync(id);

        if (existingLancamento == null || existingLancamento.UsuarioId != usuarioId)
            return Unauthorized();

        existingLancamento.Valor = lancamento.Valor;
        existingLancamento.Descricao = lancamento.Descricao;
        existingLancamento.CategoriaId = lancamento.CategoriaId;
        existingLancamento.Tipo = lancamento.Tipo;

        _context.Lancamentos.Update(existingLancamento);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuarioId = ObterUsuarioId();
        var lancamento = await _context.Lancamentos.FindAsync(id);

        if (lancamento == null || lancamento.UsuarioId != usuarioId)
            return Unauthorized();

        _context.Lancamentos.Remove(lancamento);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    
    [HttpGet("categorias")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
    {
        return await _context.Categorias.ToListAsync();
    }
}