using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.Model.Entities
{
	public class Recurso<T>
	{
		public T Data { get; set; }
		public List<Link> Links { get; set; }

		public Recurso(T data, List<Link> links)
		{
			Data = data;
			Links = links;
		}
	}
}
