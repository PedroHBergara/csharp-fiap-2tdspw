# API de Gerenciamento de Motos

## Descrição do Projeto

A ideia do nosso projeto é ter um controle total sobre o galpão sabendo onde esta cada moto e como elas estão por isso fizemos essa API para ter um controle sobre nossas motos que usaremos em outras partes do projeto para mapear e localizar dentro do galpao com precisão.

**Observação:** Esta versão utiliza um banco de dados em memória para persistência de dados, o que significa que os dados serão perdidos quando a aplicação for reiniciada.

## Tecnologias Utilizadas

*   .NET 8 (ou a versão do seu SDK)
*   ASP.NET Core Minimal API
*   Entity Framework Core (com provedor In-Memory)
*   C#
*   OpenAPI (via Swashbuckle/Scalar) para documentação

## Rotas da API (Endpoints)

A base da URL para todos os endpoints é `/motos`.

| Verbo HTTP | Rota             | Descrição                                    | Corpo da Requisição (JSON) | Respostas Possíveis                                    |
| :--------- | :--------------- | :------------------------------------------- | :------------------------- | :----------------------------------------------------- |
| `GET`      | `/`              | Lista todas as motos cadastradas.            | N/A                        | `200 OK` (com a lista de motos)                        |
| `GET`      | `/{id}`          | Busca uma moto específica pelo seu ID.       | N/A                        | `200 OK` (com a moto), `404 Not Found`                 |
| `POST`     | `/`              | Cria uma nova moto.                          | Objeto `Moto`              | `201 Created` (com a moto criada), `400 Bad Request` |
| `PUT`      | `/{id}`          | Atualiza uma moto existente pelo seu ID.     | Objeto `Moto`              | `204 No Content`, `404 Not Found`, `400 Bad Request` |
| `DELETE`   | `/{id}`          | Deleta uma moto existente pelo seu ID.       | N/A                        | `204 No Content`, `404 Not Found`                      |

**Estrutura do Objeto `Moto` (JSON):**

```json
{
  "id": 0, // Ignorado na criação (POST), usado na resposta
  "modelo": "string (obrigatório, max 50)",
  "status": true, // ou false
  "placa": "string (obrigatório, max 7)"
}
```

## Instalação e Execução

1.  **Pré-requisitos:**
    *   SDK do .NET instalado (versão 8 ou compatível).
2.  **Clonar o Repositório (se aplicável):**
    ```bash
    git clone <url-do-seu-repositorio>
    cd <pasta-do-projeto>
    ```
3.  **Restaurar Dependências:**
    ```bash
    dotnet restore
    ```
4.  **Executar a Aplicação:**
    ```bash
    dotnet run
    ```
    A API estará rodando geralmente em `https://localhost:xxxx` e `http://localhost:yyyy` (as portas serão exibidas no console).

## Como Usar

*   Após executar a aplicação, você pode acessar a documentação interativa da API (Scalar UI) no seu navegador, geralmente em `/scalar` (ex: `http://localhost:yyyy/scalar`).
*   Você pode usar ferramentas como `curl`, Postman, Insomnia ou a própria interface do Scalar para enviar requisições para os endpoints listados acima.

