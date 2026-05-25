# Documentação Técnica: Dominó Ponta de Quina - CP03

Documentação simplificada do desenvolvimento da solução do Dominó Ponta de Quina do CP03 de C# 

---

## Integrantes - 3ESPG

| Nome | RM |
| :--- | :--- |
| João Victor Soave | RM557595 |
| Maria Alice Freitas Araújo | RM557516 |
| Pedro Henrique Mendes dos Santos |  RM555332 |
| Rafael Teofilo Lucena | RM555600 |
| Vinicius Fernandes Tavares Bittencourt | RM558909 |

---

## 1. Estrutura de Domínio Implementada

A arquitetura do jogo foi dividida seguindo uma hierarquia clara de responsabilidades: `Partida` ➔ `Rodadas` ➔ `Jogadas`.

### Classe `Rodada.cs`
Gerencia o estado e o fluxo de uma rodada específica, desde a distribuição das cartas até a definição do fim do jogo.
* **`DistribuirPecas`**: Gera as 28 peças exclusivas do dominó (sem duplicatas espelhadas de `[0|1]` e `[1|0]`) e aplica o algoritmo **Fisher-Yates** para um embaralhamento uniforme e livre de estouros de índice. Cada jogador recebe exatamente 7 peças.
* **`GetPrimeiroJogador`**: Determina quem inicia o jogo. Na primeira rodada, busca o jogador que possui a sena (`[6|6]`). Nas rodadas subsequentes, adota o vencedor da rodada anterior.
* **`OrganizaJogadores`**: Monta e limpa a fila circular de execução (`Queue<MaoJogador>`) utilizando o operador de módulo (`%`) a partir do primeiro jogador definido.
* **`RegistrarJogada`**: Valida se a jogada é nula, aplica a peça fisicamente ao `Tabuleiro`, insere o movimento na pilha de histórico, processa a pontuação e avança o turno de forma sequencial e protegida.
* **`VerificarBatida` e `VerificarTabuleiroTravado`**: Avaliam se o jogador atual zerou suas peças ou se nenhum jogador possui peças que encaixem nas pontas externas, atualizando o `Status` para `Finalizada` e o `TipoFinalizacao` correspondente.
* **`CalcularPontuacao`**: Executa após cada jogada para verificar se a soma das pontas livres do tabuleiro é múltipla de 5 para computar os pontos da rodada.

### Classe `Partida.cs`
Controla o estado global do jogo e o acúmulo de pontos entre as rodadas para decretar o time vencedor.
* **`GetPontuacaoTimes`**: Mapeia de forma direta a propriedade rica de estado `Pontuacao` de cada `Time` participante para um dicionário, evitando loops redundantes ou reprocessamento de regras antigas.
* **`VerificaPontuacaoAlvoAtingida` e `GetTimeVencedor`**: Utilizam expressões LINQ (`Any` e `FirstOrDefault`) para checar se algum time cruzou a barreira da `PontuacaoAlvo` estabelecida (padrão 50 pontos).

---

## 2. Tratamento de Exceções Customizadas

Para blindar o domínio e impedir o vazamento de exceções nativas do ecossistema .NET (`System`), isolamos os fluxos de erro nas seguintes exceções customizadas:
* **`JogadaInvalidaException`**: Disparada ao interceptar jogadas nulas ou movimentos que violem as regras físicas de colagem do tabuleiro.
* **`PartidaInvalidaException`**: Atua como barreira de segurança no motor de orquestração do jogo, impedindo ações inconsistentes como tentar jogar sem uma partida ativa ou abrir novas rodadas em uma partida já finalizada.

---

## 3. Convenções e Código Limpo Adotados

* **Sintaxe Moderna do C#**: Uso de expressões de coleção (*Collection Expressions* `[.. _jogadores]`) para clonagem e conversão segura de coleções.
* **Padrões de Nomenclatura**: Uso rigoroso de *PascalCase* para métodos públicos, *camelCase* com underline (`_jogadores`) para campos privados, e tipagem estrita contra valores nulos (*Nullable Reference Types*).
* **Documentação XML**: Todos os membros públicos e métodos herdados das interfaces possuem cabeçalhos `/// <summary>` documentando parâmetros e comportamentos.