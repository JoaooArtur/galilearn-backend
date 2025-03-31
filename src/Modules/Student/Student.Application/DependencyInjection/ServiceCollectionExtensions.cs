using Microsoft.Extensions.DependencyInjection;
using Student.Application.UseCases.Events;

namespace Student.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventInteractors(this IServiceCollection services)
            => services.AddScoped<IProjectStudentWhenStudentChangedEventHandler, ProjectStudentWhenStudentChangedEventHandler>()
            .AddScoped<IProjectAttemptWhenAttemptChangedEventHandler, ProjectAttemptWhenAttemptChangedEventHandler>()
            .AddScoped<IProjectLessonProgressWhenLessonProgressChangedEventHandler, ProjectLessonProgressWhenLessonProgressChangedEventHandler>()
            .AddScoped<IProjectSubjectProgressWhenSubjectProgressChangedEventHandler, ProjectSubjectProgressWhenSubjectProgressChangedEventHandler>();
    }
}
