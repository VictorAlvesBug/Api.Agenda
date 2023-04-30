using Api.Agenda.Business.Services;
using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Agenda.Controllers
{
	[Route("api/pessoas")]
	[ApiController]
	public class ContatoController : ControllerBase
	{
		private readonly IPessoaService _pessoaService;
		private readonly IContatoService _contatoService;
		private readonly ITipoContatoService _tipoContatoService;
		private readonly IUnitOfWork _unitOfWork;
		public ContatoController(
			IPessoaService pessoaService,
			IContatoService contatoService,
			ITipoContatoService tipoContatoService,
			IUnitOfWork unitOfWork)
		{
			_pessoaService = pessoaService;
			_contatoService = contatoService;
			_tipoContatoService = tipoContatoService;
			_unitOfWork = unitOfWork;
		}

		[HttpGet("{codigoPessoa}/contatos")]
		public async Task<IActionResult> Listar(int codigoPessoa)
		{
			try
			{
				_unitOfWork.BeginTransaction();

				if (await _pessoaService.Retornar(codigoPessoa) == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Pessoa não encontrada" });
				}

				List<Contato> listaContatos = await _contatoService.Listar(codigoPessoa);

				if (listaContatos == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Lista de contatos não encontrada" });
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

				if (await _pessoaService.Retornar(codigoPessoa) == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Pessoa não encontrada" });
				}

				Contato contato = await _contatoService.Retornar(codigoPessoa, codigoContato);

				if (contato == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Contato não encontrado" });
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

				if (await _pessoaService.Retornar(codigoPessoa) == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Pessoa não encontrada" });
				}

				var tipoContato = await _tipoContatoService.Retornar(contato.CodigoTipoContato);
				
				if (tipoContato == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Tipo de contato não encontrado" });
				}

				contato.TipoContato = tipoContato;
				contato.CodigoPessoa = codigoPessoa;

				if (!contato.EhValido(out string mensagemErro))
				{
					_unitOfWork.Rollback();
					return BadRequest(new { erro = mensagemErro });
				}

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

				_unitOfWork.Rollback();
				return BadRequest(new { erro = "Erro ao cadastrar contato" });
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

				if (await _pessoaService.Retornar(codigoPessoa) == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Pessoa não encontrada" });
				}

				if (await _contatoService.Retornar(codigoPessoa, codigoContato) == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Contato não encontrado" });
				}

				var tipoContato = await _tipoContatoService.Retornar(contato.CodigoTipoContato);

				if (tipoContato == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Tipo de contato não encontrado" });
				}

				contato.TipoContato = tipoContato;
				contato.Codigo = codigoContato;
				contato.CodigoPessoa = codigoPessoa;

				if (!contato.EhValido(out string mensagemErro))
				{
					_unitOfWork.Rollback();
					return BadRequest(new { erro = mensagemErro });
				}

				if (await _contatoService.Alterar(contato))
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
				return BadRequest(new { erro = "Erro ao alterar contato" });
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[HttpDelete("{codigoPessoa}/contatos/{codigoContato}")]
		public async Task<IActionResult> Desativar(int codigoPessoa, int codigoContato)
		{
			try
			{
				_unitOfWork.BeginTransaction();

				if (await _pessoaService.Retornar(codigoPessoa) == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Pessoa não encontrada" });
				}

				if (await _contatoService.Retornar(codigoPessoa, codigoContato) == null)
				{
					_unitOfWork.Commit();
					return NotFound(new { erro = "Contato não encontrado" });
				}

				if (await _contatoService.Desativar(codigoPessoa, codigoContato))
				{
					_unitOfWork.Commit();
					return NoContent();
				}

				_unitOfWork.Rollback();
				return BadRequest(new { erro = "Erro ao desativar contato" });
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
