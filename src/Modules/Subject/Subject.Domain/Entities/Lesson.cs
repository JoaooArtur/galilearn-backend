using Core.Domain.Primitives;

namespace Subject.Domain.Entities;

public class Lesson : Entity
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public int Index { get; private set; }

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
        CreatedAt = createdAt;
    }

    public static Lesson Create(Guid id, string title, string content, int index)
        => new(id, title, content, index, DateTimeOffset.Now);

    public static Lesson Undefined
        => new(Guid.Empty, "Undefined", "Undefined", 0, DateTimeOffset.Now);
}
