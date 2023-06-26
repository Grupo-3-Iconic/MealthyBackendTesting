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
public sealed class IngredientServiceStepDefinition : WebApplicationFactory<Program>
{
    private readonly WebApplicationFactory<Program> _factory;
    IngredientServiceStepDefinition(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    private HttpClient Client { get; set; }
    private Uri BaseUri { get; set; }
    private Task<HttpResponseMessage> Response { get; set; }
    [Given(@"the endpoint https://localhost:(.*)/api/v(.*)/ingredient is available")]
    public void GivenTheEndpointHttpsLocalhostApiVIngredientIsAvailable(int port, int version)
    {
        BaseUri = new Uri($"https://localhost:{port}/api/v{version}/ingredient");
        Client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = BaseUri
        });
    }
    [When(@"a Post ingredient request is sent")]
    public void WhenAPostIngredientRequestIsSent(Table saveIngredientResource)
    {
        var resource = saveIngredientResource.CreateSet<SaveIngredientResource>().First();
        var content = new StringContent(resource.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
        Response = Client.PostAsync(BaseUri, content);
    }
    [Then(@"the ingredient response should be status(.*)")]
    public void ThenTheIngredientResponseShouldBeStatus(int expectedStatus)
    {
        var expectedStatusCode = ((HttpStatusCode)expectedStatus).ToString();
        var actualStatusCode = Response.Result.StatusCode.ToString();
        Assert.Equal(expectedStatusCode, actualStatusCode);
    }
    [Then(@"the ingredient response body should be")]
    public async Task ThenTheIngredientResponseBodyShouldBe(Table expectedIngredientResource)
    {
        var expectedResource = expectedIngredientResource.CreateSet<IngredientResource>().First();
        var responseData = await Response.Result.Content.ReadAsStringAsync();
        var resource = JsonConvert.DeserializeObject<IngredientResource>(responseData);
        Assert.Equal(expectedResource.Name, resource.Name);
    }
    
    [Then(@"the response should be status (.*)")]
    public void ThenTheResponseShouldBeStatus(int expectedStatus)
    {
        var expectedStatusCode = ((HttpStatusCode)expectedStatus).ToString();
        var actualStatusCode = Response.Result.StatusCode.ToString();
        Assert.Equal(expectedStatusCode, actualStatusCode);
    }
}