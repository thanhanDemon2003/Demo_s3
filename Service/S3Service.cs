using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using test_s3.Models;

namespace test_s3.Service;
public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3Service(IOptions<AwsSettings> settings)
    {
        var awsSettings = settings.Value;
        var config = new AmazonS3Config
        {
            ServiceURL = awsSettings.ServiceUrl, 
            ForcePathStyle = true 
        };

        _s3Client = new AmazonS3Client(
            awsSettings.AccessKey, 
            awsSettings.SecretKey, 
            config
        );
        _bucketName = awsSettings.BucketName;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}-{file.FileName}";
        
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = ms,
            ContentType = file.ContentType
        };

        await _s3Client.PutObjectAsync(request);
        return fileName;
    }

    public async Task<byte[]> DownloadFileAsync(string fileName)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        using var response = await _s3Client.GetObjectAsync(request);
        using var ms = new MemoryStream();
        await response.ResponseStream.CopyToAsync(ms);
        return ms.ToArray();
    }

    public async Task DeleteFileAsync(string fileName)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName
        };

        await _s3Client.DeleteObjectAsync(request);
    }

    public string GetPreSignedUrl(string fileName, double durationInMinutes = 60)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.AddMinutes(durationInMinutes)
        };

        return _s3Client.GetPreSignedURL(request);
    }
    public async Task CreateFolderAsync(string folderName)
    {
        var folderKey = $"{folderName.TrimEnd('/')}/";

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = folderKey,
            ContentBody = string.Empty
        };

        await _s3Client.PutObjectAsync(putRequest);
    }

    public async Task<string> UploadToFolderAsync(string folderName, IFormFile file)
    {
        var fileName = $"{folderName.TrimEnd('/')}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileName,
            InputStream = ms,
            ContentType = file.ContentType
        };

        await _s3Client.PutObjectAsync(putRequest);
        return fileName;
    }

    public async Task<List<string>> ListFilesInFolderAsync(string folderName)
    {
        var request = new ListObjectsV2Request
        {
            BucketName = _bucketName,
            Prefix = $"{folderName.TrimEnd('/')}/"
        };

        var result = await _s3Client.ListObjectsV2Async(request);
        return result.S3Objects
            .Select(obj => obj.Key)
            .Where(key => !key.EndsWith("/"))
            .ToList();
    }
}