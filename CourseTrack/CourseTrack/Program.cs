using BusinessLayer.Account;
using BusinessLayer.CourseWorks;
using BusinessLayer.Services;
using BusinessLayer.Students;
using CourseTrack.Controllers;
using DataLayer.Account;
using DataLayer.CourseWorks;
using DataLayer.Data;
using DataLayer.Lecturers;
using DataLayer.Students;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// If needed, Clear default providers
builder.Logging.ClearProviders();

// Use Serilog
builder.Host.UseSerilog((hostContext, services, configuration) =>
{
    configuration
        .WriteTo.File("logs.json")
        .WriteTo.Console();
});

var connectionString = builder.Configuration.GetConnectionString("CourseTrackConnectionDb");
builder.Services.AddDbContext<CourseTrackDbContext>(options =>
    options.UseNpgsql(connectionString!));


#region jwt

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:SecretKey"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            // handle token validation event
            return Task.CompletedTask;
        },
        OnAuthenticationFailed = context =>
        {
            // handle authentication failure event
            return Task.CompletedTask;
        }
    };

    options.Events.OnMessageReceived = context =>
    {

        if (context.Request.Cookies.ContainsKey("X-Access-Token"))
        {
            context.Token = context.Request.Cookies["X-Access-Token"];
        }

        return Task.CompletedTask;
    };
});


#endregion

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IAccountFacade, AccountFacade>();

builder.Services.AddScoped<IStudentFacade, StudentFacade>();

builder.Services.AddScoped<ICourseWorkFacade, CourseWorkFacade>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();

builder.Services.AddScoped<ICourseWorkRepository, CourseWorkRepository>();

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
