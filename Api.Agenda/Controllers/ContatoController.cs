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
		private readonly IUnitOfWork _unitOfWork;
		public ContatoController(
			IContatoService contatoService,
			IUnitOfWork unitOfWork)
		{
			_contatoService = contatoService;
			_unitOfWork = unitOfWork;
		}

		[HttpGet("{codigoPessoa}/contatos")]
		public async Task<IActionResult> Listar(int codigoPessoa)
		{
			try
			{
				_unitOfWork.BeginTransaction();

				List<Contato> listaContatos = await _contatoService.Listar(codigoPessoa);

				if (listaContatos == null)
				{
					_unitOfWork.Rollback();
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

				_unitOfWork.Commit();
				return Ok(retorno);
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{codigoPessoa}/contatos/{codigoContato}")]
		public async Task<IActionResult> Retornar(int codigoPessoa, int codigoContato)
		{
			try
			{
				_unitOfWork.BeginTransaction();

				Contato contato = await _contatoService.Retornar(codigoContato);

				if (contato == null)
				{
					_unitOfWork.Rollback();
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

				_unitOfWork.Commit();
				return Ok(retorno);
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost("{codigoPessoa}/contatos")]
		public async Task<IActionResult> Cadastrar(int codigoPessoa, [FromBody] Contato contato)
		{
			try
			{
				_unitOfWork.BeginTransaction();

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

					_unitOfWork.Commit();
					return Created($"/api/pessoas/{codigoPessoa}/contatos/{contato.Codigo}", retorno);
				}

				return NotFound();
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPut("{codigoPessoa}/contatos/{codigoContato}")]
		public async Task<IActionResult> Alterar(int codigoPessoa, int codigoContato, [FromBody] Contato contato)
		{
			try
			{
				_unitOfWork.BeginTransaction();

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

					_unitOfWork.Commit();
					return Ok(retorno);
				}

				_unitOfWork.Rollback();
				return NotFound();
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{codigoPessoa}/contatos/{codigoContato}")]
		public async Task<IActionResult> Desativar(int codigoContato)
		{
			try
			{
				_unitOfWork.BeginTransaction();

				if (await _contatoService.Desativar(codigoContato))
				{
					_unitOfWork.Commit();
					return NoContent();
				}

				_unitOfWork.Rollback();
				return NotFound();
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
