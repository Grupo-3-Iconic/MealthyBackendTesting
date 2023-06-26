using System.Net;
using System.Net.Mime;
using System.Text;
using Mealthy.Mealthy.Resources;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SpecFlow.Internal.Json;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace MealthyTest.Steps;

[Binding]
public sealed class MarketServiceStepDefinition : WebApplicationFactory<Program>
{
    private readonly WebApplicationFactory<Program> _factory;
    public MarketServiceStepDefinition(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    private HttpClient Client { get; set; }
    private Uri BaseUri { get; set; }
    private Task<HttpResponseMessage> Response { get; set; }
    [Given(@"the endpoint https://localhost:(.*)/api/v(.*)/market is available")]
    public void GivenTheEndpointHttpsLocalhostApiVMarketIsAvailable(int port, int version)
    {
        BaseUri = new Uri($"https://localhost:{port}/api/v{version}/market");
        Client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = BaseUri
        });
    }
    [When(@"a post market request is sent")]
    public void WhenAPostMarketRequestIsSent(Table saveMarketResource)
    {
        var resource = saveMarketResource.CreateSet<SaveMarketResource>().First();
        var content = new StringContent(resource.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
        Response = Client.PostAsync(BaseUri, content);
    }
    [Then(@"the post market response should be status (.*)")]
    public void ThenThePostMarketResponseShouldBeStatus(int expectedStatus)
    {
        var expectedStatusCode = ((HttpStatusCode)expectedStatus).ToString();
        var actualStatusCode = Response.Result.StatusCode.ToString();
        Assert.Equal(expectedStatusCode, actualStatusCode, ignoreCase: true);
    }

    [Then(@"the post market response body should be")]
    public async Task ThenThePostMarketResponseBodyShouldBe(Table expectedMarketResource)
    {
        var expectedResource = expectedMarketResource.CreateSet<MarketResource>().First();
        var responseData = await Response.Result.Content.ReadAsStringAsync();
        var resource = JsonConvert.DeserializeObject<MarketResource>(responseData);
        Assert.Equal(expectedResource.storeName, resource.storeName);
    }
}