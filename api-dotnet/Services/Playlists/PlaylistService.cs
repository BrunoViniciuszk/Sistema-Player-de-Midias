using api_dotnet.Models;
using api_dotnet.Models.Dtos;
using api_dotnet.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api_dotnet.Services.Playlists
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaylistService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlaylistDto>> GetAllAsync()
        {
            var playlists = await _unitOfWork.Playlists.GetAllAsync();
            return _mapper.Map<IEnumerable<PlaylistDto>>(playlists);
        }

        public async Task<PlaylistDto> GetByIdAsync(int id)
        {
            var playlist = await _unitOfWork.Playlists.GetByIdAsync(id);
            if (playlist == null) throw new KeyNotFoundException("Playlist não encontrada");

            return _mapper.Map<PlaylistDto>(playlist);
        }

        public async Task<PlaylistNomeDto> UpdateAsync(int id, string novoNome)
        {
            var playlist = await _unitOfWork.Playlists.GetByIdAsync(id);
            if (playlist == null)
                throw new KeyNotFoundException("Playlist não encontrada");

            playlist.Nome = novoNome;

            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlaylistNomeDto>(playlist);
        }

        public async Task<PlaylistDto> CreateAsync(Playlist playlist)
        {
            var created = await _unitOfWork.Playlists.CreateAsync(playlist);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<PlaylistDto>(created);
        }

        public async Task DeleteAsync(int id)
        {
            var playlist = await _unitOfWork.Playlists.GetByIdAsync(id);
            if (playlist == null) throw new KeyNotFoundException("Playlist não encontrada");

            await _unitOfWork.Playlists.DeleteAsync(playlist);
            await _unitOfWork.CommitAsync();
        }

        public async Task AddMidiaAsync(int playlistId, int midiaId, bool exibirNoPlayer = true)
        {
            var existing = await GetMidiaPlaylistOrThrow(playlistId, midiaId, throwIfExists: true);

            var midiaPlaylist = new MidiaPlaylist
            {
                PlaylistId = playlistId,
                MidiaId = midiaId,
                ExibirNoPlayer = exibirNoPlayer
            };

            await _unitOfWork.Playlists.AddMidiaPlaylistAsync(midiaPlaylist);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveMidiaAsync(int playlistId, int midiaId)
        {
            var midiaPlaylist = await GetMidiaPlaylistOrThrow(playlistId, midiaId);
            await _unitOfWork.Playlists.RemoveMidiaPlaylistAsync(midiaPlaylist);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateExibirNoPlayer(int playlistId, int midiaId, bool exibirNoPlayer)
        {
            var midiaPlaylist = await GetMidiaPlaylistOrThrow(playlistId, midiaId);
            midiaPlaylist.ExibirNoPlayer = exibirNoPlayer;

            await _unitOfWork.CommitAsync();
        }

        private async Task<MidiaPlaylist> GetMidiaPlaylistOrThrow(int playlistId, int midiaId, bool throwIfExists = false)
        {
            var midiaPlaylist = await _unitOfWork.Playlists.GetMidiaPlaylistAsync(playlistId, midiaId);

            if (throwIfExists && midiaPlaylist != null)
                throw new InvalidOperationException("Mídia já existe na playlist");

            if (!throwIfExists && midiaPlaylist == null)
                throw new KeyNotFoundException("Mídia não encontrada na playlist");

            return midiaPlaylist;
        }
    }
}
