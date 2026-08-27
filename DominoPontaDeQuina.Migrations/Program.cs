using System.Diagnostics;
using DominoPontaDeQuina.Domain.Entities;
using DominoPontaDeQuina.Repository.Context;
using DominoPontaDeQuina.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

// Startup project das migrations. Executado diretamente, aplica as migrations
// pendentes e roda uma verificacao rapida do mapeamento e dos repositorios.
using var context = new DominoDbContext();
context.Database.Migrate();
Console.WriteLine($"Migrations aplicadas em: {context.Database.GetConnectionString()}");

var usuarios = new UsuarioRepository(context);
var jogadores = new JogadorRepository(context);
var partidas = new PartidaRepository(context);
var participacoes = new ParticipacaoPartidaRepository(context);

if (!context.Usuarios.Any())
{
    var ana = usuarios.Adicionar(new Usuario { Nome = "Ana Souza", Email = "ana@exemplo.com", HashSenha = "hash-ana" });
    var bruno = usuarios.Adicionar(new Usuario { Nome = "Bruno Lima", Email = "bruno@exemplo.com", HashSenha = "hash-bruno" });

    var anaJogador = jogadores.Adicionar(new Jogador { UsuarioId = ana.Id, NomeExibicao = "ana_quina" });
    var brunoJogador = jogadores.Adicionar(new Jogador { UsuarioId = bruno.Id, NomeExibicao = "bruno_bate" });

    partidas.Adicionar(new Partida
    {
        Status = StatusPartida.Finalizado,
        FinalizadoEm = DateTime.UtcNow,
        Participacoes =
        {
            new ParticipacaoPartida { JogadorId = anaJogador.Id, Posicao = 1, Pontuacao = 30, Vencedor = true },
            new ParticipacaoPartida { JogadorId = brunoJogador.Id, Posicao = 2, Pontuacao = 12 }
        }
    });

    partidas.Adicionar(new Partida { Status = StatusPartida.EmAndamento });
}

// Verificacao: relacionamentos navegam e as consultas LINQ respondem.
var ana2 = usuarios.ObterPorEmail("ana@exemplo.com");
Debug.Assert(ana2 is not null, "usuario nao encontrado por email");
Debug.Assert(usuarios.ObterComJogadores(ana2!.Id)!.Jogadores.Count == 1, "jogador nao carregado via Include");

var perfilAna = jogadores.ObterPorNomeExibicao("ana_quina")!;
Debug.Assert(perfilAna.Usuario.Nome == "Ana Souza", "navegacao Jogador -> Usuario falhou");
Debug.Assert(partidas.ListarPorStatus(StatusPartida.Finalizado).Count == 1, "filtro por status falhou");
Debug.Assert(partidas.ListarPorJogador(perfilAna.Id).Count == 1, "filtro de partidas por jogador falhou");

var finalizada = partidas.ListarPorStatus(StatusPartida.Finalizado).Single();
Debug.Assert(partidas.ObterComParticipacoes(finalizada.Id)!.Participacoes.Count == 2, "ThenInclude falhou");
Debug.Assert(participacoes.ObterVencedor(finalizada.Id)!.JogadorId == perfilAna.Id, "vencedor incorreto");
Debug.Assert(participacoes.TotalDePontos(perfilAna.Id) == 30, "soma de pontos incorreta");

Debug.Assert(usuarios.EmailEmUso("ana@exemplo.com") && !usuarios.EmailEmUso("x@y.com"), "EmailEmUso incorreto");
Debug.Assert(usuarios.BuscarPorNome("Souza").Count == 1, "busca por nome falhou");
Debug.Assert(usuarios.SemJogadores().Count == 0, "todos os usuarios tem jogador");
Debug.Assert(jogadores.ListarPorUsuario(ana2.Id).Count == 1, "jogadores por usuario falhou");
Debug.Assert(jogadores.TotalDePartidas(perfilAna.Id) == 1, "total de partidas incorreto");
Debug.Assert(partidas.ListarFinalizadasEntre(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1)).Count == 1, "intervalo de finalizacao falhou");
Debug.Assert(partidas.Travadas(TimeSpan.FromDays(1)).Count == 0, "partida recente nao pode estar travada");
Debug.Assert(participacoes.ListarPorPartida(finalizada.Id).First().Posicao == 1, "ordenacao por posicao falhou");
Debug.Assert(participacoes.ListarPorJogador(perfilAna.Id).Count == 1, "participacoes por jogador falhou");
Debug.Assert(participacoes.MediaDePontosPorPosicao().First() == (1, 30d), "media por posicao incorreta");

var ranking = jogadores.Ranking();
Debug.Assert(ranking.First().NomeExibicao == "ana_quina" && ranking.First().Vitorias == 1, "ranking incorreto");

Console.WriteLine("Verificacao do modelo e dos repositorios: OK");
foreach (var linha in ranking)
{
    Console.WriteLine($"  {linha.NomeExibicao}: {linha.Vitorias} vitoria(s), {linha.Pontos} ponto(s)");
}
