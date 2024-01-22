using Microsoft.AspNetCore.Mvc;

namespace ObjectStorageApp.Controllers;

[ApiController]
[Route("files/[controller]")]
public class ObjectStorageController : ControllerBase
{
   //  private readonly ILogger<ObjectStorageController> _logger;
   //  private readonly IMinioClient minioClient;
   //
   //  public ObjectStorageController(ILogger<ObjectStorageController> logger, IMinioClient minioClient)
   //  {
   //      _logger = logger;
   //      this.minioClient = minioClient;
   //  }
   //
   //  [HttpGet("ping")]
   //  public IActionResult GetOk()
   //  {
   //      return Ok(new
   //      {
   //          Result = "Connected"
   //      });
   //  }
   //  
   //  [HttpPost("upload-avatars")]
   //  [Authorize]
   //  public async Task<IActionResult> UploadAvatarFile(IFormFile avatarFile)
   //  {
   //      var userId = JwtUtil.GetClaimsFromToken(Request.Headers.Authorization!)["id"];
   //      var obj = await minioClient
   //          .PutObjectAsync(new PutObjectArgs()
   //              .WithBucket("avatars")
   //              .WithObject(userId)
   //              .WithFileName(avatarFile.FileName)
   //              .WithContentType(avatarFile.ContentType)
   //              .WithObjectSize(avatarFile.Length)
   //              .WithStreamData(avatarFile.OpenReadStream())
   //          );
   //      var result = await minioClient
   //          .PresignedGetObjectAsync(new PresignedGetObjectArgs()
   //              .WithBucket("avatars")
   //              .WithObject(userId));
   //      return Ok(result);
   //  }
   //
   //  [HttpGet("get-avatar")]
   //  [Authorize]
   //  public async Task<IActionResult> GetAvatarFileUrl()
   //  {
   //      var userId = JwtUtil.GetClaimsFromToken(Request.Headers.Authorization!)["id"];
   //      var result = await minioClient
   //          .PresignedGetObjectAsync(new PresignedGetObjectArgs()
   //              .WithBucket("avatars")
   //              .WithObject(userId));
   //
   //      return Ok(result);
   //  }
   //
   //  [HttpPost("upload-project-file/{responseId}")]
   // // [Authorize]
   //  public async Task<IActionResult> UploadProjectFile(int responseId, IFormFile projectFile)
   //  {
   //      //var userId = JwtUtil.GetClaimsFromToken(Request.Headers.Authorization!)["id"];
   //      await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
   //          .WithBucket("project-results")
   //          .WithObject($"{responseId}"));
   //      
   //      var obj = await minioClient
   //          .PutObjectAsync(new PutObjectArgs()
   //              .WithBucket("project-results")
   //              .WithObject($"{responseId}")
   //              .WithContentType(projectFile.ContentType)
   //              .WithObjectSize(projectFile.Length)
   //              .WithStreamData(projectFile.OpenReadStream())
   //          );
   //
   //      return Ok();
   //      // var result = await minioClient
   //      //     .GetObjectAsync(new  GetObjectArgs()
   //      //         .WithBucket("project-results")
   //      //         .WithObject($"{responseId}")
   //      //         );
   //      // return Ok(result);
   //  }
   //
   //  [HttpGet("get-project-file/{responseId}")]
   //  //[Authorize]
   //  public async Task<FileResult> GetProjectFileUrl(int responseId)
   //  {
   //      var downloadStream = new MemoryStream();
   //      var result = await minioClient
   //          .GetObjectAsync(new GetObjectArgs()
   //              .WithBucket("project-results")
   //              .WithObject($"{responseId}")
   //              .WithCallbackStream(x =>
   //              {
   //                  x.CopyTo(downloadStream);
   //                  downloadStream.Seek(0, SeekOrigin.Begin);
   //              })
   //          );
   //      return new FileStreamResult(downloadStream,result.ContentType);
   //  }
}