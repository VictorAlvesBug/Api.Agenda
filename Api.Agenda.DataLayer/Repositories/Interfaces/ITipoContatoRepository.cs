using Api.Agenda.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.DataLayer.Repositories.Interfaces
{
	public interface ITipoContatoRepository
	{
		Task<IEnumerable<TipoContato>> Listar();
		Task<TipoContato> Retornar(int codigo);
		Task<int> Cadastrar(TipoContato tipoContato);
		Task<bool> Alterar(TipoContato tipoContato);
		Task<bool> Desativar(int codigo);
	}
}
