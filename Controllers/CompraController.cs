using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLibrary.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly ICompraService _compraService;

        public CompraController(ICompraService compraService)
        {
            _compraService = compraService;
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<ActionResult<PaginaResponseDto<CompraResponseDto>>> BuscarPorClienteId(
        int idCliente,
        [FromQuery] PaginacaoRequestDto paginacao)
        {
            var compras = await _compraService.BuscarPorClienteIdAsync(idCliente, paginacao);
            return Ok(compras);
        }
        [HttpGet("{idCompra}")]
        public async Task<ActionResult<CompraResponseDto>> BuscarPorId(int idCompra)
        {
            var compra = await _compraService.BuscarPorIdAsync(idCompra);

            if (compra is null)
            {
                return NotFound();
            }

            return Ok(compra);
        }

        [HttpPost]
        public async Task<ActionResult<CompraResponseDto>> Comprar(CompraRequestDto compraDto)
        {
            try
            {
                var compra = await _compraService.ComprarAsync(compraDto);
                return CreatedAtAction(nameof(BuscarPorId), new { idCompra = compra.IdCompra }, compra);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
