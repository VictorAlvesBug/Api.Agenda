﻿using Api.Agenda.DataLayer.ConnectionFactories;
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
	public class TipoContatoRepository : ITipoContatoRepository
	{
		public async Task<IEnumerable<TipoContato>> Listar()
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						SELECT
							*
						FROM
							TipoContato
						WHERE
							Ativo = 1;
						";

				return await connection.QueryAsync<TipoContato>(query);
			}
		}

		public async Task<TipoContato> Retornar(int codigo)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						SELECT
							*
						FROM
							TipoContato
						WHERE
							Codigo = @codigo;
						";

				return await connection.QueryFirstOrDefaultAsync<TipoContato>(query, new { codigo });
			}
		}

		public async Task<int> Cadastrar(TipoContato tipoContato)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						INSERT INTO TipoContato
							(Nome, RegexValidacao)
						VALUES
							(@Nome, @RegexValidacao);
						SELECT @@IDENTITY;
						";

				return await connection.QueryFirstOrDefaultAsync<int>(query, tipoContato);
			}
		}

		public async Task<bool> Alterar(TipoContato tipoContato)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						UPDATE
							TipoContato
						SET
							CodigoPessoa = @CodigoPessoa,
							CodigoTipoContato = @CodigoTipoContato,
							Valor = @Valor
						WHERE
							Codigo = @Codigo;
						";

				return await connection.ExecuteAsync(query, tipoContato) > 0;
			}
		}

		public async Task<bool> Desativar(int codigo)
		{
			using (var connection = await ConnectionFactory.ConexaoAsync("Agenda"))
			{
				string query = @"
						UPDATE
							TipoContato
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