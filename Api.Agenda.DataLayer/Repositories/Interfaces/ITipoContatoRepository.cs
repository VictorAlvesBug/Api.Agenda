using Api.Agenda.Model.Entities;

namespace Api.Agenda.DataLayer.Repositories.Interfaces
{
	public interface ITipoContatoRepository
	{
		Task<List<TipoContato>> Listar();
		Task<TipoContato> Retornar(int codigoTipoContato);
		Task<int> Cadastrar(TipoContato tipoContato);
		Task<bool> Alterar(TipoContato tipoContato);
		Task<bool> Desativar(int codigoTipoContato);
	}
}
