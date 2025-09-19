using Microsoft.AspNetCore.Mvc;

namespace api_dotnet.Models.Dtos
{
    public class UpdateMidiaDto
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }

        [FromForm(Name = "nome")]
        public string Nome { get; set; }

        [FromForm(Name = "descricao")]
        public string Descricao { get; set; }
    }
}
