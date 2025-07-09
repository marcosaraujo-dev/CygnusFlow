<div id="top"></div>

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]


<p align="center">
  <img src="images/CygnusFlow.png" width="50%" />
</p>

<h1 align="center">CygnusFlow</h1>
<h4 align="center"><em>Fluxo inteligente, gestão eficiente</em></h4>

<!-- ABOUT THE PROJECT -->

## Sobre o Projeto

O **CygnusFlow** é uma plataforma modular de gestão de projetos voltada para equipes técnicas, líderes de produto e stakeholders. Ele permite organizar e acompanhar o ciclo de vida completo de um projeto — do planejamento à execução — com foco em **transparência, produtividade e controle**.

O projeto segue os princípios da **Clean Architecture**, sendo adaptável para uso em ambientes desktop (via WinForms) e web (via API + frontend React).


## 📐 Arquitetura

O backend foi desenvolvido em **.NET 9** com separação clara por camadas:

Cada camada é isolada e respeita o princípio de inversão de dependência, tornando o projeto altamente testável, escalável e sustentável.

---

## 🧠 Funcionalidades

- 📋 Cadastro e edição de projetos e tarefas
- 🧑‍💼 Controle de acesso por papéis (Admin, P.O., Dev, Viewer)
- 📅 Gantt Chart com comparativos (Planejado vs Realizado)
- 📊 Relatórios gerenciais e analíticos
- 🔔 Notificações de prazos
- 📈 Dashboard com indicadores (burndown, atrasos, confiabilidade)
- 💬 Histórico e anotações por projeto
- 📤 Exportação para Excel/PDF
- 🔐 Autenticação JWT
- 💾 Suporte a múltiplos bancos: SQL Server, PostgreSQL, SQLite

---

## 🧪 Testes

O projeto contém testes unitários organizados por camada:

- `CygnusFlow.Domain`: Validações e regras puras
- `CygnusFlow.Application`: Casos de uso, regras de fluxo
- `CygnusFlow.Infrastructure`: Repositórios mockados
- Frameworks: **xUnit**, **Moq**, **FluentAssertions**


---

## 🛡️ Segurança

- Autenticação com JWT
- Armazenamento seguro de senhas (hash + salt)
- Proteções contra SQL Injection e XSS
- Comunicação segura via TLS
- Controle de acesso por perfil e escopo

---

## 📈 Roadmap (Resumo)

| Fase | Objetivo                                     | Status   |
|------|----------------------------------------------|----------|
| 1    | MVP Desktop com WinForms                     | ✅ Em andamento |
| 2    | Módulo de permissões e relatórios            | 🔜 Em planejamento |
| 3    | Dashboard completo e indicadores gerenciais  | 🔜 Futuro |
| 4    | Criação da API e frontend em React           | 🔜 Futuro |

<p align="right">(<a href="#top">back to top</a>)</p>

# Tecnologias



### Linguagens
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)

### Frameworks, Platforms and Libraries
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![WinForms](https://img.shields.io/badge/WinForms-007ACC?style=for-the-badge&logo=windows&logoColor=white)
![image](https://img.shields.io/badge/CSS3-1572B6?style=for-the-badge&logo=css3&logoColor=white)
![image](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![image](https://img.shields.io/badge/Node.js-43853D?style=for-the-badge&logo=node.js&logoColor=white)
![Express.js](https://img.shields.io/badge/express.js-%23404d59.svg?style=for-the-badge&logo=express&logoColor=%2361DAFB)
![image](https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white)
![image](https://img.shields.io/badge/styled--components-DB7093?style=for-the-badge&logo=styled-components&logoColor=white)
![image](https://img.shields.io/badge/Material--UI-0081CB?style=for-the-badge&logo=material-ui&logoColor=white)
![Insomnia](https://img.shields.io/badge/Insomnia-black?style=for-the-badge&logo=insomnia&logoColor=5849BE)
![Vite](https://img.shields.io/badge/vite-%23646CFF.svg?style=for-the-badge&logo=vite&logoColor=white)


### Banco de Dados
![SQL Server](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-07405E?style=for-the-badge&logo=sqlite&logoColor=white)

![image](https://img.shields.io/badge/Microsoft_Azure-0089D6?style=for-the-badge&logo=microsoft-azure&logoColor=white)

![image](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![image](https://img.shields.io/badge/Git-E34F26?style=for-the-badge&logo=git&logoColor=white)

### Designer
![Figma](https://img.shields.io/badge/figma-%23F24E1E.svg?style=for-the-badge&logo=figma&logoColor=white)
![Figma](https://img.shields.io/badge/draw.io-%002E3B.svg?style=for-the-badge&logo=draw.io&logoColor=white)

### Testes
![xUnit](https://img.shields.io/badge/xUnit-Blue?style=for-the-badge)
![Moq](https://img.shields.io/badge/Moq-LightGray?style=for-the-badge)
![FluentAssertions](https://img.shields.io/badge/FluentAssertions-purple?style=for-the-badge)

![cypress](https://img.shields.io/badge/-cypress-%23E5E5E5?style=for-the-badge&logo=cypress&logoColor=058a5e)
![Jest](https://img.shields.io/badge/-jest-%23C21325?style=for-the-badge&logo=jest&logoColor=white)
![NestJS](https://img.shields.io/badge/nestjs-%23E0234E.svg?style=for-the-badge&logo=nestjs&logoColor=white)


<!-- CONTACT -->

## Contatos
---

Marcos Araujo - [@linkedIn](https://www.linkedin.com/in/marcosaraujosouza/) - marcos.araso@hotmail.com

Project Link: [Projeto_Base_Typescript](https://github.com/marcosaraujo-dev/Projeto_Base_Typescript)

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->

## Agradecimentos
![image](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)
![image](https://img.shields.io/badge/Medium-12100E?style=for-the-badge&logo=medium&logoColor=white)
![image](https://img.shields.io/badge/dev.to-0A0A0A?style=for-the-badge&logo=dev.to&logoColor=white)



-   [XXXXXXX](https://XXXXXX/)

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[contributors-shield]: https://img.shields.io/github/contributors/marcosaraujo-dev/CygnusFlow.svg?style=for-the-badge
[contributors-url]: https://github.com/marcosaraujo-dev/CygnusFlow/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/marcosaraujo-dev/CygnusFlow.svg?style=for-the-badge
[forks-url]: https://github.com/marcosaraujo-dev/CygnusFlow/network/members
[stars-shield]: https://img.shields.io/github/stars/marcosaraujo-dev/CygnusFlow.svg?style=for-the-badge
[stars-url]: https://github.com/marcosaraujo-dev/CygnusFlow/stargazers
[issues-shield]: https://img.shields.io/github/issues/marcosaraujo-dev/CygnusFlow.svg?style=for-the-badge
[issues-url]: https://github.com/marcosaraujo-dev/CygnusFlow/issues
[license-shield]: https://img.shields.io/github/license/marcosaraujo-dev/CygnusFlow.svg?style=for-the-badge
[license-url]: https://github.com/marcosaraujo-dev/CygnusFlow/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/marcosaraujosouza/
