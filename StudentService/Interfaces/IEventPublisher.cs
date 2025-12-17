using StudentService.Events;

namespace StudentService.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishStudentRegisteredAsync(StudentRegisteredEvent @event);
    }
}
