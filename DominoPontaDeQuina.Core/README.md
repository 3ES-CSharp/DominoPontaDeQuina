# Integrantes
Enzo Motta RM555372 <br>
Matheus Hostim RM556517 <br>
Guilherme Ulacco RM558418<br>
Eduardo Silva RM554804<br>
Estevam Melo RM555124<br>

# Domino Ponta de Quina

O projeto Domino Ponta de Quina foi desenvolvido em C# com o objetivo de representar a lógica de uma partida de dominó utilizando conceitos de programação orientada a objetos. A aplicação modela elementos essenciais do jogo, como jogadores, times, peças, jogadas, rodadas e tabuleiro, mantendo as responsabilidades bem separadas para facilitar manutenção e evolução do sistema.

A estrutura do projeto foi organizada para representar o domínio do jogo de forma clara. A classe Jogador representa os participantes da partida, enquanto Time agrupa jogadores e controla a pontuação acumulada. As peças do dominó são representadas pela struct Peca, que contém funcionalidades importantes como soma dos valores, inversão dos lados e identificação da peça [6|6].

O controle das peças de cada jogador é feito pela classe MaoJogador, responsável por adicionar, remover e verificar possíveis jogadas válidas. Já o Tabuleiro controla as peças jogadas e valida os encaixes permitidos nas pontas. Caso uma jogada seja inválida, o sistema utiliza a exceção personalizada JogadaInvalidaException.

As ações realizadas durante o jogo são representadas pela classe Jogada, enquanto a Rodada controla o fluxo completo de uma rodada, incluindo distribuição de peças, verificação de batida, travamento do tabuleiro e definição do vencedor. A classe Partida funciona como o nível principal da aplicação, armazenando rodadas, times e verificando quando a pontuação alvo é atingida.

O projeto utiliza recursos modernos do C#, como LINQ, coleções genéricas e propriedades imutáveis, além de aplicar conceitos importantes como encapsulamento, separação de responsabilidades e modelagem de domínio.
