using Api.Agenda.Business.Services;
using Api.Agenda.Business.Services.Interfaces;

namespace Api.Agenda.Initializers
{
	public class BusinessInitializer
	{
		public void Initialize(IServiceCollection services)
		{
			services.AddTransient<IPessoaService, PessoaService>();
			services.AddTransient<IContatoService, ContatoService>();
			services.AddTransient<ITipoContatoService, TipoContatoService>();
		}
	}
}
