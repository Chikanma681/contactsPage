using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace server.Tests;

public class BasicTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    
    public BasicTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task GetAllContacts_ReturnsOkResult()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/contacts");

        if(!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Request failed with status code {response.StatusCode}: {content}");
        }

        var contacts = await response.Content.ReadAsAsync<IEnumerable<GetEmployeeResponse>>();
        Assert.NotNull(contacts);
    }

    [Fact]
    public async Task GetContactById_ReturnsOkResult()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/contacts/1");

        if(!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Request failed with status code {response.StatusCode}: {content}");
        }

        var contact = await response.Content.ReadAsAsync<GetEmployeeResponse>();
        Assert.NotNull(contact);
    }

    [Fact]
    public async Task CreateContact_ReturnsCreatedResult
    {
        var client = _factory.CreateClient();
        var response = client.PostasJsonAsync("/api/contacts", new Contacts { FirstName = "John", LastName="Doe" });

        response.EnsureSuccessStatusCode();
    }


    // [Fact]
    // public async Task CreateE
}