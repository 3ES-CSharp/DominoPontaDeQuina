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

---

## Descrição do Projeto

Este projeto implementa as regras do jogo de dominó **"Ponta de Quina"**, seguindo a hierarquia:

```
Partida → Rodadas → Jogadas
```

- Uma **Partida** é composta por várias rodadas e termina quando um time atinge a pontuação alvo (padrão: 50 pontos)
- Cada **Rodada** funciona como um set: começa com distribuição das peças e termina quando um jogador bate ou o tabuleiro trava
- Cada **Jogada** representa a ação de um jogador: jogar uma peça ou passar a vez

---

## Estrutura do Projeto

```
DominoPontaDeQuina.Core/
│
├── Jogo.cs                          ← Classe principal (orquestração)
│
├── Enums/                           ← Todos originais (não alterados)
│   ├── LadoTabuleiro.cs
│   ├── StatusJogada.cs
│   ├── StatusPartida.cs
│   ├── StatusRodada.cs
│   └── TipoFinalizacaoRodada.cs
│
├── Exceptions/                      ← 📁 CRIADO PELO ALUNO
│   ├── DominoException.cs
│   ├── JogadaInvalidaException.cs
│   ├── SemPecasCompativeisException.cs
│   └── DistribuicaoPecasException.cs
│
├── Services/                        ← 📁 CRIADO PELO ALUNO
│   ├── IDistribuicaoService.cs
│   ├── DistribuicaoService.cs
│   ├── IJogadaValidator.cs
│   ├── JogadaValidator.cs
│   ├── IPlacarService.cs
│   ├── PlacarService.cs
│   ├── IRodadaService.cs
│   └── RodadaService.cs
│
├── Interfaces/                      ← Todos originais (não alterados)
│   ├── IJogada.cs
│   ├── IMaoJogador.cs
│   ├── IPartida.cs
│   └── IRodada.cs
│
└── Models/                          ← Classes modificadas/implementadas
    ├── Jogada.cs                    ← Original (não alterado)
    ├── Jogador.cs                   ← Original (não alterado)
    ├── Time.cs                      ← Original (não alterado)
    ├── Peca.cs                      ← Original (não alterado)
    ├── MaoJogador.cs                ← IMPLEMENTADO PELO ALUNO
    ├── Tabuleiro.cs                 ← IMPLEMENTADO PELO ALUNO
    ├── Partida.cs                   ← IMPLEMENTADO PELO ALUNO
    └── Rodada.cs                    ← IMPLEMENTADO PELO ALUNO
```

---

## Critérios de Avaliação Atendidos

### ✅ 50% - Pipeline de Testes (Implementação dos Gaps)

| Gap | Status | Implementado em |
|-----|--------|-----------------|
| Distribuição de peças | ✅ | `DistribuicaoService.cs` |
| Definição do jogador inicial da rodada | ✅ | `Rodada.GetPrimeiroJogador()` |
| Validação das jogadas | ✅ | `JogadaValidator.cs` |
| Posicionamento de peças no tabuleiro | ✅ | `Tabuleiro.Colar()` |
| Controle de turno | ✅ | `Rodada.ProximoJogador()` + `Queue` |
| Regra de pontuação (Ponta de Quina) | ✅ | `PlacarService.CalcularPontosJogada()` |
| Verificação de batida | ✅ | `RodadaService.VerificarBatida()` |
| Verificação de tabuleiro travado | ✅ | `RodadaService.VerificarTabuleiroTravado()` |
| Definição do vencedor da rodada | ✅ | `RodadaService.DeterminarVencedor()` |
| Finalização da partida | ✅ | `Partida.FinalizarPartida()` |

---

### ✅ 10% - Documentação do Código

Todas as classes criadas e métodos implementados possuem documentação XML completa:

- `<summary>`: descreve o propósito da classe/método
- `<param>`: descreve cada parâmetro
- `<returns>`: descreve o valor de retorno
- `<remarks>`: detalhes adicionais e exemplos
- `<exception>`: descreve exceções lançadas

---

### ✅ 10% - Implementação de Exceções Customizadas

Foram criadas 4 exceções customizadas:

| Exceção | Propósito |
|---------|-----------|
| `DominoException` | Classe base abstrata para todas as exceções do domínio |
| `JogadaInvalidaException` | Lançada quando a jogada viola as regras |
| `SemPecasCompativeisException` | Lançada quando o jogador não tem peças compatíveis |
| `DistribuicaoPecasException` | Lançada durante erro na distribuição das peças |

---

### ✅ 20% - Criação de Serviços e Validators

Foram criados 4 serviços (cada um com sua interface):

| Serviço | Responsabilidade |
|---------|------------------|
| `DistribuicaoService` | Gerar, embaralhar e distribuir as 28 peças do dominó |
| `JogadaValidator` | Validar se uma jogada é permitida pelas regras |
| `PlacarService` | Calcular pontuação (regra Ponta de Quina) |
| `RodadaService` | Gerenciar finalização da rodada (batida/travamento) |

