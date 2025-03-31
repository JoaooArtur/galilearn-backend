using Core.Application.Messaging;
using Core.Shared.Errors;
using Core.Shared.Results;
using Subject.Domain;
using Subject.Persistence.Projections;
using Subject.Shared.Queries;
using Subject.Shared.Response;

namespace Subject.Application.UseCases.Queries
{
    public class GetRandomQuestionsByLessionIdHandler(ISubjectProjection<Projection.Question> projectionGateway) : IQueryHandler<ListRandomQuestionsByLessonIdQuery, List<RandomQuestionsByLessonIdResponse>>
    {
        public async Task<Result<List<RandomQuestionsByLessonIdResponse>>> Handle(ListRandomQuestionsByLessonIdQuery query, CancellationToken cancellationToken)
        {
            var questions = await projectionGateway.ListAsync(x => x.LessonId == query.LessonId, cancellationToken);

            if (questions is null)
                return Result.Failure<List<RandomQuestionsByLessonIdResponse>>(new NotFoundError(new/*DomainError.BillNotFound*/("QuestionsNotFound", "Questions não encontradas")));

            Random rand = new Random();

            var shuffled = questions.OrderBy(q => rand.Next())
                .Take(5)
                .Select(question => new RandomQuestionsByLessonIdResponse(
                    question.Id,
                    question.LessonId,
                    question.Text,
                    question.Level,
                    question.Answers.Select(answer => new AnswerResponse(
                        answer.Id,
                        answer.Text)).ToList(),
                        question.CreatedAt))
                .ToList();

            return Result.Success(shuffled);
        }
    }
}
