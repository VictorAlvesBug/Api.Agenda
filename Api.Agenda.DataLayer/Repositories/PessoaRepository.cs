using Api.Agenda.DataLayer.ConnectionFactories;
using Api.Agenda.DataLayer.Repositories.Interfaces;
using Api.Agenda.Model.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.DataLayer.Repositories
{
	public class PessoaRepository : IPessoaRepository
	{
		public async Task<IEnumerable<Pessoa>> Listar()
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						SELECT
							*
						FROM
							Pessoa
						WHERE
							Ativo = 1;
						";

				return await connection.QueryAsync<Pessoa>(query);
			}
		}

		public async Task<Pessoa> Retornar(int codigo)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						SELECT
							*
						FROM
							Pessoa
						WHERE
							Codigo = @codigo;
						";

				return await connection.QueryFirstOrDefaultAsync<Pessoa>(query, new { codigo });
			}
		}

		public async Task<int> Cadastrar(Pessoa pessoa)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						INSERT INTO Pessoa
							(Nome)
						VALUES
							(@Nome);
						SELECT @@IDENTITY;
						";

				return await connection.QueryFirstOrDefaultAsync<int>(query, pessoa);
			}
		}

		public async Task<bool> Alterar(Pessoa pessoa)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						UPDATE
							Pessoa
						SET
							Nome = @Nome
						WHERE
							Codigo = @Codigo;
						";

				return await connection.ExecuteAsync(query, pessoa) > 0;
			}
		}

		public async Task<bool> Desativar(int codigo)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						UPDATE
							Pessoa
						SET
							Ativo = 0
						WHERE
							Codigo = @codigo;
						";

				return await connection.ExecuteAsync(query, new { codigo }) > 0;
			}
		}
	}
}
