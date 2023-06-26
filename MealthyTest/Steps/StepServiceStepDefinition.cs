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
public sealed class StepServiceStepDefinition : WebApplicationFactory<Program>
{
    private readonly WebApplicationFactory<Program> _factory;

    public StepServiceStepDefinition(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    private HttpClient Client { get; set; }
    private Uri BaseUri { get; set; }
    private Task<HttpResponseMessage> Response { get; set; }
    [Given(@"the endpoint https://localhost:(.*)/api/v(.*)/step is available")]
    public void GivenTheEndpointHttpsLocalhostApiVStepIsAvailable(int port, int version)
    {
        BaseUri = new Uri($"https://localhost:{port}/api/v{version}/step");
        Client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = BaseUri
        });
    }
    [When(@"a Post step request is sent")]
    public void WhenAPostStepRequestIsSent(Table saveStepResource)
    {
        var resource = saveStepResource.CreateSet<SaveStepResource>().First();
        var content = new StringContent(resource.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
        Response = Client.PostAsync(BaseUri, content);
    }
    [Then(@"the post step response should be status (.*)")]
    public void ThenThePostStepResponseShouldBeStatus(int expectedStatus)
    {
        var expectedStatusCode = ((HttpStatusCode)expectedStatus).ToString();
        var actualStatusCode = Response.Result.StatusCode.ToString();
        Assert.Equal(expectedStatusCode, actualStatusCode);
    }
    [Then(@"the post step response body should be")]
    public async Task ThenThePostStepResponseBodyShouldBe(Table expectedStepResource)
    {
        var expectedResource = expectedStepResource.CreateSet<StepResource>().First();
        var responseData = await Response.Result.Content.ReadAsStringAsync();
        var resource = JsonConvert.DeserializeObject<StepResource>(responseData);
        Assert.Equal(expectedResource.Description, resource.Description);
    }
}