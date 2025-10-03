using AutoMapper;
using Playlist.Application.Dtos;
using Playlist.Application.Interfaces;
using Playlist.Domain.Entities;

namespace Playlist.Application.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PlaylistService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PlaylistDto>> GetAllAsync(CancellationToken ct = default)
        {
            var playlists = await _uow.Playlists.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<PlaylistDto>>(playlists);
        }

        public async Task<PlaylistDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var playlist = await _uow.Playlists.GetByIdAsync(id, ct);
            return playlist == null ? null : _mapper.Map<PlaylistDto>(playlist);
        }

        public async Task<PlaylistDto> CreateAsync(string nome, CancellationToken ct = default)
        {
            var playlist = new PlaylistEntity(nome);
            await _uow.Playlists.CreateAsync(playlist, ct);
            await _uow.CommitAsync(ct);
            return _mapper.Map<PlaylistDto>(playlist);
        }

        public async Task<PlaylistDto?> UpdateAsync(int id, string nome, CancellationToken ct = default)
        {
            var playlist = await _uow.Playlists.GetByIdAsync(id, ct);
            if (playlist == null) return null;

            playlist.Renomear(nome);
           
            await _uow.Playlists.UpdateAsync(playlist, ct);
            await _uow.CommitAsync(ct);
            return _mapper.Map<PlaylistDto>(playlist);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var playlist = await _uow.Playlists.GetByIdAsync(id, ct);
            if (playlist == null) return false;

            await _uow.Playlists.DeleteAsync(playlist, ct);
            await _uow.CommitAsync(ct);
            return true;
        }

        public async Task<bool> AddMidiaAsync(int playlistId, int midiaId, bool exibirNoPlayer = true, CancellationToken ct = default)
        {
            var playlist = await _uow.Playlists.GetByIdAsync(playlistId, ct);
            if (playlist == null) return false;

            var added = playlist.AdicionarMidia(midiaId, exibirNoPlayer);
            if (!added) return false;

            await _uow.Playlists.UpdateAsync(playlist, ct);
            await _uow.CommitAsync(ct);
            return true;
        }

        public async Task<bool> RemoveMidiaAsync(int playlistId, int midiaId, CancellationToken ct = default)
        {
            var playlist = await _uow.Playlists.GetByIdAsync(playlistId, ct);
            if (playlist == null) return false;

            var removedFromAggregate = playlist.RemoverMidia(midiaId);
            if (!removedFromAggregate) return false;

            await _uow.Playlists.UpdateAsync(playlist, ct);
            await _uow.CommitAsync(ct);
            return true;
        }

        public async Task<bool> UpdateExibirNoPlayerAsync(int playlistId, int midiaId, bool exibirNoPlayer, CancellationToken ct = default)
        {
            var playlist = await _uow.Playlists.GetByIdAsync(playlistId, ct);
            if (playlist == null) return false;

            var updatedInAggregate = playlist.AtualizarExibirNoPlayer(midiaId, exibirNoPlayer);
            if (!updatedInAggregate) return false;

            await _uow.Playlists.UpdateAsync(playlist, ct);
            await _uow.CommitAsync(ct);
            return true;
        }
    }
}
