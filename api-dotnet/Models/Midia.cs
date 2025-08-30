namespace api_dotnet.Models
{
    public class Midia
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string UrlMidia{ get; set; } = string.Empty;

        public List<MidiaPlaylist> Playlists { get; set; } = new();
    }
}
