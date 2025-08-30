namespace api_dotnet.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<MidiaPlaylist> Midias { get; set; } = new();
    }

    public class MidiaPlaylist
    {
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = null!;

        public int MidiaId { get; set; }
        public Midia Midia { get; set; } = null!;
        
        public bool ExibirNoPlayer { get; set; } = true;

    }
}
