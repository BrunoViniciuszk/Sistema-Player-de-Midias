using Amazon.S3;
using Amazon.S3.Transfer;
using api_dotnet.Storage.Strategy.Interfaces;

namespace api_dotnet.Storage.Strategy.Implementations
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

        public async Task<string> SaveAsync(IFormFile file, string fileName)
        {
            using var newMemoryStream = new MemoryStream();
            await file.CopyToAsync(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = fileName,
                BucketName = _bucketName,
                ContentType = file.ContentType
            };

            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);

            return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
        }

        public async Task DeleteAsync(string path)
        {
            var key = Path.GetFileName(path);
            await _s3Client.DeleteObjectAsync(_bucketName, key);
        }
    }
}
