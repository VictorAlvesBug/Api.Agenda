using Api.Agenda.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.DataLayer.Repositories.Interfaces
{
	public interface IContatoRepository
	{
		Task<IEnumerable<Contato>> Listar(int codigoPessoa);
		Task<Contato> Retornar(int codigo);
		Task<int> Cadastrar(Contato contato);
		Task<bool> Alterar(Contato contato);
		Task<bool> Desativar(int codigo);
	}
}
