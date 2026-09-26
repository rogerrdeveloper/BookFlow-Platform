using EmprestimoLibrary.DTOs;
using EmprestimoLibrary.Exceptions;
using EmprestimoLibrary.Models;
using EmprestimoLibrary.Repositories.interfaces;

namespace EmprestimoLibrary.Services
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _livroRepository;

        public LivroService(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }

        private LivroResponseDto MapearResponseDto(Livro livro)
        {
            return new LivroResponseDto(
                livro.IdLivro,
                livro.LivroTitulo,
                livro.LivroAutor,
                livro.LivroEditora,
                livro.LivroEdicao,
                livro.LivroQuantidade,
                livro.LivroPreco,
                livro.SituacaoLivro,
                livro.IsEmprestavel
            );
        }

        public async Task<PaginaResponseDto<LivroResponseDto>> BuscarTodosAsync(PaginacaoRequestDto paginacao)
        {
            var (livros, total) = await _livroRepository.BuscarTodosAsync(
                paginacao.Pagina,
                paginacao.TamanhoPagina,
                paginacao.Titulo
            );

            var dados = livros
                .Select(MapearResponseDto)
                .ToList();

            return new PaginaResponseDto<LivroResponseDto>(
                dados,
                paginacao.Pagina,
                paginacao.TamanhoPagina,
                total,
                (int)Math.Ceiling((double)total / paginacao.TamanhoPagina)
            );
        }

        public async Task<PaginaResponseDto<LivroResponseDto>> BuscarPorEditoraAsync(int idEditora, PaginacaoRequestDto paginacao)
        {
            var (livros, total) = await _livroRepository.BuscarPorEditoraAsync(
                idEditora,
                paginacao.Pagina,
                paginacao.TamanhoPagina
            );

            var dados = livros
                .Select(MapearResponseDto)
                .ToList();

            return new PaginaResponseDto<LivroResponseDto>(
                dados,
                paginacao.Pagina,
                paginacao.TamanhoPagina,
                total,
                (int)Math.Ceiling((double)total / paginacao.TamanhoPagina)
            );
        }

        public async Task<LivroResponseDto?> BuscarPorIdAsync(int id)
        {
            var livro = await _livroRepository.BuscarPorIdAsync(id);

            if (livro is null)
                return null;

            return MapearResponseDto(livro);
        }

        public async Task<LivroResponseDto> AdicionarAsync(LivroRequestDto livroDto, int idEditora)
        {
            var livro = new Livro
            {
                LivroTitulo = livroDto.LivroTitulo,
                LivroAutor = livroDto.LivroAutor,
                LivroEditora = livroDto.LivroEditora,
                LivroEdicao = livroDto.LivroEdicao,
                LivroQuantidade = livroDto.LivroQuantidade,
                LivroPreco = livroDto.LivroPreco,
                IsEmprestavel = livroDto.IsEmprestavel,
                IdEditora = idEditora // sempre do token, nunca do body
            };

            var livroAdicionado = await _livroRepository.AdicionarAsync(livro);

            return MapearResponseDto(livroAdicionado);
        }

        public async Task AtualizarAsync(int id, LivroRequestDto livroDto, int idEditora)
        {
            var livro = await _livroRepository.BuscarPorIdAsync(id);

            if (livro is null)
                throw new BusinessException("Livro não encontrado");

            // Editora só pode editar seus próprios livros
            if (livro.IdEditora != idEditora)
                throw new BusinessException("Você não tem permissão para editar este livro");

            livro.LivroTitulo = livroDto.LivroTitulo;
            livro.LivroAutor = livroDto.LivroAutor;
            livro.LivroEditora = livroDto.LivroEditora;
            livro.LivroEdicao = livroDto.LivroEdicao;
            livro.LivroQuantidade = livroDto.LivroQuantidade;
            livro.LivroPreco = livroDto.LivroPreco;
            livro.IsEmprestavel = livroDto.IsEmprestavel;

            await _livroRepository.AtualizarAsync(livro);
        }

        public async Task ExcluirAsync(int id, int idEditora)
        {
            var livro = await _livroRepository.BuscarPorIdAsync(id);

            if (livro is null)
                throw new BusinessException("Livro não encontrado");

            // Editora só pode excluir seus próprios livros
            if (livro.IdEditora != idEditora)
                throw new BusinessException("Você não tem permissão para excluir este livro");

            await _livroRepository.ExcluirAsync(livro);
        }
    }
}
