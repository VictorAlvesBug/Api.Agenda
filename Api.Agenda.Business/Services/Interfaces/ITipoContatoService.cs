using Api.Agenda.Model.Entities;

namespace Api.Agenda.Business.Services.Interfaces
{
	public interface ITipoContatoService
	{
		Task<List<TipoContato>> Listar();
		Task<TipoContato> Retornar(int codigoTipoContato);
		Task<bool> Cadastrar(TipoContato tipoContato);
		Task<bool> Alterar(TipoContato tipoContato);
		Task<bool> Desativar(int codigoTipoContato);
	}
}
