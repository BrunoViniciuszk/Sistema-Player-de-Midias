using api_dotnet.Models;

namespace api_dotnet.Factories.Interfaces
{
    public interface IMidiaFactory
    {
        Midia Create(string nome, string descricao, string url);
    }
}
