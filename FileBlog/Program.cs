using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var secretKey = builder.Configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT secret key is not found");

builder.Services.AddSingleton<IPostStorage, FilePostStorage>();
builder.Services.AddSingleton<IUserStorage, FileUserStorage>();
builder.Services.AddSingleton(new TokenGenerator(secretKey));
builder.Services.AddScoped<PasswordHasher<User>>();
builder.Services.AddScoped<CreatePostHandler>();
builder.Services.AddScoped<CreatePostValidator>();
builder.Services.AddScoped<GetPostBySlugHandler>();
builder.Services.AddScoped<GetAllPostsHandler>();
builder.Services.AddScoped<DeletePostHandler>();
builder.Services.AddScoped<UpdatePostValidator>();
builder.Services.AddScoped<UpdatePostHandler>();
builder.Services.AddScoped<RegisterUserValidator>();
builder.Services.AddScoped<RegisterUserHandler>();
builder.Services.AddScoped<LoginValidator>();
builder.Services.AddScoped<LoginHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "FileBlog",
        ValidAudience = "http://localhost:5054/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllowedToCreatePosts", policy => policy.RequireRole("Author"));
    options.AddPolicy("AllowedToDeletePosts", policy => policy.RequireRole("Author", "Admin"));
    options.AddPolicy("AllowedToUpdatePosts", policy => policy.RequireRole("Author", "Editor"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

CreatePostEndpoint.Map(app);
GetPostBySlugEndpoint.Map(app);
GetAllPostsEndpoint.Map(app);
DeletePostEndpoint.Map(app);
UpdatePostEndpoint.Map(app);
RegisterUserEndpoint.Map(app);
LoginEndpoint.Map(app);
app.Run();