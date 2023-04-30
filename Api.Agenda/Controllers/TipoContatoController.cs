using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Api.Agenda.Controllers
{
	[Route("api/tiposcontato")]
	[ApiController]
	public class TipoContatoController : ControllerBase
	{
		private readonly ITipoContatoService _tipoContatoService;
		public TipoContatoController(ITipoContatoService tipoContatoService)
		{
			_tipoContatoService = tipoContatoService;
		}

		[HttpGet]
		public async Task<IActionResult> Listar()
		{
			try
			{
				List<TipoContato> listaTiposContato = await _tipoContatoService.Listar();

				if (listaTiposContato == null)
				{
					return NotFound();
				}

				var links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Href = $"/api/tiposcontato",
						Method = "GET"
					}
				};

				listaTiposContato.ForEach(tipoContato =>
				{
					links.Add(new Link
					{
						Rel = "related",
						Href = $"/api/tiposcontato/{tipoContato.Codigo}",
						Method = "GET"
					});
				});

				var retorno = new Recurso<List<TipoContato>>(listaTiposContato, links);

				return Ok(retorno);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{codigoTipoContato}")]
		public async Task<IActionResult> Retornar(int codigoTipoContato)
		{
			try
			{
				TipoContato tipoContato = await _tipoContatoService.Retornar(codigoTipoContato);

				if (tipoContato == null)
				{
					return NotFound();
				}

				var links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Href = $"/api/tiposcontato/{codigoTipoContato}",
						Method = "GET"
					},
					new Link
					{
						Rel = "list",
						Href = $"/api/tiposcontato",
						Method = "GET"
					}
				};

				var retorno = new Recurso<TipoContato>(tipoContato, links);

				return Ok(retorno);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Cadastrar([FromBody] TipoContato tipoContato)
		{
			try
			{
				if (await _tipoContatoService.Cadastrar(tipoContato))
				{
					var links = new List<Link>
					{
						new Link
						{
							Rel = "self",
							Href = $"/api/tiposcontato/{tipoContato.Codigo}",
							Method = "GET"
						},
						new Link
						{
							Rel = "list",
							Href = $"/api/tiposcontato",
							Method = "GET"
						}
					};

					var retorno = new Recurso<TipoContato>(tipoContato, links);

					return Created($"/api/tiposcontato/{tipoContato.Codigo}", retorno);
				}

				return NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut("{codigoTipoContato}")]
		public async Task<IActionResult> Alterar(int codigoTipoContato, [FromBody] TipoContato tipoContato)
		{
			try
			{
				if (await _tipoContatoService.Alterar(codigoTipoContato, tipoContato))
				{
					var links = new List<Link>
					{
						new Link
						{
							Rel = "self",
							Href = $"/api/tiposcontato/{tipoContato.Codigo}",
							Method = "GET"
						},
						new Link
						{
							Rel = "list",
							Href = $"/api/tiposcontato",
							Method = "GET"
						}
					};

					var retorno = new Recurso<TipoContato>(tipoContato, links);

					return Ok(retorno);
				}

				return NotFound();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{codigoTipoContato}")]
		public async Task<IActionResult> Desativar(int codigoTipoContato)
		{
			try
			{
				if (await _tipoContatoService.Desativar(codigoTipoContato))
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
