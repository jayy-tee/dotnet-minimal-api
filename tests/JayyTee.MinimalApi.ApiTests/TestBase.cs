using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RestSharp;

namespace JayyTee.MinimalApi.ApiTests;

public class TestBase
{
    private readonly ApiWebApplicationFactory _factory;
    private WebApplication? _webApp;

    protected HttpClient Client { get; private set; }
    protected string? ServerAddress;
    
    protected IRestClient? RestClient;
    
    [SetUp]
    public async Task InitializeTest()
    {
        _webApp = ConfigureInProcessHosting();

        await _webApp.StartAsync();

        Client = new HttpClient();
        Client.BaseAddress = GetBaseAddress();

        ServerAddress = Client.BaseAddress.ToString();
        RestClient = new RestClient(ServerAddress);
    }

    public virtual void ConfigureInProcessServices(IServiceCollection services)
    {
        
    }

    private WebApplication ConfigureInProcessHosting()
    {
        var apiAssembly = typeof(Program).Assembly;
        var apiAssemblyDirectory = Path.GetDirectoryName(apiAssembly.Location);

        var options = new WebApplicationOptions
        {
            ApplicationName = apiAssembly.GetName().Name,
            WebRootPath = Path.Combine(apiAssemblyDirectory!, "wwwroot")
        };

        var builder = WebApplication.CreateBuilder(options);
        Program.ConfigureWebApplicationBuilder(builder);
        
        ConfigureInProcessServices(builder.Services);

        // Override the Kestrel Listen Address/Port
        builder.WebHost.UseKestrel(kestrelOptions => kestrelOptions.Listen(IPAddress.Loopback, 0));

        var app = builder.Build();
        Program.ConfigureWebApplication(app);
        
        return app;
    }

    private Uri GetBaseAddress()
    {
        var server = _webApp.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        return addresses!.Addresses
            .Select(x => new Uri(x))
            .Last();
    }

    [OneTimeTearDown]
    public async Task BaseTearDown()
    {
        Client.Dispose();
        _factory?.Dispose();
        
        if (_webApp is not null)
        {
            await _webApp.StopAsync();
            await _webApp.DisposeAsync();
        }


    }
}