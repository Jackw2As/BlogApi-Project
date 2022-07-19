using BlogAPI.Storage.DatabaseModels;
using Domain.Interface;
using BlogAPI.Storage.InMemory;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InMemoryDBContext>(
    options =>
    {
        options.UseSqlite("DataSource=file::memory:?cache=shared", options =>
        {
        });
    });

builder.Services.AddTransient<IRepository<Blog>, InMemoryRepository<Blog>>(
    (provider) =>
    {
        var context = provider.GetRequiredService<InMemoryDBContext>();
        return new InMemoryRepository<Blog>(context);
    });
builder.Services.AddTransient<IRepository<Post>, InMemoryRepository<Post>>(
    provider =>
    {
        var context = provider.GetRequiredService<InMemoryDBContext>();
        return new InMemoryRepository<Post>(context);
    });

builder.Services.AddTransient<IRepository<Comment>, InMemoryRepository<Comment>>(
    provider =>
    {
        var context = provider.GetRequiredService<InMemoryDBContext>();
        return new InMemoryRepository<Comment>(context);
    });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDeveloperExceptionPage();

//Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }
