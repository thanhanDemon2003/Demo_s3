# S3/iDrive E2 Storage Integration - .NET Core Web API

## Overview
A RESTful API service built with .NET Core 8.0 for managing files and folders on S3/iDrive E2 storage. The project demonstrates proper integration with cloud storage services while maintaining secure and efficient file operations.

## Features

### File Management
- **Upload/Download Files**
- **Generate Pre-Signed URLs**
- **Delete Files**

### Folder Operations
- **Create Folders**
- **Upload Files to Specific Folders**
- **List Files Within Folders**

### Security
- **Pre-Signed URL Generation**
- **File Validation**
- **Error Handling**

## Tech Stack
- **.NET 8.0**
- **AWS SDK for .NET**
- **AWS S3/iDrive E2**
- **Swagger/OpenAPI**

## Prerequisites
- .NET 8.0 SDK
- AWS/iDrive E2 Account
- Access Key and Secret Key
- Visual Studio/VS Code

## Project Setup

### Installation
```bash
# Clone repository
git clone [repository-url]
cd [project-name]

# Restore packages
dotnet restore

# Run application
dotnet run
```

### Configuration
Update the `appsettings.json` file:
```json
{
  "AwsSettings": {
    "AccessKey": "your_access_key",
    "SecretKey": "your_secret_key",
    "BucketName": "your_bucket_name",
    "ServiceUrl": "https://your-endpoint-url"
  }
}
```

### Project Structure
```
├── Controllers/
│   └── FileController.cs
├── Models/
│   └── AwsSettings.cs
├── Services/
│   └── S3Service.cs
├── appsettings.json
└── Program.cs
```

## API Documentation

### File Operations

#### Upload File
**POST** `/File`

**Headers:** `Content-Type: multipart/form-data`

**Request:**
```json
{
  "file": "File"
}
```

**Response:** `200 OK`
```json
{
  "fileName": "string"
}
```

#### Download File
**GET** `/File?fileName=string`

**Response:** `200 OK`

**Headers:** `Content-Type: application/octet-stream`

#### Get Pre-Signed URL
**GET** `/File/url?fileName=string`

**Response:** `200 OK`
```json
{
  "url": "string"
}
```

### Folder Operations

#### Create Folder
**POST** `/File/create-folder?folderName=string`

**Response:** `200 OK`
```json
{
  "message": "Folder {folderName} created successfully"
}
```

#### Upload to Folder
**POST** `/File/upload-to-folder`

**Headers:** `Content-Type: multipart/form-data`

**Request:**
```json
{
  "folderName": "string",
  "file": "File"
}
```

**Response:** `200 OK`
```json
{
  "fileName": "string"
}
```

#### List Files in Folder
**GET** `/File/list-files/{folderName}`

**Response:** `200 OK`
```json
[
  "string"
]
```

## Implementation Details

### File Controller
```csharp
[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly S3Service _s3Service;

    public FileController(S3Service s3Service)
    {
        _s3Service = s3Service;
    }

    // Endpoints implementation...
}
```

### S3 Service
```csharp
public class S3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsSettings _awsSettings;

    public S3Service(IAmazonS3 s3Client, IOptions<AwsSettings> awsSettings)
    {
        _s3Client = s3Client;
        _awsSettings = awsSettings.Value;
    }

    // Service methods...
}
```

### Error Handling
#### Status Codes
- **200:** Successful operation
- **400:** Bad request (empty file, invalid input)
- **500:** Server error

#### Error Response Format
```json
{
  "error": "Error message description"
}
```

## Best Practices

### Security
- Validate file types
- Implement file size limits
- Use secure pre-signed URLs
- Sanitize file names

### Performance
- Use async/await properly
- Implement proper stream handling
- Handle large files efficiently

### Error Handling
- Proper exception handling
- Meaningful error messages
- Logging of operations

## Common Issues & Solutions

### Invalid ServiceURL
**Error:** Value for ServiceURL is not a valid URL

**Solution:** Ensure URL starts with `https://`

### Authentication Failed
**Error:** The request signature we calculated does not match

**Solution:** Verify Access Key and Secret Key

## Usage Examples

### Upload File
```javascript
const formData = new FormData();
formData.append('file', file);

await fetch('/File', {
  method: 'POST',
  body: formData
});
```

### Create Folder and Upload
```javascript
// Create folder
await fetch('/File/create-folder?folderName=images', {
  method: 'POST'
});

// Upload to folder
const formData = new FormData();
formData.append('file', file);
formData.append('folderName', 'images');

await fetch('/File/upload-to-folder', {
  method: 'POST',
  body: formData
});
```

## Future Enhancements
- Multi-file upload
- File versioning
- Folder deletion
- File moving between folders
- Advanced search functionality
- Caching implementation

## Development Setup

### Required NuGet Packages
```csproj
<PackageReference Include="AWSSDK.S3" Version="3.7.x" />
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="8.0.x" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.x" />
```
### Development Configuration
- Clone repository
- Set up user secrets or environment variables
- Configure AWS credentials
- Run application

### Testing
- Use Swagger UI for API testing
- Postman collection available
- Unit tests for service layer

### Deployment
- Configure production settings
- Set up proper authentication
- Configure CORS if needed
- Set up proper logging

## Contributing
1. Fork repository
2. Create feature branch
3. Commit changes
4. Create pull request

## Support
For support:
- Open an issue
- Contact [Facebook](https://www.facebook.com/thanhan.demon26/)
- Check documentation

## Acknowledgments
- AWS SDK for .NET
- .NET Core Team
- Contributors

## References
- [AWS SDK for .NET Documentation](https://docs.aws.amazon.com/sdk-for-net/index.html)
- [iDrive E2 Documentation](https://www.idrive.com/)
- [Swagger/OpenAPI Documentation](https://swagger.io/docs/)

## Contact
For inquiries [Mail](mailto:demon@annt.tech)
