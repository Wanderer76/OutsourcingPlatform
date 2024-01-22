using Minio;
using Minio.DataModel.Args;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMinio(client =>
    client
        .WithEndpoint("158.160.110.86", 9000)
        .WithCredentials("artyom", "12345678")
        .WithSSL(false)
        .Build());
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var minioClient = app.Services.CreateScope().ServiceProvider.GetRequiredService<IMinioClient>();
if (!await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket("avatars")))
    await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket("avatars"));
if (!await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket("project-results")))
    await minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket("project-results"));

app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();