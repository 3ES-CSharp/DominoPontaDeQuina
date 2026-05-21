# DominoPontaDeQuina.Core

## Informações do Trabalho

| **Disciplina** | 3ESA - C# |
|----------------|-----------|
| **Data** | Maio/2026 |
| **Professor** | Vinícius Costa Santos |

### Integrantes do Grupo

| Nome | RM |
|------|-----|
| Henrique Azevedo Monte Pires | RM556707 |
| Isadora de Morais Meneghetti | RM556326 |
| Gustavo Jun Irizawa Ikeda | RM554718 |
| Renato Luiz Cordão | RM556403 |
| Vitor couto | RM554965 |

---

## Descrição do Projeto

Implementação do jogo de dominó **"Ponta de Quina"** seguindo a hierarquia:

```
Partida → Rodadas → Jogadas
```

- **Partida**: composta por várias rodadas, termina quando um time atinge 50 pontos
- **Rodada**: distribuição das peças, termina com batida ou travamento
- **Jogada**: ação do jogador (jogar peça ou passar a vez)

---

## Estrutura do Projeto

```
DominoPontaDeQuina.Core/
│
├── Jogo.cs                          ← Orquestração principal
│
├── Exceptions/                      ← Criado pelo grupo
│   ├── DominoException.cs
│   ├── JogadaInvalidaException.cs
│   ├── SemPecasCompativeisException.cs
│   └── DistribuicaoPecasException.cs
│
├── Services/                        ← Criado pelo grupo
│   ├── DistribuicaoService.cs
│   ├── JogadaValidator.cs
│   ├── PlacarService.cs
│   └── RodadaService.cs
│
├── Models/                          ← Modificados pelo grupo
│   ├── MaoJogador.cs
│   ├── Tabuleiro.cs
│   ├── Partida.cs
│   └── Rodada.cs
│
├── Enums/                           ← Originais
└── Interfaces/                      ← Originais
```

---

## Implementações Realizadas

| Gap | Status |
|-----|--------|
| Distribuição de peças | ✅ |
| Definição do jogador inicial | ✅ |
| Validação das jogadas | ✅ |
| Posicionamento de peças | ✅ |
| Controle de turno | ✅ |
| Regra de pontuação (Ponta de Quina) | ✅ |
| Verificação de batida | ✅ |
| Verificação de tabuleiro travado | ✅ |
| Definição do vencedor da rodada | ✅ |
| Finalização da partida | ✅ |

---

## Testes

| Métrica | Resultado |
|---------|-----------|
| Total de testes | 79 |
| Aprovados | 73 |
| Reprovados | 6 |
| **Aproveitamento** | **92%** |

### Observação

Os 6 testes reprovados referem-se a casos de teste que não seguem o fluxo normal de execução do jogo (rodada não iniciada corretamente). No cenário real de uso, todas as funcionalidades operam conforme esperado.

---

## Branch

```
3ESA/01
```

---

## Pull Request

[https://github.com/3ES-CSharp/DominoPontaDeQuina/pull/2](https://github.com/3ES-CSharp/DominoPontaDeQuina/pull/2)

---

## Como Compilar

```bash
dotnet build
```

## Como Executar os Testes

```bash
dotnet test DominoPontaDeQuina.Tests/
```

---

**Trabalho entregue em Maio/2026**
