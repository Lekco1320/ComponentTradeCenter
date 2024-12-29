using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Data.Base;

namespace ComponentTradeCenter.Server.Components.Base
{
    public interface IEditableTablePage<TItem> where TItem : EntityBase
    {
        bool IsAddable { get; }

        bool IsEditable { get; }

        bool IsDeleteable { get; }

        Task<TItem> OnAddAsync();

        Task<bool> OnSaveAsync(TItem entity, ItemChangedType changedType);

        Task<bool> OnDeleteAsync(IEnumerable<TItem> items);
    }
}
