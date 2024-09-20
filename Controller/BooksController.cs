using System.Threading.Tasks;
using AiHackLibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiHackLibraryApi.Controllers
{
    /// <summary>
    /// Controlador para gerenciar a busca de livros utilizando a Google Books API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly GoogleBooksService _googleBooksService;

        /// <summary>
        /// Construtor para injetar o serviço de Google Books.
        /// </summary>
        /// <param name="googleBooksService">O serviço de busca de livros na Google Books API.</param>
        public BooksController(GoogleBooksService googleBooksService)
        {
            _googleBooksService = googleBooksService;
        }

        /// <summary>
        /// Busca um livro por título.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite que você busque informações sobre um livro utilizando seu título como critério de pesquisa.
        /// As informações retornadas incluem o título, os autores e a descrição do livro.
        /// </remarks>
        /// <param name="title">O título do livro que deseja buscar.</param>
        /// <returns>Retorna os detalhes do livro, incluindo título, autores e descrição.</returns>
        /// <response code="200">Livro encontrado com sucesso.</response>
        /// <response code="400">Erro na requisição ou título inválido.</response>
        /// <response code="404">Nenhum livro encontrado com o título fornecido.</response>
        [HttpGet("{title}")]
        public async Task<IActionResult> GetBookByTitle(string title)
        {
            try
            {
                var book = await _googleBooksService.GetBookByTitleAsync(title);
                if (book == null)
                {
                    return NotFound(new { message = "Nenhum livro encontrado para o título fornecido." });
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
