# Gallilearn

- Link domínio backend: 
  
- Link domínio frontend: 

## Identificação, proposta e solução
- O Gallilearn tem o objetivo de ajudar quem está em busca de aprender ou se aprofundar em conteúdos de astrofísica de forma simples e direta, inspirado no modelo de ensino do Duolingo.
- A ideia é facilitar o estudo e a prática por meio de módulos e lições, dispondo de explicações, exercícios e um sistema de gamificação (com pontos ou conquistas).
- Na prática, o usuário poderá navegar pelo catálogo de conteúdos, divididos por nível ou tema, e acompanhar seu progresso.

## Escopo

**Desenvolvimento**

- O projeto consiste em um sistema web, desenvolvido com C# e Vue:
- C# para criar a API que fornecerá as informações e funcionalidades (como cadastro de usuários, registro de progresso, etc.).
- Vue para construir a interface do usuário, exibindo os módulos de estudo e interagindo com a API.

**Qualidade do produto**

- Estão sendo desenvolvidos testes unitários no backend, visando melhor cobertura de qualidade do projeto.
- É utilizado o CodeCov para auxiliar na análise e melhoria contínua dos cenários de testes unitários.

## Restrições

- O sistema não fará streaming de vídeo nem oferecerá download de materiais audiovisuais (a disponibilização desses recursos, se necessária, poderá ser feita via links externos).
- O site não possui responsividade no momento, sendo preferencialmente acessado por desktops ou laptops.

## Requisitos funcionais

Identificação                | Objetivo                                                                                         |
---------------------------- | ----------------------------------------------------------------------------------------         |
Navegar pelo site            | Visualizar lições e módulos de diferentes níveis ou temas de astrofísica                         |
Utilizar o campo de pesquisa | Pesquisar tópicos específicos para obter informações e exercícios relacionados                   |
Registro                     | 	Registrar-se para salvar o progresso em cada lição e gerenciar seu desempenho                   |
Detalhes dos conteúdos       | Ser capaz de clicar em um módulo/assunto específico para ver detalhes, introdução teórica etc.   |
Resolver questões            | Ser capaz de responder as questões de cada modúlo                                                |


## Trade-offs

- Portabilidade: O sistema, por enquanto, não é adaptado para uso em dispositivos móveis. É necessário um navegador em desktop ou notebook.
- Funcionalidade: O foco é a simplicidade e a usabilidade, simulando a experiência de plataformas como Duolingo. Portanto, existem poucas funcionalidades, mas todas são essenciais (progresso, lições, exercícios, etc.).
- Confiabilidade: O sistema armazena o progresso de cada usuário. É necessário estar logado com credenciais válidas para acessar ou atualizar esses dados.
- Usabilidade: A interface foi planejada de maneira objetiva, para que qualquer pessoa interessada em astrofísica consiga começar seus estudos de forma intuitiva.
- Eficiência: O sistema deve responder às pesquisas e carregamento de conteúdos em até 2 segundos, garantindo fluidez na navegação.

## C4 Model

- [Acesse este caminho para ser redirecionado ao C4 Model.](files/c4-model.md)

## Casos de Uso

- [Acesse este caminho para ser redirecionado aos requisitos e os casos de uso.](files/requirements-nonrequirementsl.md)

# Documentação da Infraestrutura

## Implantação e Hospedagem no Azure

### 1. Ferramentas Utilizadas

### 2. Configuração do GitHub Actions
O GitHub Actions é usado para automação do processo de CI/CD. O arquivo de configuração `.yml` é criado no repositório GitHub para definir os passos necessários para build, test e deploy da aplicação.

### 3. Etapas da publicação

## Modelagem por funcionalidade
- Com o próprio GitHub, na opção de Projetos, as tarefas estão sendo dividas em processos no estilo Kanban.
  
## Stack
- BE: C# e .Net Core 8.0
- FE:  Vue.js, Javascript e CSS.
- Database: PostgreSQL e MongoDB
- Qualidade nos cenários de testes: CodeCov
- Observalidade: Azure Application Insights

## Rodar localmente o projeto
- Ferramentas: PostgreSQL (PgAdmin 4), MongoDbCompass, Docker, VisualStudio Community, Runtime do .Net Core 8.0
