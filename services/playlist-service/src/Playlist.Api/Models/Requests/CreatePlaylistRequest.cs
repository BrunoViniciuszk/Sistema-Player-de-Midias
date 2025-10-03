namespace Playlist.Api.Models.Requests
{
    public class CreatePlaylistRequest
    {
        public string Nome { get; set; } = string.Empty;
    }

    public class UpdatePlaylistRequest
    {
        public string Nome { get; set; } = string.Empty;
    }
}
