using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Models;

namespace EmprestimoLibrary.Services
{
    public interface IEditoraService
    {
        Task<EditoraResponseDto> AdicionarAsync(EditoraRequestDto editoraDto);
        Task<EditoraResponseDto?> BuscarPorIdAsync(int id);
        Task AtualizarAsync(int id, EditoraRequestDto editoraDto);
    }
}
