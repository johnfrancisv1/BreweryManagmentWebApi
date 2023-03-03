using BreweryWholesaleService.Core.EntityModels;
using BreweryWholesaleService.Core.Interfaces.Repositories;
using BreweryWholesaleService.Core.Interfaces.Services;
using BreweryWholesaleService.Core.Services;
using BreweryWholesaleService.Infrastructure.EntityModels;
using BreweryWholesaleService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BreweryWebApplicationContextConnection") ?? throw new InvalidOperationException("Connection string 'BreweryWebApplicationContextConnection' not found.");
// Add services to the container.

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddDbContext<BreweryContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = false;
    options.Password.RequireNonAlphanumeric = false;



})
    .AddEntityFrameworkStores<BreweryContext>();


builder.Services.AddTransient<IBeerRepository, BeerRepository>();
builder.Services.AddTransient<IBeerService, BeerService>();



var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("SecurityKey"));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var UserMachine = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var user = UserMachine.GetUserAsync(context.HttpContext.User);

            if (user == null)
            {
                context.Fail("Unauthorized");
            }


            return Task.CompletedTask;
        }
    };
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Standerd Authorization header using the Bearer scheme (\"brarer {Token})",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
