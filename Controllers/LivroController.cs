using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLibrary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public LivroController(ILivroService livroService)
        {
            _livroService = livroService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginaResponseDto<LivroResponseDto>>> BuscarTodos(
            [FromQuery] PaginacaoRequestDto paginacao)
        {
            var resultado = await _livroService.BuscarTodosAsync(paginacao);
            return Ok(resultado);
        }

        [Authorize(Roles = "Editora")]
        [HttpGet("editora/{idEditora}")]
        public async Task<ActionResult<PaginaResponseDto<LivroResponseDto>>> BuscarPorEditora(
            int idEditora,
            [FromQuery] PaginacaoRequestDto paginacao)
        {
            var idToken = int.Parse(User.FindFirst("idEditora")!.Value);
            
            if (idEditora != idToken)
                return Forbid();

            var resultado = await _livroService.BuscarPorEditoraAsync(idEditora, paginacao);
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<LivroResponseDto>> BuscarPorId(int id)
        {
            var livro = await _livroService.BuscarPorIdAsync(id);

            if (livro is null)
                return NotFound();

            return Ok(livro);
        }

        [Authorize(Roles = "Editora")]
        [HttpPost]
        public async Task<ActionResult<LivroResponseDto>> Adicionar(LivroRequestDto livroDto)
        {
            var idEditora = int.Parse(User.FindFirst("idEditora")!.Value);

            var livro = await _livroService.AdicionarAsync(livroDto, idEditora);

            return CreatedAtAction(nameof(BuscarPorId), new { id = livro.IdLivro }, livro);
        }

        [Authorize(Roles = "Editora")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, LivroRequestDto livroDto)
        {
            var idEditora = int.Parse(User.FindFirst("idEditora")!.Value);

            try
            {
                await _livroService.AtualizarAsync(id, livroDto, idEditora);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [Authorize(Roles = "Editora")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var idEditora = int.Parse(User.FindFirst("idEditora")!.Value);

            try
            {
                await _livroService.ExcluirAsync(id, idEditora);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}