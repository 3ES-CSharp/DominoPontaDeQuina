using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

public class JogadorRepository
{
    private readonly DominoDbContext _context;

    public JogadorRepository(DominoDbContext context)
    {
        _context = context;
    }

    public void Adicionar(Jogador jogador)
    {
        _context.Jogadores.Add(jogador);
        _context.SaveChanges();
    }

    public List<Jogador> Listar()
    {
        return _context.Jogadores
            .Include(jogador => jogador.Usuario)
            .ToList();
    }

    public Jogador? BuscarPorId(Guid id)
    {
        return _context.Jogadores
            .Include(jogador => jogador.Usuario)
            .FirstOrDefault(jogador => jogador.Id == id);
    }

    public List<Jogador> BuscarPorNome(string nome)
    {
        return _context.Jogadores
            .Where(jogador => jogador.NomeExibicao.Contains(nome))
            .ToList();
    }

    public List<Jogador> BuscarPorUsuario(Guid usuarioId)
    {
        return _context.Jogadores
            .Where(jogador => jogador.UsuarioId == usuarioId)
            .ToList();
    }

    public void Atualizar(Jogador jogador)
    {
        _context.Jogadores.Update(jogador);
        _context.SaveChanges();
    }

    public void Excluir(Guid id)
    {
        var jogador = _context.Jogadores
            .FirstOrDefault(jogador => jogador.Id == id);

        if (jogador != null)
        {
            _context.Jogadores.Remove(jogador);
            _context.SaveChanges();
        }
    }
}