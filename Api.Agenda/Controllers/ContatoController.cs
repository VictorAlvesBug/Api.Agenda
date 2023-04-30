using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Agenda.Controllers
{
	[Route("api/pessoas")]
	[ApiController]
	public class ContatoController : ControllerBase
	{
		private readonly IContatoService _contatoService;
		public ContatoController(IContatoService contatoService)
		{
			_contatoService = contatoService;

		}

		[HttpGet("{codigoPessoa}/contatos")]
		public async Task<IActionResult> Listar(int codigoPessoa)
		{
			try
			{
				List<Contato> listaContatos = (await _contatoService.Listar(codigoPessoa)).ToList();

				if (listaContatos == null)
				{
					return NotFound();
				}

				var links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Href = $"/api/pessoas/{codigoPessoa}/contatos",
						Method = "GET"
					}
				};

				listaContatos.ForEach(contato =>
				{
					links.Add(new Link
					{
						Rel = "related",
						Href = $"/api/pessoas/{codigoPessoa}/contatos/{contato.Codigo}",
						Method = "GET"
					});
				});

				var retorno = new Recurso<List<Contato>>(listaContatos, links);

				return Ok(retorno);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{codigoPessoa}/contatos/{codigoContato}")]
		public async Task<IActionResult> Retornar(int codigoPessoa, int codigoContato)
		{
			try
			{
				Contato contato = await _contatoService.Retornar(codigoContato);

				if (contato == null)
				{
					return NotFound();
				}

				var links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Href = $"/api/pessoas/{codigoPessoa}/contatos/{codigoContato}",
						Method = "GET"
					},
					new Link
					{
						Rel = "list",
						Href = $"/api/pessoas/{codigoPessoa}/contatos",
						Method = "GET"
					},
					new Link
					{
						Rel = "related",
						Href = $"/api/pessoas/{codigoPessoa}",
						Method = "GET"
					}
				};

				var retorno = new Recurso<Contato>(contato, links);

				return Ok(retorno);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost("{codigoPessoa}/contatos")]
		public async Task<IActionResult> Cadastrar(int codigoPessoa, [FromBody] Contato contato)
		{
			try
			{
				contato.CodigoPessoa = codigoPessoa;

				if (await _contatoService.Cadastrar(contato))
				{
					var links = new List<Link>
					{
						new Link
						{
							Rel = "self",
							Href = $"/api/pessoas/{codigoPessoa}/contatos/{contato.Codigo}",
							Method = "GET"
						},
						new Link
						{
							Rel = "list",
							Href = $"/api/pessoas/{codigoPessoa}/contatos",
							Method = "GET"
						},
						new Link
						{
							Rel = "related",
							Href = $"/api/pessoas/{codigoPessoa}",
							Method = "GET"
						}
					};

					var retorno = new Recurso<Contato>(contato, links);

					return Created($"/api/pessoas/{codigoPessoa}/contatos/{contato.Codigo}", retorno);
				}

				return NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut("{codigoPessoa}/contatos/{codigoContato}")]
		public async Task<IActionResult> Alterar(int codigoPessoa, int codigoContato, [FromBody] Contato contato)
		{
			try
			{
				if (await _contatoService.Alterar(codigoContato, contato))
				{
					var links = new List<Link>
					{
						new Link
						{
							Rel = "self",
							Href = $"/api/pessoas/{codigoPessoa}/contatos/{codigoContato}",
							Method = "GET"
						},
						new Link
						{
							Rel = "list",
							Href = $"/api/pessoas/{codigoPessoa}/contatos",
							Method = "GET"
						},
						new Link
						{
							Rel = "related",
							Href = $"/api/pessoas/{codigoPessoa}",
							Method = "GET"
						}
					};

					var retorno = new Recurso<Contato>(contato, links);

					return Ok(retorno);
				}

				return NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{codigoPessoa}/contatos/{codigoContato}")]
		public async Task<IActionResult> Desativar(int codigoContato)
		{
			try
			{
				if (await _contatoService.Desativar(codigoContato))
				{
					return NoContent();
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
