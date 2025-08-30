namespace api_dotnet.Models.Dtos
{
    public class PlaylistDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<MidiaDto> Midias { get; set; } = new();
    }

    public class PlaylistListDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
