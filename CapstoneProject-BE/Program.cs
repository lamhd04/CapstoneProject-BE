
using CapstoneProject_BE.Models;
using CapstoneProject_BE.Scheduler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<InventoryManagementContext>(opt
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("InventoryManagement")));

//controller
builder.Services.AddControllers()
    .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

//JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateLifetime=true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
//quartz
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionScopedJobFactory();
    // Just use the name of your job that you created in the Jobs folder.
    var jobKey = new JobKey("InventoryFinalization");
    var jobKey1 = new JobKey("ClearTrashToken");
    q.AddJob<InventoryFinalization>(opts => opts.WithIdentity(jobKey));
    q.AddJob<InventoryFinalization>(opts => opts.WithIdentity(jobKey1));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("InventoryFinalization-trigger")
        //This Cron interval can be described as "run every month"
        .WithCronSchedule("0 0 12 L * ?")

    );
    q.AddTrigger(opts => opts
    .ForJob(jobKey1)
    .WithIdentity("ClearTrashToken-trigger")
    //This Cron interval can be described as "run every minute" (when second is zero)
    .WithCronSchedule("0 0 12 L * ?"));
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Owner", policy =>
                      policy.RequireClaim("RoleId", "1"));
    options.AddPolicy("Manager", policy =>
                  policy.RequireClaim("RoleId","1","2"));
    options.AddPolicy("Seller", policy =>
                  policy.RequireClaim("RoleId", "3"));
});
//swagger
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Project API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();
app.UseCors(option => option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "imsdAPI v1"));
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();