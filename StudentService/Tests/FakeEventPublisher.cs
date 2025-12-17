public class FakeEventPublisher : IEventPublisher
{
    public List<StudentRegisteredEvent> PublishedEvents { get; } = new List<StudentRegisteredEvent>();

    public Task PublishStudentRegisteredAsync(StudentRegisteredEvent @event)
    {
        PublishedEvents.Add(@event);
        Console.WriteLine($"[FakeKafka] Event published: {@event.Email}");
        return Task.CompletedTask;
    }
}
