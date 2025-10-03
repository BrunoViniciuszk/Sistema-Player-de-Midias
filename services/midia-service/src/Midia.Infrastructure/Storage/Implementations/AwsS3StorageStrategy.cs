using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Midia.Application.Interfaces;

namespace Midia.Infrastructure.Storage.Implementations
{
    public class AwsS3StorageStrategy : IStorageStrategy
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public AwsS3StorageStrategy(IAmazonS3 amazonS3, IConfiguration configuration)
        {
            _s3Client = amazonS3;
            _bucketName = configuration["Storage:Aws:BucketName"]
                          ?? throw new ArgumentNullException("BucketName não configurado");
        }

        public async Task<string> SaveAsync(Stream fileStream, string fileName, string contentType)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                Key = fileName,
                BucketName = _bucketName,
                ContentType = contentType
            };

            var transferUtility = new TransferUtility(_s3Client);
            await transferUtility.UploadAsync(uploadRequest);

            return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
        }

        public async Task DeleteAsync(string path)
        {
            var key = Path.GetFileName(path);
            await _s3Client.DeleteObjectAsync(_bucketName, key);
        }
    }
}
