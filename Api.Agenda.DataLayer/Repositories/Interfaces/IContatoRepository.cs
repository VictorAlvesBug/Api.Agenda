using Api.Agenda.Model.Entities;

namespace Api.Agenda.DataLayer.Repositories.Interfaces
{
	public interface IContatoRepository
	{
		Task<List<Contato>> Listar(int codigoPessoa);
		Task<Contato> Retornar(int codigoPessoa, int codigoContato);
		Task<int> Cadastrar(Contato contato);
		Task<bool> Alterar(Contato contato);
		Task<bool> Desativar(int codigoPessoa, int codigoContato);
	}
}
