using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JayyTee.MinimalApi.ApiTests;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    public string ServerAddress
    {
        get
        {
            EnsureHostIsInstantiated();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    private void EnsureHostIsInstantiated()
    {
        if (_host is null)
        {
            using var _ = CreateDefaultClient();
        }
    }

    private IHost? _host;
    private bool _disposed;
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Reference: https://github.com/martincostello/dotnet-minimal-api-integration-testing/blob/main/tests/TodoApp.Tests/HttpServerFixture.cs
        var testHost = builder.Build();

        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel(options => options.Listen(IPAddress.Loopback, 0)));

        _host = builder.Build();
        _host.Start();
        
        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses
            .Select(x => new Uri(x))
            .Last();
        
        testHost.Start();
        return testHost;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseUrls("http://127.0.0.1:0");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (_disposed || !disposing) return;
        
        _host?.Dispose();
        _disposed = true;
    }
}