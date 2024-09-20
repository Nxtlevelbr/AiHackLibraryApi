using System.Net.Http;
using System.Threading.Tasks;
using AiHackLibraryApi.Models;
using Newtonsoft.Json;

namespace AiHackLibraryApi.Services
{
    public class GoogleBooksService
    {
        private readonly HttpClient _httpClient;

        public GoogleBooksService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BookModel?> GetBookByTitleAsync(string title)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://www.googleapis.com/books/v1/volumes?q={title}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Falha ao se comunicar com a Google Books API.");
                }

                var content = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(content);

                if (result.items == null || result.items.Count == 0)
                {
                    throw new Exception("Nenhum livro encontrado para o título fornecido.");
                }

                var book = result.items[0]?.volumeInfo;

                if (book == null)
                {
                    throw new Exception("Erro ao processar as informações do livro.");
                }

                // Verificar e converter os valores para string, ou usar valor padrão
                string titleText = book.title != null ? (string)book.title : "Título não encontrado";
                string authorsText = book.authors != null ? string.Join(", ", book.authors.ToObject<string[]>()) : "Autor desconhecido";
                string descriptionText = book.description != null ? (string)book.description : "Sem descrição disponível";

                // Retornar o modelo do livro com os valores processados
                return new BookModel(
                    titleText,
                    authorsText,
                    descriptionText
                );
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Erro na comunicação com a API externa: " + ex.Message);
            }
            catch (JsonException ex)
            {
                throw new Exception("Erro ao processar a resposta da API: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao buscar o livro: " + ex.Message);
            }
        }
    }
}
