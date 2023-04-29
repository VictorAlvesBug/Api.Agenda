using Api.Agenda.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.Business.Services.Interfaces
{
	public interface IPessoaService
	{
		//Task<ListaParametros> Listar(ListaParametros parametros);
		Task<IEnumerable<Pessoa>> Listar();
		Task<Pessoa> Retornar(int codigo);
		Task<bool> Cadastrar(Pessoa pessoa);
		Task<bool> Alterar(Pessoa pessoa);
		Task<bool> Desativar(int codigo);
	}
}
