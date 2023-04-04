using IBM.API.Endpoints;
using IBM.Application.Constants;
using IBM.Application.Models.DTOs.Common;
using IBM.Application.Models.DTOs.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace IBM.Integration.Tests;

public class TransactionEndpointsTests
{
    [Fact]
    public async Task GetAllTransactionsEndpoint_DefaultPagination_ValidResult()
    {
        // Arrange
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateDefaultClient();

        // Act
        var response = await httpClient.GetAsync(Endpoints.Transactions.GetAll);
        var result = await response.Content.ReadFromJsonAsync<PaginatedListDTO<TransactionDTO>>();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Items.Any());
        Assert.Equal(PaginationDTO.DefaultPage, result.Page);
        Assert.Equal(PaginationDTO.DefaultPageSize, result.PageSize);
        Assert.True(result.TotalItems != 0);
    }

    [Fact]
    public async Task GetAllTransactionsEndpoint_InvalidPagination_BadRequest()
    {
        // Arrange
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateDefaultClient();
        string url = $"{Endpoints.Transactions.GetAll}?page=1&pageSize=1000";
        // Act
        var response = await httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal($"Page size must be between {PaginationDTO.MinPageSize} and {PaginationDTO.MaxPageSize}.", result!.Title);
    }

    [Fact]
    public async Task GetTransactionsBySkuEndpoint_DefaultCurrency_ValidResult()
    {
        // Arrange
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateDefaultClient();
        string sku = "R2008";
        string url = Endpoints.Transactions.GetBySku.Replace("{sku}", sku);

        // Act
        var response = await httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<SkuTransactionsDTO>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Currencies.EUR, result.Currency);
        Assert.Equal(sku, result.Sku);
        Assert.True(result.Transactions.Any());
        Assert.True(result.Total != 0);
    }

    [Fact]
    public async Task GetTransactionsBySkuEndpoint_InvalidCurrency_BadRequest()
    {
        // Arrange
        var webAppFactory = new WebApplicationFactory<Program>();
        var httpClient = webAppFactory.CreateDefaultClient();
        string sku = "R2008";
        string currency = "ZXC";
        string url = $"{Endpoints.Transactions.GetBySku.Replace("{sku}", sku)}?currency={currency}";

        // Act
        var response = await httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal($"Currency '{currency}' is invalid", result!.Title);
    }
}