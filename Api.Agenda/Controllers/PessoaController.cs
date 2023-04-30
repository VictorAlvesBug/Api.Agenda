using Api.Agenda.Business.Services;
using Api.Agenda.Business.Services.Interfaces;
using Api.Agenda.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Agenda.Controllers
{
	[Route("api/pessoas")]
	[ApiController]
	public class PessoaController : ControllerBase
	{
		private readonly IPessoaService _pessoaService;
		private readonly ITipoContatoService _tipoContatoService;
		private readonly IUnitOfWork _unitOfWork;
		public PessoaController(
			IPessoaService pessoaService,
			ITipoContatoService tipoContatoService,
			IUnitOfWork unitOfWork)
		{
			_pessoaService = pessoaService;
			_tipoContatoService = tipoContatoService;
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
					return NotFound(new { erro = "Lista de pessoas não encontrada" });
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
					return NotFound(new { erro = "Pessoa não encontrada" });
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

				foreach(var contato in pessoa.ListaContatos)
				{
					var tipoContato = await _tipoContatoService.Retornar(contato.CodigoTipoContato);

					if (tipoContato == null)
					{
						_unitOfWork.Rollback();
						return NotFound(new { erro = "Tipo de contato não encontrado" });
					}
					contato.TipoContato = tipoContato;
					contato.CodigoPessoa = pessoa.Codigo;
				}

				if (!pessoa.EhValida(out string mensagemErro))
				{
					_unitOfWork.Rollback();
					return BadRequest(new { erro = mensagemErro });
				}

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
				return BadRequest(new { erro = "Erro ao cadastrar pessoa" });
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

				if (await _pessoaService.Retornar(codigoPessoa) == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Pessoa não encontrada" });
				}

				pessoa.Codigo = codigoPessoa;

				foreach (var contato in pessoa.ListaContatos)
				{
					var tipoContato = await _tipoContatoService.Retornar(contato.CodigoTipoContato);

					if (tipoContato == null)
					{
						_unitOfWork.Rollback();
						return NotFound(new { erro = "Tipo de contato não encontrado" });
					}
					contato.TipoContato = tipoContato;
					contato.CodigoPessoa = pessoa.Codigo;
				}

				if (!pessoa.EhValida(out string mensagemErro))
				{
					return BadRequest(new { erro = mensagemErro });
				}


				if (await _pessoaService.Alterar(pessoa))
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
				return BadRequest(new { erro = "Erro ao alterar pessoa" });
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

				if (await _pessoaService.Retornar(codigoPessoa) == null)
				{
					_unitOfWork.Rollback();
					return NotFound(new { erro = "Pessoa não encontrada" });
				}

				if (await _pessoaService.Desativar(codigoPessoa))
				{
					_unitOfWork.Commit();
					return NoContent();
				}

				_unitOfWork.Rollback();
				return BadRequest(new { erro = "Erro ao desativar pessoa" });
			}
			catch (Exception ex)
			{
				_unitOfWork.Rollback();
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
