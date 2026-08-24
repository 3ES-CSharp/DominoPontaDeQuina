using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class UsuarioRepository
{
    private readonly DominoDbContext _context;

    public UsuarioRepository(DominoDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
    }

    public List<Usuario> Listar()
    {
        return _context.Usuarios
            .Include(usuario => usuario.Jogadores)
            .ToList();
    }

    public Usuario? BuscarPorId(Guid id)
    {
        return _context.Usuarios
            .Include(usuario => usuario.Jogadores)
            .FirstOrDefault(usuario => usuario.Id == id);
    }

    public Usuario? BuscarPorEmail(string email)
    {
        return _context.Usuarios
            .FirstOrDefault(usuario => usuario.Email == email);
    }

    public List<Usuario> BuscarPorNome(string nome)
    {
        return _context.Usuarios
            .Where(usuario => usuario.Nome.Contains(nome))
            .ToList();
    }

    public void Atualizar(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        _context.SaveChanges();
    }

    public void Excluir(Guid id)
    {
        var usuario = _context.Usuarios
            .FirstOrDefault(usuario => usuario.Id == id);

        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }
    }
}