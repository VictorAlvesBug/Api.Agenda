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
	public class ContatoRepository : IContatoRepository
	{
		public async Task<List<Contato>> Listar(int codigoPessoa)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						SELECT
							*
						FROM
							Contato
						WHERE
							Ativo = 1
							AND CodigoPessoa = @codigoPessoa;
						";

				return (await connection.QueryAsync<Contato>(query, new { codigoPessoa })).ToList();
			}
		}

		public async Task<Contato> Retornar(int codigo)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						SELECT
							*
						FROM
							Contato
						WHERE
							Codigo = @codigo;
						";

				return await connection.QueryFirstOrDefaultAsync<Contato>(query, new { codigo });
			}
		}

		public async Task<int> Cadastrar(Contato contato)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						INSERT INTO Contato
							(CodigoPessoa, CodigoTipoContato, Valor)
						VALUES
							(@CodigoPessoa, @CodigoTipoContato, @Valor);
						SELECT @@IDENTITY;
						";

				contato.Codigo = await connection.QueryFirstOrDefaultAsync<int>(query, contato);

				return contato.Codigo;
			}
		}

		public async Task<bool> Alterar(Contato contato)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						UPDATE
							Contato
						SET
							CodigoTipoContato = @CodigoTipoContato,
							Valor = @Valor
						WHERE
							Codigo = @Codigo;
						";

				return await connection.ExecuteAsync(query, contato) > 0;
			}
		}

		public async Task<bool> Desativar(int codigo)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						UPDATE
							Contato
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
