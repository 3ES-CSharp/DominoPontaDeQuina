# DominoPontaDeQuina — Entrega 3ESPF/04

## Grupo

| Nome | RM |
|------|----|
| Glauco Heitor Gonçalves | 555978 |
| Pedro Henrique Junqueira | 556278 |

---

## Sobre o Projeto

Implementação das regras do jogo **Dominó Ponta de Quina** sobre o esqueleto fornecido pelo professor.
A hierarquia do domínio é `Partida → Rodadas → Jogadas`.

---

## O que foi implementado

### Modelos — gaps preenchidos

| Classe | Método/Comportamento implementado |
|--------|----------------------------------|
| `Tabuleiro` | `PodeColar` — valida compatibilidade da peça com a ponta do lado escolhido |
| `Tabuleiro` | `Colar` — posiciona a peça no lado correto, invertendo quando necessário |
| `Tabuleiro` | `EstaTravado` — detecta quando nenhum jogador possui peça compatível |
| `MaoJogador` | `GetJogada` — seleciona a melhor peça disponível ou passa a vez |
| `MaoJogador` | `DefazerJogada` — restaura a peça à mão em caso de rollback |
| `Rodada` | `DistribuirPecas` — gera as 28 peças, embaralha (Fisher-Yates) e distribui igualmente |
| `Rodada` | `GetPrimeiroJogador` — sena na 1ª rodada; vencedor anterior nas demais |
| `Rodada` | `OrganizaJogadores` — monta a fila circular a partir do primeiro jogador |
| `Rodada` | `VerificarBatida` — detecta jogador sem peças e finaliza a rodada |
| `Rodada` | `VerificarTabuleiroTravado` — detecta travamento e elege vencedor por menor soma |
| `Rodada` | `GetVencedor` — retorna o jogador que venceu a rodada |
| `Rodada` | `CalcularPontuacao` — cola a peça no tabuleiro, atribui pontos (se soma % 5 == 0) e rotaciona o turno |
| `Partida` | `GetPontuacaoTimes` — retorna dicionário com pontuação acumulada de cada time |
| `Partida` | `GetTimeVencedor` — retorna o time que atingiu ou superou a pontuação alvo |
| `Partida` | `VerificaPontuacaoAlvoAtingida` — verifica se algum time chegou ao alvo |
| `Jogo` | `RegistrarTimesAsync` — registra dois times de dois jogadores cada |
| `Jogo` | `ValidarJogada` — delega ao `JogadaValidator` a verificação de compatibilidade |

---

## Exceções Customizadas

Localizadas em `DominoPontaDeQuina.Core/Exceptions/`:

| Exceção | Herança | Quando é lançada |
|---------|---------|-----------------|
| `DominoDomainException` | `InvalidOperationException` | Classe base de todas as exceções do domínio |
| `JogadaInvalidaException` | `DominoDomainException` | Jogada nula ou incompatível com o tabuleiro |
| `PartidaInvalidaException` | `DominoDomainException` | Operação não permitida no estado atual da partida/rodada |

Todas as exceções do sistema pertencem ao namespace `DominoPontaDeQuina.Core.Exceptions`, satisfazendo os testes que verificam `Assert.StartsWith("DominoPontaDeQuina", ...)`.

---

## Serviços e Validators

### `PontuacaoService` — `DominoPontaDeQuina.Core/Services/`

Responsável por calcular e atribuir pontos ao time do jogador após cada jogada.

```
soma das pontas externas % 5 == 0  →  pontos = soma / 5  →  Time.SomarPontos(pontos)
```

Separado de `Rodada` para isolar a regra de pontuação e facilitar testes unitários.

### `JogadaValidator` — `DominoPontaDeQuina.Core/Validators/`

Centraliza a lógica de validação de jogadas.

- Passar vez → sempre válido
- Jogada real → `Tabuleiro.PodeColar(peca, lado)`

Utilizado por `Jogo.ValidarJogada`, mantendo o orquestrador livre de regras de domínio.

---

## Regras de Negócio Implementadas

### Distribuição
- 28 peças únicas geradas (`[0|0]` a `[6|6]`)
- Embaralhamento Fisher-Yates garante aleatoriedade uniforme
- Divisão inteira: `peças.Count / jogadores.Count` por jogador

### Compatibilidade (Tabuleiro)
- Tabuleiro vazio → qualquer peça em qualquer lado
- Lado esquerdo → a peça deve ter valor igual à `PontaEsquerda`
- Lado direito → a peça deve ter valor igual à `PontaDireita`
- Peça invertida automaticamente se necessário ao colar

### Pontuação
- Após cada jogada: `PontaEsquerda + PontaDireita`
- Resultado divisível por 5 → `pontos = soma / 5` creditados ao time do jogador

### Fim de Rodada
| Condição | Tipo | Vencedor |
|----------|------|---------|
| Jogador fica sem peças | `JogadorBateu` | Quem esvaziou a mão |
| Nenhum pode jogar | `TabuleiroTravado` | Menor soma de peças na mão |

### Fim de Partida
- Qualquer time com `Pontuacao >= PontuacaoAlvo` encerra a partida

---

## Estrutura de Arquivos Adicionados

```
DominoPontaDeQuina.Core/
├── Exceptions/
│   ├── DominoDomainException.cs
│   ├── JogadaInvalidaException.cs
│   └── PartidaInvalidaException.cs
├── Services/
│   └── PontuacaoService.cs
└── Validators/
    └── JogadaValidator.cs
```

---

## Convenções Adotadas

- Nomenclatura C# padrão (PascalCase para tipos/membros públicos, camelCase para locais)
- Documentação XML em todos os tipos e membros públicos (`<summary>`, `<param>`, `<returns>`)
- Membros `internal` para expor apenas o necessário entre classes do mesmo assembly
- Exceções de domínio em namespace dedicado, herdando de base comum
- Métodos estáticos nos serviços/validators (sem estado próprio)
