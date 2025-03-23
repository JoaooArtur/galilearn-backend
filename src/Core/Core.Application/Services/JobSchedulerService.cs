using Hangfire;
using MediatR;
using System.ComponentModel;

namespace Core.Application.Services;
public class JobSchedulerService : IJobSchedulerService
{
    public void Enqueue(string jobName, IRequest request)
    {
        var client = new BackgroundJobClient();
        client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request));
    }

    public void Enqueue(IRequest request)
    {
        var client = new BackgroundJobClient();
        client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
    }

    public void Schedule(string jobName, TimeSpan scheduleAt, IRequest request)
    {
        var client = new BackgroundJobClient();
        client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), scheduleAt);
    }

    public void Schedule<T>(string jobName, TimeSpan scheduleAt, IRequest<T> request)
    {
        var client = new BackgroundJobClient();
        client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), scheduleAt);
    }

    public void Schedule(string jobName, DateTimeOffset scheduleAt, IRequest request)
    {
        var client = new BackgroundJobClient();
        client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), scheduleAt);
    }

    public void Schedule<T>(string jobName, DateTimeOffset scheduleAt, IRequest<T> request)
    {
        var client = new BackgroundJobClient();
        client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), scheduleAt);
    }

    public void ScheduleRecurring<T>(string jobName, string cronExpression, IRequest<T> request)
    {
        var manager = new RecurringJobManager();
        manager.AddOrUpdate<MediatorHangfireBridge>(jobName, bridge => bridge.Send(jobName, request), cronExpression);
    }
}

public class MediatorHangfireBridge(IMediator mediator)
{
    private readonly IMediator _mediator = mediator;
    public async Task Send(IRequest command)
        => await _mediator.Send(command);

    [DisplayName("{0}")]
    public async Task Send(string _, IRequest command)
        => await _mediator.Send(command);
    public async Task Send<T>(string _, IRequest<T> command)
        => await _mediator.Send(command);
}