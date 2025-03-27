using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
//builder.Configuration
//builder.Services
//builder.Environment

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

app.Run(async (HttpContext context) =>
{
    context.Response.Headers["MyKey"] = "My value";// can be customized
    context.Response.Headers["Date"] = "Thu, 27 Dec 1990 02:34:01 GMT";// take a look of this~ lol
    context.Response.Headers["Content-type"] = "text/html";
    // header is a dict

    // response header
    // has key value pair like Date Server
    // Content-type(text/plain, text/html/ application/json/application/xml),
    // Content length(bytes)...
    // Cashe-control: indicates number of seconds that the response can be cached at the broser(max-age = 60)
    // Set-Cookie: Contains cookies to send to browser (x=10)
    // Access-Control-Allow_Origin:  used to enable CORS (cross origin resource sharing) (Access-Control-Allow-Origin: http://www.exp.com)
    // Location: Contatins url to redirect (http://www.exp.com)
    // you can modify the value of default key wink wink

    context.Response.StatusCode = 200;
    await context.Response.WriteAsync("<h1>Hello there</h1>");
    await context.Response.WriteAsync("<h2>2nd response</h2>");

    string path = context.Request.Path;
    string method = context.Request.Method;
    await context.Response.WriteAsync($"<h2>{path}</h2>");
    await context.Response.WriteAsync($"<h2>{method}</h2>");

    //query string
    if (context.Request.Method == "GET")
    {
        if (context.Request.Query.ContainsKey("id"))
        {
            string id = context.Request.Query["id"];
            await context.Response.WriteAsync($"<h2>{id}</h2>");
        }
    }

    // request headers:
    // Accept: represend MIME type of content tobe accepted by client (text/html...)
    // Accept-Language Represend natrural lang of res centeng tobe accepted by client (en-US...)
    // Content-Type: MIME type of req body(text/x-www-form-urlencoded, application/json, application/xml, multipart/form-data...)
    // Centent-Length: length(bytes) of req body
    // Date: date and time of req
    // host Server domain name (www.exp.com)
    // User-Agent browser details (Mozilla/5.0, firefox/12.0...)

    string? userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
    if (!string.IsNullOrEmpty(userAgent))
    {
        await context.Response.WriteAsync($"<h2>{userAgent}</h2>");
    }

    //// read body of req
    StreamReader reader = new StreamReader(context.Request.Body);
    string body = await reader.ReadToEndAsync();
    //await context.Response.WriteAsync($"<h2>{body}</h2>");
    // convert query to dict// stringValues can represend more than one value
    Dictionary<string, StringValues> queryDict = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);

    if(queryDict.ContainsKey("firstName"))
    {
        string firstName = queryDict["firstName"][0];
        await context.Response.WriteAsync($"<h2>{firstName}</h2>");

        foreach (var num in queryDict["age"])
        {
            await context.Response.WriteAsync($"<h2>{num.ToString()}</h2>");
        }
    }
});

app.Run();