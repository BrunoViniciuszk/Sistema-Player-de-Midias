namespace api_dotnet.Models.Dtos
{
    public class MidiaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; }
        public string UrlMidia { get; set; } = string.Empty;
        public bool ExibirNoPlayer { get; set; }
    }

    public class CreateMidiaDto
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}
