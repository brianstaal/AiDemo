using AiDemo.Entities;
using RestSharp;
using System.Text.Json;

namespace AiDemo.Implementations;

public class LmStudioEngine
{
    private string lmStudioUrl { get; set; }

    private string lmStudioToken { get; set; }

    public LmStudioEngine(string endpoint, string token)
    {
        lmStudioUrl = endpoint;
        lmStudioToken = token;
    }

    public async Task<string> SendRequestAsync(Instruction instruction)
    {
        var json = JsonSerializer.Serialize(instruction);

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
            return $"Request failed with status code: {response.StatusCode}";
        }
        if (!string.IsNullOrEmpty(response.Content))
        {
            return response.Content;
        }

        return "";
    }
}
