# 📖 BookFlow — Frontend

Bem-vindo ao repositório frontend do **BookFlow**, a interface da plataforma completa de empréstimo e venda de livros digitais.

Esta aplicação foi construída com um forte foco em **User Experience (UX)** e **User Interface (UI)**, com um design minimalista (Preto, Branco e tons de cinza) inspirado no Notion, transições suaves e responsividade total. 

## 🚀 Tecnologias e Stack

- **React 19**
- **TypeScript** — Tipagem estática forte, espelhando os DTOs do backend.
- **Tailwind CSS** — Estilização utilitária focada em alta produtividade e consistência.
- **Axios** — Cliente HTTP configurado com interceptadores globais.
- **React Router Dom** — Navegação e proteção de rotas (Protected Routes).
- **Lucide React** — Ícones modernos e limpos.

## 🛡️ Decisões de Segurança e Arquitetura

1. **Autenticação Segura:** 
   O token JWT não é salvo no `localStorage`. Ele é mantido apenas em memória dentro do `AuthContext`. Isso elimina vetores de ataque **XSS (Cross-Site Scripting)** visando roubo de tokens.
   
2. **Interceptor Global (Axios):**
   Qualquer requisição que retorne HTTP `401 Unauthorized` ou `403 Forbidden` é automaticamente interceptada. O usuário é desconectado de forma segura e redirecionado para a tela de login.

3. **Rotas Protegidas por Perfil (Role-Based):**
   A plataforma possui dois mundos completamente isolados: **Cliente** e **Editora**. O componente `<ProtectedRoute>` avalia o perfil antes de renderizar a tela. Se um Cliente tentar acessar a URL do Dashboard da Editora, ele recebe uma página customizada de "Acesso Negado".

4. **Componentização Modular:**
   Os componentes de UI (Botões, Inputs, Cards, Modais, Paginação) foram criados "do zero" utilizando Tailwind, sem depender de bibliotecas pesadas de componentes como Material UI. Isso mantém o bundle leve e o design 100% autoral.

## 💡 Funcionalidades Principais

### Visão da Editora 🏢
- **Métricas:** Dashboard estilo _fintech_ com um gráfico SVG próprio exibindo receita acumulada, total de livros vendidos e top performers da editora.
- **Carteira:** Acompanhamento do saldo atual em tempo real e solicitação de saque de receitas (Vendas).
- **Meus Livros:** Gestão do catálogo próprio, onde a Editora decide título, preço, quantidade em estoque e se o livro é **emprestável** ou não.

### Visão do Cliente 👤
- **Catálogo:** Exploração dos livros disponíveis com busca em tempo real por título.
- **Carteira:** Depósito de saldo fictício para possibilitar compras.
- **Compras & Empréstimos:** Compra direta com débito em carteira ou empréstimo gratuito.
- **Meus Empréstimos:** Painel para visualizar empréstimos ativos, histórico de devolução e atrasos (com aviso claro de multa de R$ 3,00/dia em caso de vencimento).

## 💻 Como rodar o projeto localmente

1. Clone o repositório e navegue até a pasta do frontend:
   ```bash
   cd bookflow-front
   ```

2. Instale as dependências:
   ```bash
   npm install
   ```

3. Configure as variáveis de ambiente:
   - Crie um arquivo `.env` na raiz do projeto (se já não existir).
   - Defina a URL da sua API local. Exemplo:
   ```env
   REACT_APP_API_URL=https://localhost:7157/api
   ```

4. Inicie o servidor de desenvolvimento:
   ```bash
   npm start
   ```

5. Abra [http://localhost:3000](http://localhost:3000) no seu navegador.

---

_Feito com muita dedicação. Cada detalhe visual e de arquitetura foi pensado para entregar uma aplicação que seja tão bonita de usar quanto é segura nos bastidores._
