# Sistema-Player-de-Midias

## Projeto de Teste - Gestão de Mídias e Playlists

### Descrição do Projeto
Este projeto consiste em três frentes separadas, simulando um ambiente real de gerenciamento e exibição de mídias:

1. **API .NET**: CRUD de mídias e playlists, gerenciando dados no PostgreSQL.  
2. **Admin Web (React + TypeScript)**: Sistema de administração para cadastrar, editar e organizar mídias e playlists.  
3. **Player Web (React + TypeScript)**: Player responsável apenas por reproduzir as mídias das playlists selecionadas.  

---

### Estrutura de Pastas

```
projeto-teste/
│
├── api-dotnet/          # Backend .NET + PostgreSQL
├── admin-web-react/     # Frontend de administração
└── player-web-react/    # Player web
```

---

### Tecnologias Utilizadas

**Backend (.NET)**
- .NET 8, C#  
- Entity Framework Core  
- PostgreSQL  
- Swagger (para testes de endpoints)  

**Frontend Admin**
- React + TypeScript  
- Ant Design  
- Axios  
- Controle de estado: Luffie / Context API / Redux  

**Frontend Player**
- React + TypeScript  
- Axios  

---

### Instruções para Rodar Localmente

#### 1. API .NET
```bash
cd api-dotnet
dotnet restore
dotnet ef database update
dotnet run
```

#### 2. Admin Web (React)
```bash
cd admin-web-react
npm install
npm start
```

#### 3. Player Web (React)
```bash
cd player-web-react
npm install
npm start
```

---

### Fluxo do Sistema
```
[Admin Web] --- Axios ---> [API .NET + PostgreSQL] --- Axios ---> [Player Web]
```

---

### Fases Concluídas
- [x] CRUD de mídias (Fase 1)  
- [x] Playlists e associação com mídias (Fase 2)  
- [x] Player preview básico (Fase 3)  
- [x] JWT e FadeIn implementados (Fase 4)  

---

### O Que Faria Com Mais Tempo
- Implementar **WebSocket** ou polling para atualização em tempo real das playlists  
- Melhorar a **responsividade avançada** do Admin e Player  
- Criar **testes unitários e automatizados** no backend e frontend  
- Melhorar **UI/UX** das aplicações (fluxo, cores, botões, feedback visual)  
- Automatizar deploy com **Docker + CI/CD**  

