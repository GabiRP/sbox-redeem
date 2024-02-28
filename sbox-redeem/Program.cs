using sbox_redeem.WebDriver;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Driver driver = new();
app.MapPost("/{key?}", async (string? key) =>
{
    if (string.IsNullOrEmpty(key)) return "No key provided.";
    return await driver.TryRedeem(key);
});

app.MapPost("/session/{key?}", async (string? key) =>
{
    if(string.IsNullOrEmpty(key)) return;
    driver.GetNewSessionGuid(key);
});

app.Run();