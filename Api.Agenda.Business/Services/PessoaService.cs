using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.DataLayer.Repositories.Interfaces;
using Api.Agenda.Model.Entities;
using System.Linq;

namespace Api.Agenda.Business.Services
{
	public class PessoaService : IPessoaService
	{
		private readonly IPessoaRepository _pessoaRepository;
		private readonly IContatoRepository _contatoRepository;

		public PessoaService(IPessoaRepository pessoaRepository, IContatoRepository contatoRepository)
        {
			_pessoaRepository = pessoaRepository;
			_contatoRepository = contatoRepository;
		}

        public async Task<IEnumerable<Pessoa>> Listar()
		{
			var listaPessoas = await _pessoaRepository.Listar();

			foreach (var pessoa in listaPessoas)
			{
				pessoa.ListaContatos = await _contatoRepository.Listar(pessoa.Codigo);
			}

			return listaPessoas;
		}

		public async Task<Pessoa> Retornar(int codigo)
		{
			Pessoa pessoa = await _pessoaRepository.Retornar(codigo);

			if (pessoa == null)
				return new Pessoa();

			pessoa.ListaContatos = await _contatoRepository.Listar(codigo);

			return pessoa;
		}

		public async Task<bool> Cadastrar(Pessoa pessoa)
		{
			if(pessoa == null)
				return false;

			int codigoCadastrado = await _pessoaRepository.Cadastrar(pessoa);

			if(codigoCadastrado == 0)
				return false;

			if (pessoa.ListaContatos == null || !pessoa.ListaContatos.Any())
				return true;

			pessoa.Codigo = codigoCadastrado;

			return await CadastrarContatos(pessoa);
		}

		public async Task<bool> Alterar(Pessoa pessoa)
		{
			if(pessoa == null)
				return false;

			bool sucesso = await _pessoaRepository.Alterar(pessoa);

			if (sucesso == false)
				return false;

			return await AlterarContatos(pessoa);
		}

		public async Task<bool> Desativar(int codigo)
		{
			return await _pessoaRepository.Desativar(codigo);
		}

		private async Task<bool> CadastrarContatos(Pessoa pessoa)
		{
			bool sucesso = true;

			foreach (Contato contato in pessoa.ListaContatos)
			{

				if (contato == null)
				{
					return false;
				}

				sucesso &= await _contatoRepository.Cadastrar(contato) > 0;
			}

			return sucesso;
		}

		private async Task<bool> AlterarContatos(Pessoa pessoa)
		{
			bool sucesso = true;

			if (await VerificarListaContatosMudou(pessoa))
			{
				var listaCodigoContatosAntigos = (await _contatoRepository.Listar(pessoa.Codigo)).Select(contato => contato.Codigo);

				foreach (var codigoContatoAntigo in listaCodigoContatosAntigos)
				{
					sucesso &= await _contatoRepository.Desativar(codigoContatoAntigo);
				}

				foreach (var contatoNovo in pessoa.ListaContatos)
				{
					contatoNovo.CodigoPessoa = pessoa.Codigo;
					sucesso &= await _contatoRepository.Cadastrar(contatoNovo) > 0;
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

		private string RetornarStringComparacaoContatos(IEnumerable<Contato> listaContatos)
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
