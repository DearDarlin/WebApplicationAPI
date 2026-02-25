
using Microsoft.EntityFrameworkCore;
using WebApplicationAPI.Abstracts;
using WebApplicationAPI.DAL;
using WebApplicationAPI.DAL.Abstracts;
using WebApplicationAPI.DAL.Repositories;
using WebApplicationAPI.Services;

namespace WebApplicationAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
            // Игнорируем циклические ссылки при сериализации в JSON
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

        // Опционально: делаем JSON более читаемым (красивые отступы)
                options.JsonSerializerOptions.WriteIndented = true;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();

            // 3. Реєстрація Сервісу бібліотеки
            builder.Services.AddScoped<ILibraryService, LibraryService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowReactApp");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
