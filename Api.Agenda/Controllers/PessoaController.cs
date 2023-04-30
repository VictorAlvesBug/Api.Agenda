using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Api.Agenda.Controllers
{
	[Route("api/pessoas")]
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
			try
			{
				List<Pessoa> listaPessoas = (await _pessoaService.Listar()).ToList();

				if (listaPessoas == null)
				{
					return NotFound();
				}

				var links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Href = $"/api/pessoas",
						Method = "GET"
					}
				};

				listaPessoas.ForEach(pessoa =>
				{
					links.Add(new Link
					{
						Rel = "related",
						Href = $"/api/pessoas/{pessoa.Codigo}",
						Method = "GET"
					});

					links.Add(new Link
					{
						Rel = "collection",
						Href = $"/api/pessoas/{pessoa.Codigo}/contatos",
						Method = "GET"
					});
				});

				var retorno = new Recurso<List<Pessoa>>(listaPessoas, links);

				return Ok(retorno);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{codigo}")]
		public async Task<IActionResult> Retornar(int codigo)
		{
			try
			{
				Pessoa pessoa = await _pessoaService.Retornar(codigo);

				if (pessoa == null)
				{
					return NotFound();
				}

				var links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Href = $"/api/pessoas/{codigo}",
						Method = "GET"
					},
					new Link
					{
						Rel = "collection",
						Href = $"/api/pessoas/{codigo}/contatos",
						Method = "GET"
					},
					new Link
					{
						Rel = "list",
						Href = $"/api/pessoas",
						Method = "GET"
					}
				};

				var retorno = new Recurso<Pessoa>(pessoa, links);

				return Ok(retorno);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Cadastrar([FromBody] Pessoa pessoa)
		{
			try
			{
				if (await _pessoaService.Cadastrar(pessoa))
				{
					var links = new List<Link>
					{
						new Link
						{
							Rel = "self",
							Href = $"/api/pessoas/{pessoa.Codigo}",
							Method = "GET"
						},
						new Link
						{
							Rel = "collection",
							Href = $"/api/pessoas/{pessoa.Codigo}/contatos",
							Method = "GET"
						},
						new Link
						{
							Rel = "list",
							Href = $"/api/pessoas",
							Method = "GET"
						}
					};

					var retorno = new Recurso<Pessoa>(pessoa, links);

					return Created($"/api/pessoas/{pessoa.Codigo}", retorno);
				}

				return NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut("{codigo}")]
		public async Task<IActionResult> Alterar(int codigo, [FromBody] Pessoa pessoa)
		{
			try
			{
				if (await _pessoaService.Alterar(codigo, pessoa))
				{
					var links = new List<Link>
					{
						new Link
						{
							Rel = "self",
							Href = $"/api/pessoas/{pessoa.Codigo}",
							Method = "GET"
						},
						new Link
						{
							Rel = "collection",
							Href = $"/api/pessoas/{pessoa.Codigo}/contatos",
							Method = "GET"
						},
						new Link
						{
							Rel = "list",
							Href = $"/api/pessoas",
							Method = "GET"
						}
					};

					var retorno = new Recurso<Pessoa>(pessoa, links);

					return Ok(retorno);
				}

				return NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{codigo}")]
		public async Task<IActionResult> Desativar(int codigo)
		{
			try
			{
				if (await _pessoaService.Desativar(codigo))
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
