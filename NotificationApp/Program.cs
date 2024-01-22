// using Microsoft.EntityFrameworkCore;
// using NotificationApp.BackgroundService;
// using NotificationApp.Hubs;
// using NotificationApp.Persistance;
// using NotificationApp.Services;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
//
// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
// builder.Services.AddScoped(typeof(NotificationService));
// builder.Services.AddSingleton(typeof(NotificationManager));
// builder.Services.AddDbContext<NotificationDbContext>(options =>
// {
//     options.UseNpgsql(
//             Environment.GetEnvironmentVariable("DockerConnection") ??
//             builder.Configuration["DefaultConnection:Pg"])
//         .EnableDetailedErrors()
//         .EnableSensitiveDataLogging()
//         .LogTo(
//             Console.WriteLine,
//             LogLevel.Information);
// });
// builder.Services.AddHostedService<BrokerListener>();
// builder.Services.AddSignalR();
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
//
// app.UseHttpsRedirection();
//
// app.UseAuthorization();
//
// app.MapControllers();
// app.MapHub<NotificationHub>("/notifications");
// app.Run();