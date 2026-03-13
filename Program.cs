using Microsoft.Extensions.Configuration;
using RestSharp;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var options = new RestClientOptions("http://localhost:1234")
{
  Timeout = TimeSpan.FromSeconds(30)
};
var client = new RestClient(options);
var request = new RestRequest("/api/v1/chat", Method.Post);
request.AddHeader("Content-Type", "application/json");

var lmStudioToken = configuration["LmStudio:Token"];

if (string.IsNullOrEmpty(lmStudioToken))
{
  Console.WriteLine("LmStudio token is not set. Please set it in the configuration.");
  return;
}



request.AddHeader("Authorization", $"Bearer {lmStudioToken}");
var body = """
{
    "model": "google/gemma-3-27b",
    "input": "Are you there?",
    "context_length": 8000
  }
""";

request.AddStringBody(body, DataFormat.Json);
RestResponse response = await client.ExecuteAsync(request);
Console.WriteLine(response.Content);
