using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Selects
{
    public partial class TraderSelect : EntitySelectBase<Trader>
    {
        protected override void OnInitialized()
        {
            foreach (var t in DbContext.Traders.Where(t => t.DeleteTime == null).AsNoTracking())
            {
                var selectedItem = new SelectedItem(t.Id.ToString(), t.Name);
                Items.Add(selectedItem, t);
            }
            base.OnInitialized();
        }
    }
}
