using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Data.Base;

namespace ComponentTradeCenter.Server.Components.Base
{
    public abstract class ModelTablePageBase<TItem> : EntityTablePageBase<TItem> where TItem : ModelBase, ICustomIdProvider
    {
        protected virtual Task<string> FormatModelId(object? d)
        {
            var ret = "";
            if (d is TableColumnContext<TItem, object?> odata && odata.Value is int id)
            {
                ret = TItem.GetCustomId(id);
            }
            else if (d is TableColumnContext<TItem, int> idata)
            {
                ret = TItem.GetCustomId(idata.Value);
            }
            return Task.FromResult(ret);
        }
    }
}
