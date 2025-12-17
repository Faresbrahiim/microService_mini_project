using System.Text.Json;
using Confluent.Kafka;
using StudentService.Events;
using StudentService.Interfaces;

namespace StudentService.Infrastructure
{
    public class KafkaEventPublisher : IEventPublisher
    {
        private readonly IProducer<Null, string> _producer;
        private const string TopicName = "student-registered";

        public KafkaEventPublisher()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task PublishStudentRegisteredAsync(StudentRegisteredEvent @event)
        {
            var message = JsonSerializer.Serialize(@event);

            await _producer.ProduceAsync(
                TopicName,
                new Message<Null, string> { Value = message }
            );
        }
    }
}
