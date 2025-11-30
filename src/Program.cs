using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;
using src.Services;

class Program
{
    public static void Main(string[] args)
    {
        QuestionService.ImportMarkdown(args[0]);
        var builder = WebApplication.CreateBuilder([]);

        builder.Services.AddOpenApi();

        // Add services to the container.

        builder.Services.AddControllers();
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

