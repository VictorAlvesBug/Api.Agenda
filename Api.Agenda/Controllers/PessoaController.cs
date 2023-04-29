using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Agenda.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PessoaController : ControllerBase
	{
		private readonly IPessoaService _pessoaService;
		public PessoaController(IPessoaService pessoaService)
		{
			_pessoaService = pessoaService;

		}

		[HttpGet]
		public async Task<IActionResult> Listar()
		{
			//ListaParametros pessoa = new ListaParametros();
			IEnumerable<Pessoa> listaPessoas;
			try
			{
				listaPessoas = await _pessoaService.Listar();

				if (listaPessoas is not null)
				{
					return Ok(listaPessoas);
				}

				return NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
