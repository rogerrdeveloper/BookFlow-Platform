using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLibrary.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginaResponseDto<ClienteResponseDto>>> BuscarTodos(
            [FromQuery] PaginacaoRequestDto paginacao)
        {
            var clientes = await _clienteService.BuscarTodosAsync(paginacao);
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteResponseDto>> BuscarPorId(int id)
        {
            var cliente = await _clienteService.BuscarPorIdAsync(id);

            if (cliente is null)
                return NotFound();

            return Ok(cliente);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ClienteResponseDto>> Adicionar(ClienteRequestDto clienteDto)
        {
            var cliente = await _clienteService.AdicionarAsync(clienteDto);
            return CreatedAtAction(nameof(BuscarPorId), new { id = cliente.IdCliente }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, ClienteAtualizarRequestDto clienteDto)
        {
            var cliente = await _clienteService.BuscarPorIdAsync(id);

            if (cliente is null)
                return NotFound();

            await _clienteService.AtualizarAsync(id, clienteDto);
            return NoContent();
        }

        [HttpPut("{id}/desativar")]
        public async Task<IActionResult> Desativar(int id)
        {
            var cliente = await _clienteService.BuscarPorIdAsync(id);

            if (cliente is null)
                return NotFound();

            await _clienteService.DesativarContaAsync(id);
            return NoContent();
        }
    }
}