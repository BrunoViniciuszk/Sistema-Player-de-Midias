using api_dotnet.Data;
using api_dotnet.Models;
using api_dotnet.Models.Dtos;
using api_dotnet.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Services.Playlists
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMapper _mapper;

        public PlaylistService(IPlaylistRepository playlistRepository, IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlaylistDto>> GetAllAsync()
        {
            var playlists = await _playlistRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PlaylistDto>>(playlists);
        }

        public async Task<PlaylistDto> GetByIdAsync(int id)
        {
            var playlist = await _playlistRepository.GetByIdAsync(id);
            if (playlist == null) throw new KeyNotFoundException("Playlist não encontrada");

            return _mapper.Map<PlaylistDto>(playlist);
        }


        public async Task<PlaylistNomeDto> UpdateAsync(int id, string novoNome)
        {
            var playlist = await _playlistRepository.GetByIdAsync(id);
            if (playlist == null)
                throw new KeyNotFoundException("Playlist não encontrada");

            playlist.Nome = novoNome;
            await _playlistRepository.SaveChangesAsync();

            return _mapper.Map<PlaylistNomeDto>(playlist);
        }

        public async Task<PlaylistDto> CreateAsync(Playlist playlist)
        {
            var created = await _playlistRepository.CreateAsync(playlist);
            return _mapper.Map<PlaylistDto>(created);
        }

        public async Task DeleteAsync(int id)
        {
            var playlist = await _playlistRepository.GetByIdAsync(id);
            if (playlist == null) throw new KeyNotFoundException("Playlist não encontrada");

            await _playlistRepository.DeleteAsync(playlist);
        }

        public async Task AddMidiaAsync(int playlistId, int midiaId, bool exibirNoPlayer = true)
        {
            var existing = await _playlistRepository.GetMidiaPlaylistAsync(playlistId, midiaId);
            if (existing != null) throw new InvalidOperationException("Mídia já existe na playlist");

            var midiaPlaylist = new MidiaPlaylist
            {
                PlaylistId = playlistId,
                MidiaId = midiaId,
                ExibirNoPlayer = exibirNoPlayer
            };

            await _playlistRepository.AddMidiaPlaylistAsync(midiaPlaylist);
        }

        public async Task RemoveMidiaAsync(int playlistId, int midiaId)
        {
            var midiaPlaylist = await _playlistRepository.GetMidiaPlaylistAsync(playlistId, midiaId);
            if (midiaPlaylist == null) throw new KeyNotFoundException("Mídia não encontrada na playlist");

            await _playlistRepository.RemoveMidiaPlaylistAsync(midiaPlaylist);
        }

        public async Task UpdateExibirNoPlayer(int playlistId, int midiaId, bool exibirNoPlayer)
        {
            var midiaPlaylist = await _playlistRepository.GetMidiaPlaylistAsync(playlistId, midiaId);
            if (midiaPlaylist == null) throw new KeyNotFoundException("Mídia não encontrada na playlist");

            midiaPlaylist.ExibirNoPlayer = exibirNoPlayer;
            await _playlistRepository.SaveChangesAsync();
        }
    }

}
