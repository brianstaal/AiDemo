using AiDemo.Entities;
using System.Text.Json;

namespace AiDemo.Implementations
{
    public class OpenAiEngine
    {
        private string endpoint { get; set; }

        private string token { get; set; }

        public OpenAiEngine(string endpoint, string token)
        {
            this.endpoint = endpoint;
            this.token = token;
        }

        public async Task<string> SendRequestAsync(Instruction instruction)
        {
            var json = JsonSerializer.Serialize(instruction);



            return "";
        }


    }
}
