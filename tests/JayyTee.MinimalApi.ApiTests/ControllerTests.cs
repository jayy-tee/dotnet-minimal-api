using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RestSharp;

namespace JayyTee.MinimalApi.ApiTests;

[TestFixture]
public class ControllerTests : TestBase
{
    [Test]
    public async Task Can_Access_A_Contoller_Method()
    {
        var response = RestClient.Get(new RestRequest("/dummy", Method.GET));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Test]
    public async Task Can_Access_StaticFiles()
    {
        var response = RestClient.Get(new RestRequest("/test.json", Method.GET));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}