using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.DataLayer.Repositories;
using Api.Agenda.DataLayer.Repositories.Interfaces;
using Api.Agenda.Model.Entities;
using System.Linq;

namespace Api.Agenda.Business.Services
{
	public class ContatoService : IContatoService
	{
		private readonly IContatoRepository _contatoRepository;

		public ContatoService(IContatoRepository contatoRepository)
        {
			_contatoRepository = contatoRepository;
		}

        public async Task<List<Contato>> Listar(int codigoPessoa)
		{
			return await _contatoRepository.Listar(codigoPessoa);
		}

		public async Task<Contato> Retornar(int codigo)
		{
			return await _contatoRepository.Retornar(codigo);
		}

		public async Task<bool> Cadastrar(Contato contato)
		{
			if(contato == null)
				return false;

			return await _contatoRepository.Cadastrar(contato) > 0;
		}

		public async Task<bool> Alterar(int codigo, Contato contato)
		{
			if(contato == null)
				return false;

			contato.Codigo = codigo;

			return await _contatoRepository.Alterar(contato);
		}

		public async Task<bool> Desativar(int codigo)
		{
			return await _contatoRepository.Desativar(codigo);
		}
	}
}
