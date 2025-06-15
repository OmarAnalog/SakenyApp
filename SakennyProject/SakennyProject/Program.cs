
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sakenny.Core.Services;
using Sakenny.Repository;
using Sakenny.Repository.Data;
using Sakenny.Services;
using SakennyProject.Helper;
using SakennyProject.Middlewares;
using SakennyProject.Erorrs;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Sakenny.Services.ImageService;
using Microsoft.AspNetCore.Identity;
using Sakenny.Repository.Data.DataSeed;
using Sakenny.Core.Models;
using Sakenny.Core.Repositories;
using SakennyProject.Hubs;
using System.Threading.RateLimiting;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
namespace SakennyProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<SakennyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #region adding to DI Container
            builder.Services.AddDbContext<SakennyDbContext>();
            builder.Services.AddScoped<ITokenService,TokenService>();
            builder.Services.AddScoped<IEmailService,EmailService>();
            builder.Services.AddScoped<ChatRepo>();
            builder.Services.AddScoped<FavourityListRepository>();
            builder.Services.AddScoped<RatingRepository>();
            builder.Services.AddScoped<PostFavouriteListRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count > 0)
                    .SelectMany(p => p.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                    var ValidationErrorResponse = new ValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);

                };
            });
            builder.Services.AddScoped<UsersRepo>();
            builder.Services.AddScoped<PostRepo>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.Configure<CloudinarySettings>
                (builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddSingleton<Cloudinary>(sp =>
            {
                var settings=sp.GetService<IOptions<CloudinarySettings>>()?.Value;
                var account=new Account(settings?.CloudName,settings?.ApiKey,settings?.ApiSecret);
                return new Cloudinary(account);
            });
            builder.Services.AddScoped<ImageService>();
            builder.Services.AddScoped<ReportRepo>();
            builder.Services.AddScoped<NotifcationRepo>();
            builder.Services.AddScoped<ChatService>();
            builder.Services.AddScoped<FCM>();
            #endregion
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });
            #region AddRateLimit
            //builder.Services.AddRateLimiter(Options => {

            //    Options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
            //        httpContext =>
            //        {
            //            var key = httpContext.User.Identity?.IsAuthenticated == true
            //            ? httpContext.User.Identity.Name
            //            : httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
            //            return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
            //            {
            //                PermitLimit = 100,
            //                Window = TimeSpan.FromMinutes(1),
            //                QueueLimit = 0,
            //                QueueProcessingOrder = QueueProcessingOrder.OldestFirst
            //            });
            //        });
            //    Options.OnRejected = async (context,_) =>
            //    {
            //        context.HttpContext.Response.StatusCode = 429;
            //        await context.HttpContext.Response.WriteAsync("Too many request in short period");
            //    };
            //});
            #endregion
            builder.Services.AddSignalR();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var serviceAccountPath = Path.Combine(Directory.GetCurrentDirectory(), "sakeny-fb401-firebase-adminsdk.json");
            if (!File.Exists(serviceAccountPath))
            {
                Console.WriteLine($"Error: firebase-adminsdk.json not found at {serviceAccountPath}");
                throw new FileNotFoundException("Firebase service account file not found. Please ensure 'firebase-adminsdk.json' is in the root directory of your project.");
            }
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(serviceAccountPath)
            });
            builder.Services.AddScoped<NotificationService>();
            var app = builder.Build();
            #region Update Database
            using var ScopedServices = app.Services.CreateScope();// create instance of scoped services
            var Services = ScopedServices.ServiceProvider; // get all scoped services
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var AppDbcontext = Services.GetRequiredService<SakennyDbContext>();
                var userManager = Services.GetRequiredService<UserManager<User>>();
                var rolemanager = Services.GetRequiredService<RoleManager<IdentityRole>>();
                await AppDbcontext.Database.MigrateAsync();
                await AdminSeeding.SeedRolesAdmin(userManager, rolemanager, AppDbcontext);
                await AppDbcontext.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "There were an error while migrating Database");
            }
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddlleWare>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseRateLimiter();
            app.MapHub<NotificationHub>("/Hubs/Notication");
            app.MapHub<ChatHub>("/Hubs/Chat");             
            app.MapControllers();

            app.Run();
        }
    }
}
