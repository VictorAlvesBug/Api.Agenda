using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.Model.Entities
{
	public class TipoContato
	{
		public int Codigo { get; set; }
		public string Nome { get; set; }
		public string RegexValidacao { get; set; }
		public DateTime DataHoraCadastro { get; set; }
		public bool Ativo { get; set; }
	}
}
