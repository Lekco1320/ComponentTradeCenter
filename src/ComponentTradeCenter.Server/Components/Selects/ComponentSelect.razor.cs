using BootstrapBlazor.Components;
using ComponentTradeCenter.Server.Components.Base;
using ComponentTradeCenter.Server.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ComponentTradeCenter.Server.Components.Selects
{
    public partial class ComponentSelect : EntitySelectBase<Component>
    {
        protected override void OnInitialized()
        {
            foreach (var c in DbContext.Components.Where(c => c.DeleteTime == null).AsNoTracking())
            {
                var selectedItem = new SelectedItem(c.Id.ToString(), c.Name);
                Items.Add(selectedItem, c);
            }
            base.OnInitialized();
        }
    }
}
