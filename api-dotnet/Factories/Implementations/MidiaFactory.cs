using api_dotnet.Models;
using api_dotnet.Factories.Interfaces;

namespace api_dotnet.Factories.Implementations
{
    public class MidiaFactory : IMidiaFactory
    {
        public Midia Create(string nome, string descricao, string url)
        {
            return new Midia
            {
                Nome = nome,
                Descricao = descricao,
                UrlMidia = url
            };
        }
    }
}
