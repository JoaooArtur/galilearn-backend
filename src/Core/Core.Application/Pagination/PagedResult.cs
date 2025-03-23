using Core.Domain.Primitives;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;

namespace Core.Application.Pagination
{
    public record PagedResult<TItem>(IReadOnlyCollection<TItem> Items, Paging Paging)
        : IPagedResult<TItem> where TItem : class
    {
        public Page Page => new(Items.Count > Paging.Size, Paging.Number > 0, Paging.Number, Paging.Size) { };

        [JsonIgnore]
        private Paging Paging { get; } = Paging;

        public static IPagedResult<TItem> Create
            (Paging paging, IQueryable<TItem> source)
            => new PagedResult<TItem>(ApplyPagination(paging, source)?.ToList(), paging);

        private static IQueryable<TItem> ApplyPagination(Paging paging, IQueryable<TItem> source)
            => source.Skip(paging.Size * (paging.Number - 1)).Take(paging.Size + 1);
    }

}
