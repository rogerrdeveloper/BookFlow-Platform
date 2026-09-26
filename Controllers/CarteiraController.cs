using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLibrary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarteiraController : ControllerBase
    {
        private readonly ICarteiraService _carteiraService;

        public CarteiraController(ICarteiraService carteiraService)
        {
            _carteiraService = carteiraService;
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<CarteiraResponseDto>> BuscarPorClienteId(int idCliente)
        {
            var carteira = await _carteiraService.BuscarPorClienteIdAsync(idCliente);

            if (carteira is null)
                return NotFound();

            return Ok(carteira);
        }

        [HttpPost("{idCliente}/depositar")]
        public async Task<ActionResult<CarteiraResponseDto>> Depositar(int idCliente, CarteiraOperacaoRequestDto operacao)
        {
            var carteira = await _carteiraService.DepositarAsync(idCliente, operacao);
            return Ok(carteira);
        }

        [HttpPost("{idCliente}/sacar")]
        public async Task<ActionResult<CarteiraResponseDto>> Sacar(int idCliente, CarteiraOperacaoRequestDto operacao)
        {
            var carteira = await _carteiraService.SacarAsync(idCliente, operacao);
            return Ok(carteira);
        }

        [HttpGet("{idCliente}/extrato")]
        public async Task<ActionResult<PaginaResponseDto<MovimentacaoCarteiraResponseDto>>> ObterExtrato(
            int idCliente,
            [FromQuery] PaginacaoRequestDto paginacao)
        {
            var extrato = await _carteiraService.ObterExtratoAsync(idCliente, paginacao);
            return Ok(extrato);
        }

        [HttpPost("{idCliente}/cobrar-multas")]
        public async Task<IActionResult> CobrarMultasPendentes(int idCliente)
        {
            await _carteiraService.CobrarMultasPendentesAsync(idCliente);
            return NoContent();
        }

        // --- Endpoints exclusivos para Editora ---

        [HttpGet("editora/{idEditora}")]
        public async Task<ActionResult<CarteiraResponseDto>> BuscarPorEditoraId(int idEditora)
        {
            var carteira = await _carteiraService.BuscarPorEditoraIdAsync(idEditora);

            if (carteira is null)
                return NotFound();

            return Ok(carteira);
        }

        [HttpPost("editora/{idEditora}/sacar")]
        public async Task<ActionResult<CarteiraResponseDto>> SacarEditora(int idEditora, CarteiraOperacaoRequestDto operacao)
        {
            var carteira = await _carteiraService.SacarEditoraAsync(idEditora, operacao);
            return Ok(carteira);
        }

        [HttpGet("editora/{idEditora}/extrato")]
        public async Task<ActionResult<PaginaResponseDto<MovimentacaoCarteiraResponseDto>>> ObterExtratoEditora(
            int idEditora,
            [FromQuery] PaginacaoRequestDto paginacao)
        {
            var extrato = await _carteiraService.ObterExtratoEditoraAsync(idEditora, paginacao);
            return Ok(extrato);
        }
    }
}