using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using OutsourcePlatformApp.Service;
using OutsourcePlatformApp.Utils;
using Shared;

namespace OutsourcePlatformApp.Controllers;

[ApiController]
[Route("files/[controller]")]
public class ObjectStorageController : ControllerBase
{
    private readonly ILogger<ObjectStorageController> _logger;
    private readonly IMinioClient minioClient;
    private readonly ActionNotificationService notificationService;
    private readonly PersonalAreaService personalAreaService;

    public ObjectStorageController(ILogger<ObjectStorageController> logger,
        ActionNotificationService notificationService, PersonalAreaService personalAreaService)
    {
        _logger = logger;
        this.minioClient = new MinioClient()
            //.WithEndpoint("158.160.110.86", 9000)
            //.WithCredentials("artyom", "12345678")
            .WithEndpoint("play.min.io", 9000)
            .WithCredentials("Q3AM3UQ867SPQQA43P2F", "zuf+tfteSlswRu7BJ86wekitnifILbZam1KYY3TG")
            .WithSSL(false)
            .Build();
        this.notificationService = notificationService;
        this.personalAreaService = personalAreaService;
    }

    [HttpGet("ping")]
    public IActionResult GetOk()
    {
        return Ok(new
        {
            Result = "Connected"
        });
    }

    [HttpPost("upload-avatars")]
    [Authorize]
    public async Task<IActionResult> UploadAvatarFile(IFormFile avatarFile)
    {
        var userId = JwtUtil.GetClaimsFromToken(Request.Headers.Authorization!)["id"];
        var obj = await minioClient
            .PutObjectAsync(new PutObjectArgs()
                .WithBucket("avatars")
                .WithObject(userId)
                .WithFileName(avatarFile.FileName)
                .WithContentType(avatarFile.ContentType)
                .WithObjectSize(avatarFile.Length)
                .WithStreamData(avatarFile.OpenReadStream())
            );
        var result = await minioClient
            .PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket("avatars")
                .WithObject(userId));
        return Ok(result);
    }

    [HttpGet("get-avatar")]
    [Authorize]
    public async Task<IActionResult> GetAvatarFileUrl()
    {
        var userId = JwtUtil.GetClaimsFromToken(Request.Headers.Authorization!)["id"];
        var result = await minioClient
            .PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket("avatars")
                .WithObject(userId));

        return Ok(result);
    }

    [HttpPost("upload-project-file/{orderId}/{responseId}")]
    [Authorize]
    public async Task<IActionResult> UploadProjectFile(int orderId, int responseId, IFormFile projectFile)
    {
        var user = await personalAreaService.GetUserFromToken(Request.Headers.Authorization);
        try
        {
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket("project-results")
                .WithObject($"{responseId}"));


            var obj = await minioClient
                .PutObjectAsync(new PutObjectArgs()
                    .WithBucket("project-results")
                    .WithObject($"{responseId}")
                    .WithContentType(projectFile.ContentType)
                    .WithObjectSize(projectFile.Length)
                    .WithStreamData(projectFile.OpenReadStream())
                );
            await notificationService.CreateActionNotification(Request.Headers.Authorization, user, user, orderId,
                NotificationMessages.FileUploadSuccess);
            return Ok();
        }
        catch (Exception e)
        {
            await notificationService.CreateActionNotification(Request.Headers.Authorization, user, user, orderId,
                NotificationMessages.FileUploadError);
            return BadRequest(e.Message);
            // var result = await minioClient
            //     .GetObjectAsync(new  GetObjectArgs()
            //         .WithBucket("project-results")
            //         .WithObject($"{responseId}")
            //         );
            // return Ok(result);
        }
    }

    [HttpGet("get-project-file/{responseId}")]
    //[Authorize]
    public async Task<FileResult> GetProjectFileUrl(int responseId)
    {
        var downloadStream = new MemoryStream();
        var result = await minioClient
            .GetObjectAsync(new GetObjectArgs()
                .WithBucket("project-results")
                .WithObject($"{responseId}")
                .WithCallbackStream(x =>
                {
                    x.CopyTo(downloadStream);
                    downloadStream.Seek(0, SeekOrigin.Begin);
                })
            );
        return new FileStreamResult(downloadStream, result.ContentType);
    }
}