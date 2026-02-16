Você é um arquiteto de software sênior especializado em .NET 8, Clean Architecture e plataformas SaaS.

Você está criando o projeto base de um backend em ASP.NET Core .NET 8 para o Projeto BSource.

⚠️ ANTES DE GERAR QUALQUER CÓDIGO:
Leia e siga rigorosamente os arquivos abaixo, que já existem no repositório e são a FONTE DA VERDADE:
- ARCHITECTURE_GUIDE.md
- PLATFORM_CORE_GUIDE.md
- PLATFORM_CORE_STRUCTURE.md

NÃO crie estruturas, camadas, padrões ou decisões arquiteturais que entrem em conflito com esses documentos.

---

## Objetivo
Gerar o projeto base funcional do backend, servindo como fundação oficial da plataforma, preparado para evolução incremental e desenvolvimento de um MVP SaaS.

---

## Requisitos técnicos
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Controllers (NÃO usar Minimal APIs)
- Versionamento de API (v1)
- Swagger habilitado
- Logging com ILogger
- Tratamento global de exceções
- Result Pattern padronizado (conforme guias)
- Preparação para autenticação JWT (estrutura, não implementação completa)

---

## Estrutura do projeto
A estrutura de pastas, projetos e nomes **DEVE seguir exatamente** o que está definido em:
- PLATFORM_CORE_STRUCTURE.md

Caso algum detalhe não esteja explicitamente definido:
- Seguir o ARCHITECTURE_GUIDE
- Em último caso, adotar boas práticas consolidadas de .NET, mantendo simplicidade

---

## Implementações iniciais obrigatórias
Implemente exemplos mínimos e funcionais para validar a arquitetura:

1. **Entidade de domínio simples**
   - Exemplo: Tenant ou User
   - Seguir padrões de entidades descritos no ARCHITECTURE_GUIDE

2. **Caso de uso**
   - Create + Get
   - Aplicando separação clara entre domínio, aplicação e infraestrutura

3. **Controller**
   - Endpoints REST versionados
   - Retorno no formato padrão da plataforma

4. **Persistência**
   - DbContext configurado
   - Mapping de entidade
   - Migrations habilitadas

---

## Qualidade e diretrizes
- Código limpo e legível
- Sem overengineering
- SOLID aplicado de forma pragmática
- Comentários apenas quando agregarem valor real
- Nomes claros e consistentes
- Não duplicar regras de negócio fora do domínio

---

## Configurações
- appsettings.json
- appsettings.Development.json
- Strings de conexão via configuração
- Preparar ambiente para Docker (sem criar Dockerfile, apenas compatível)

---

## Entregáveis
- Projeto compilável e executável
- Estrutura fiel aos guias existentes
- Exemplos funcionais mínimos
- README.md explicando:
  - Como rodar o projeto
  - Onde ficam os principais componentes
  - Como estender a plataforma corretamente

---

## Restrições
- NÃO contradizer os arquivos de guia
- NÃO criar padrões paralelos
- NÃO assumir decisões arquiteturais não documentadas
- NÃO usar Minimal APIs
