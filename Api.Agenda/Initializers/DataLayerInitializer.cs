using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.DataLayer.Repositories;
using Api.Agenda.DataLayer.Repositories.Interfaces;

namespace Api.Agenda.Initializers
{
	public class DataLayerInitializer
	{
		public void Initialize(IServiceCollection services)
		{
			services.AddTransient<IPessoaRepository, PessoaRepository>();
			services.AddTransient<IContatoRepository, ContatoRepository>();
			services.AddTransient<ITipoContatoRepository, TipoContatoRepository>();
		}
	}
}
