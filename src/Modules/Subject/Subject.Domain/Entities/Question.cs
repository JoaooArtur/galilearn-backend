using Core.Domain.Primitives;
using Subject.Domain.Enumerations;
using Subject.Domain.ValueObjects;

namespace Subject.Domain.Entities;

public class Question : Entity
{
    public string Text { get; private set; }
    public QuestionLevel Level { get; private set; }
    public List<AnswerOption> AnswerOptions { get; private set; }

    public Question(
        Guid id,
        string text,
        List<AnswerOption> answers,
        QuestionLevel level,
        DateTimeOffset createdAt)
    {
        Id = id;
        Text = text;
        AnswerOptions = answers;
        Level = level;
        CreatedAt = createdAt;
    }

    public static Question Create(Guid id, string text, QuestionLevel level)
        => new(id, text, [], level, DateTimeOffset.Now);

    public void AddAnswer(Guid answerId, string text, bool IsRightAnswer = false) 
        => AnswerOptions.Add(AnswerOption.Create(new(answerId, text, IsRightAnswer)));

    public static Question Undefined
        => new(Guid.Empty, "Undefined", [], QuestionLevel.Introduction, DateTimeOffset.Now);
}
