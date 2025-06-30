namespace Prova.Models;
public class Lancamento
{
    public int Id { get; set; }
    public decimal Valor { get; set; }
    public string Descricao { get; set; }
    public DateTime Data { get; set; }
    public int UsuarioId { get; set; }
    public int CategoriaId { get; set; }
    public string Tipo { get; set; }
    public Usuario Usuario { get; set; }
    public Categoria Categoria { get; set; }
}