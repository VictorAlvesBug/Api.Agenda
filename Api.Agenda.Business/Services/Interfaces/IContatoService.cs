using Api.Agenda.Model.Entities;

namespace Api.Agenda.Business.Services.Interfaces
{
	public interface IContatoService
	{
		Task<List<Contato>> Listar(int codigoPessoa);
		Task<Contato> Retornar(int codigoPessoa, int codigoContato);
		Task<bool> Cadastrar(Contato contato);
		Task<bool> Alterar(Contato contato);
		Task<bool> Desativar(int codigoPessoa, int codigoContato);
	}
}
