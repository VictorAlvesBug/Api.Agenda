using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.DataLayer.Repositories.Interfaces;
using Api.Agenda.Model.Entities;

namespace Api.Agenda.Business.Services
{
	public class ContatoService : IContatoService
	{
		private readonly IContatoRepository _contatoRepository;
		private readonly ITipoContatoRepository _tipoContatoRepository;

		public ContatoService(
			IContatoRepository contatoRepository,
			ITipoContatoRepository tipoContatoRepository)
        {
			_contatoRepository = contatoRepository;
			_tipoContatoRepository = tipoContatoRepository;
		}

        public async Task<List<Contato>> Listar(int codigoPessoa)
		{
			return await _contatoRepository.Listar(codigoPessoa);
		}

		public async Task<Contato> Retornar(int codigoPessoa, int codigoContato)
		{
			return await _contatoRepository.Retornar(codigoPessoa, codigoContato);
		}

		public async Task<bool> Cadastrar(Contato contato)
		{
			if(contato == null)
				return false;

			contato.TipoContato = await _tipoContatoRepository.Retornar(contato.CodigoTipoContato);

			if(!contato.EhValido(out var _))
				return false;

			return await _contatoRepository.Cadastrar(contato) > 0;
		}

		public async Task<bool> Alterar(Contato contato)
		{
			if(contato == null)
				return false;

			contato.TipoContato = await _tipoContatoRepository.Retornar(contato.CodigoTipoContato);

			if (!contato.EhValido(out var _))
				return false;

			return await _contatoRepository.Alterar(contato);
		}

		public async Task<bool> Desativar(int codigoPessoa, int codigoContato)
		{
			return await _contatoRepository.Desativar(codigoPessoa, codigoContato);
		}
	}
}
