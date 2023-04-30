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
		private readonly IUnitOfWork _unitOfWork;
		public PessoaController(
			IPessoaService pessoaService,
			IUnitOfWork unitOfWork)
		{
			_pessoaService = pessoaService;
			_unitOfWork = unitOfWork;
		}

		[HttpGet]
		public async Task<IActionResult> Listar()
		{
			try
			{
				_unitOfWork.BeginTransaction();

				List<Pessoa> listaPessoas = await _pessoaService.Listar();

				if (listaPessoas == null)
				{
					_unitOfWork.Rollback();
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

				_unitOfWork.Commit();
				return Ok(retorno);
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpGet("{codigoPessoa}")]
		public async Task<IActionResult> Retornar(int codigoPessoa)
		{
			try
			{
				_unitOfWork.BeginTransaction();

				Pessoa pessoa = await _pessoaService.Retornar(codigoPessoa);

				if (pessoa == null)
				{
					_unitOfWork.Rollback();
					return NotFound();
				}

				var links = new List<Link>
				{
					new Link
					{
						Rel = "self",
						Href = $"/api/pessoas/{codigoPessoa}",
						Method = "GET"
					},
					new Link
					{
						Rel = "collection",
						Href = $"/api/pessoas/{codigoPessoa}/contatos",
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

				_unitOfWork.Commit();
				return Ok(retorno);
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Cadastrar([FromBody] Pessoa pessoa)
		{
			try
			{
				_unitOfWork.BeginTransaction();

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

					_unitOfWork.Commit();
					return Created($"/api/pessoas/{pessoa.Codigo}", retorno);
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

		[HttpPut("{codigoPessoa}")]
		public async Task<IActionResult> Alterar(int codigoPessoa, [FromBody] Pessoa pessoa)
		{
			try
			{
				_unitOfWork.BeginTransaction();

				if (await _pessoaService.Alterar(codigoPessoa, pessoa))
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

		[HttpDelete("{codigoPessoa}")]
		public async Task<IActionResult> Desativar(int codigoPessoa)
		{
			try
			{
				_unitOfWork.BeginTransaction();

				if (await _pessoaService.Desativar(codigoPessoa))
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
