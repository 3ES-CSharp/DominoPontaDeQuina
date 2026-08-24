# Desenvolvendo um Dominó Diferenciado

Nesta atividade foi realizada a configuração da camada de persistência do projeto **Domino Ponta de Quina** utilizando **Entity Framework Core**.

As entidades `Usuario` e `Jogador` foram configuradas com **Data Annotations**, definindo informações como chave primária, campos obrigatórios, limites de tamanho e relacionamentos entre as entidades.

A entidade `Jogo` foi mantida sem configurações adicionais para que o **Entity Framework Core** interpretasse sua estrutura através das convenções padrão.

Também foi implementado o `DominoDbContext`, responsável por representar o contexto do banco de dados e disponibilizar as entidades por meio dos `DbSet`.

Para a persistência, o projeto foi configurado para utilizar **SQL Server** através do provider `Microsoft.EntityFrameworkCore.SqlServer`.

Após a configuração do contexto e das entidades, foi criada a migration inicial:

```bash
dotnet ef migrations add Inicial --project DominoPontaDeQuina.Repository --startup-project DominoPontaDeQuina.Migrations
```

Em seguida, a migration foi aplicada ao banco de dados:

```bash
dotnet ef database update --project DominoPontaDeQuina.Repository --startup-project DominoPontaDeQuina.Migrations
```

Com isso, o Entity Framework Core gerou a estrutura do banco `DominoDB` e suas respectivas tabelas com base nas entidades do projeto.

## Próximos Passos

Como evolução do projeto, os próximos passos podem incluir:

* Implementar repositories para manipulação dos dados;
* Criar operações de cadastro, consulta, atualização e exclusão;
* Implementar a camada de serviços;
* Criar uma API para acesso aos dados;
* Implementar autenticação de usuários;
* Adicionar autenticação e autorização com JWT;
* Integrar a persistência com o fluxo das partidas;
* Criar interfaces para cadastro de usuários e jogadores;
* Ampliar os testes automatizados do projeto.
