using BCrypt.Net;
using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Exceptions;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;

namespace EmprestimoLibrary.Services
{
    public class EditoraService : IEditoraService
    {
        private readonly IEditoraRepository _editoraRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ICarteiraRepository _carteiraRepository;

        public EditoraService(
            IEditoraRepository editoraRepository,
            IClienteRepository clienteRepository,
            ICarteiraRepository carteiraRepository)
        {
            _editoraRepository = editoraRepository;
            _clienteRepository = clienteRepository;
            _carteiraRepository = carteiraRepository;
        }

        private async Task<EditoraResponseDto> MapearResponseDtoAsync(Editora editora)
        {
            var carteira = await _carteiraRepository.BuscarPorEditoraIdAsync(editora.IdEditora);
            var saldo = carteira?.SaldoCarteira ?? 0m;

            return new EditoraResponseDto(
                editora.IdEditora,
                editora.NomeEditora,
                editora.EmailEditora,
                saldo
            );
        }

        public async Task<EditoraResponseDto> AdicionarAsync(EditoraRequestDto editoraDto)
        {
            var emailExistenteEditora = await _editoraRepository.ExistePorEmailAsync(editoraDto.EmailEditora);

            if (emailExistenteEditora)
                throw new BusinessException("Email já cadastrado como editora");

            // Verifica se já existe um Cliente com esse email para evitar conflito de carteiras
            var emailExistenteCliente = await _clienteRepository.ExistePorEmailAsync(editoraDto.EmailEditora);

            if (emailExistenteCliente)
                throw new BusinessException("Este email já está associado a uma conta de cliente. Use um email diferente para a editora.");

            var editora = new Editora
            {
                NomeEditora = editoraDto.NomeEditora,
                EmailEditora = editoraDto.EmailEditora,
                SenhaEditora = BCrypt.Net.BCrypt.HashPassword(editoraDto.SenhaEditora)
            };

            var editoraAdicionada = await _editoraRepository.AdicionarAsync(editora);

            // Criar carteira automaticamente para a nova editora
            var carteira = new Carteira
            {
                IdEditora = editoraAdicionada.IdEditora,
                SaldoCarteira = 0m
            };
            await _carteiraRepository.AdicionarAsync(carteira);

            return await MapearResponseDtoAsync(editoraAdicionada);
        }

        public async Task<EditoraResponseDto?> BuscarPorIdAsync(int id)
        {
            var editora = await _editoraRepository.BuscarPorIdAsync(id);

            if (editora is null)
                return null;

            return await MapearResponseDtoAsync(editora);
        }

        public async Task AtualizarAsync(int id, EditoraRequestDto editoraDto)
        {
            var editora = await _editoraRepository.BuscarPorIdAsync(id);

            if (editora is null)
                throw new BusinessException("Editora não encontrada");

            var emailExistente = await _editoraRepository
                .ExistePorEmailAsync(editoraDto.EmailEditora, id);

            if (emailExistente)
                throw new BusinessException("Email já cadastrado para outra editora");

            editora.NomeEditora = editoraDto.NomeEditora;
            editora.EmailEditora = editoraDto.EmailEditora;

            // Senha só atualiza se uma nova for enviada
            if (!string.IsNullOrWhiteSpace(editoraDto.SenhaEditora))
                editora.SenhaEditora = BCrypt.Net.BCrypt.HashPassword(editoraDto.SenhaEditora);

            await _editoraRepository.AtualizarAsync(editora);
        }
    }
}