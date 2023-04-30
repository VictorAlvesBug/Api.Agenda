using Api.Agenda.Model.Entities;

namespace Api.Agenda.Business.Services.Interfaces
{
	public interface IPessoaService
	{
		Task<List<Pessoa>> Listar();
		Task<Pessoa> Retornar(int codigoPessoa);
		Task<bool> Cadastrar(Pessoa pessoa);
		Task<bool> Alterar(Pessoa pessoa);
		Task<bool> Desativar(int codigoPessoa);
	}
}
