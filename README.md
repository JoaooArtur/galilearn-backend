# Gallilearn

- **Link domínio backend:** [https://galilearn-backend.onrender.com/swagger/index.html](https://galilearn-backend.onrender.com/swagger/index.html)
- **Link domínio frontend:** [https://gallilearn-frontend.lovable.app/](https://gallilearn-frontend.lovable.app/)

## Identificação, proposta e solução
- **Gallilearn** tem o objetivo de resolver a dificuldade no acesso a um conteúdo de astrofísica que seja ao mesmo tempo engajador, estruturado e acessível, transformando o aprendizado em uma jornada gamificada.

- A ideia é facilitar a jornada de aprendizado do usuário, oferecendo um catálogo de assuntos de astrofísica divididos em lições. Ao final de cada lição, o usuário testa seus conhecimentos com um quiz, ganha experiência para subir de nível e pode competir em um ranking com amigos.

- Na prática, o usuário navega pelos assuntos, completa as lições e seus respectivos quizzes, e acompanha seu progresso e o de seus amigos. A solução proposta é oferecer uma plataforma prática e motivadora para o estudo da astrofísica.


## Escopo

**Desenvolvimento**

- O projeto será um sistema web, desenvolvido com **C# (.NET 8)** e **React (com TypeScript)**.
- O **backend** será construído em C# com uma arquitetura robusta baseada nos princípios de **DDD (Domain-Driven Design)**, **CQRS (Command Query Responsibility Segregation)** e **Arquitetura Orientada a Eventos (EDA)**. A biblioteca **MediatR** será usada para a implementação do padrão CQRS, e o **MassTransit** para a abstração do envio de mensagens para o broker **CloudAMQP**.
- O **frontend** será uma SPA (Single Page Application) moderna, desenvolvida com React e TypeScript para garantir tipagem estática e maior escalabilidade da interface.

**Qualidade do produto**

- Testes de unidade e integração (utilizando **xUnit**, **Moq** e **FluentAssertions**) serão implementados no backend para garantir a qualidade e a cobertura de código das regras de negócio, comandos e queries. A qualidade é continuamente aferida via CodeCov e SonarQube Cloud.

## Restrições

- O sistema não oferecerá aulas ao vivo ou tutoria com especialistas.
- O sistema não será um aplicativo móvel nativo (iOS/Android), funcionando exclusivamente via navegador web.
- O conteúdo se limitará aos tópicos de astrofísica cadastrados na plataforma.

## Requisitos funcionais

| Identificação | Objetivo |
| :--- | :--- |
| Navegar pelos Assuntos | Visualizar os diferentes assuntos (ex: "Galáxias", "Estrelas") e as lições dentro deles. |
| Assistir uma Lição | Acessar o conteúdo de uma lição específica para estudo. |
| Realizar um Quiz | Concluir um quiz de 5 perguntas de múltipla escolha, geradas aleatoriamente, para finalizar uma lição. |
| Subir de Nível | Ganhar pontos de experiência ao concluir lições e quizzes para progredir no sistema de níveis. |
| Adicionar Amigos | Enviar e aceitar pedidos de amizade para outros usuários da plataforma. |
| Visualizar Ranking | Acessar um ranking que mostra o nível dos amigos, incentivando a competição saudável. |
| Registro e Login | Registrar-se e autenticar-se para salvar o progresso, nível e lista de amigos. |


## Trade-offs

- **Portabilidade:** O sistema será desenvolvido com foco em responsividade (web-first), garantindo total compatibilidade com navegadores em desktops e dispositivos móveis, em detrimento do desenvolvimento de aplicativos nativos.
- **Funcionalidade:** O foco do sistema é na simplicidade e no ciclo de aprendizado (estudar -> praticar -> progredir). Funcionalidades complexas como fóruns de discussão não serão implementadas para manter o uso fácil e direto.
- **Confiabilidade:** A confiabilidade está centrada na correta persistência do progresso do usuário. A arquitetura CQRS/EDA garante resiliência e consistência eventual dos dados entre os modelos de escrita e leitura.
- **Usabilidade:** A interface será limpa e intuitiva, com uma jornada de usuário clara para atender desde estudantes curiosos a entusiastas da astrofísica.
- **Eficiência:** A separação dos bancos de dados (PostgreSQL para escrita, MongoDB para leitura) otimiza a performance. Comandos de escrita são robustos, enquanto as consultas de leitura (lições, rankings) são extremamente rápidas.
- **Manutenibilidade:** A arquitetura com DDD, CQRS e EDA promove um baixo acoplamento e alta coesão, facilitando a manutenção e a evolução de diferentes partes do sistema de forma independente. No entanto, aumenta a complexidade inicial de configuração e infraestrutura.

## C4 Model

- [Acesse este caminho para ser redirecionado ao C4 Model.](files/c4-model.md)

## Casos de Uso

- [Acesse este caminho para ser redirecionado aos requisitos e os casos de uso.](files/requirements-nonrequirementsl.md)

# Documentação da Infraestrutura

## Implantação e Hospedagem

### 1. Ferramentas Utilizadas
- **Backend (API)**: Hospedado no **Render** como um serviço web, a partir de um `Dockerfile`.
- **Frontend**: Hospedado na plataforma **Lovable**.
- **Banco de Dados de Escrita**: **PostgreSQL** hospedado no **Google Cloud SQL**.
- **Banco de Dados de Leitura**: **MongoDB** (MongoDB Atlas).
- **Mensageria**: **CloudAMQP** (RabbitMQ as a Service).
- **Monitoramento**: **New Relic** (configurado via Dockerfile).
- **CI/CD**: **GitHub Actions**.

### 2. Fluxo de CI/CD
O projeto utiliza um fluxo de CI/CD robusto com pipelines centralizadas em um repositório dedicado ([gallilearn-pipelines](https://github.com/JoaooArtur/gallilearn-pipelines)), que são invocadas via `workflow_call`. O principal gatilho para o processo de qualidade é a abertura de um Pull Request para a branch `main`.

O processo é o seguinte:
1.  Quando um PR é aberto, uma pipeline inicial é acionada para **realizar o build** da aplicação, garantindo que o código compila corretamente.
2.  Após o sucesso do build, duas pipelines são chamadas para rodar em paralelo:
    - **Pipeline de Testes:** Executa todos os testes unitários. Ao final, gera um relatório de cobertura e o envia para o **CodeCov**, permitindo a análise da cobertura de testes do PR.
    - **Pipeline de Análise Estática:** Realiza uma varredura completa do código com o **SonarQube Cloud**, que analisa a qualidade, identifica code smells, bugs e vulnerabilidades de segurança, publicando um resumo no próprio PR.

Apenas após a aprovação em todas as etapas (build, testes e análise de qualidade) o PR pode ser mesclado à `main`, o que então dispara o deploy para produção.

### 3. Monitoramento com New Relic
A performance da aplicação e a observabilidade dos logs são monitoradas através do New Relic. O agente é instalado e configurado diretamente no `Dockerfile` da API, permitindo uma visão detalhada das transações, erros e saúde geral do sistema em tempo real.

![Painel New Relic](https://github.com/user-attachments/assets/64a7c1a3-1727-480e-a8b8-ba7494467a45)

## Stack

- **BE**: C#, .NET 8, ASP.NET Core
- **Arquitetura**: DDD, CQRS, Event-Driven Architecture
- **Bibliotecas BE**:
  - **Core**: MediatR, MassTransit, Entity Framework Core, Dapper
  - **Validação e Utilitários**: FluentValidation, Ardalis, Newtonsoft.Json
  - **Jobs e Logging**: Hangfire, Serilog
- **FE**: React, TypeScript, CSS
- **Banco de Dados (Escrita)**: PostgreSQL (Google Cloud SQL)
- **Banco de Dados (Leitura)**: MongoDB (Atlas)
- **Mensageria**: CloudAMQP (RabbitMQ)
- **Testes**: xUnit, Moq, FluentAssertions
- **Qualidade e Monitoramento**: [SonarQube Cloud](https://sonarcloud.io/project/overview?id=JoaooArtur_galilearn-backend), [CodeCov](https://app.codecov.io/gh/JoaooArtur/gallilearn-backend), New Relic
- **Infraestrutura**: Render, Docker

## Rodar localmente o projeto
- **Ferramentas**: .NET 8 SDK, Node.js, Docker e Docker Compose, Visual Studio ou VS Code.

### 1. Rodar a Infraestrutura Local
Para simplificar o ambiente de desenvolvimento, todos os serviços de infraestrutura (PostgreSQL, MongoDB, RabbitMQ) são gerenciados via Docker.

Primeiro, clone o repositório de infraestrutura [gallilearn-docker-compose](https://github.com/JoaooArtur/galilearn-docker-compose.git) e inicie os contêineres:
```bash
git clone https://github.com/JoaooArtur/galilearn-docker-compose.git
cd galilearn-docker-compose
docker-compose up -d
```
Os serviços estarão disponíveis nas portas padrão (Postgres: 5432, Mongo: 27017, RabbitMQ: 5672/15672).

### 2. Rodar o Backend
O backend roda em um contêiner Docker separado que se conecta à rede de infraestrutura criada anteriormente.

Primeiro, clone o repositório e navegue até a pasta raiz:
```bash
git clone https://github.com/JoaooArtur/gallilearn-backend.git
cd gallilearn-backend
```
Em seguida, compile a imagem Docker a partir do Dockerfile do projeto. O Dockerfile principal está localizado em src/Web/WebBff:
```bash
docker build -t gallilearn-backend:local -f ./src/Web/WebBff/Dockerfile .
```
Finalmente, execute o contêiner. O comando abaixo irá conectar o backend à rede do Docker Compose e passar as connection strings necessárias como variáveis de ambiente.

- **Nota:** O nome da rede do Docker Compose é geralmente **<nome_da_pasta>_default**. Verifique o nome da sua rede com docker network ls. O exemplo abaixo usa **galilearn-docker-compose_default**.
```bash
docker run --rm -it \
  -p 7001:8080 \
  --network galilearn-docker-compose_default \
  -e "ConnectionStrings__PostgresConnection=Host=postgres;Port=5432;Database=gallilearndb;Username=user;Password=password" \
  -e "ConnectionStrings__MongoConnection=mongodb://root:example@mongo:27017/" \
  -e "ConnectionStrings__RabbitMqConnection=amqp://guest:guest@rabbitmq:5672/" \
  gallilearn-backend:local
```
Após esses passos, o backend estará acessível em **https://localhost:7001**.

### 3. Rodar o Frontend
Clone o repositório [gallilearn-frontend](https://github.com/JoaooArtur/gallilearn-frontend.git), instale as dependências e inicie a aplicação:
```bash
git clone https://github.com/JoaooArtur/gallilearn-frontend.git
cd gallilearn-frontend
npm install
```
Depois, crie um arquivo .env.local e adicione a URL do seu backend local:
**REACT_APP_API_URL=https://localhost:7001**
Finalmente, execute o frontend:
```bash
npm start
```
