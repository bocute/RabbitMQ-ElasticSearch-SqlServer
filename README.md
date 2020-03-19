# Serviço de integração entre banco de dados utilizando RabbitMQ
Desenvolvido em ASP.NET Core 3.1 utilizando RabbitMQ, SQL Server e ElasticSearch visando exemplificar uma transferência de dados entre banco de dados possibilitando dessa forma uma aplicação trabalhar com CQRS e mais de um banco de dados, sendo um para escrita e outro para leitura.

# Configurações
- Para uma configuração simples pode-se utilizar Docker.
- RabbitMQ
Executar o comando: docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 -e RABBITMQ_DEFAULT_USER=user -e RABBITMQ_DEFAULT_PASS=password rabbitmq:3-management
Após concluído verifique se é possível acessar http://localhost:15672 com o usuário e senha informados.

- ElasticSearch
Executar o comando: docker run -d --name elasticsearch --net somenetwork -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" elasticsearch:tag
Após concluído verifique se é possível acessar http://localhost:9200 com o usuário e senha informados.

- SQL Server
Executar o comando: docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=yourStrong(!)Password" -e "MSSQL_PID=Express" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu

# Divisão da Solução
ServiceRabbitMQ.Consumer
  - Serviço que ficará escutando a fila do RabbitMQ e adicionando os dados recebidos no ElasticSearch

ServiceRabbitMQ.Producer
  - Serviço que ficará buscando os dados no SQL Server e enviando para a fila do RabbitMQ

ServiceRabbitMQ.Domain
  - Mapeamento das entidades e business

ServiceRabbitMQ.Data.ElasticSearch
  - Configurações do ElasticSearch
  
ServiceRabbitMQ.Data.SQLServer
  - Onde são criados:
    - Contextos;
    - Mapeamentos para migrations;
    - Repositórios;

ServiceRabbitMQ.IoC
  - Projeto de injeção de dependências;
  
# Autor
Rafael Bocute
