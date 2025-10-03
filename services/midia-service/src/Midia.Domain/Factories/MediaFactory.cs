using Midia.Domain.Entities;
using Midia.Domain.Factories.Interfaces;

namespace Midia.Domain.Factories
{
    public class MediaFactory : IMediaFactory
    {
        public Media Create(string nome, string descricao, string url)
        {
            return new Media(nome, descricao, url);
        }
    }
}
