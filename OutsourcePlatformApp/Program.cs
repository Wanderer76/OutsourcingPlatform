using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Minio.DataModel.Args;
using OutsourcePlatformApp.Hubs;
using OutsourcePlatformApp.Hubs.Chat;
using OutsourcePlatformApp.Models;
using OutsourcePlatformApp.Repository;
using OutsourcePlatformApp.Service;
using OutsourcePlatformApp.Service.BackgroundServices;
using OutsourcePlatformApp.Service.EmailServices;
using OutsourcePlatformApp.Service.MessageBrocker;
using OutsourcePlatformApp.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseNpgsql(
                Environment.GetEnvironmentVariable("DockerConnection") ??
                builder.Configuration["DefaultConnection:Pg"],
                optionsBuilder => optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(
                Console.WriteLine,
                LogLevel.Information);
    });
builder.Services.AddHostedService<TimedHostedService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompetenciesRepository, CompetenciesRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IExecutorRepository, ExecutorRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IOrderRoleRepository, OrderRoleRepository>();
builder.Services.AddScoped<IResponseRepository, ResponseRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IActivationTokenRepository, ActivationTokenRepository>();
builder.Services.AddScoped<IEmailSenderService, SendVerificationEmail>();
builder.Services.AddScoped<IMessageProducer, MessageSender>();
builder.Services.AddScoped(typeof(AuthService));
builder.Services.AddScoped(typeof(PersonalAreaService));
builder.Services.AddScoped(typeof(OrderService));
builder.Services.AddScoped(typeof(CompetenciesService));
builder.Services.AddScoped(typeof(ExecutorService));
builder.Services.AddScoped(typeof(JwtTokenService));
builder.Services.AddScoped(typeof(CustomerService));
builder.Services.AddScoped(typeof(ActionNotificationService));
builder.Services.AddScoped(typeof(ResponseService));
builder.Services.AddScoped(typeof(UserService));
builder.Services.AddScoped(typeof(AdminService));
builder.Services.AddScoped(typeof(ChatRoomService));
builder.Services.AddScoped(typeof(NotificationHub));
builder.Services.AddScoped(typeof(ChatHub));

builder.Services.AddSingleton<NotificationManager>();
builder.Services.AddSingleton<ChatManager>();

builder.Services.AddHttpContextAccessor();
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
        //  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });
builder.Services.AddMinio(client =>
    client
        .WithEndpoint("158.160.110.86",9000)
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSignalR()
    .AddHubOptions<ChatHub>(options => { options.AddFilter<ChatFilter>(); });

var app = builder.Build();

void CreateDatabase()
{
    using (var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>())
    {
        // db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        if (!db.Users.Any())
            new SeedData(db).SetSeedData();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    CreateDatabase();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapControllers();
app.UseMiddleware<AccountVerificationMiddleware>();
app.UseMiddleware<BannedMiddleware>();

app.MapHub<NotificationHub>("/notifications");
app.MapHub<ChatHub>("/chats");

app.Run();