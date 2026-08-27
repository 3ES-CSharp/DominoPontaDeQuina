using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class UsuarioRepository : RepositoryBase<Usuario>
{
    public UsuarioRepository(DominoDbContext context) : base(context)
    {
    }

    public Usuario? ObterPorEmail(string email) =>
        Entidades.FirstOrDefault(u => u.Email == email);

    public Usuario? ObterComJogadores(Guid id) =>
        Entidades.Include(u => u.Jogadores)
                 .FirstOrDefault(u => u.Id == id);

    public List<Usuario> BuscarPorNome(string trecho) =>
        Entidades.Where(u => u.Nome.Contains(trecho))
                 .OrderBy(u => u.Nome)
                 .ToList();

    public bool EmailEmUso(string email) =>
        Entidades.Any(u => u.Email == email);

    /// <summary>Usuarios que ainda nao criaram nenhum perfil de jogador.</summary>
    public List<Usuario> SemJogadores() =>
        Entidades.Where(u => !u.Jogadores.Any())
                 .OrderByDescending(u => u.CriadoEm)
                 .ToList();
}
