using EaseyReportsDomain.Interfaces;
using Microsoft.Extensions.Configuration;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace EasyReportsDataAccess.ConnectionModelIA
{
    public class ConnectionModelIA : IConnectionModelIA
    {
        const string apiKey = "IAApiKey";
        const string endpoint = "https://api.openai.com/v1/chat/completions";
        const string model = "gpt-4o-mini";
        private readonly string OpenAIApiKey;

        public ConnectionModelIA(IConfiguration config)
        {
            OpenAIApiKey = config.GetConnectionString(apiKey);
        }
        public async Task<string> GenerateSQlQueryString(string prompt)
        {
            var openaAIService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = OpenAIApiKey,
            });

            var completionResult = await openaAIService.ChatCompletion.CreateCompletion(
                new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromUser(prompt)
                    },
                    Model = model
                });

            if (!completionResult.Successful)
            {
                throw new Exception(completionResult.Error.Message);
            }
            var response = completionResult.Choices.First().Message.Content;
            return response.Substring(response.IndexOf("SELECT"));
        }
    }

}
