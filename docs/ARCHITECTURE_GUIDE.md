# ARCHITECTURE_GUIDE

## 1. Objetivo

Este documento define as **diretrizes arquiteturais obrigatórias** para todos os projetos da plataforma.

Ele estabelece **regras normativas**, não sugestões, sobre:

* Estrutura do projeto
* Separação de responsabilidades
* Contratos entre camadas
* Padrões técnicos obrigatórios

Estas diretrizes **independem do domínio de negócio** e devem ser seguidas em todos os sistemas.

---

## 2. Estrutura Padrão do Projeto

Todo projeto **DEVE** seguir a estrutura abaixo:

```
/docs
/docker
/src
 ├── NomeProjeto.API
 ├── NomeProjeto.Domain
 ├── NomeProjeto.Application
 ├── NomeProjeto.Infrastructure
 └── NomeProjeto.Shared
```

---

## 3. Princípios Arquiteturais

3.1 Separação de responsabilidades clara

3.2 Dependências sempre apontam para dentro:

```
API → Application → Domain
API → Application → Infrastructure (via abstrações)
```

3.3 Nenhuma camada externa conhece detalhes internos de outra

3.4 Regras de negócio **NUNCA** dependem de infraestrutura

---

## 4. Padrão de Mediação

4.1 Toda comunicação entre API e Application **DEVE** ocorrer via padrão de mediação (MediatR)

4.2 Controllers **NÃO DEVEM** chamar serviços diretamente

4.3 Application expõe apenas **Commands e Queries**

---

## 5. Responsabilidades das Camadas

### 5.1 API

* Atua exclusivamente como **adaptador HTTP**
* Expõe endpoints
* Executa autenticação e autorização
* Mapeia Request → Command / Query
* Mapeia Result → Response

### 5.2 Application

* Implementa casos de uso
* Orquestra fluxos
* Define Commands, Queries e Handlers
* Contém DTOs internos
* Contém Validators (FluentValidation)

### 5.3 Domain

* Contém regras invariantes
* Modela o negócio
* Entidades e Value Objects

### 5.4 Infrastructure

* Persistência
* Integrações externas
* Serviços técnicos

### 5.5 Shared

* Abstrações
* Contratos
* Utilitários transversais

---

## 6. Contratos da Camada API

### 6.1 Princípio

A camada API **NÃO expõe estruturas internas da aplicação**.

Ela atua apenas como adaptador entre HTTP e Application.

---

### 6.2 Regras Obrigatórias

6.2.1 A API **NÃO DEVE** utilizar DTOs da camada Application

6.2.2 A API **DEVE** trabalhar apenas com:

* Request (entrada HTTP)
* Response / Result (saída HTTP)

6.2.3 DTOs pertencem **exclusivamente à camada Application**

6.2.4 Controllers **NÃO DEVEM** acessar Domain ou Infrastructure diretamente

6.2.5 Controllers **DEVEM** apenas:

* Receber Requests
* Autorizar acesso
* Mapear para Commands / Queries
* Delegar para Application
* Mapear resultado para Responses

---

### 6.3 Fluxo Arquitetural Oficial

```
HTTP Request
   ↓
API Request
   ↓
Command / Query
   ↓
Application
   ↓
DTO (interno)
   ↓
Map
   ↓
API Response
```

---

## 7. Validações

7.1 Validações de entrada **DEVEM** ser feitas na Application

7.2 O padrão obrigatório é **FluentValidation**

7.3 Validators validam:

* Formato
* Consistência de dados
* Regras de caso de uso

7.4 O Domain contém apenas **invariantes essenciais**

---

## 8. Autenticação e Autorização

8.1 Autenticação baseada em JWT

8.2 Login é o único endpoint público

8.3 Todos os demais endpoints **DEVEM** exigir token válido

8.4 Autorização baseada em **permissões**, nunca roles fixas

8.5 Endpoints protegidos **DEVEM** declarar policies explicitamente

---

## 9. Auditoria

9.1 Todas as entidades persistidas **DEVEM** conter:

```
CreatedById   (Guid?)
CreatedAt     (DateTimeOffset)
UpdatedById   (Guid?)
UpdatedAt     (DateTimeOffset)
```

9.2 Preenchimento automático via DbContext

---

## 10. Status

10.1 Todas as entidades persistidas **DEVEM** conter:

- Uma chave única. Não criar entidades com chaves compostas
- A chave deve seguir o padrão NomeDaEntidadeId

---

## 11. Relação de Entidades

11.1 Todas as entidades persistidas **DEVEM** conter:

```
Status   (BaseStatus) => BaseStatus.Active
```

- BaseStatus é um enum com os seguintes status: Active, Inactive, Pending, Deleted
- O uso deste status elimina a necessidade de usar propriedades IsActive, IsDeleted etc.

---

## 12. DbContext

12.1 Deve existir:

* Um DbContext de escrita
* Um DbContext somente leitura

12.2 Queries **DEVEM** utilizar o DbContext de leitura

---

## 13. Platform Core

13.1 Funcionalidades transversais obrigatórias:

* Autenticação
* Usuários
* Grupos
* Permissões

13.2 O Platform Core é definido em documentos próprios:

* PLATFORM_CORE_GUIDE.md
* PLATFORM_CORE_STRUCTURE.md

13.3 O Platform Core **NÃO depende** do domínio de negócio

---

## 14. Decisões Arquiteturais Obrigatórias

* API não usa DTO
* Application usa FluentValidation
* Comunicação via MediatR
* Autorização baseada em permissões
* Auditoria obrigatória

---

## 15. Regra Final

Estas diretrizes são **contratos arquiteturais**.

Qualquer exceção:

* Deve ser consciente
* Deve ser documentada
* Deve ser justificada
