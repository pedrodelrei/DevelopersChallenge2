<h1 align="center">Setup</h1>

## Backend
    dotnet restore
    dotnet build
    dotnet run

## Frontend
    npm install
    npm run serve
    npm run build

<h1 align="center">Manual</h1>
Para fazer upload de um arquivo, basta clicar na aba <b>UPLOAD NEW OFX FILE</b>, clicar no botão <b>Import OFX File</b>, selecionar o arquivo e clicar no botão ao lado (que será desbloqueado assim que o arquivo for selecionado).
Assim que o arquivo for carregado no backend, uma mensagem de texto aparecerá dizendo o resultado da operação.

A aba <b>VIEW TRANSACTIONS</b> exibe uma tabela paginada com todas as transações

<h1 align="center">Backend</h1>
**Stack**:

    .Net 2.2.207
    LiteDB

A escolha de usar o LiteDB foi somente pela facilidade de implementação e escalabilidade.

A lógica de leitura do arquivo OFX está na classe OFXFileReader. A pesar do código estar muito extenso, acredito que era isso que foi esperado, já que não era para utilizar soluções prontas.

## Database

O banco não foi utilizado de forma relacional por limitações no LiteDB.

Não foram utilizadas chaves compostas pois o LiteDB não possui essa funcionalidade.

A verificação de duplicidade da transação é feita com o método <b>GetTransaction</b>.

<h1 align="center">Frontend</h1>

**Stack**

    Vue
    Vuetify

<h1 align="center">Referência</h1>
Para entender melhor a estrutura dos arquivos OFX, utilizou-se o seu manual de especificação: <a href="https://www.ofx.net/downloads/OFX%202.2.pdf"></a>


<h1 align="center">Observações</h1>
Por conta do tempo, ficaram faltando algumas coisas:

    Validação de extensão do arquivo OFX pelo backend
    Considerar a moeda (currency) das transações
    Considerar o timezone das transações
    Salvar o saldo atualizado baseado no OFX mais recente e exibir essa informação
    Possibilitar que o usuário possa filtrar os resultados por data
