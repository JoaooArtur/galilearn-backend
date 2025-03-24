using Core.Domain.Primitives;
using Subject.Domain.Enumerations;

namespace Subject.Domain.Entities;

public class Lesson : Entity
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public int Index { get; private set; }
    public List<Question> Questions { get; private set; }

    public Lesson(
        Guid id,
        string title,
        string content,
        int index,
        DateTimeOffset createdAt)
    {
        Id = id;
        Title = title;
        Index = index;
        Content = content;
        CreatedAt = createdAt;
    }

    public static Lesson Create(Guid id, string title, string content, int index)
        => new(id, title, content, index, DateTimeOffset.Now);

    public Guid AddQuestion(string text, QuestionLevel level)
    {
        var questionId = Guid.NewGuid(); 
        Questions.Add(Question.Create(questionId, text, level));
        return questionId;
    }

    public static Lesson Undefined
        => new(Guid.Empty, "Undefined", "Undefined", 0, DateTimeOffset.Now);
}
