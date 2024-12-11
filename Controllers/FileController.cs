using Microsoft.AspNetCore.Mvc;
using test_s3.Service;

namespace test_s3.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly S3Service _s3Service;

    public FileController(S3Service s3Service)
    {
        _s3Service = s3Service;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("Không có file được upload");

        try
        {
            var fileName = await _s3Service.UploadFileAsync(file);
            return Ok(new { fileName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi upload: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Download(string fileName)
    {
        try
        {
            var fileBytes = await _s3Service.DownloadFileAsync(fileName);
            return File(fileBytes, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi download: {ex.Message}");
        }
    }

    [HttpGet("url")]
    public IActionResult GetDownloadUrl(string fileName)
    {
        try
        {
            var url = _s3Service.GetPreSignedUrl(fileName);
            return Ok(new { url });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi tạo URL: {ex.Message}");
        }
    }
    [HttpPost("create-folder")]
    public async Task<IActionResult> CreateFolder(string folderName)
    {
        try
        {
            await _s3Service.CreateFolderAsync(folderName);
            return Ok(new { message = $"Folder {folderName} created successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("upload-to-folder")]
    public async Task<IActionResult> UploadToFolder(string folderName, IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("No file selected");

        try
        {
            var fileName = await _s3Service.UploadToFolderAsync(folderName, file);
            return Ok(new { fileName });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("list-files/{folderName}")]
    public async Task<IActionResult> ListFiles(string folderName)
    {
        try
        {
            var files = await _s3Service.ListFilesInFolderAsync(folderName);
            return Ok(files);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}