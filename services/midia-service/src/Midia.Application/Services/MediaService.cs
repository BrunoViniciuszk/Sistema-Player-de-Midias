using AutoMapper;
using Midia.Application.Dtos;
using Midia.Application.Interfaces;
using Midia.Domain.Factories.Interfaces;
using Midia.Domain.Repositories;

public class MediaService : IMediaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediaFactory _midiaFactory;
    private readonly IStorageStrategy _storageStrategy;
    private readonly IMapper _mapper;

    private static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png" };
    private static readonly string[] AllowedVideoExtensions = { ".mp4", ".avi", ".mov" };

    public MediaService(
        IUnitOfWork unitOfWork,
        IMediaFactory midiaFactory,
        IStorageStrategy storageStrategy,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _midiaFactory = midiaFactory;
        _storageStrategy = storageStrategy;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MediaDto>> GetAllAsync()
    {
        var midias = await _unitOfWork.Medias.GetAllAsync();
        return _mapper.Map<IEnumerable<MediaDto>>(midias);
    }

    public async Task<MediaDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var media = await _unitOfWork.Medias.GetByIdAsync(id, cancellationToken);
        return media == null ? null : _mapper.Map<MediaDto>(media);
    }

    public async Task<MediaDto> UploadAndCreateMidiaAsync(UploadMediaDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.File == null || dto.File.Length == 0)
            throw new ArgumentException("Arquivo inválido");

        var fileName = GenerateUniqueFileName(dto.File.FileName);

        ValidateFile(dto.File.FileName);

        using var stream = dto.File.OpenReadStream();
        var url = await _storageStrategy.SaveAsync(stream, fileName, dto.File.ContentType);

        var media = _midiaFactory.Create(dto.Nome, dto.Descricao, url);

        var created = await _unitOfWork.Medias.CreateAsync(media, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<MediaDto>(created);
    }

    public async Task<MediaDto> UpdateAsync(int id, UpdateMediaDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _unitOfWork.Medias.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Mídia não encontrada");

        if (!string.IsNullOrWhiteSpace(dto.Nome))
            existing.AtualizarNome(dto.Nome);

        if (!string.IsNullOrWhiteSpace(dto.Descricao))
            existing.AtualizarDescricao(dto.Descricao);

        if (dto.File != null && dto.File.Length > 0)
        {            
            await _storageStrategy.DeleteAsync(existing.UrlMidia);

            var fileName = GenerateUniqueFileName(dto.File.FileName);

            ValidateFile(dto.File.FileName);

            using var stream = dto.File.OpenReadStream();
            var url = await _storageStrategy.SaveAsync(stream, fileName, dto.File.ContentType);

            existing.AtualizarUrl(url);
        }

        await _unitOfWork.Medias.UpdateAsync(existing, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _mapper.Map<MediaDto>(existing);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var midia = await _unitOfWork.Medias.GetByIdAsync(id, cancellationToken)
                    ?? throw new KeyNotFoundException("Mídia não encontrada");

        await _storageStrategy.DeleteAsync(midia.UrlMidia);
        await _unitOfWork.Medias.DeleteAsync(midia, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    #region Helpers

    private void ValidateFile(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLower();
        if (!AllowedImageExtensions.Contains(ext) && !AllowedVideoExtensions.Contains(ext))
            throw new ArgumentException("Formato de arquivo não suportado");
    }

    private string GenerateUniqueFileName(string originalFileName)
    {
        return $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
    }

    #endregion
}
