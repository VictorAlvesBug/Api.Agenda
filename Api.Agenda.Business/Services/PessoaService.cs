using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.DataLayer.Repositories.Interfaces;
using Api.Agenda.Model.Entities;

namespace Api.Agenda.Business.Services
{
	public class PessoaService : IPessoaService
	{
		private readonly IPessoaRepository _pessoaRepository;
		private readonly IContatoRepository _contatoRepository;
		private readonly ITipoContatoRepository _tipoContatoRepository;

		public PessoaService(
			IPessoaRepository pessoaRepository,
			IContatoRepository contatoRepository,
			ITipoContatoRepository tipoContatoRepository)
        {
			_pessoaRepository = pessoaRepository;
			_contatoRepository = contatoRepository;
			_tipoContatoRepository = tipoContatoRepository;
		}

        public async Task<List<Pessoa>> Listar()
		{
			return await _pessoaRepository.Listar();
		}

		public async Task<Pessoa> Retornar(int codigoPessoa)
		{
			return await _pessoaRepository.Retornar(codigoPessoa);
		}

		public async Task<bool> Cadastrar(Pessoa pessoa)
		{
			if(pessoa == null)
				return false;

			if(await _pessoaRepository.Cadastrar(pessoa) == 0)
				return false;

			if (pessoa.ListaContatos == null || !pessoa.ListaContatos.Any())
				return true;

			return await CadastrarAlterarContatos(pessoa);
		}

		public async Task<bool> Alterar(Pessoa pessoa)
		{
			if(pessoa == null)
				return false;

			bool sucesso = await _pessoaRepository.Alterar(pessoa);

			if (sucesso == false)
				return false;

			return await CadastrarAlterarContatos(pessoa);
		}

		public async Task<bool> Desativar(int codigoPessoa)
		{
			return await _pessoaRepository.Desativar(codigoPessoa);
		}

		private async Task<bool> CadastrarAlterarContatos(Pessoa pessoa)
		{
			bool sucesso = true;

			if (await VerificarListaContatosMudou(pessoa))
			{
				var listaCodigoContatosAntigos = (await _contatoRepository.Listar(pessoa.Codigo)).Select(contato => contato.Codigo);

				foreach (var codigoContatoAntigo in listaCodigoContatosAntigos)
				{
					sucesso = sucesso && await _contatoRepository.Desativar(pessoa.Codigo, codigoContatoAntigo);
				}

				foreach (var contatoNovo in pessoa.ListaContatos)
				{
					contatoNovo.CodigoPessoa = pessoa.Codigo;
					sucesso = sucesso && await _tipoContatoRepository.Retornar(contatoNovo.CodigoTipoContato) != null;
					sucesso = sucesso && await _contatoRepository.Cadastrar(contatoNovo) > 0;
				}
			}

			return sucesso;
		}

		private async Task<bool> VerificarListaContatosMudou(Pessoa pessoa)
		{
			if (pessoa == null)
				return false;

			var listaContatosAntigos = await _contatoRepository.Listar(pessoa.Codigo);
			string strComparacaoContatosAntigos = RetornarStringComparacaoContatos(listaContatosAntigos);
			string strComparacaoContatosNovos = RetornarStringComparacaoContatos(pessoa.ListaContatos);

			return strComparacaoContatosAntigos != strComparacaoContatosNovos;
		}

		private static string RetornarStringComparacaoContatos(List<Contato> listaContatos)
		{
			if (listaContatos == null)
				return string.Empty;

			return string.Join(";",
					listaContatos
					.Select(contato => $"{contato.CodigoTipoContato}-{contato.Valor}")
				);
		}
	}
}
