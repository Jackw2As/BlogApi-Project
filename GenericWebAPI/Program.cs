using BlogAPI.Storage.DatabaseModels;
using Domain.Interface;
using BlogAPI.Storage.InMemory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

public class Program : IDisposable
{
    private static SqliteConnection _connection = new("DataSource=file::memory:?cache=shared");

    static public void Main(string[] args)
    {
        var builder = BuildWebApplication(args);

        var app = builder.Build();

        app.UseDeveloperExceptionPage();

        //Swagger
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }

    private static WebApplicationBuilder BuildWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<InMemoryDBContext>(
            options =>
            {
                options.UseSqlite(_connection);
                options.EnableSensitiveDataLogging(true);
            });

        builder.Services.AddScoped<IRepository<Blog>, InMemoryRepository<Blog>>(
            (provider) =>
            {
                var context = provider.GetRequiredService<InMemoryDBContext>();
                return new InMemoryRepository<Blog>(context);
            });
        builder.Services.AddScoped<IRepository<Post>, InMemoryRepository<Post>>(
            provider =>
            {
                var context = provider.GetRequiredService<InMemoryDBContext>();
                return new InMemoryRepository<Post>(context);
            });

        builder.Services.AddScoped<IRepository<Comment>, InMemoryRepository<Comment>>(
            provider =>
            {
                var context = provider.GetRequiredService<InMemoryDBContext>();
                return new InMemoryRepository<Comment>(context);
            });


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    public void Dispose() => _connection?.Dispose();
}
