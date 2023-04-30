using Api.Agenda.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.Business.Services.Interfaces
{
	public interface ITipoContatoService
	{
		Task<List<TipoContato>> Listar();
		Task<TipoContato> Retornar(int codigo);
		Task<bool> Cadastrar(TipoContato tipoContato);
		Task<bool> Alterar(int codigo, TipoContato tipoContato);
		Task<bool> Desativar(int codigo);
	}
}
