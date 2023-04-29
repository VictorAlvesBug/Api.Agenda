using Api.Agenda.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.DataLayer.Repositories.Interfaces
{
	public interface IPessoaRepository
	{
		Task<IEnumerable<Pessoa>> Listar();
		Task<Pessoa> Retornar(int codigo);
		Task<int> Cadastrar(Pessoa pessoa);
		Task<bool> Alterar(Pessoa pessoa);
		Task<bool> Desativar(int codigo);
	}
}
