using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.DataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.Business.Services
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDbSession _session;

		public UnitOfWork(IDbSession session)
		{
			_session = session;
		}

		public void BeginTransaction()
		{
			_session.Transaction = _session.Connection.BeginTransaction();
		}

		public void Commit()
		{
			_session.Transaction.Commit();
			Dispose();
		}

		public void Rollback()
		{
			_session.Transaction.Rollback();
			Dispose();
		}

		public void Dispose() => _session.Connection?.Dispose();
	}
}
