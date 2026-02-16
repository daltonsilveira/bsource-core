
# PLATFORM_CORE_GUIDE

## 1. Objetivo
Este documento define o Platform Core: o conjunto mínimo e obrigatório de capacidades técnicas
que toda aplicação deve possuir desde o seu nascimento, independentemente das regras de negócio.

O Platform Core NÃO contém regras de negócio específicas e DEVE ser reutilizável.

## 2. Conceitos Fundamentais

### 2.1 Tenant
Representa uma é uma instância dedicada e isolada configurada para um cliente específico.
- Estará ligado a todas as entidades

### 2.2 User
Representa um ator autenticável do sistema.
- Identidade
- Credenciais
- Status
- Associação a Grupos
- Associação a um Tenant

### 2.3 Group
Representa um agrupamento lógico de usuários.
- Facilita gestão de permissões
- Um usuário pode pertencer a múltiplos grupos
- Associação a um Tenant

### 2.4 Permission
Representa uma capacidade explícita do sistema.
- Associada a Grupos
- Utilizada no processo de autorização
- Associação a um Tenant

## 3. Responsabilidades do Platform Core
O Platform Core DEVE fornecer:
- Autenticação JWT
- Autorização baseada em permissões
- Cadastro e gestão de tenants
- Cadastro e gestão de usuários
- Cadastro e gestão de grupos
- Cadastro e gestão de permissões
- Auditoria básica
- Contexto do usuário autenticado

## 4. Independência de Domínio
O Platform Core:
- NÃO conhece regras de negócio
- NÃO referencia domínios específicos
- DEVE ser extensível

## 5. Evolução
Novas capacidades podem ser adicionadas sem quebrar contratos existentes.
