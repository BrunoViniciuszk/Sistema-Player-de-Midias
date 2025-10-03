using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Midia.Application.Interfaces;
using Midia.Infrastructure.Storage.Implementations;

namespace Midia.Infrastructure.Storage.Factory
{
    public class StorageStrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public StorageStrategyFactory(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public IStorageStrategy Create()
        {
            var provider = _configuration["Storage:Provider"];

            return provider switch
            {
                "AwsS3" => _serviceProvider.GetRequiredService<AwsS3StorageStrategy>(),
                "Local" => _serviceProvider.GetRequiredService<LocalStorageStrategy>(),
                _ => throw new NotImplementedException($"Provider {provider} não suportado.")
            };
        }
    }
}
