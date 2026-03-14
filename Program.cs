using AiDemo.Entities;
using AiDemo.Implementations;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Spectre.Console;
using Spectre.Console.Json;

AnsiConsole.Clear();

// 1. Show Banner
AnsiConsole.Write(
    new FigletText("AI Demo App")
        .LeftJustified()
        .Color(Color.Purple));

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var lmStudioUrl = configuration["LmStudio:Url"];
var lmStudioClient = configuration["LmStudio:Client"];
var lmStudioModel = configuration["LmStudio:Model"];
var lmStudioToken = configuration["LmStudio:Token"];

if (string.IsNullOrEmpty(lmStudioUrl) || string.IsNullOrEmpty(lmStudioModel) || string.IsNullOrEmpty(lmStudioToken))
{
    Console.WriteLine("LM Studio url, model or token is not set. Please set it in the configuration & secrets.");
    return;
}

var rootDir = AppContext.BaseDirectory;
var instructionFile = Path.Combine(rootDir, "AppData", "instructions.txt");
var appDataPath = Path.Combine(rootDir, "AppData", "Uploads");
var files = Directory.GetFiles(appDataPath);

var instructionContent = File.ReadAllText(instructionFile);

var instruction = new Instruction(lmStudioModel, instructionContent, 8000);
foreach (var file in files)
{
    await instruction.AddAttachment(file);
}

// For debug
//var jsonText = new JsonText(json);
//AnsiConsole.Write(jsonText);

AnsiConsole.MarkupLine("[green]Starting....[/]");

string? responseContent;

switch (lmStudioClient)
{
    case "OpenAI":
        var openAiEngine = new OpenAiEngine(lmStudioUrl, lmStudioToken);
        responseContent = await openAiEngine.SendRequestAsync(instruction);
        break;


    default:
        var lmEngine = new LmStudioEngine(lmStudioUrl, lmStudioToken);
        responseContent = await lmEngine.SendRequestAsync(instruction);
        break;
}

if (!string.IsNullOrEmpty(responseContent))
{
    var jsonText = new JsonText(responseContent);
    AnsiConsole.Write(jsonText);
}
else
{
    AnsiConsole.MarkupLine("[red]No response content received.[/]");
}

