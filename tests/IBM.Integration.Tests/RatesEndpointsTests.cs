using IBM.API.Endpoints;
using IBM.Application.Models.DTOs.Rates;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace IBM.Integration.Tests;

public class RatesEndpointsTests
{
    [Fact]
    public async Task GetAllRatesEndpoint_NoParams_ValidResult()
    {
        // Arrange
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateDefaultClient();

        // Act
        var response = await httpClient.GetAsync(Endpoints.Rates.GetAll);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<RateDTO>>();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Any());
    }
}