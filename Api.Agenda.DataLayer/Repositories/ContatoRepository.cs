using Api.Agenda.DataLayer.Repositories.Interfaces;
using Api.Agenda.Model.Entities;
using Dapper;
using System.Data;

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
								AND TipoContato.Ativo = 1
						WHERE
							Contato.Ativo = 1
							AND Contato.CodigoPessoa = @codigoPessoa;
						";

			var lookupContato = new Dictionary<int, Contato>();

			await connection.QueryAsync<Contato, TipoContato, Contato>(query,
				(contato, tipoContato) =>
				{
					if (!lookupContato.TryGetValue(contato.Codigo, out var contatoExistente))
					{
						contatoExistente = contato;
						lookupContato.Add(contato.Codigo, contatoExistente);
					}

					contatoExistente.TipoContato = tipoContato;

					return null;
				},
				new { codigoPessoa },
				splitOn: "Codigo",
				transaction: _dbSession.Transaction);

			return lookupContato.Values.ToList();
		}

		public async Task<Contato> Retornar(int codigoPessoa, int codigoContato)
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
								AND TipoContato.Ativo = 1
						WHERE
							
							Contato.Ativo = 1
							AND Contato.Codigo = @codigoContato
							AND Contato.CodigoPessoa = @codigoPessoa;
						";

			return (await connection.QueryAsync<Contato, TipoContato, Contato>(
				query,
				(contato, tipoContato) =>
				{
					contato.TipoContato = tipoContato;
					return contato;
				},
				new { codigoPessoa, codigoContato },
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
						SELECT LAST_INSERT_ID();
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

			return await connection.ExecuteAsync(
				query,
				contato,
				transaction: _dbSession.Transaction) > 0;
		}

		public async Task<bool> Desativar(int codigoPessoa, int codigoContato)
		{
			IDbConnection connection = await _dbSession.GetConnectionAsync("Agenda");
			string query = @"
						UPDATE
							Contato
						SET
							Ativo = 0
						WHERE
							Codigo = @codigoContato
							AND CodigoPessoa = @codigoPessoa;
						";

			return await connection.ExecuteAsync(
				query,
				new
				{
					codigoPessoa,
					codigoContato
				},
				transaction: _dbSession.Transaction) > 0;
		}
	}
}
