namespace AiHackLibraryApi.Models
{
    public class BookModel
    {
        public string Title { get; set; } = string.Empty;
        public string Authors { get; set; } = "Autor desconhecido";
        public string Description { get; set; } = "Sem descrição disponível";

        // Construtor do BookModel que exige strings não nulas
        public BookModel(string title, string authors, string description)
        {
            Title = title ?? "Título não encontrado";
            Authors = authors ?? "Autor desconhecido";
            Description = description ?? "Sem descrição disponível";
        }
    }
}