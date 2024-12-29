using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Selects
{
    public partial class SupplySelect : EntitySelectBase<Supply>
    {
        protected override void OnInitialized()
        {
            foreach (var s in DbContext.Supplies.Where(s => s.DeleteTime == null).AsNoTracking())
            {
                var selectedItem = new SelectedItem(s.Id.ToString(), s.Name);
                Items.Add(selectedItem, s);
            }
            base.OnInitialized();
        }
    }
}
