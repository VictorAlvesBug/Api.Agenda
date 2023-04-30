using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
