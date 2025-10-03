using Microsoft.AspNetCore.Http;

namespace Midia.Application.Dtos
{
    public class UploadMediaDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
