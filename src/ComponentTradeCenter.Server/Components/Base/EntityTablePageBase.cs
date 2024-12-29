using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Data.Base;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Base
{
    public abstract class EntityTablePageBase<TItem> : OnlineComponentBase where TItem : EntityBase
    {
        protected abstract DbSet<TItem> DbSet { get; }

        protected abstract IQueryable<TItem> Items { get; }

        protected virtual IEnumerable<int> PageItemsSource => [20, 30, 40, 50];

        protected virtual Task<QueryData<TItem>> OnQueryAsync(QueryPageOptions options)
        {
            var items = Items.Where(options.ToFilterLambda<TItem>());
            var isSorted = false;
            if (!string.IsNullOrEmpty(options.SortName))
            {
                items = items.Sort(options.SortName, options.SortOrder);
                isSorted = true;
            }

            var total = items.Count();
            return Task.FromResult(new QueryData<TItem>()
            {
                Items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems),
                TotalCount = total,
                IsFiltered = true,
                IsSorted = isSorted,
                IsSearch = true
            });
        }

        protected virtual Task<string> GetModelCustomId<T>(object? d) where T : ICustomIdProvider
        {
            var ret = "";
            if (d is TableColumnContext<TItem, object?> odata && odata.Value is int id)
            {
                ret = T.GetCustomId(id);
            }
            else if (d is TableColumnContext<TItem, int> idata)
            {
                ret = T.GetCustomId(idata.Value);
            }
            return Task.FromResult(ret);
        }
    }
}
