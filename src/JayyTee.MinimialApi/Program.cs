using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

Console.WriteLine("Starting up");

var builder = WebApplication.CreateBuilder();
ConfigureWebApplicationBuilder(builder);

var app = builder.Build();
ConfigureWebApplication(app);

app.Run();

public partial class Program
{
    /*
     * We encapsulate the infra bootstrapping and so forth here, that way, the configuration can be leveraged by
     * self-hosted test assemblies.
     */

    public static void ConfigureWebApplicationBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.DocInclusionPredicate((name, api) => true);
            c.TagActionsBy(api =>
            {
                if (api.GroupName != null)
                    return new List<string> { api.GroupName };

                var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                return new List<string> { controllerActionDescriptor?.ControllerName ?? Assembly.GetExecutingAssembly().GetName().Name! };
            });
        });
    }

    public static WebApplication ConfigureWebApplication(WebApplication app)
    {
        app.UseStaticFiles();
        app.UseSwagger();

        app.MapGet("/", () => "Hello World!").WithGroupName("Blah").WithName("HelloWorld");
        app.MapControllers();

        app.UseSwaggerUI();

        return app;
    }
}