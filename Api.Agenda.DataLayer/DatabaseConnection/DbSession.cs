using Api.Agenda.DataLayer.ConnectionFactories;
using Api.Agenda.DataLayer.Repositories.Interfaces;
using System.Data;

namespace Api.Agenda.DataLayer.DatabaseConnection
{
	public class DbSession : IDisposable, IDbSession
	{
		public IDbConnection Connection { get; set; }
		public IDbTransaction Transaction { get; set; }

		public DbSession(string banco = "railway")
		{
			Task.Run(() => GenerateSessionDB(banco)).Wait();
		}

		public async Task<bool> GenerateSessionDB(string banco)
		{
			Connection = await ConnectionFactory.ConexaoAsync(banco);

			if (Connection.State != ConnectionState.Open)
				Connection.Open();

			return Connection != null;
		}

		public async Task<IDbConnection> GetConnectionAsync(string banco)
		{
			if (Connection == null)
			{
				await GenerateSessionDB(banco);
			}

			Connection.ChangeDatabase(banco);

			return Connection;
		}

		public async Task<IDbTransaction> GetTransactionAsync() => Transaction;

		public void Dispose() => Connection?.Dispose();
	}
}
