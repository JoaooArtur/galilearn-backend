using Core.Application.EventBus;
using Core.Application.EventStore;
using Core.Application.Services;
using Core.Application;
using Student.Persistence;
using Student.Application.Services;

namespace Student.Infrastructure.Services
{
    public class StudentApplicationService(IEventStore<StudentDbContext> eventStore, IEventBus eventBusGateway, IUnitOfWork<StudentDbContext> unitOfWork) : ApplicationService<StudentDbContext>(eventStore, eventBusGateway, unitOfWork), IStudentApplicationService { }
}
