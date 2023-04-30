namespace Api.Agenda.Business.Services.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		void BeginTransaction();
		void Commit();
		void Rollback();
	}
}
