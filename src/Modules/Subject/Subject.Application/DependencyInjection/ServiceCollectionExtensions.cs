using Microsoft.Extensions.DependencyInjection;
using Subject.Application.UseCases.Events;

namespace Subject.Application.DependencyInjection
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventInteractors(this IServiceCollection services)
            => services.AddScoped<IProjectLessonWhenLessonChangedEventHandler, ProjectLessonWhenLessonChangedEventHandler>()
                    .AddScoped<IProjectQuestionWhenQuestionChangedEventHandler, ProjectQuestionWhenQuestionChangedEventHandler>()
                    .AddScoped<IProjectSubjectWhenSubjectChangedEventHandler, ProjectSubjectWhenSubjectChangedEventHandler>();
    }
}