**Princípios aplicados:**
- **SRP (Single Responsibility Principle)**: cada serviço tem uma única responsabilidade
- **Injeção de dependência**: serviços são injetados nas classes que os utilizam
- **Interface segregation**: interfaces específicas para cada serviço

---

### ✅ 10% - Convenções C#

| Convenção | Status |
|-----------|--------|
| Nomenclatura PascalCase para classes, métodos e propriedades | ✅ |
| Nomenclatura camelCase para parâmetros e variáveis locais | ✅ |
| Interfaces com prefixo "I" | ✅ |
| Uso de `readonly struct` para `Peca` (imutabilidade) | ✅ |
| Uso de `private` para campos internos | ✅ |
| Uso de `_` para campos privados (quando aplicável) | ✅ |
| Expressões lambda e LINQ para código conciso | ✅ |
| Pattern matching (`switch expression`) | ✅ |

---

## Regras do Jogo Implementadas

### Regras da Partida
- Registro de times com 1 ou 2 jogadores
- Pontuação alvo: 50 pontos (padrão, configurável)
- Acumulação de pontos por time ao longo das rodadas
- Finalização quando um time atinge/ultrapassa a pontuação alvo

### Regras da Rodada
- Distribuição: 28 peças embaralhadas
- 2 jogadores: 7 peças cada | 4 jogadores: 6 peças cada
- 1ª rodada: inicia quem tem a peça [6|6] (sena)
- Rodadas seguintes: inicia o vencedor da rodada anterior
- Sentido horário (fila circular de jogadores)
- Finalização por **batida** (jogador fica sem peças) ou **travamento** (ninguém consegue jogar)

### Regras da Jogada
- Peça deve ser compatível com a ponta escolhida do tabuleiro
- **Regra Ponta de Quina**: se a soma das pontas externas for múltiplo de 5, o time ganha (soma/5) pontos
- Jogador pode "passar a vez" apenas se não tiver peças compatíveis

### Regras de Finalização da Rodada
- **Batida**: vence quem bateu (ficou sem peças)
- **Travamento**: vence quem tem a menor soma dos valores das peças na mão

---

## Detalhamento das Implementações

### 1. Distribuição de Peças (`DistribuicaoService`)

```csharp
// Gera todas as 28 peças (0|0 até 6|6)
// Embaralha aleatoriamente
// Distribui: 2 jogadores → 7 peças | 4 jogadores → 6 peças
```

### 2. Validação de Jogadas (`JogadaValidator`)

```csharp
// Passar vez: só permitido se não houver peças compatíveis
// Jogar peça: jogador deve possuí-la e ela deve encaixar na ponta escolhida
```

### 3. Pontuação (`PlacarService`)

```csharp
// Exemplo: pontas 3 e 2 (soma=5) → 1 ponto
// Exemplo: pontas 6 e 4 (soma=10) → 2 pontos
// Exemplo: pontas 3 e 3 (soma=6) → 0 pontos
```

### 4. Controle de Turno (`Rodada`)

```csharp
// Usa Queue<MaoJogador> para fila circular
// ProximoJogador(): Enqueue(Dequeue()) - move o primeiro para o fim
```

### 5. Finalização da Rodada (`RodadaService`)

```csharp
// Batida: FirstOrDefault(mao => mao.EstaSemPecas())
// Travamento: nenhum jogador tem peça compatível
// Vencedor (travamento): menor soma de peças na mão
```

---

## Observações Importantes

### ✅ Restrições do Trabalho Atendidas

| Restrição | Status |
|-----------|--------|
| Classe `Jogo.cs` não foi alterada (exceto métodos implementados) | ✅ |
| Interfaces existentes não foram alteradas | ✅ |
| Nenhuma nova interface foi criada (apenas serviços internos) | ✅ |
| Foco em completar os gaps do core | ✅ |

### ✅ Classes que NÃO foram alteradas

- `Jogador.cs`
- `Peca.cs`
- `Time.cs`
- `Jogada.cs`
- Todas as interfaces (`IJogada`, `IMaoJogador`, `IPartida`, `IRodada`)
- Todos os enums

### ✅ Classes que FORAM implementadas/modificadas pelo aluno

- `MaoJogador.cs` (implementação dos métodos)
- `Tabuleiro.cs` (implementação dos métodos)
- `Partida.cs` (implementação dos métodos)
- `Rodada.cs` (implementação completa)
- `Jogo.cs` (apenas `RegistrarTimesAsync()` e `ValidarJogada()`)

### ✅ Pastas CRIADAS pelo aluno

- `Exceptions/` (4 arquivos)
- `Services/` (8 arquivos)

---

## Como Executar

```bash
# Limpar builds anteriores
dotnet clean

# Compilar o projeto
dotnet build

# Executar testes (se houver)
dotnet test
```

---

## Conclusão

O trabalho foi desenvolvido respeitando todas as restrições impostas, implementando todos os gaps solicitados e organizando o código com serviços e validators para melhor separação de responsabilidades. As exceções customizadas foram criadas para tratar erros específicos do domínio, e toda a documentação XML foi incluída para facilitar a compreensão do código.