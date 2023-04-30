using Api.Agenda.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.Business.Services.Interfaces
{
	public interface IContatoService
	{
		Task<List<Contato>> Listar(int codigoPessoa);
		Task<Contato> Retornar(int codigo);
		Task<bool> Cadastrar(Contato contato);
		Task<bool> Alterar(int codigo, Contato contato);
		Task<bool> Desativar(int codigo);
	}
}
