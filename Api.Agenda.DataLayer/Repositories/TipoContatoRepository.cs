﻿using Api.Agenda.DataLayer.ConnectionFactories;
using Api.Agenda.DataLayer.Repositories.Interfaces;
using Api.Agenda.Model.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.DataLayer.Repositories
{
	public class TipoContatoRepository : ITipoContatoRepository
	{
		private readonly IDbSession _dbSession;

		public TipoContatoRepository(IDbSession dbSession)
		{
			_dbSession = dbSession;
		}

		public async Task<List<TipoContato>> Listar()
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						SELECT
							*
						FROM
							TipoContato
						WHERE
							Ativo = 1;
						";

				return (await connection.QueryAsync<TipoContato>(
					query,
					transaction: _dbSession.Transaction)).ToList();
		}

		public async Task<TipoContato> Retornar(int codigo)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						SELECT
							*
						FROM
							TipoContato
						WHERE
							Codigo = @codigo;
						";

				return await connection.QueryFirstOrDefaultAsync<TipoContato>(
					query, 
					new { codigo }, 
					transaction: _dbSession.Transaction);
		}

		public async Task<int> Cadastrar(TipoContato tipoContato)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						INSERT INTO TipoContato
							(Nome, RegexValidacao)
						VALUES
							(@Nome, @RegexValidacao);
						SELECT @@IDENTITY;
						";

				tipoContato.Codigo = await connection.QueryFirstOrDefaultAsync<int>(
					query, 
					tipoContato, 
					transaction: _dbSession.Transaction);

				return tipoContato.Codigo;
		}

		public async Task<bool> Alterar(TipoContato tipoContato)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						UPDATE
							TipoContato
						SET
							Nome = @Nome,
							RegexValidacao = @RegexValidacao
						WHERE
							Codigo = @Codigo;
						";

				return await connection.ExecuteAsync(
					query, 
					tipoContato, 
					transaction: _dbSession.Transaction) > 0;
		}

		public async Task<bool> Desativar(int codigo)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						UPDATE
							TipoContato
						SET
							Ativo = 0
						WHERE
							Codigo = @codigo;
						";

				return await connection.ExecuteAsync(
					query, 
					new { codigo }, 
					transaction: _dbSession.Transaction) > 0;
		}
	}
}
