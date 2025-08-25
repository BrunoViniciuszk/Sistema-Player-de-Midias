# Sistema-Player-de-Midias

# Projeto de Teste - Gestão de Mídias e Playlists

## Descrição do Projeto
Este projeto consiste em três frentes separadas, simulando um ambiente real de gerenciamento e exibição de mídias:

1. **API .NET**: CRUD de mídias e playlists, gerenciando dados no PostgreSQL.  
2. **Admin Web (React + TypeScript)**: Sistema de administração para cadastrar, editar e organizar mídias e playlists.  
3. **Player Web (React + TypeScript)**: Player responsável apenas por reproduzir as mídias das playlists selecionadas.  

---

## Estrutura de Pastas

projeto-teste/
│
├── api-dotnet/ # Backend .NET + PostgreSQL
├── admin-web-react/ # Frontend de administração
└── player-web-react/ # Player web


---

## Tecnologias Utilizadas

**Backend (.NET)**
- .NET 8, C#  
- Entity Framework Core  
- PostgreSQL  
- Swagger (para testes de endpoints)  

**Frontend Admin**
- React + TypeScript  
- Ant Design  
- Axios  
- Controle de estado: Luffie / Context API / Redux (conforme implementado)  

**Frontend Player**
- React + TypeScript  
- Axios  

---

## Instruções para Rodar Localmente

### 1. API .NET
1. Navegue até a pasta `api-dotnet`:
   ```bash
   cd api-dotnet
   dotnet restore
   dotnet ef database update
   dotnet run
   https://localhost:5001

### 2. Admin Web (React)
1. Navegue até a pasta `admin-web-react`:
   ```bash
   cd admin-web-react
   npm install
   npm start
   http://localhost:3000

### 2. Player Web (React)
1. Navegue até a pasta `player-web-react`:
   ```bash
   cd player-web-react
   npm install
   npm start
   http://localhost:3001

## Fluxo do Sistema
[Admin Web] --- Axios ---> [API .NET + PostgreSQL] --- Axios ---> [Player Web]


