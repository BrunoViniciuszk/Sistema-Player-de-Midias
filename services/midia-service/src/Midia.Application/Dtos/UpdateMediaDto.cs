using Microsoft.AspNetCore.Http;

namespace Midia.Application.Dtos
{
    public class UpdateMediaDto
    {
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public IFormFile? File { get; set; }
    }
}
