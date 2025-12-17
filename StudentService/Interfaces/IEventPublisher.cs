using StudentService.Events;

namespace StudentService.Interfaces
{
    public interface IEventPublisher
    {
        // event is already reserved word in C# that's why we use @
        Task PublishStudentRegisteredAsync(StudentRegisteredEvent @event);
    }
}
