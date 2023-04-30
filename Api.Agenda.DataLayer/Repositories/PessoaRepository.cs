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
	public class PessoaRepository : IPessoaRepository
	{
		private readonly IDbSession _dbSession;

		public PessoaRepository(IDbSession dbSession)
		{
			_dbSession = dbSession;
		}

		public async Task<List<Pessoa>> Listar()
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						SELECT
							Pessoa.Codigo,
							Pessoa.Nome,
							Pessoa.DataHoraCadastro,
							Pessoa.Ativo,
							Contato.Codigo,
							Contato.CodigoPessoa,
							Contato.CodigoTipoContato,
							Contato.Valor,
							Contato.DataHoraCadastro,
							Contato.Ativo,
							TipoContato.Codigo,
							TipoContato.Nome,
							TipoContato.RegexValidacao,
							TipoContato.DataHoraCadastro,
							TipoContato.Ativo
						FROM
							Pessoa
							LEFT JOIN Contato
								ON Pessoa.Codigo = Contato.CodigoPessoa
							LEFT JOIN TipoContato
								ON Contato.CodigoTipoContato = TipoContato.Codigo
						WHERE
							Pessoa.Ativo = 1;
						";

			var lookupPessoa = new Dictionary<int, Pessoa>();

			await connection.QueryAsync<Pessoa, Contato, TipoContato, Pessoa>(query,
				(pessoa, contato, tipoContato) =>
				{

					if (!lookupPessoa.TryGetValue(pessoa.Codigo, out var pessoaExistente))
					{
						pessoaExistente = pessoa;
						lookupPessoa.Add(pessoa.Codigo, pessoaExistente);
					}

					pessoaExistente.ListaContatos ??= new List<Contato>();

					if (contato != null)
					{
						contato.TipoContato = tipoContato;
						pessoaExistente.ListaContatos.Add(contato);
					}

					return null;
				}, splitOn: "Codigo",
				transaction: _dbSession.Transaction);

			return lookupPessoa.Values.ToList();
		}

		public async Task<Pessoa> Retornar(int codigo)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						SELECT
							Pessoa.Codigo,
							Pessoa.Nome,
							Pessoa.DataHoraCadastro,
							Pessoa.Ativo,
							Contato.Codigo,
							Contato.CodigoPessoa,
							Contato.CodigoTipoContato,
							Contato.Valor,
							Contato.DataHoraCadastro,
							Contato.Ativo,
							TipoContato.Codigo,
							TipoContato.Nome,
							TipoContato.RegexValidacao,
							TipoContato.DataHoraCadastro,
							TipoContato.Ativo
						FROM
							Pessoa
							LEFT JOIN Contato
								ON Pessoa.Codigo = Contato.CodigoPessoa
							LEFT JOIN TipoContato
								ON Contato.CodigoTipoContato = TipoContato.Codigo
						WHERE
							Pessoa.Codigo = @codigo;
						";

			var lookupPessoa = new Dictionary<int, Pessoa>();

			return (await connection.QueryAsync<Pessoa, Contato, TipoContato, Pessoa>(query,
				(pessoa, contato, tipoContato) =>
				{
					pessoa.ListaContatos ??= new List<Contato>();

					if (contato != null)
					{
						contato.TipoContato = tipoContato;
						pessoa.ListaContatos.Add(contato);
					}
					return pessoa;
				}, 
				new { codigo }, 
				splitOn: "Codigo",
				transaction: _dbSession.Transaction)).FirstOrDefault();
		}

		public async Task<int> Cadastrar(Pessoa pessoa)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						INSERT INTO Pessoa
							(Nome)
						VALUES
							(@Nome);
						SELECT @@IDENTITY;
						";

			pessoa.Codigo = await connection.QueryFirstOrDefaultAsync<int>(
				query, 
				pessoa,
				transaction: _dbSession.Transaction);

			return pessoa.Codigo;
		}

		public async Task<bool> Alterar(Pessoa pessoa)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						UPDATE
							Pessoa
						SET
							Nome = @Nome
						WHERE
							Codigo = @Codigo;
						";

			return await connection.ExecuteAsync(
				query,
				pessoa,
				transaction: _dbSession.Transaction) > 0;
		}

		public async Task<bool> Desativar(int codigo)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						UPDATE
							Pessoa
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
