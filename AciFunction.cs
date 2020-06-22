using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Katalyse.Functions.Services;
using Newtonsoft.Json;
using Katalyse.Functions.DTO;
using System.Collections.Generic;
using System.Text;

namespace Katalyse.Functions
{
    public class AciFunction
    {
        private readonly IContainerService service;
        private readonly FunctionConfig config;

        public AciFunction(IContainerService service, FunctionConfig config)
        {
            this.service = service;
            this.config = config;
        }

        [FunctionName("AciFunction")]
        [return: ServiceBus("afcommands", Connection = "ServiceBusConnectionString")]
        public async Task<Microsoft.Azure.ServiceBus.Message> RunAsync(
            [ServiceBusTrigger("afcommands", "AciFunction", Connection = "ServiceBusConnectionString")] string message,
            ILogger log)
        {
            log.LogInformation($"Function started.");
            Microsoft.Azure.ServiceBus.Message result;
            Message parsedMessage = ParseMessage(message, log);

            try
            {
                switch (parsedMessage.MessageType)
                {
                    case MessageTypes.START_CONTAINER:
                        var resourceId = await CreateAndStartContainerAsync(parsedMessage);
                        result = BuildOutputMessage(MessageTypes.START_CONTAINER_DONE, resourceId);
                        break;
                    case MessageTypes.DELETE_CONTAINER:
                        await CleanUpTerminatedContainerAsync(parsedMessage);
                        result = BuildOutputMessage(MessageTypes.DELETE_CONTAINER_DONE, parsedMessage.Payload);
                        break;
                    default:
                        throw new Exception($"Message type {parsedMessage.MessageType} not supported.");
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                result = BuildOutputMessage(MessageTypes.START_CONTAINER_ERROR, new { Error = ex.Message });
            }

            log.LogInformation("Function completed.");
            return result;
        }

        private Message ParseMessage(string message, ILogger log)
        {
            log.LogInformation("Deserializing message.");

            try
            {
                var result = JsonConvert.DeserializeObject<Message>(message);
                log.LogInformation($"Message type found: {result.MessageType}");

                return result;
            }
            catch (Exception ex)
            {
                log.LogError(ex.ToString());
                throw;
            }
        }

        private Microsoft.Azure.ServiceBus.Message BuildOutputMessage(string messageType, object payload = null)
        {
            var outputMessage = new Microsoft.Azure.ServiceBus.Message();
            outputMessage.ContentType = messageType;

            if (payload != null)
                outputMessage.Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));

            return outputMessage;
        }

        private async Task<string> CreateAndStartContainerAsync(Message message)
        {
            // Add here environment variables needed by your container, if any.
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("EnvVarExample", "ValueExample");

            return await service.CreateContainerAsync(config.ImageName, dictionary);
        }

        private async Task CleanUpTerminatedContainerAsync(Message message)
        {
            if(string.IsNullOrEmpty(message.Payload))
                throw new Exception("Message payload does not container ACI resource ID.");

            await service.DeleteContainerAsync(message.Payload);
        }
    }
}
