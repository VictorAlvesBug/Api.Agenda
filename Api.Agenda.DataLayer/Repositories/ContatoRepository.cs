using Api.Agenda.DataLayer.ConnectionFactories;
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
	public class ContatoRepository : IContatoRepository
	{
		private readonly IDbSession _dbSession;

		public ContatoRepository(IDbSession dbSession)
		{
			_dbSession = dbSession;
		}

		public async Task<List<Contato>> Listar(int codigoPessoa)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						SELECT
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
							Contato
							INNER JOIN TipoContato
								ON Contato.CodigoTipoContato = TipoContato.Codigo
						WHERE
							Contato.Ativo = 1
							AND Contato.CodigoPessoa = @codigoPessoa;
						";

			var lookupContato = new Dictionary<int, Contato>();

			await connection.QueryAsync<Contato, TipoContato, Contato>(query, 
				(contato, tipoContato) => 
				{
					if(!lookupContato.TryGetValue(contato.Codigo, out var contatoExistente))
					{
						contatoExistente = contato;
						lookupContato.Add(contato.Codigo, contatoExistente);
					}

					contatoExistente.TipoContato = tipoContato;

					return null;
				},
				new { codigoPessoa },
				splitOn:"Codigo",
				transaction: _dbSession.Transaction);

			return lookupContato.Values.ToList();
		}

		public async Task<Contato> Retornar(int codigo)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						SELECT
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
							Contato
							INNER JOIN TipoContato
								ON Contato.CodigoTipoContato = TipoContato.Codigo
						WHERE
							Contato.Codigo = @codigo;
						";

			return (await connection.QueryAsync<Contato, TipoContato, Contato>(
				query, 
				(contato, tipoContato) => {
					contato.TipoContato = tipoContato;
					return contato;
				},
				new { codigo },
				splitOn: "Codigo",
				transaction: _dbSession.Transaction)).FirstOrDefault();
		}

		public async Task<int> Cadastrar(Contato contato)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						INSERT INTO Contato
							(CodigoPessoa, CodigoTipoContato, Valor)
						VALUES
							(@CodigoPessoa, @CodigoTipoContato, @Valor);
						SELECT @@IDENTITY;
						";

			contato.Codigo = await connection.QueryFirstOrDefaultAsync<int>(query, contato,
		transaction: _dbSession.Transaction);

			return contato.Codigo;
		}

		public async Task<bool> Alterar(Contato contato)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						UPDATE
							Contato
						SET
							CodigoTipoContato = @CodigoTipoContato,
							Valor = @Valor
						WHERE
							Codigo = @Codigo;
						";

			return await connection.ExecuteAsync(query, contato,
		transaction: _dbSession.Transaction) > 0;
		}

		public async Task<bool> Desativar(int codigo)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						UPDATE
							Contato
						SET
							Ativo = 0
						WHERE
							Codigo = @codigo;
						";

			return await connection.ExecuteAsync(query, new { codigo },
		transaction: _dbSession.Transaction) > 0;
		}
	}
}
