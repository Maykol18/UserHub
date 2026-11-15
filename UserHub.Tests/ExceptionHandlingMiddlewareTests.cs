

using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using UserHub.Api.Middleware;

namespace UserHub.Tests;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task MiddlewareTest_Returns500InternalErrorForReques()
    {
        //Arrange: create a tes web Host with our middleware.
        using var host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .Configure(app =>
                    {
                        app.UseMiddleware<ExceptionHandlingMiddleware>();

                        //Simulate an endpoint that throws an exception.
                        app.Run(_ =>
                            Task.FromException(
                                new InvalidOperationException(
                                    "Sensitive database information"
                                )
                            ));
                    });
            }).StartAsync();

            var client = host.GetTestClient();

            //Act: Send a request to the test server.
            using var response = await client.GetAsync("/Users");

            //Assert: Verify the HTTP response
            Assert.Equal(
                "application/problem+json",
                response.Content.Headers.ContentType?.MediaType
            );

            var responseBody = await response.Content.ReadAsStringAsync();

            using var jsonDocument = JsonDocument.Parse(responseBody);

            var root = jsonDocument.RootElement;

            //verify ProblemDetails properties.
            Assert.Equal(500, root.GetProperty("status").GetInt32());

            Assert.Equal("An unexpected error occured.", root.GetProperty("title").GetString());

            Assert.Equal("/Users", root.GetProperty("instance").GetString());

            //Verify that a trace ID is included.
            Assert.True(root.TryGetProperty("traceId", out var traceId));
            Assert.False(string.IsNullOrWhiteSpace(traceId.GetString()));

            //Verify that sensitive exception details are not exposed.
            Assert.DoesNotContain("Sensitive database information", responseBody);
    }
}