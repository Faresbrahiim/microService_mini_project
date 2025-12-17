using System.Text.Json;
using Confluent.Kafka;
using StudentService.Events;
using StudentService.Interfaces;

namespace StudentService.Infrastructure
{
    public class KafkaEventPublisher : IEventPublisher
    {
        // Kafka producer instance used to send messages to Kafka topics
        private readonly IProducer<Null, string> _producer;
        // name of the Kafka topic to publish events to
        private const string TopicName = "student-registered";

        public KafkaEventPublisher()
        {
            // Kafka producer configuration with bootstrap server address localhost:9092 
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            // ready to produce messages with null key and string value
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishStudentRegisteredAsync(StudentRegisteredEvent @event)
        {
            // serialize the event object to JSON string
            var message = JsonSerializer.Serialize(@event);
            // produce the message to the specified Kafka topic asynchronously
            await _producer.ProduceAsync(
                TopicName,
                new Message<Null, string> { Value = message }
            );
        }
    }
}
