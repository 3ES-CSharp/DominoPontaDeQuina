# 🂎 Dominó Ponta de Quina - Core Engine

Este repositório contém a implementação completa das regras e da orquestração do jogo **Dominó Ponta de Quina**, desenvolvido em **C# (.NET 8.0)**. O projeto preenche com excelência todos os 15 gaps de domínio planejados, utilizando uma arquitetura profissional baseada em separação de responsabilidades com validadores e serviços, além de tratamento de erro robusto com exceções customizadas.

---

## 👥 Integrantes do Grupo

- **Gabriel Galerani** - RM 557421
- **Leonardo Taschin** - RM 554583

---

## 📋 Regras de Negócio do Jogo

O jogo segue o padrão clássico do dominó Ponta de Quina, cuja dinâmica e pontuação se dão por:

1. **Estrutura Geral**: Partidas compostas de múltiplas rodadas organizadas de forma circular e jogadas em sentido horário.
2. **Ciclo de Início**: Cada jogador recebe 7 peças de forma aleatória. Na primeira rodada, o turno começa com quem possuir a sena `[6|6]`. Nas rodadas seguintes, com o vencedor da rodada anterior.
3. **Múltiplos de 5 (Ponta de Quina)**: Após cada jogada, calcula-se a soma das pontas externas do tabuleiro (`PontaEsquerda + PontaDireita`). Sempre que essa soma for um múltiplo de 5, o time do jogador pontua (`soma / 5`).
4. **Finalização**: A rodada se encerra quando um jogador zera sua mão (**batida**) ou quando nenhum jogador tem jogadas válidas possíveis (**travamento**). Em caso de travamento, vence quem possuir a menor soma de peças na mão.
5. **Vencedor da Partida**: A partida encerra assim que um dos times atinge ou supera a **Pontuação Alvo** (padrão: 50 pontos).

---

## 🏛️ Arquitetura e Engenharia de Software

Para garantir a máxima legibilidade de código e aderir aos critérios de projeto de software da disciplina, estruturamos o core com três novas frentes arquiteturais:

### 1. Camada de Validação (`Validators`)
- **`JogadaValidator`**: Centraliza toda a validação lógica das jogadas. Valida se a peça pode ser acoplada nas extremidades do tabuleiro e assegura que um movimento de "passar a vez" seja legítimo (o jogador não possui nenhuma outra peça jogável em sua mão).

### 2. Camada de Serviços (`Services`)
- **`BaralhoService`**: Desacopla a responsabilidade de criação e embaralhamento do baralho de dominó. Garante um embaralhamento imparcial e uniforme por meio do algoritmo de embaralhamento de *Fisher-Yates*.

### 3. Exceções de Domínio Customizadas (`Exceptions`)
Evita o vazamento de exceções nativas de baixo nível (`System`), provendo tratamento de estados indevidos com:
- **`DominoException`**: Base para as exceções do jogo.
- **`JogadaInvalidaException`**: Lançada ao infringir as regras de compatibilidade de peça ou passagem de vez indevida.
- **`PartidaInvalidaException`**: Lançada em violações do ciclo de vida da partida (ex: tentar iniciar jogadas em partidas já concluídas).

---

## 🧪 Cobertura de Testes Automatizados

A robustez da implementação é certificada pela aprovação unânime de todos os testes unitários e de fluxo integrados na suíte:

- **Total de Testes**: 78
- **Aprovados**: 78
- **Falhas**: 0
- **Status Geral**: 🟢 **100% de Sucesso**

Para executar os testes na sua máquina local, certifique-se de ter o SDK do .NET 8 instalado e execute na pasta raiz:

```bash
dotnet test
```
