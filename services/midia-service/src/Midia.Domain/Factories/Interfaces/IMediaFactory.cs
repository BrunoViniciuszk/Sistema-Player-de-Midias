using Midia.Domain.Entities;

namespace Midia.Domain.Factories.Interfaces
{
    public interface IMediaFactory
    {
        Media Create(string nome, string descricao, string url);
    }
}
