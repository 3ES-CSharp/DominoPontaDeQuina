using DominoPontaDeQuina.Core.Enums;
using DominoPontaDeQuina.Core.Models;
using DominoPontaDeQuina.Core.Services;
using System.Collections.Generic;

namespace DominoPontaDeQuina.Core.Validators;

internal static class TabuleiroValidator
{
    public static bool PodeColar(Tabuleiro tabuleiro, Peca peca, LadoTabuleiro lado) =>
        TabuleiroService.PodeColar(tabuleiro, peca, lado);

    public static bool EstaTravado(Tabuleiro tabuleiro, IEnumerable<MaoJogador> maosJogadores) =>
        TabuleiroService.EstaTravado(tabuleiro, maosJogadores);
}
