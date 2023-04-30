using System.Drawing;
using System.Text;

namespace Api.Agenda.Model.Entities
{
	public class Pessoa
	{
        public int Codigo { get; set; }
        public string? Nome { get; set; }
        public DateTime? DataHoraCadastro { get; set; }
        public bool Ativo { get; set; }

        public List<Contato> ListaContatos { get; set; }

        public bool EhValida(out string mensagemErro)
		{
			bool ehValido = true;
			StringBuilder sbMensagemErro = new StringBuilder();

			string mensagemErroNome = string.Empty;
			ehValido = ehValido && ValidarNome(out mensagemErroNome);
			sbMensagemErro.Append(mensagemErroNome);

			string mensagemErroListaContatos = string.Empty;
			ehValido = ehValido && ValidarListaContatos(out mensagemErroListaContatos);
			sbMensagemErro.Append(mensagemErroListaContatos);

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
				mensagemErro = "O nome da pessoa deve ter de 3 até 100 caracteres.\n";

			return ehValido;
		}

		private bool ValidarListaContatos(out string mensagemErro)
        {
            bool ehValido = true;
			StringBuilder sbMensagemErro = new StringBuilder();

			this.ListaContatos.ForEach((contato) => {
				string mensagemErroContato = string.Empty;
				ehValido = ehValido && contato.EhValido(out mensagemErroContato);
				sbMensagemErro.Append(mensagemErroContato);
			});

			mensagemErro = sbMensagemErro.ToString();

			return ehValido;
		}
	}
}
