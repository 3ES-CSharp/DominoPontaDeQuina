# Laboratório EF Core + LINQ

Nesta etapa do projeto **Domino Ponta de Quina**, o modelo de dados foi evoluído utilizando **Entity Framework Core**, **Fluent API** e **LINQ**.

A classe `Jogo` foi renomeada para `Partida` e a classe `ParticipacaoJogo` passou a se chamar `ParticipacaoPartida`, deixando o modelo mais coerente com o domínio da aplicação.

Também foram atualizadas as referências relacionadas a essas entidades, como propriedades de navegação, chaves estrangeiras e coleções.

Os relacionamentos entre as entidades passaram a ser configurados diretamente no `DominoDbContext` através do método `OnModelCreating`, utilizando **Fluent API**.

Foram mapeados os relacionamentos entre:

* `Usuario` e `Jogador`;
* `Jogador` e `ParticipacaoPartida`;
* `Partida` e `ParticipacaoPartida`.

Além disso, foi criada uma camada **Repository** para cada uma das principais entidades:

* `UsuarioRepository`;
* `JogadorRepository`;
* `PartidaRepository`;
* `ParticipacaoPartidaRepository`.

Os repositories são responsáveis pelo acesso aos dados e utilizam **LINQ** para realizar buscas e consultas, através de operações como:

* `Where`;
* `FirstOrDefault`;
* `ToList`;
* `Include`.

Também foram implementadas operações básicas de inclusão, consulta, atualização e exclusão das entidades utilizando o `DominoDbContext`.

Após as alterações no modelo, foi criada uma nova migration para registrar a evolução da estrutura do banco de dados e, em seguida, a migration foi aplicada ao banco com o Entity Framework Core.

## Próximos Passos

Como evolução do projeto, os próximos passos podem incluir:

* Criar interfaces para os repositories;
* Implementar uma camada de serviços;
* Adicionar validações das regras de negócio;
* Criar consultas LINQ mais avançadas;
* Implementar operações assíncronas com `async` e `await`;
* Criar testes automatizados para os repositories;
* Integrar a camada de persistência com o fluxo das partidas;
* Criar uma API para disponibilizar as operações do sistema.
