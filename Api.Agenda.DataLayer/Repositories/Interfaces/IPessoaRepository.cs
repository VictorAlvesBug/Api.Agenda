using Api.Agenda.Model.Entities;

namespace Api.Agenda.DataLayer.Repositories.Interfaces
{
	public interface IPessoaRepository
	{
		Task<List<Pessoa>> Listar();
		Task<Pessoa> Retornar(int codigoPessoa);
		Task<int> Cadastrar(Pessoa pessoa);
		Task<bool> Alterar(Pessoa pessoa);
		Task<bool> Desativar(int codigoPessoa);
	}
}
