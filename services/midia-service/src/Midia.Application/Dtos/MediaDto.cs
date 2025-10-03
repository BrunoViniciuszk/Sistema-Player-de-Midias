namespace Midia.Application.Dtos
{
    public class MediaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string UrlMidia { get; set; }
        public bool ExibirNoPlayer { get; set; }
    }
}
