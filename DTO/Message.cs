using System;
using Newtonsoft.Json;

namespace Katalyse.Functions.DTO
{
    public class Message
    {

        [JsonProperty("messageType")]
        public string MessageType { get; }

        [JsonProperty("payload")]
        public string Payload { get; }

        public Message(string messageType, string payload)
        {
            if (string.IsNullOrEmpty(messageType))
                throw new ArgumentNullException(nameof(messageType));

            MessageType = messageType;
            Payload = payload;
        }
    }
}