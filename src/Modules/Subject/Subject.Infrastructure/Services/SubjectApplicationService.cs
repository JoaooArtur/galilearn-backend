using Core.Application.EventBus;
using Core.Application.EventStore;
using Core.Application.Services;
using Core.Application;
using Subject.Persistence;
using Subject.Application.Services;

namespace Subject.Infrastructure.Services
{
    public class SubjectApplicationService(IEventStore<SubjectDbContext> eventStore, IEventBus eventBusGateway, IUnitOfWork<SubjectDbContext> unitOfWork) : ApplicationService<SubjectDbContext>(eventStore, eventBusGateway, unitOfWork), ISubjectApplicationService { }
}
