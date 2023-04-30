using Api.Agenda.Business.Services;
using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.DataLayer.DatabaseConnection;
using Api.Agenda.DataLayer.Repositories.Interfaces;

namespace Api.Agenda.Initializers
{
	public class UnitOfWorkInitializer
	{
		public void Initialize(IServiceCollection services)
		{
			services.AddScoped<IDbSession, DbSession>();
			services.AddTransient<IUnitOfWork, UnitOfWork>();
		}
	}
}
