

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
        // using var host = await new HostBuilder()
        //     .ConfigureWebHost(webBuilder =>
        //         webBuilder
        //             .UseTestServer()
        //             .Configure(app =>{
        //                 app.UseMiddleware<ExceptionHandlingMiddleware>();
        //             }
        //     )).StartAsync();

        //     var server = host.GetTestServer();
        //     server.BaseAddress = new Uri("https://example.com/A/Path");

        //     var context = await server.SendAsync(c =>
        //     {
        //         c.Request.Method = HttpMethods.Post;
        //         c.Request.Path = "/and/file.txt";
        //         c.Request.QueryString = new QueryString("?and=query");
        //     });

        //     Assert.Equal(500, await ExceptionHandlingMiddleware.);
    }
}