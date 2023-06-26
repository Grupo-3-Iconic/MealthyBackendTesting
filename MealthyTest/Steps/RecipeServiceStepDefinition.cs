using System.Net;
using System.Net.Mime;
using System.Text;
using Mealthy.Mealthy.Resources;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using SpecFlow.Internal.Json;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace MealthyTest.Steps;

[Binding]
public sealed class RecipeServiceStepDefinition : WebApplicationFactory<Program>
{
    private readonly WebApplicationFactory<Program> _factory;
    public RecipeServiceStepDefinition(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    private HttpClient Client { get; set; }
    private Uri BaseUri { get; set; }
    private Task<HttpResponseMessage> Response { get; set; }
    [Given(@"the endpoint https://localhost:(.*)/api/v(.*)/recipe is available")]
    public void GivenTheEndpointHttpsLocalhostApiVRecipeIsAvailable(int port, int version)
    {
        BaseUri = new Uri($"https://localhost:{port}/api/v{version}/recipe");
        Client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = BaseUri
        });
    }
    [When(@"a Post request is sent")]
    public void WhenAPostRequestIsSent(Table saveRecipeResource)
    {
        var resource = saveRecipeResource.CreateSet<SaveRecipeResource>().First();
        var content = new StringContent(resource.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
        Response = Client.PostAsync(BaseUri, content);
    }
    [Then(@"A Response is returned with Status (.*)")]
    public void ThenAResponseIsReturnedWithStatus(int expectedStatus)
    {
        var expectedStatusCode = ((HttpStatusCode)expectedStatus).ToString();
        var actualStatusCode = Response.Result.StatusCode.ToString();
        Assert.Equal(expectedStatusCode, actualStatusCode);
    }
    [Then(@"A Recipe Resource is included in the response body")]
    public async Task ThenARecipeResourceIsIncludedInTheResponseBody(Table expectedRecipeResource)
    {
        var expectedResource = expectedRecipeResource.CreateSet<RecipeResource>().First();
        var responseData = await Response.Result.Content.ReadAsStringAsync();
        var resource = JsonConvert.DeserializeObject<RecipeResource>(responseData);
        Assert.Equal(expectedResource.Title, resource.Title);
    }
    [Given(@"a recipe is already stored")]
    public async void GivenARecipeIsAlreadyStored(Table saveRecipeResource)
    {
        var resource = saveRecipeResource.CreateSet<SaveRecipeResource>().First();
        var content = new StringContent(resource.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
        Response = Client.PostAsync(BaseUri, content);
        
        var responseData = await Response.Result.Content.ReadAsStringAsync();
        var responseResource = JsonConvert.DeserializeObject<RecipeResource>(responseData);
        Assert.Equal(resource.Title, responseResource.Title);
    }
    [Then(@"An Error is returned with value ""(.*)""")]
    public void ThenAnErrorIsReturnedWithValue(string expectedError)
    {
        var message = Response.Result.Content.ReadAsStringAsync().Result;
        Assert.Equal(expectedError, message);
    }
}