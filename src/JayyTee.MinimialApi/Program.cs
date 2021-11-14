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
    }
    
    public static WebApplication ConfigureWebApplication(WebApplication app)
    {
        app.UseStaticFiles();
        
        app.MapGet("/", () => "Hello World!");
        app.MapControllers();
        
        return app;
    }
    
} 