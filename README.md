# Web.Agenda

Este projeto foi desenvolvido com objetivo de participar de um processo seletivo.
Sua proposta é gerenciar **Pessoas**, **Contatos** e **Tipos de Contato** de uma
**Agenda**.

## Configurando o projeto

Primeiramente, configure as seguintes variáveis de ambiente, conforme o servidor 
MySQL que deseja utilizar, em **Windows** > **Variáveis de Ambiente** > 
**Variáveis de Ambiente...**:
- **MYSQL_ADDRESS**: Endereço do servidor de banco;
- **MYSQL_PORT**: Porta de acesso;
- **MYSQL_USER**: Usuário de login do banco;
- **MYSQL_PASSWORD**: Senha de login do banco;

**OBS.:** Caso seja necessário alterar o valor destas variáveis enquanto o 
projeto estiver **aberto no Visual Studio**, será necessário **reiniciar a IDE**, 
pois os valores das variáveis de ambiente são recuperados apenas no ato de abrir
o **Visual Studio**.

Execute o 
[Script SQL](https://github.com/VictorAlvesBug/Api.Agenda/blob/master/Script%20MySql.sql) 
de criação de **banco**, **tabelas** e inserção dos **dados iniciais** de 
algumas tabelas.

Realize o clone dos repositórios com os comandos abaixo:

```bash
git clone https://github.com/VictorAlvesBug/Api.Agenda.git
git clone https://github.com/VictorAlvesBug/Web.Agenda.git
```

Abra o projeto **Api.Agenda** no **Visual Studio** e pressione **IIS Express**.

Abra o projeto **Web.Agenda** no **Visual Studio Code** e instale o plugin 
**Live Server**.

Clique com o botão direito no arquivo **Pages** > **listar-pessoas.html** e 
clique na opção **Abrir com Live Server**.
