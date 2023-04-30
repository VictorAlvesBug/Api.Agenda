using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.DataLayer.Repositories.Interfaces;
using Api.Agenda.Model.Entities;
using System.Linq;

namespace Api.Agenda.Business.Services
{
	public class TipoContatoService : ITipoContatoService
	{
		private readonly ITipoContatoRepository _tipoContatoRepository;

		public TipoContatoService(ITipoContatoRepository tipoContatoRepository)
        {
			_tipoContatoRepository = tipoContatoRepository;
		}

        public async Task<List<TipoContato>> Listar()
		{
			return await _tipoContatoRepository.Listar();
		}

		public async Task<TipoContato> Retornar(int codigo)
		{
			return await _tipoContatoRepository.Retornar(codigo);
		}

		public async Task<bool> Cadastrar(TipoContato tipoContato)
		{
			if(tipoContato == null)
				return false;

			return await _tipoContatoRepository.Cadastrar(tipoContato) > 0;
		}

		public async Task<bool> Alterar(int codigo, TipoContato tipoContato)
		{
			if(tipoContato == null)
				return false;

			tipoContato.Codigo = codigo;

			return await _tipoContatoRepository.Alterar(tipoContato);
		}

		public async Task<bool> Desativar(int codigo)
		{
			return await _tipoContatoRepository.Desativar(codigo);
		}
	}
}
