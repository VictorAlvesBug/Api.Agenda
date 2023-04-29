using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.Model.Entities
{
	public class Contato
	{
		public int Codigo { get; set; }
		public int CodigoPessoa { get; set; }
		public int CodigoTipoContato { get; set; }
		public string Valor { get; set; }
		public DateTime DataHoraCadastro { get; set; }
		public bool Ativo { get; set; }

		public Pessoa Pessoa { get; set; }
		public TipoContato TipoContato { get; set; }
	}
}
