# Checkpoit 3 - C Software Development: 
# DominoPontaDeQuina.Core 

---

## Descriação
Projeto desenvolvido para a **Checkpoint 3** da disciplina **Software Development C#** – 3ES/2026.

**Integrantes do grupo:**  
- Matheus Taylor, RM556211  
- Henrique Maldonado, RM557270  

**Branch de entrega:** `/3ESPF/03`

---

## Sobre o Jogo

**Dominó Ponta de Quina** é uma variação do dominó clássico onde a pontuação é obtida quando a soma das pontas externas do tabuleiro é múltipla de 5.  
A partida termina quando um time atinge ou ultrapassa a **pontuação alvo** (padrão: 50 pontos).

### Regras resumidas

- **Estrutura:** Partida → Rodadas → Jogadas  
- **Times:** podem ter 1 ou 2 jogadores.  
- **Distribuição:** 28 peças (0|0 a 6|6) embaralhadas, distribuídas igualmente.  
- **Início da primeira rodada:** jogador que possui a sena `[6|6]`.  
- **Rodadas seguintes:** começa o vencedor da rodada anterior.  
- **Sentido:** horário.  
- **Jogada:** escolher uma peça compatível com a ponta esquerda ou direita do tabuleiro.  
  - Se não tiver peça compatível, o jogador **passa a vez**.  
- **Pontuação da jogada:**  
  - Soma das pontas externas (esquerda + direita).  
  - Se a soma for múltipla de 5 → time do jogador ganha `soma / 5` pontos.  
- **Finalização da rodada:**  
  - **Batida:** um jogador fica sem peças. Vence a rodada.  
  - **Tabuleiro travado:** nenhum jogador consegue jogar. Vence quem tiver a **menor soma** das peças na mão.  
- **Finalização da partida:** quando um time atinge ou ultrapassa a pontuação alvo.

---

## Arquitetura do Projeto

O projeto segue uma arquitetura em camadas orientada ao domínio, com clara separação de responsabilidades.

### Hierarquia principal

```
Jogo (orquestrador)
 └── Partida
      └── Rodada
           └── Jogada
```

### Estrutura de pastas (após implementação)

```
DominoPontaDeQuina.Core/
├── Enums/                 # Status, tipos de finalização, lados do tabuleiro
├── Exceptions/            # Exceções customizadas do domínio
├── Interfaces/            # Contratos internos (já existentes, não alterados)
├── Models/                # Entidades de domínio (Peca, Jogador, Time, etc.)
├── Services/              # Classes auxiliares (baralho, validação, pontuação)
├── Jogo.cs                # Orquestrador (não modificado além dos gaps permitidos)
└── README.md              # Este arquivo
```

### Principais classes e responsabilidades

| Classe | Responsabilidade |
|--------|------------------|
| `Jogo` | Controla o fluxo global: cria partidas, executa rodadas e jogadas. |
| `Partida` | Gerencia times, pontuação alvo, histórico de rodadas e decide vencedor. |
| `Rodada` | Distribui peças, controla turnos, verifica batida/travamento, calcula pontuação. |
| `Tabuleiro` | Mantém as peças coladas, calcula pontas externas, valida compatibilidade. |
| `MaoJogador` | Representa a mão de um jogador, permite escolher jogada e desfazer. |
| `Jogada` | Registra a ação de um jogador (peça, lado, ou passar vez). |
| `Peca` | Estrutura imutável com valores, suporte a inversão e comparação. |

### Serviços implementados

- **`BaralhoService`** – cria as 28 peças e as embaralha (Fisher‑Yates).  
- **`JogadaValidator`** – valida se uma jogada é permitida (peça pertence à mão, compatível com o tabuleiro).  
- **`PontuacaoService`** – calcula os pontos com base na soma das pontas externas.

### Exceções customizadas

Todas herdam de `DominoException` (namespace `DominoPontaDeQuina.Core.Exceptions`):

- `JogadaInvalidaException` – jogada não permitida.  
- `PartidaFinalizadaException` – operação inválida em partida encerrada.  
- `RodadaNaoIniciadaException` – acesso a estado da rodada antes da inicialização.

---

## Como executar

### Pré‑requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)  
- Git (opcional, mas recomendado)

### Clonar e compilar

```bash
git clone https://github.com/3ES-CSharp/DominoPontaDeQuina.git
cd DominoPontaDeQuina
dotnet build
```

### Executar os testes

```bash
dotnet test DominoPontaDeQuina.Tests
```

Para ver os testes com detalhes:

```bash
dotnet test --verbosity detailed
```

### Utilizar a biblioteca em outro projeto

Adicione a referência ao projeto `DominoPontaDeQuina.Core.csproj` e use as classes públicas (`Jogo`, `Partida`, `Jogador`, `Time`, etc.).

Exemplo mínimo para iniciar uma partida:

```csharp
var jogo = new Jogo();
await jogo.IniciarNovaPartida();   // inicia automaticamente a primeira rodada
```

> **Atenção:** A classe `Jogo` já contém a orquestração completa. O aluno **não deve alterá-la**. Toda a lógica de negócio foi encapsulada nas demais classes.

---

## Cobertura de testes

O projeto possui **mais de 70 testes** distribuídos em:

- Testes básicos de entidades (`PecaTests`, `TabuleiroTests`, `MaoJogadorTests`, `PartidaTests`)  
- Testes de fluxo (`PartidaFluxoTests`, `RodadaGapTests`, `TabuleiroGapTests`)  
- Testes de exceções customizadas (`JogoTests`, `RodadaExcecaoTests`)

Todos os **gaps** identificados no relatório `GAPS_RELATORIO.md` foram implementados e validados pelos testes.

Para executar apenas os testes de um arquivo específico:

```bash
dotnet test --filter "FullyQualifiedName~TabuleiroGapTests"
```

---

## Decisões de implementação

1. **Não alterar interfaces e `Jogo`** – respeitamos rigorosamente essa restrição.  
2. **Uso de serviços estáticos** – escolhemos classes estáticas (`BaralhoService`, `JogadaValidator`, `PontuacaoService`) para manter a simplicidade e separar responsabilidades sem introduzir novas dependências.  
3. **Exceções customizadas** – todas as situações inválidas (jogada inválida, partida finalizada, rodada não iniciada) lançam exceções específicas, facilitando o tratamento e a rastreabilidade.  
4. **Documentação XML** – todos os membros públicos estão documentados em português, conforme as diretrizes do enunciado.  
5. **Propriedade `Partida` em `Rodada`** – adicionamos uma referência para a partida atual permitindo que `CalcularPontuacao` atribua pontos ao time correto, sem quebrar o contrato das interfaces.  
6. **Método `ObterMaoDoJogador` em `Rodada`** – necessário para a validação em `Jogo.ValidarJogada`, mas mantido como método público auxiliar.

---

## Possíveis melhorias futuras

- Implementar uma interface de usuário (console ou Web API) para interação real.  
- Adicionar persistência do histórico de partidas.  
- Suporte a diferentes modos de pontuação alvo.  
- Logging detalhado das jogadas.

---

## Referências

- [Regras oficiais do Dominó Ponta de Quina](https://www.dominosp.com.br/pontadequina)  
- [Microsoft C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)  
- [xUnit Documentation](https://xunit.net/)

---

**Desenvolvido para fins acadêmicos – Checkpoint 3 – FIAP 3ES/2026**  
Todos os direitos reservados aos autores.
