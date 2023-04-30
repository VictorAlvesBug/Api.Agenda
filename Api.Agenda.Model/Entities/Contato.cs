using System.Text;
using System.Text.RegularExpressions;

namespace Api.Agenda.Model.Entities
{
	public class Contato
	{
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public int CodigoTipoContato { get; set; }
		public string? Valor { get; set; }
		public DateTime DataHoraCadastro { get; set; }
		public bool Ativo { get; set; }

		public Pessoa? Pessoa { get; set; }
		public TipoContato? TipoContato { get; set; }

		public bool EhValido(out string mensagemErro)
		{
			bool ehValido = true;
			StringBuilder sbMensagemErro = new StringBuilder();

			string mensagemErroCodigoTipoContato = string.Empty;
			ehValido = ehValido && ValidarCodigoTipoContato(out mensagemErroCodigoTipoContato);
			sbMensagemErro.Append(mensagemErroCodigoTipoContato);

			string mensagemErroValor = string.Empty;
			ehValido = ehValido && ValidarValor(out mensagemErroValor);
			sbMensagemErro.Append(mensagemErroValor);

			string mensagemErroPessoa = string.Empty;
			ehValido = ehValido && (Pessoa == null || Pessoa.EhValida(out mensagemErroPessoa));
			sbMensagemErro.Append(mensagemErroPessoa);

			string mensagemErroTipoContato = string.Empty;
			ehValido = ehValido && (TipoContato == null || TipoContato.EhValido(out mensagemErroTipoContato));
			sbMensagemErro.Append(mensagemErroTipoContato);

			mensagemErro = sbMensagemErro.ToString();

			return ehValido;
		}

		private bool ValidarCodigoTipoContato(out string mensagemErro)
		{
			bool ehValido = CodigoTipoContato > 0;

			mensagemErro = string.Empty;

			if (!ehValido)
			{
				mensagemErro = "Informe um codigoTipoContato válido para o contato.\n";
			}

			return ehValido;
		}

		private bool ValidarValor(out string mensagemErro)
		{
			bool ehValido = true;
			StringBuilder sbMensagemErro = new StringBuilder();

			ehValido = ehValido && !string.IsNullOrEmpty(Valor);
			ehValido = ehValido && Valor.Length >= 1;
			ehValido = ehValido && Valor.Length <= 100;

			if (!ehValido)
			{
				sbMensagemErro.Append("O valor do contato deve ter de 1 até 100 caracteres.\n");
			}
			else if (TipoContato != null
				&& TipoContato.RegexValidacao != null
				&& TipoContato.EhValido(out var _))
			{
				string regexValidacao = TipoContato.RegexValidacao;
				ehValido = ehValido && Regex.IsMatch(Valor, regexValidacao);

				if (!ehValido)
				{
					sbMensagemErro.Append($"O valor deve seguir o formato de {TipoContato.Nome} (Regex: /{regexValidacao}/)");
				}
			}

			mensagemErro = sbMensagemErro.ToString();

			return ehValido;
		}
	}
}
