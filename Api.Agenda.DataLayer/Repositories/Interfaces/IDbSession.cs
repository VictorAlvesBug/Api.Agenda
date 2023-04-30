using System.Data;

namespace Api.Agenda.DataLayer.Repositories.Interfaces
{
	public interface IDbSession
	{
		IDbConnection Connection { get; set; }
		IDbTransaction Transaction { get; set; }

		Task<IDbConnection> GetConnectionAsync(string banco);
		Task<IDbTransaction> GetTransactionAsync();
	}
}
