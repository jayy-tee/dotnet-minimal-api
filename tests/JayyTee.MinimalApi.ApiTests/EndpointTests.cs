using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RestSharp;

namespace JayyTee.MinimalApi.ApiTests;

[TestFixture]
public class EndpointTests : TestBase
{

    [Test]
    public async Task Can_Access_Endpoint_WithHttpClient()
    {
        var response = await Client.GetAsync("/");
        
        response.EnsureSuccessStatusCode(); 
    }
    
    [Test]
    public void Can_Access_Endpoint_WithRestClient()
    {
        var response = RestClient.Get(new RestRequest("/", Method.GET));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}