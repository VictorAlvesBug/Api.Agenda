using System.Text;
using System.Text.RegularExpressions;

namespace Api.Agenda.Model.Entities
{
	public class TipoContato
	{
		public int Codigo { get; set; }
		public string? Nome { get; set; }
		public string? RegexValidacao { get; set; }
		public DateTime? DataHoraCadastro { get; set; }
		public bool Ativo { get; set; }

		public bool EhValido(out string mensagemErro)
		{
			bool ehValido = true;
			StringBuilder sbMensagemErro = new StringBuilder();

			string mensagemErroNome = string.Empty;
			ehValido = ehValido && ValidarNome(out mensagemErroNome);
			sbMensagemErro.Append(mensagemErroNome);

			string mensagemErroRegexValidacao = string.Empty;
			ehValido = ehValido && ValidarRegexValidacao(out mensagemErroRegexValidacao);
			sbMensagemErro.Append(mensagemErroRegexValidacao);

			mensagemErro = sbMensagemErro.ToString();

			return ehValido;
		}

		private bool ValidarNome(out string mensagemErro)
		{
			bool ehValido = true;
			ehValido = ehValido && !string.IsNullOrEmpty(Nome);
			ehValido = ehValido && Nome.Length > 3;
			ehValido = ehValido && Nome.Length <= 100;

			mensagemErro = string.Empty;

			if (!ehValido)
				mensagemErro = "Informe um nome válido para o tipo de contato.\n";

			return ehValido;
		}

		private bool ValidarRegexValidacao(out string mensagemErro)
		{
			bool ehValido = true;
			ehValido = ehValido && (string.IsNullOrEmpty(RegexValidacao)
				|| EhRegexValido(RegexValidacao ?? ""));

			mensagemErro = string.Empty;

			if (!ehValido)
				mensagemErro = "Informe um regex de validação válido para o tipo de contato.\n";

			return ehValido;
		}

		private static bool EhRegexValido(string pattern)
		{
			try
			{
				Regex.Match("", pattern);
			}
			catch (ArgumentException)
			{
				return false;
			}

			return true;
		}
	}
}
