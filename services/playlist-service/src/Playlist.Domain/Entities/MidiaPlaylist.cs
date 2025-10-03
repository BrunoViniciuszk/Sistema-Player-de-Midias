namespace Playlist.Domain.Entities
{
    public class MidiaPlaylist
    {
        public int PlaylistId { get; private set; }
        public int MidiaId { get; private set; }
        public bool ExibirNoPlayer { get; private set; }

        public PlaylistEntity? Playlist { get; private set; }

        protected MidiaPlaylist() { } 

        public MidiaPlaylist(int playlistId, int midiaId, bool exibirNoPlayer = true)
        {
            PlaylistId = playlistId;
            MidiaId = midiaId;
            ExibirNoPlayer = exibirNoPlayer;
        }

        public void AtualizarExibicao(bool exibirNoPlayer)
        {
            ExibirNoPlayer = exibirNoPlayer;
        }
    }
}