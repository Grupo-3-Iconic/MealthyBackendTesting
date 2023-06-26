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
public sealed class SupplyServiceStepDefinition : WebApplicationFactory<Program>
{
    private readonly WebApplicationFactory<Program> _factory;
    public SupplyServiceStepDefinition(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    private HttpClient Client { get; set; }
    private Uri BaseUri { get; set; }
    private Task<HttpResponseMessage> Response { get; set; }
    [Given(@"the endpoint https://localhost:(.*)/api/v(.*)/supply is available")]   
    public void GivenTheEndpointHttpsLocalhostApiVSupplyIsAvailable(int port, int version)
    {
        BaseUri = new Uri($"https://localhost:{port}/api/v{version}/supply");
        Client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = BaseUri
        });
    }
    [When(@"a post supply request is sent")]
    public void WhenAPostSupplyRequestIsSent(Table saveSupplyResource)
    {
        var resource = saveSupplyResource.CreateSet<SaveSupplyResource>().First();
        var content = new StringContent(resource.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
        Response = Client.PostAsync(BaseUri, content);
    }
    [Then(@"the post supply response should be status (.*)")]
    public void ThenThePostSupplyResponseShouldBeStatus(int expectedStatus)
    {
        var expectedStatusCode = ((HttpStatusCode)expectedStatus).ToString();
        var actualStatusCode = Response.Result.StatusCode.ToString();
        Assert.Equal(expectedStatusCode, actualStatusCode);
    }
    [Then(@"the post supply response body should be")]
    public async Task ThenThePostSupplyResponseBodyShouldBe(Table expectedSupplyResource)
    {
        var expectedResource = expectedSupplyResource.CreateSet<SupplyResource>().First();
        var responseData = await Response.Result.Content.ReadAsStringAsync();
        var resource = JsonConvert.DeserializeObject<SupplyResource>(responseData);
        Assert.Equal(expectedResource.Name, resource.Name);
    }
}