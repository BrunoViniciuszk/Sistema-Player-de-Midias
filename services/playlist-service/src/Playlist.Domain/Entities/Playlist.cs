namespace Playlist.Domain.Entities
{
    public class PlaylistEntity
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;

        private readonly List<MidiaPlaylist> _midias = new();
        public IReadOnlyCollection<MidiaPlaylist> Midias => _midias.AsReadOnly();

        protected PlaylistEntity() { } 

        public PlaylistEntity(string nome)
        {
            Renomear(nome);
        }

        public void Renomear(string novoNome)
        {
            if (string.IsNullOrWhiteSpace(novoNome))
                throw new ArgumentException("Nome inválido", nameof(novoNome));
            Nome = novoNome.Trim();
        }

        public bool AdicionarMidia(int midiaId, bool exibirNoPlayer = true)
        {
            if (_midias.Any(m => m.MidiaId == midiaId))
                return false;

            _midias.Add(new MidiaPlaylist(Id, midiaId, exibirNoPlayer));
            return true;
        }

        public bool RemoverMidia(int midiaId)
        {
            var midia = _midias.FirstOrDefault(m => m.MidiaId == midiaId);
            if (midia is null) return false;

            _midias.Remove(midia);
            return true;
        }

        public bool AtualizarExibirNoPlayer(int midiaId, bool exibirNoPlayer)
        {
            var midia = _midias.FirstOrDefault(m => m.MidiaId == midiaId);
            if (midia is null) return false;

            midia.AtualizarExibicao(exibirNoPlayer);
            return true;
        }
    }
}