namespace Core.Domain.Primitives
{
    public interface IPagedResult<out TObject>
    {
        IReadOnlyCollection<TObject> Items { get; }
        Page Page { get; }
    }
}
