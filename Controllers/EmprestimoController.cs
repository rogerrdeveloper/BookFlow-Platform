using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLibrary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ControllerBase
    {
        private readonly IEmprestimoService _emprestimoService;

        public EmprestimoController(IEmprestimoService emprestimoService)
        {
            _emprestimoService = emprestimoService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginaResponseDto<EmprestimoResponseDto>>> BuscarTodos(
            [FromQuery] PaginacaoRequestDto paginacao)
        {
            var emprestimos = await _emprestimoService.BuscarTodosAsync(paginacao);
            return Ok(emprestimos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmprestimoResponseDto>> BuscarPorId(int id)
        {
            var emprestimo = await _emprestimoService.BuscarPorIdAsync(id);

            if (emprestimo is null)
                return NotFound();

            return Ok(emprestimo);
        }

        [HttpPost]
        public async Task<ActionResult<EmprestimoResponseDto>> Adicionar(EmprestimoRequestDto emprestimoDto)
        {
            var emprestimo = await _emprestimoService.AdicionarAsync(emprestimoDto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = emprestimo.IdEmprestimo }, emprestimo);
        }

        [HttpPut("{id}/devolver")]
        public async Task<IActionResult> Devolver(int id)
        {
            await _emprestimoService.DevolverAsync(id);
            return NoContent();
        }

        [HttpGet("ativos")]
        public async Task<ActionResult<PaginaResponseDto<EmprestimoResponseDto>>> BuscarAtivos(
            [FromQuery] PaginacaoRequestDto paginacao)
        {
            var emprestimos = await _emprestimoService.BuscarAtivosAsync(paginacao);
            return Ok(emprestimos);
        }

        [HttpGet("inativos")]
        public async Task<ActionResult<PaginaResponseDto<EmprestimoResponseDto>>> BuscarInativos(
            [FromQuery] PaginacaoRequestDto paginacao)
        {
            var emprestimos = await _emprestimoService.BuscarInativosAsync(paginacao);
            return Ok(emprestimos);
        }

        [HttpGet("atrasados")]
        public async Task<ActionResult<PaginaResponseDto<EmprestimoResponseDto>>> BuscarAtrasados(
            [FromQuery] PaginacaoRequestDto paginacao)
        {
            var emprestimos = await _emprestimoService.BuscarAtrasadosAsync(paginacao);
            return Ok(emprestimos);
        }
    }
}