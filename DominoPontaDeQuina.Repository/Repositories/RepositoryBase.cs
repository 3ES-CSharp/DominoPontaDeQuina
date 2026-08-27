using DominoPontaDeQuina.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace DominoPontaDeQuina.Repository.Repositories;

/// <summary>CRUD comum a todas as entidades; cada repositorio adiciona suas consultas LINQ.</summary>
public abstract class RepositoryBase<T> where T : class
{
    protected readonly DominoDbContext Context;

    protected RepositoryBase(DominoDbContext context) => Context = context;

    protected DbSet<T> Entidades => Context.Set<T>();

    public T? ObterPorId(Guid id) => Entidades.Find(id);

    public List<T> Listar() => Entidades.ToList();

    public T Adicionar(T entidade)
    {
        Entidades.Add(entidade);
        Context.SaveChanges();
        return entidade;
    }

    public void Atualizar(T entidade)
    {
        Entidades.Update(entidade);
        Context.SaveChanges();
    }

    public void Remover(T entidade)
    {
        Entidades.Remove(entidade);
        Context.SaveChanges();
    }
}
