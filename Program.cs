using AiDemo;
using Spectre.Console;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using RestSharp;
using Spectre.Console.Json;

AnsiConsole.Clear();

// 1. Show Banner
AnsiConsole.Write(
    new FigletText("AI Demo App")
        .LeftJustified()
        .Color(Color.Lime));

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var lmStudioUrl = configuration["LmStudio:Url"];
var lmStudioToken = configuration["LmStudio:Token"];

if (string.IsNullOrEmpty(lmStudioToken) || string.IsNullOrEmpty(lmStudioUrl))
{
    Console.WriteLine("LmStudio token or url is not set. Please set it in the configuration.");
    return;
}

var rootDir = AppContext.BaseDirectory;
var instructionFile = Path.Combine(rootDir, "AppData", "instructions.txt");
var appDataPath = Path.Combine(rootDir, "AppData", "Uploads");
var files = Directory.GetFiles(appDataPath);

var instructionContent = File.ReadAllText(instructionFile);

var instruction = new Instruction("google/gemma-3-27b", instructionContent, 8000);
foreach (var file in files)
{
    await instruction.AddAttachment(file);
}

var json = JsonSerializer.Serialize(instruction);
//var jsonText = new JsonText(json);
//AnsiConsole.Write(jsonText);


var options = new RestClientOptions(lmStudioUrl)
{
    Timeout = TimeSpan.FromSeconds(120)
};
var client = new RestClient(options);
var request = new RestRequest("/api/v1/chat", Method.Post);
request.AddHeader("Content-Type", "application/json");

request.AddHeader("Authorization", $"Bearer {lmStudioToken}");
var body = json;

request.AddStringBody(body, DataFormat.Json);
RestResponse response = await client.ExecuteAsync(request);
Console.WriteLine(response.Content);
if (!response.IsSuccessful)
{
    Console.WriteLine($"Request failed with status code: {response.StatusCode}");
    return;
}
if (!string.IsNullOrEmpty(response.Content))
{
    var jsonText = new JsonText(response.Content);
    AnsiConsole.Write(jsonText);
}

