var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPostStorage, FilePostStorage>();
builder.Services.AddScoped<CreatePostHandler>();
builder.Services.AddScoped<CreatePostValidator>();
builder.Services.AddScoped<GetPostBySlugHandler>();
builder.Services.AddScoped<GetAllPostsHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

CreatePostEndpoint.Map(app);
GetPostBySlugEndpoint.Map(app);
GetAllPostsEndpoint.Map(app);
app.Run();