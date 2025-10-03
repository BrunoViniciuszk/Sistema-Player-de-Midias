namespace Midia.Domain.Entities
{
    public class Media
    {
        public int Id { get; protected set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public string UrlMidia { get; private set; }

        protected Media() { }

        public Media(string nome, string descricao, string urlMidia)
        {
            Nome = nome;
            Descricao = descricao;
            UrlMidia = urlMidia;
        }

        public void AtualizarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome inválido");

            Nome = nome;
        }

        public void AtualizarDescricao(string descricao)
        {
            Descricao = descricao ?? Descricao;
        }

        public void AtualizarUrl(string urlMidia)
        {
            if (string.IsNullOrWhiteSpace(urlMidia))
                throw new ArgumentException("Url inválida");

            UrlMidia = urlMidia;
        }
    }
}
