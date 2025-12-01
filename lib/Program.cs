using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;
using lib.Services;

class Program
{
    public static void Main(string[] args)
    {
        QuestionService.ImportMarkdown(args[0]);
        var builder = WebApplication.CreateBuilder([]);

        // Set CORS 
        var  LocalHostOrigin = "_localHostOrigin";
        builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: LocalHostOrigin,
                                policy  =>
                                {
                                    policy.WithOrigins("http://localhost:3000",
                                                        "http://127.0.0.1:3000");
                                });
            });

        builder.Services.AddOpenApi();

        // Add services to the container.

        builder.Services.AddControllers();
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseCors(LocalHostOrigin);
        
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

