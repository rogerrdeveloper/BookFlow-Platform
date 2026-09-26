using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditoraController : ControllerBase
    {
        private readonly IEditoraService _editoraService;

        public EditoraController(IEditoraService editoraService)
        {
            _editoraService = editoraService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<EditoraResponseDto>> Adicionar(EditoraRequestDto editoraDto)
        {
            try
            {
                var editora = await _editoraService.AdicionarAsync(editoraDto);
                return CreatedAtAction(nameof(BuscarPorId), new { id = editora.IdEditora }, editora);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [Authorize(Roles = "Editora")]
        [HttpGet("{id}")]
        public async Task<ActionResult<EditoraResponseDto>> BuscarPorId(int id)
        {
            // Editora só pode ver a si mesma
            var idEditoraToken = int.Parse(User.FindFirst("idEditora")!.Value);

            if (id != idEditoraToken)
                return Forbid();

            var editora = await _editoraService.BuscarPorIdAsync(id);

            if (editora is null)
                return NotFound();

            return Ok(editora);
        }

        [Authorize(Roles = "Editora")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, EditoraRequestDto editoraDto)
        {
            // Editora só pode editar a si mesma
            var idEditoraToken = int.Parse(User.FindFirst("idEditora")!.Value);

            if (id != idEditoraToken)
                return Forbid();

            try
            {
                await _editoraService.AtualizarAsync(id, editoraDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}