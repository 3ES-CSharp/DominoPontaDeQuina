using DominoPontaDeQuina.Core.Enums;

namespace DominoPontaDeQuina.Core.Models;

/// <summary>
/// Representa o tabuleiro no nivel da rodada dentro da hierarquia Partida -> Rodadas -> Jogadas.
/// Neste nivel ficam as pecas ja coladas e as informacoes necessarias para validar jogadas,
/// calcular pontuacao pelas pontas externas e verificar situacoes de travamento.
/// </summary>
public class Tabuleiro
{
    /// <summary>
    /// Obtem as pecas posicionadas no tabuleiro na ordem em que foram coladas.
    /// </summary>
    public List<Peca> Pecas { get; } = [];
    public List<Peca> PecasY { get; } = [];

    Peca _pecaMeio { get; set; } 

    /// <summary>
    /// Indica se o tabuleiro ainda nao possui pecas coladas.
    /// </summary>
    public bool EstaVazio => (Pecas.Count+ PecasY.Count) == 0;

    /// <summary>
    /// Obtem a ponta esquerda atualmente exposta no tabuleiro.
    /// Quando o tabuleiro estiver vazio, nao existe ponta externa disponivel.
    /// </summary>
    public int? PontaEsquerda
    {
        get
        {
            if (EstaVazio) return null;

            if (Pecas.Count == 0) return _pecaMeio.ValorA;

            return Pecas[0].ValorA;
        }
    }

    /// <summary>
    /// Obtem a ponta direita atualmente exposta no tabuleiro.
    /// Quando o tabuleiro estiver vazio, nao existe ponta externa disponivel.
    /// </summary>
    public int? PontaDireita
    {
        get
        {
            if (EstaVazio) return null;

            if (Pecas.Count == 0) return _pecaMeio.ValorB;

            return Pecas[^1].ValorB;
        }
    }

    /// <summary>
    /// Obtem a ponta direita atualmente exposta no tabuleiro.
    /// Quando o tabuleiro estiver vazio, nao existe ponta externa disponivel.
    /// </summary>
    public int? PontaInferior
    {
        get
        {
            if (EstaVazio) return null;

            if (PecasY.Count == 0) return _pecaMeio.ValorB;

            return PecasY[^1].ValorB;
        }
    }

    /// <summary>
    /// Obtem a ponta direita atualmente exposta no tabuleiro.
    /// Quando o tabuleiro estiver vazio, nao existe ponta externa disponivel.
    /// </summary>
    public int? PontaSuperior
    {
        get
        {
            if (EstaVazio) return null;

            if (PecasY.Count == 0) return _pecaMeio.ValorA;

            return PecasY[0].ValorA;
        }
    }

    /// <summary>
    /// Determina se uma peca pode ser colada no lado informado.
    /// A regra esperada e validar se a peca possui valor compativel com a ponta externa do lado escolhido.
    /// </summary>
    /// <param name="peca">A peca a ser verificada.</param>
    /// <param name="lado">O lado do tabuleiro.</param>
    /// <returns><see langword="true"/> quando a peca puder ser colada; caso contrario, <see langword="false"/>.</returns>
    public bool PodeColar(Peca peca, LadoTabuleiro lado)
    {
        // TODO ALUNO: validar se a peca pode ser colada no lado escolhido.
        bool pode = false;
        if (EstaVazio) return pode = true;

        switch (lado)
        {
            case LadoTabuleiro.Esquerda:
                if(PontaEsquerda != null) pode = peca.PossuiValor(PontaEsquerda!.Value);
                break;
            case LadoTabuleiro.Direita:
                if(PontaDireita != null) pode = peca.PossuiValor(PontaDireita!.Value);
                break;
            case LadoTabuleiro.Cima:
                if(PontaSuperior != null) pode = peca.PossuiValor(PontaSuperior!.Value);
                break;
            case LadoTabuleiro.Baixo:
                if(PontaInferior != null) pode = peca.PossuiValor(PontaInferior!.Value);
                break;
        }

        return pode;
    }

    /// <summary>
    /// Cola uma peca no lado informado do tabuleiro.
    /// A regra esperada e posicionar a peca no lado correto, invertendo seus valores quando necessario.
    /// </summary>
    /// <param name="peca">A peca a ser colada.</param>
    /// <param name="lado">O lado do tabuleiro.</param>
    public void Colar(Peca peca, LadoTabuleiro lado)
    {
        // TODO ALUNO: posicionar a peca no lado escolhido, invertendo quando necessario.
        if (EstaVazio && peca.EhSena || EstaVazio && peca.EhCarroca) {Pecas.Add(peca); return; }
        bool Pode = PodeColar(peca, lado);
        

        switch (lado)
        {
            case LadoTabuleiro.Esquerda when Pode:
                
                if(!peca.EhCarroca) if(peca.ValorA == PontaEsquerda) peca = peca.Inverter();
                Pecas.Insert(0, peca);
                if(EstaVazio) _pecaMeio = peca;
                break;

            case LadoTabuleiro.Direita when Pode:
                if (!peca.EhCarroca) if (peca.ValorB == PontaDireita) peca = peca.Inverter();
                Pecas.Add(peca);
                if (EstaVazio) _pecaMeio = peca;
                break;

            case LadoTabuleiro.Cima when Pode:
                if (!peca.EhCarroca) if (peca.ValorA == PontaSuperior) peca = peca.Inverter();
                PecasY.Insert(0, peca);
                if (EstaVazio) _pecaMeio = peca;
                break;

            case LadoTabuleiro.Baixo when Pode:
                if (!peca.EhCarroca) if (peca.ValorB == PontaInferior) peca = peca.Inverter();
                PecasY.Add(peca);
                if (EstaVazio) _pecaMeio = peca;
                break;
        }
    }

    /// <summary>
    /// Soma os valores das pontas externas atualmente expostas.
    /// Essa soma e a base para regras de pontuacao em que a rodada concede pontos quando o resultado for multiplo de 5.
    /// </summary>
    /// <returns>A soma das pontas externas, ou 0 quando o tabuleiro estiver vazio.</returns>
    public int SomarPontasExternas() =>
        EstaVazio ? 0 : PontaEsquerda!.Value + PontaDireita!.Value + PontaInferior!.Value + PontaSuperior!.Value;

    /// <summary>
    /// Determina se o tabuleiro esta travado.
    /// O travamento e esperado quando nenhuma mao de jogador possuir peca compativel com as pontas externas atuais.
    /// </summary>
    /// <param name="maosJogadores">As maos dos jogadores da rodada.</param>
    /// <returns><see langword="true"/> quando o tabuleiro estiver travado; caso contrario, <see langword="false"/>.</returns>
    public bool EstaTravado(IEnumerable<MaoJogador> maosJogadores)
    {
        // TODO ALUNO: implementar a regra de travamento do tabuleiro.
        
        foreach (MaoJogador mao in maosJogadores) if (mao.GetJogada(this).Status != StatusJogada.Invalida) return false;
        
        return true;
    }

    /// <summary>
    /// Limpa o tabuleiro para preparar uma nova rodada.
    /// </summary>
    public void Limpar() =>
        Pecas.Clear();
}